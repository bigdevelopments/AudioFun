namespace Cycle.Core.Audio;

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
		float phaseIncrement = 2f * MathF.PI * _frequency.Value.X / SampleRate;
		_phase += phaseIncrement;
		if (_phase >= 2f * MathF.PI) _phase -= 2f * MathF.PI;
		_output.Value = new Vector2(_amplitude.Value.X * MathF.Cos(_phase), _amplitude.Value.Y * MathF.Cos(_phase));
		//_output.Value = new Vector2(_amplitude.Value.X * _phase, _amplitude.Value.X * _phase);
	}
}
