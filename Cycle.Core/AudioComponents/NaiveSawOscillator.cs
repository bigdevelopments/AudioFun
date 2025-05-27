namespace Cycle.Core.AudioComponents;

[Primitive("naive_saw", "Simple non band-limiting saw wave oscillator, adjust duty from saw to triangle to inverse saw")]
public class NaiveSawOscillator : Component
{
	// inputs and outputs
	private readonly SignalInput _frequency;
	private readonly SignalInput _amplitude;
	private readonly SignalInput _duty;
	private readonly SignalOutput _output;

	// state
	private float _phase;

	public NaiveSawOscillator()
	{
		_frequency = AddSignalInput("frq");
		_amplitude = AddSignalInput("amp");
		_duty = AddSignalInput("duty", new Vector2(0.5f, 0.5f));
		_output = AddSignalOutput("out");
	}

	// tick
	public override void Tick()
	{
		float phaseIncrement = _frequency.Value.X / SampleRate;
		_phase += phaseIncrement;
		
		while (_phase > 1f) _phase -= 1f;

		if (_phase < _duty.Value.X)
		{
			// rising saw
			_output.Value = new Vector2(_phase / _duty.Value.X * 2f - 1f, _phase / _duty.Value.X * 2f - 1f);
		}
		else
		{
			// falling saw
			_output.Value = new Vector2((1f - (_phase - _duty.Value.X) / (1f - _duty.Value.X)) * 2f, (1f - (_phase - _duty.Value.X) / (1f - _duty.Value.X)) * 2f);
		}
	}
}
