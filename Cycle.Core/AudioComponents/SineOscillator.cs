namespace Cycle.Core.AudioComponents;

[Primitive("sine_osc", "Simple sine wave oscillator")]
public class SineOscillator : Component
{
	// inputs and outputs
	private readonly SignalInput _frequency;
	private readonly SignalInput _amplitude;
	private readonly SignalOutput _output;

	// state
	private float _phase;

	public SineOscillator() 
	{
		_frequency = AddSignalInput("frq");
		_amplitude = AddSignalInput("amp");
		_output = AddSignalOutput("out");
	}

	// tick
	public override void Tick()
	{
		float phaseIncrement = _frequency.Value.X / SampleRate;
		_phase += phaseIncrement;
		if (_phase >= 1f) _phase -= 1f;

		// default, but slow
		//_output.Value = new Vector2(_amplitude.Value.X * MathF.Sin(_phase * 2 * MathF.PI), _amplitude.Value.Y * MathF.Sin(_phase * 2 * MathF.PI));

		// this sin approximation is about 3x faster on i9-13900K, but likely depends on CPU cache state - we'll see
		_output.Value = new Vector2(_amplitude.Value.X * Maths.Sin(_phase), _amplitude.Value.Y * Maths.Sin(_phase));
	}
}
