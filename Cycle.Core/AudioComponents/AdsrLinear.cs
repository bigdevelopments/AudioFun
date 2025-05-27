namespace Cycle.Core.AudioComponents;

[Primitive("adsr_linear", "Simple ADSR envelope")]
public class AdsrLinear : Component
{
	// the different ADSR states
	public enum AdsrState { Idle, Attack, Decay, Sustain, Release };

	// inputs and outputs
	private readonly SignalInput _trigger; // triggered by non-zero value
	private readonly SignalInput _attack; // time in seconds
	private readonly SignalInput _decay; // time in seconds
	private readonly SignalInput _sustain; // level 0-1 as a fraction of max level
	private readonly SignalInput _release; // time in seconds
	private readonly SignalOutput _output;

	// state
	private AdsrState _state;   // Idle, A, D, S or R
	private bool _triggered; // true if triggered by a non-zero value
	private float _level;       // 0-1 as the current amplitude

	public AdsrLinear(params string[] parameters)
	{
		// our five inputs
		_trigger = AddSignalInput("trg");
		_attack = AddSignalInput("a");
		_decay = AddSignalInput("d");
		_sustain = AddSignalInput("s");
		_release = AddSignalInput("r");

		// just one output, and respond to midi note on/off messages
		_output = AddSignalOutput("out");

		// initial state
		_state = AdsrState.Idle;
		_level = 0;
	}

	public override void Tick()
	{
		if (!_triggered && _trigger.Value.X != 0)
		{
			// trigger the ADSR envelope
			_state = AdsrState.Attack;
			_level = 0; // reset the level - ?
			_triggered = true;
		}
		else if (_triggered && _trigger.Value.X == 0)
		{
			_triggered = false;
		}

		switch (_state)
		{
			case AdsrState.Attack:

				_level += _attack.Value.X * OneOverSampleRate;
				if (_level >= 1f)
				{
					_level = 1f;
					_state = AdsrState.Decay;
				}
				break;

			case AdsrState.Decay:

				// decay phase
				_level -= (1f - _sustain.Value.X) * OneOverSampleRate;
				if (_level <= _sustain.Value.X)
				{
					_level = _sustain.Value.X;
					_state = AdsrState.Sustain;
				}
				break;

			case AdsrState.Sustain:

				if (!_triggered) _state = AdsrState.Release;
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
