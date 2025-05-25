namespace Cycle.Core.AudioComponents;

[Primitive("adsr", "Simple ADSR envelope")]
public class Adsr : Component
{
	// the different ADSR states
	public enum AdsrState { Idle, Attack, Decay, Sustain, Release };

	// inputs and outputs
	private readonly SignalInput _attack; // time in seconds
	private readonly SignalInput _decay; // time in seconds
	private readonly SignalInput _sustain; // level 0-1 as a fraction of max level
	private readonly SignalInput _release; // time in seconds
	private readonly SignalOutput _output;

	// state
	private AdsrState _state;   // Idle, A, D, S or R
	private float _topLevel;    // 0-1 as the maximum amplitude
	private float _level;       // 0-1 as the current amplitude

	public Adsr(params string[] parameters) 
	{
		// our four inputs
		_attack = AddSignalInput("att");
		_decay = AddSignalInput("dec");
		_sustain = AddSignalInput("sus");
		_release = AddSignalInput("rel");

		// just one output, and respond to midi note on/off messages
		_output = AddSignalOutput("out");

		// initial state
		_state = AdsrState.Idle;
		_topLevel = 0;
		_level = 0;
	}

	public override Message OnNotify(Message message)
	{
		if (MidiMessage.IsNoteOn(message, out var _, out var _, out var velocity))
		{
			_state = AdsrState.Attack;
			_topLevel = velocity / 127f;
			return Message.Ok;
		}

		if (MidiMessage.IsNoteOff(message, out var _, out var _, out var _))
		{
			_state = AdsrState.Release;
			return Message.Ok;
		}

		return Message.Unsupported;
	}

	public override void Tick()
	{
		switch (_state)
		{
			case AdsrState.Attack:

				_level += _attack.Value.X * OneOverSampleRate;
				if (_level >= _topLevel)
				{
					_level = _topLevel;
					_state = AdsrState.Decay;
				}
				break;

			case AdsrState.Decay:

				// decay phase
				_level -= (_topLevel - _sustain.Value.X) * OneOverSampleRate;
				if (_level <= _sustain.Value.X)
				{
					_level = _sustain.Value.X;
					_state = AdsrState.Sustain;
				}
				break;

			case AdsrState.Sustain:

				_level = _sustain.Value.X;
				break;

			case AdsrState.Release:

				_level -= (_sustain.Value.X - _release.Value.X) * OneOverSampleRate;
				if (_level <= 0)
				{
					_level = 0;
					_state = AdsrState.Idle;
				}
				break;

			default: // idle state, do nothing
				break;
		}

		// and set the output value
		_output.Value = new Vector2(_level, _level);
	}
}
