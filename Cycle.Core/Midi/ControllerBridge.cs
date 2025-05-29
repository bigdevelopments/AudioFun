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
	private readonly ushort _channelMask;

	// controller number to respond to
	private readonly int _controllerNumber;

	// slew - how fast to move the signal output to reflect the controller value
	private readonly float _slew; // default slew value

	private float _outputValue;
	private float _desiredValue;

	public ControllerBridge(params string[] parameters)
	{
		if (parameters?.Length < 2 || !int.TryParse(parameters[0], out var controllerNumber) || !float.TryParse(parameters[1], out var slew))
		{
			throw new ArgumentException("Controller bridge requires at least two parameters: the controller number and slew", nameof(parameters));
		}

		if (controllerNumber < 0 || controllerNumber > 15)
		{
			throw new ArgumentOutOfRangeException(nameof(controllerNumber), "Controller bridge accepts controllers 0 to 15");
		}

		if (slew <= 0f || slew >1f)
		{
			throw new ArgumentOutOfRangeException(nameof(slew), "Slew must be greater than zero and less than or equal to one.");
		}
		
		_slew = slew;
		_outputValue = 0f;
		var channelMask = 0xffff; // default to all channels	

		if (channelMask < 0 || channelMask > 0xffff) throw new Exception("Channel mask must be between 0 and 65535.");
		
		_channelMask = (ushort)channelMask;
		_controllerNumber = controllerNumber;

		_midiInput = AddMidiInput("midi_in", MidiProc);
		_output = AddSignalOutput("out");
	}

	private void MidiProc(Message message)
	{
		if (MidiMessage.IsControllerChange(message, _channelMask, out var controllerNumber, out var value) && controllerNumber == _controllerNumber) _desiredValue = value;
	}

	public override void Tick()
	{
		// apply slew to the output value
		_outputValue += (_desiredValue - _outputValue) * _slew;
		_output.Value = new Vector2(_outputValue, _outputValue);
	}
}
