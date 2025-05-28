namespace Cycle.Core.Midi;

/// <summary>
/// A simple MIDI bridge that outputs controller values as a signal output
/// </summary>
[Primitive("ctrl_bridge", "MIDI Controller bridge")]
public class ControllerBridge : Component
{
	// midi input and signal output
	private readonly MidiInput _midiInput;
	private readonly SignalOutput _output;

	// channel mask (0-15) for channels to respond to
	private ushort _channelMask;

	// controller number to respond to
	private int _controllerNumber;

	public ControllerBridge(params string[] parameters)
	{
		if (parameters?.Length < 1 || !int.TryParse(parameters[0], out var controllerNumber))
		{
			throw new ArgumentException("Controller bridge requires at least one parameter: the controller number", nameof(parameters));
		}

		if (controllerNumber < 0 || controllerNumber > 15)
		{
			throw new ArgumentOutOfRangeException(nameof(controllerNumber), "Controller bridge accepts controllers 0 to 15");
		}

		var channelMask = 0xffff; // default to all channels	

		if (channelMask < 0 || channelMask > 0xffff) throw new Exception("Channel mask must be between 0 and 65535.");
		
		_channelMask = (ushort)channelMask;
		_controllerNumber = controllerNumber;

		_midiInput = AddMidiInput("midi_in", MidiProc);
		_output = AddSignalOutput("out");
	}

	private void MidiProc(Message message)
	{
		if (MidiMessage.IsControllerChange(message, _channelMask, out var controllerNumber, out var value) && controllerNumber == _controllerNumber)
		{
			_output.Value = new Vector2(value, value);
		}
	}
}
