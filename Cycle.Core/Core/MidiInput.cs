namespace Cycle.Core.Core;

public class MidiInput : Connection
{
	private readonly Action<Message> _handler;
	
	public MidiOutput MidiOutput { get; set; }

	public MidiInput(Action<Message> handler)
	{
		_handler = handler ?? throw new ArgumentNullException(nameof(handler), "Handler cannot be null.");
	}

	public void ConnectTo(MidiOutput output)
	{
		MidiOutput = output;
		output.Targets += _handler;
	}

	public void Disconnect()
	{
		if (MidiOutput == null) return;
		MidiOutput.Targets -= _handler;
		MidiOutput = null;
	}
}
