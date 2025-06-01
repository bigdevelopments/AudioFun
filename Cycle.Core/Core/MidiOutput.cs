namespace Cycle.Core.Core;


public class MidiOutput : Connection
{
	private readonly string _name;

	internal MidiOutput(string name)
	{
		_name = name;
	}

	internal Action<Message> Targets { get; set; }

	public string Name => _name;

	public void Send(Message message)
	{
		// send the message to all targets
		Targets?.Invoke(message);
	}
}
