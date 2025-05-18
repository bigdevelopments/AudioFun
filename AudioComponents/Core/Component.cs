using AudioComponents.Audio;

namespace AudioComponents.Core;

public abstract class Component
{
	// single inputs and outputs
	private readonly Dictionary<string, Input> _inputs = new Dictionary<string, Input>(StringComparer.OrdinalIgnoreCase);
	private readonly Dictionary<string, Output> _outputs = new Dictionary<string, Output>(StringComparer.OrdinalIgnoreCase);

	private ComponentSurface _surface;
	private string _name;

	private int _sampleRate;
	private int _bufferSize;

	public void Initialise(int sampleRate, int bufferSize)
	{ 
		_sampleRate = sampleRate;
		_bufferSize = bufferSize;
		OnInitialise(sampleRate, bufferSize);
	}

	public virtual void OnInitialise(int sampleRate, int bufferSize)
	{
		// left to derived classes
	}


	public void OnAdd(ComponentSurface surface, string name)
	{
		if (_name != null) throw new InvalidOperationException("Already added");
		_surface = surface;
		_name = name;
	}

	public virtual Message Notify(Message message)
	{
		// if not answered by derived class, nothing to do
		return Message.Ok;
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


	protected IOutputWriter AddOutput(string name)
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
