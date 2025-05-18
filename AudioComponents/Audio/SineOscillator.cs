using AudioComponents.Core;

using System.Numerics;

namespace AudioComponents.Audio;

public class SineOscillator : Component
{
	// inputs and outputs
	private readonly IInputReader _frequency;
	private readonly IInputReader _amplitude;
	private readonly IOutputWriter _output;

	// state
	private float _phase;

	public SineOscillator()
	{
		_frequency = AddInput("frequency");
		_amplitude = AddInput("amplitude");
		_output = AddOutput("out");
	}

	// tick
	public override void Tick()
	{
		float phaseIncrement = 2f * MathF.PI * _frequency.Value.X / SampleRate;
		_phase += phaseIncrement;
		if (_phase >= 2f * MathF.PI) _phase -= 2f * MathF.PI;
		_output.Value = new Vector2(_amplitude.Value.X * MathF.Sin(_phase), _amplitude.Value.Y * MathF.Sin(_phase));
	}
}
