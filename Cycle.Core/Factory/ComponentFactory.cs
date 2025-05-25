using System.Text.Json;

namespace Cycle.Core.Factory;

/// <summary>
/// A factory for creating both primitive and unit components
/// </summary>
public class ComponentFactory
{
	// a dictionary of named primitives
	private readonly Dictionary<string, Type> _primitives;

	// a dictionary of unit specifications
	private readonly Dictionary<string, UnitSpec> _unitSpecs;

	public ComponentFactory()
	{
		// create the dictionaries
		_primitives = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
		_unitSpecs = new Dictionary<string, UnitSpec>(StringComparer.OrdinalIgnoreCase);

		// discover all the primitive components
		var primitives = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(assembly => assembly.GetTypes())
			.Where(type => type.IsClass &&
				typeof(Component).IsAssignableFrom(type) &&
				type.GetCustomAttributes(typeof(PrimitiveAttribute), false).Any())
			.ToList();

		// add them to our primitive look up dictionary
		foreach (var primitive in primitives)
		{
			var attribute = (PrimitiveAttribute)primitive.GetCustomAttributes(typeof(PrimitiveAttribute), false).FirstOrDefault();
			_primitives[attribute.Name] = primitive;
		}
	}

	public void AddUnitSpec(string path)
	{
		if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path), "Path cannot be null or empty");
		if (!File.Exists(path)) throw new FileNotFoundException($"Unit specification file not found at '{path}'", path);
		try
		{
			using FileStream stream = File.OpenRead(path);
			using JsonDocument doc = JsonDocument.Parse(stream);
			var unitSpec = JsonSerializer.Deserialize<UnitSpec>(doc);

			AddUnitSpec(unitSpec);
		}
		catch (JsonException ex)
		{
			throw new InvalidOperationException($"Failed to deserialize unit specification from '{path}'", ex);
		}
	}


	public void AddUnitSpec(UnitSpec unitSpec)
	{
		if (unitSpec == null) throw new ArgumentNullException(nameof(unitSpec), "Unit specification cannot be null");
		if (string.IsNullOrWhiteSpace(unitSpec.Name)) throw new ArgumentException("Unit specification must have a valid name", nameof(unitSpec));
		if (_unitSpecs.ContainsKey(unitSpec.Name)) throw new ArgumentException($"Unit specification with name '{unitSpec.Name}' already exists", nameof(unitSpec));
		var errors = unitSpec.Validate();
		if (errors.Any())
		{
			throw new InvalidOperationException($"Unit specification '{unitSpec.Name}' is invalid: {string.Join(", ", errors)}");
		}
		_unitSpecs[unitSpec.Name] = unitSpec;
	}

	public Component Create(string name, params string[] parameters)
	{
		if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name), "Component name cannot be null or empty");

		var error = Validation.ValidateIdentifier(name);
		if (error != null) throw new ArgumentException(error, nameof(name));

		if (_primitives.TryGetValue(name, out var type))
		{
			try
			{
				var component = (Component)Activator.CreateInstance(type, parameters);
				return component;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Failed to create component '{name}'", ex);
			}
		}

		// not a primitive, so it must be a unit
		return CreateUnit(name);
	}

	private Component CreateUnit(string name)
	{
		if (!_unitSpecs.TryGetValue(name, out var unitSpec))
		{
			throw new KeyNotFoundException($"No component found with name '{name}'");
		}

		Unit unit = new Unit();

		foreach (var elementSpec in unitSpec.Elements)
		{
			string elementName = elementSpec.Key.Trim();
			(string elementType, string[] elementParameters) = ParseComponentName(elementSpec.Value.Trim());

			// create the component
			Component component = Create(elementType, elementParameters);

			if (component == null)
			{
				throw new InvalidOperationException($"Failed to create component '{elementType}' for unit '{unitSpec.Name}'");
			}

			// add the component to the unit
			unit.Add(elementName, component);
		}

		foreach (var connection in unitSpec.Connections)
		{
			string[] targetData = connection.Key.Split(':');
			string[] sourceData = connection.Value.Split(':');
			if (targetData.Length != 2 || sourceData.Length != 2)
			{
				throw new InvalidOperationException($"Connection '{connection.Key} => {connection.Value}' is not in the correct format. Expected 'element:connection'.");
			}
			string targetElement = targetData[0].Trim();
			string targetConnection = targetData[1].Trim();
			string sourceElement = sourceData[0].Trim();
			string sourceConnecton = sourceData[1].Trim();
			unit.Connect(sourceElement, sourceConnecton, targetElement, targetConnection);
		}

		foreach (var exposed in unitSpec.Exposures)
		{
			string externalName = exposed.Key.Trim();
			string[] connectionData = exposed.Value.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
			if (connectionData.Length != 2) throw new InvalidOperationException($"Exposure '{exposed.Key} => {exposed.Value}' is not in the correct format. Expected 'element:connection'.");
			string componentName = connectionData[0].Trim();
			string connectionName = connectionData[1].Trim();
			unit.ExposeConnection(componentName, connectionName, externalName);
		}

		return unit;
	}

	public (string name, string[] parameters) ParseComponentName(string name)
	{
		if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name), "Component name cannot be null or empty");
		int paramsIndex = name.IndexOf('(');
		if (paramsIndex == -1)
		{
			return (name.Trim(), Array.Empty<string>());
		}
		string componentName = name.Substring(0, paramsIndex).Trim();
		string paramString = name.Substring(paramsIndex + 1, name.Length - paramsIndex - 2).Trim();
		string[] parameters = paramString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
			.Select(p => p.Trim()).ToArray();
		return (componentName, parameters);
	}
}