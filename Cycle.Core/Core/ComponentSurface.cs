namespace Cycle.Core.Core;

/// <summary>
/// The surface holds all the components, including hybrids and handles the ticking
/// </summary>
public class ComponentSurface
{
	private readonly Dictionary<string, Component> _components;
	private int _sampleRate;

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

	public void Initialise(int sampleRate)
	{
		_sampleRate = sampleRate;

		foreach (var component in _components.Values)
		{
			component.Initialise(sampleRate);
		}
	}

	public void Tick()
	{
		foreach (var component in _components.Values) component.Tick();
	}

	public void Connect(string target, string targetInput, string source, string sourceOutput)
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

	public Component GetComponent(string name)
	{
		_components.TryGetValue(name, out var component);
		return component;
	}

	public int SampleRate => _sampleRate;
}
