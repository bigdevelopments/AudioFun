namespace Cycle.Core.Core;

public abstract class Component
{
	// single inputs and outputs
	private readonly Dictionary<string, Connection> _connections = new Dictionary<string, Connection>(StringComparer.OrdinalIgnoreCase);

	private string _name;

	// an isolated component has outputs decoupled from inputs, so it can be used in a circuit breaker
	private bool _isolated;

	private int _sampleRate;
	private float _oneOverSampleRate;
	private int _bufferSize;

	protected Component(bool isolated = false)
	{
		_isolated = isolated;
	}

	// Isolated components are those marked as such, or those that have no inputs
	public bool Isolated => _isolated || _connections.Count == 0;

	public void Initialise(int sampleRate)
	{ 
		_sampleRate = sampleRate;
		_oneOverSampleRate = 1f / sampleRate;
		OnInitialise(sampleRate);
	}

	public virtual void OnInitialise(int sampleRate)
	{
		// left to derived classes
	}


	public void OnAdd(ComponentSurface surface, string name)
	{
		if (_name != null) throw new InvalidOperationException("Already added");
		_name = name;
	}

	public virtual Message OnNotify(Message message)
	{
		// if not answered by derived class, nothing to do
		return Message.Ok;
	}

	internal Connection GetConnection(string name)
	{
		_connections.TryGetValue(name, out var connection);
		return connection;
	}

	//public IInput GetInput(string name)
	//{
	//	if (_connections.TryGetValue(name, out var input)) return input as IInput;
	//	return null;
	//}

	//public IOutput GetOutput(string name)
	//{
	//	if (_connections.TryGetValue(name, out var output)) return output as IOutput;
	//	return null;
	//}

	protected Connection AddConnection(string name, Connection connection)
	{
		if (_connections.ContainsKey(name))
		{
			throw new ArgumentException($"Connection with name {name} already exists.");
		}
		_connections.Add(name, connection);
		return connection;
	}

	protected SignalInput AddSignalInput(string name)
	{
		if (_connections.ContainsKey(name))
		{
			throw new ArgumentException($"Connection with name {name} already exists.");
		}
		var input = new SignalInput();
		_connections.Add(name, input);
		return input;
	}

	protected MidiInput AddMidiInput(string name, Action<Message> handler)
	{
		if (_connections.ContainsKey(name))
		{
			throw new ArgumentException($"Connection with name {name} already exists.");
		}
		var input = new MidiInput(handler);
		_connections.Add(name, input);
		return input;
	}

	protected MidiInput AddMidiInput(string name, MidiInput midiInput)
	{
		if (_connections.ContainsKey(name))
		{
			throw new ArgumentException($"Connection with name {name} already exists.");
		}
		_connections.Add(name, midiInput);
		return midiInput;
	}

	protected SignalOutput AddSignalOutput(string name)
	{
		if (_connections.ContainsKey(name))
		{
			throw new ArgumentException($"Connection with name {name} already exists.");
		}

		var output = new SignalOutput();
		_connections.Add(name, output);
		return output;
	}

	protected SignalOutput AddOutput(string name, SignalOutput output)
	{
		if (_connections.ContainsKey(name))
		{
			throw new ArgumentException($"Connection with name {name} already exists.");
		}
		_connections.Add(name, output);
		return output;
	}

	protected MidiOutput AddMidiOutput(string name)
	{
		if (_connections.ContainsKey(name))
		{
			throw new ArgumentException($"Connection with name {name} already exists.");
		}
		var output = new MidiOutput(name);
		_connections.Add(name, output);
		return output;
	}

	protected MidiOutput AddMidiOutput(string name, MidiOutput output)
	{
		if (_connections.ContainsKey(name))
		{
			throw new ArgumentException($"Connection with name {name} already exists.");
		}
		_connections.Add(name, output);
		return output;
	}


	public abstract void Tick();

	public string Name => _name;
	public int SampleRate => _sampleRate;
	public float OneOverSampleRate => _oneOverSampleRate;
	public int BufferSize => _bufferSize;
}
