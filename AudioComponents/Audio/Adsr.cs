namespace AudioComponents.Audio;

public class Adsr : Component
{
	// the different ADSR states
	public enum AdsrState { Idle, Attack, Decay, Sustain, Release };

	// inputs and outputs
	private readonly IInputReader _attack; // time in seconds
	private readonly IInputReader _decay; // time in seconds
	private readonly IInputReader _sustain; // level 0-1 as a fraction of max level
	private readonly IInputReader _release; // time in seconds
	private readonly IOutputWriter _output;

	// state
	private AdsrState _state;   // A, S, S or R
	private float _topLevel;    // 0-1 as the maximum amplitude
	private float _level;       // 0-1 as the current amplitude
	private float _time;        // time in seconds in the current state
	private float _timeScale;   // time in seconds per sample

	public Adsr(int polyphony)
	{
		// our four inputs
		_attack = AddInput("attack");
		_decay = AddInput("decay");
		_sustain = AddInput("sustain");
		_release = AddInput("release");

		// just one output, and respond to midi note on/off messages
		_output = AddOutput("out");

		// initial state
		_state = AdsrState.Idle;
		_topLevel = 0;
		_time = 0;
		_level = 0;
	}

	public override void OnInitialise(int sampleRate, int bufferSize)
	{
		// set the time scale according to sample rate (tick rate)
		_timeScale = 1f / sampleRate;
	}

	public override Message Notify(Message message)
	{
		if (MidiMessage.IsNoteOn(message, out var _, out var _, out var velocity))
		{
			_state = AdsrState.Attack;
			_topLevel = velocity / 127f;
			_time = 0;
			return Message.Ok;
		}

		if (MidiMessage.IsNoteOff(message, out var _, out var _, out var _))
		{
			_state = AdsrState.Release;
			_time = 0;
			return Message.Ok;
		}

		return Message.Unsupported;
	}

	public override void Tick()
	{
		// update the time
		_time += _timeScale;

		switch (_state)
		{
			case AdsrState.Attack:
				
				_level += _timeScale / _attack.Value.X;
				if (_level >= _topLevel)
				{
					_level = _topLevel;
					_state = AdsrState.Decay;
					_time = 0;
				}
				break;

			case AdsrState.Decay:
				
				// decay phase
				_level -= _timeScale / _decay.Value.X * (_topLevel - _sustain.Value.X);
				if (_level <= _sustain.Value.X)
				{
					_level = _sustain.Value.X;
					_state = AdsrState.Sustain;
					_time = 0;
				}
				break;
			
			case AdsrState.Sustain:

				_level = _sustain.Value.X;
				break;

			case AdsrState.Release:
				
				_level -= _timeScale / _release.Value.X * _sustain.Value.X;
				if (_level <= 0)
				{
					_level = 0;
					_state = AdsrState.Idle;
					_time = 0;
				}
				break;

			default: // idle state, do nothing
				break;
		}

		// and set the output value
		_output.Value = new Vector2(_level, _level);

	}
}
