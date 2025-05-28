namespace Cycle.Core.AudioComponents;

/// <summary>
/// Generates non-band-limiting pulse waveforms, set duty to 0.5 for a square wave.
/// </summary>
[Primitive("naive_pulse", "Simple non band-limiting pulse oscillator")]
public class NaivePulseOscillator : Component
{
	// inputs and outputs
	private readonly SignalInput _frequency;
	private readonly SignalInput _amplitude;
	private readonly SignalInput _duty;
	private readonly SignalOutput _output;

	// state
	private float _phase;

	public NaivePulseOscillator()
	{
		_frequency = AddSignalInput("frq");
		_duty = AddSignalInput("duty", new Vector2(0.5f, 0.5f));
		_output = AddSignalOutput("out");
	}

	// tick
	public override void Tick()
	{
		float phaseIncrement = _frequency.Value.X / SampleRate;
		_phase += phaseIncrement;

		while (_phase > 1f) _phase -= 1f;

		_output.Value = _phase > _duty.Value.X ? Vector2.One : -Vector2.One;
	}
}
