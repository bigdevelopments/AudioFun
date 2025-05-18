namespace AudioComponents;

public abstract class ComponentBase
{
	// single inputs and outputs
	private readonly Dictionary<string, Input> _inputs = new Dictionary<string, Input>(StringComparer.OrdinalIgnoreCase);
	private readonly Dictionary<string, Output> _outputs = new Dictionary<string, Output>(StringComparer.OrdinalIgnoreCase);

	private ComponentSurface _surface;
	private string _name;

	private int _sampleRate;
	private int _bufferSize;

	public virtual void Initialise(int sampleRate, int bufferSize)
	{ 
		_sampleRate = sampleRate;
		_bufferSize = bufferSize;
	}

	public void OnAdd(ComponentSurface surface, string name)
	{
		if (_name != null) throw new InvalidOperationException("Already added");
		_surface = surface;
		_name = name;
		OnAdded(surface, name);
	}

	protected virtual void OnAdded(ComponentSurface bse, string name)
	{
		// no implementation, leave to derived classes
	}

	public virtual void Notify(Message message)
	{
		// no implementation, leave to derived classes
	}

	public IInput GetInput(string name)
	{
		if (_inputs.TryGetValue(name, out var input)) return input;
		return null;
	}

	public IOutput GetOutput(string name)
	{
		if (_outputs.TryGetValue(name, out var output))
		{
			return output;
		}
		return null;
	}

	protected Input AddInput(string name, Input input)
	{
		if (_inputs.ContainsKey(name))
		{
			throw new ArgumentException($"Input with name {name} already exists.");
		}
		_inputs.Add(name, input);
		return input;
	}

	protected Input AddInput(string name)
	{
		if (_inputs.ContainsKey(name))
		{
			throw new ArgumentException($"Input with name {name} already exists.");
		}
		var input = new Input(name);
		_inputs.Add(name, input);
		return input;
	}


	protected Output AddOutput(string name)
	{
		if (_outputs.ContainsKey(name))
		{
			throw new ArgumentException($"Output with name {name} already exists.");
		}

		var output = new Output(name);
		_outputs.Add(name, output);
		return output;
	}

	protected Output AddOutput(string name, Output output)
	{
		if (_outputs.ContainsKey(name))
		{
			throw new ArgumentException($"Output with name {name} already exists.");
		}
		_outputs.Add(name, output);
		return output;
	}

	public abstract void Tick();

	public ComponentSurface Surface => _surface;
	public string Name => _name;
	public int SampleRate => _sampleRate;
	public int BufferSize => _bufferSize;
}
