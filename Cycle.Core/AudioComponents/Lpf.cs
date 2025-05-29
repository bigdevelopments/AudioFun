namespace Cycle.Core.AudioComponents;

// based off https://www.musicdsp.org/en/latest/Filters/29-resonant-filter.html

[Primitive("lpf", "Low pass resonant filter")]
public class Lpf : Component
{
	private readonly SignalInput _input;
	private readonly SignalInput _cutOff;
	private readonly SignalInput _resonance;
	private readonly SignalOutput _output;

	private Vector2 _h0;
	private Vector2 _h1;
	private Vector2 _h2;
	private Vector2 _h3;

	public Lpf()
	{
		_input = AddSignalInput("in");
		_cutOff = AddSignalInput("frq");
		_resonance = AddSignalInput("res");
		_output = AddSignalOutput("out");
	}

	public override void Tick()
	{
		// just encase, leave this here for debug
		//Vector2 freq = new Vector2(2f * MathF.Sin(MathF.PI * _cutOff.Value.X * OneOverSampleRate), 2f * MathF.Sin(MathF.PI * _cutOff.Value.Y * OneOverSampleRate));
		
		Vector2 freq = new Vector2(2f * Maths.Sin(_cutOff.Value.X * OneOverSampleRate *0.5f), 2f * Maths.Sin(_cutOff.Value.Y * OneOverSampleRate * 0.5f));
		Vector2 feedback = _resonance.Value + _resonance.Value / (Vector2.One - freq);

		_h0 += freq * (_input.Value - _h0 + feedback * (_h0 - _h1));
		_h1 += freq * (_h0 - _h1);
		_h2 += freq * (_h1 - _h2);
		_h3 += freq * (_h2 - _h3);

		if (float.IsNaN(_h0.X) || float.IsNaN(_h0.Y) || float.IsNaN(_h1.X) || float.IsNaN(_h1.Y) || float.IsNaN(_h2.X) || float.IsNaN(_h2.Y) || float.IsNaN(_h3.X) || float.IsNaN(_h3.Y))
		{
			Console.WriteLine("NaN!!!!");
			// reset to zero if we get NaN
			_h0 = Vector2.Zero;
			_h1 = Vector2.Zero;
			_h2 = Vector2.Zero;
			_h3 = Vector2.Zero;
		}
		_output.Value = _h3;
	}
}
