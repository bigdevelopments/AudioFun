namespace Cycle.Core.Audio;

public class Unit : Component
{
	private readonly Dictionary<string, Component> _components;

	public Unit() 
	{
		_components = new Dictionary<string, Component>(StringComparer.OrdinalIgnoreCase);
	}

	public T Add<T>(string name, T component) where T : Component
	{
		_components[name] = component;
		return component;
	}

	public void Connect(string source, string sourceOutput, string target, string targetInput)
	{
		if (!_components.ContainsKey(source)) throw new ArgumentException($"Component {source} not found.");
		if (!_components.ContainsKey(target)) throw new ArgumentException($"Component {target} not found.");
		var sourceComponent = _components[source];
		var targetComponent = _components[target];

		var input = targetComponent.GetConnection(targetInput);
		if (input == null) throw new ArgumentException($"Input [{target}.{targetInput}] not found.");
		var output = sourceComponent.GetConnection(sourceOutput);

		if (input is SignalInput signalInput)
		{
			if (!(output is SignalOutput)) throw new ArgumentException($"Output [{source}.{sourceOutput}] is not a valid SignalOutput.");
			signalInput.ConnectTo(output);
			return;
		}

		if (input is MidiInput midiInput)
		{
			if (!(output is MidiOutput)) throw new ArgumentException($"Output [{source}.{sourceOutput}] is not a valid SignalOutput.");
			midiInput.ConnectTo(output as MidiOutput);
			return;

		}
	}

	public override void OnInitialise(int sampleRate)
	{
		foreach (var component in _components.Values) component.Initialise(sampleRate);
	}

	public override void Tick()
	{
		foreach (var component in _components.Values)component.Tick();
	}

	public void ExposeConnection(string componentName, string connectionName, string externalName)
	{
		var component = _components.GetValueOrDefault(componentName);
		if (component == null) throw new ArgumentException($"Component {component} not found.");

		var connection = component.GetConnection(connectionName);
		if (connection != null)
		{
			AddConnection(externalName, connection);
			return;
		}
	}
}
