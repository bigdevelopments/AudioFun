namespace AudioComponents;

public class Base
{
	private readonly Dictionary<string, ComponentBase> _components;
	private int _sampleRate;
	private int _bufferSize;	

	public Base()
	{
		_components = new Dictionary<string, ComponentBase>(StringComparer.OrdinalIgnoreCase);	
	}

	public T Add<T>(string name, T component) where T: ComponentBase
	{
		if (_components.ContainsKey(name))
			throw new ArgumentException($"Component with name {name} already exists.");

		_components.Add(name, component);
		return component;
	}

	public void Initialise(int sampleRate, int bufferSize)
	{
		_sampleRate = sampleRate;
		_bufferSize = bufferSize;

		foreach (var component in _components.Values)
		{
			component.Initialise(this, sampleRate, bufferSize);
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

		input.FeedFrom(output);
	}

	public int SampleRate => _sampleRate;
	public int BufferSize => _bufferSize;
}
