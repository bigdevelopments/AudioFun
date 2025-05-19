namespace FunWithAudio.Common.Core;

/// <summary>
/// The surface holds all the components, including hybrids and handles the ticking
/// </summary>
public class ComponentSurface
{
	private readonly Dictionary<string, Component> _components;
	private int _sampleRate;
	private int _bufferSize;	

	public ComponentSurface()
	{
		_components = new Dictionary<string, Component>(StringComparer.OrdinalIgnoreCase);	
	}

	public T Add<T>(string name, T component) where T: Component
	{
		if (_components.ContainsKey(name))
			throw new ArgumentException($"Component with name {name} already exists.");

		_components.Add(name, component);
		component.OnAdd(this, name);
		return component;
	}

	public void Initialise(int sampleRate, int bufferSize)
	{
		_sampleRate = sampleRate;
		_bufferSize = bufferSize;

		foreach (var component in _components.Values)
		{
			component.Initialise(sampleRate, bufferSize);
		}
	}

	public void Tick()
	{
		foreach (var component in _components.Values) component.Tick();
	}

	public void Connect(string source, string sourceOutput, string target, string targetInput)
	{
		if (!_components.ContainsKey(source)) throw new ArgumentException($"Component {source} not found.");
		if (!_components.ContainsKey(target)) throw new ArgumentException($"Component {target} not found.");
		var sourceComponent = _components[source];
		var targetComponent = _components[target];

		var input = targetComponent.GetInput(targetInput);
		if (input == null) throw new ArgumentException($"Input [{target}.{targetInput}] not found.");

		var output = sourceComponent.GetOutput(sourceOutput);
		if (output == null) throw new ArgumentException($"Output [{source}.{sourceOutput}] not found.");

		input.ConnectTo(output);
	}

	public Component GetComponent(string name)
	{
		_components.TryGetValue(name, out var component);
		return component;
	}

	public int SampleRate => _sampleRate;
	public int BufferSize => _bufferSize;
}
