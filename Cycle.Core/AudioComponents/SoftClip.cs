namespace Cycle.Core.AudioComponents;

[Primitive("soft_clip", "Represents an immutable value")]
public class SoftClip : Component
{
	private readonly SignalInput _threshold;
	private readonly SignalInput _input;
	private readonly SignalOutput _output;

	public SoftClip(params string[] parameters)
	{
		_threshold = AddSignalInput("threshold", new Vector2(0.8f));
		_input = AddSignalInput("in");
		_output = AddSignalOutput("out");
	}

	public override void Tick()
	{
		var input = _input.Value;
		var threshold = _threshold.Value;

		if (input.X < -threshold.X || input.X > threshold.X)
		{
			input.X = (Math.Sign(input.X) * (threshold.X + (1 - threshold.X) * Maths.Tanh((Math.Abs(input.X) - threshold.X) / (1 - threshold.X))));
		}
		if (input.Y < -threshold.Y || input.Y > threshold.Y)
		{
			input.Y = (Math.Sign(input.Y) * (threshold.Y + (1 - threshold.Y) * Maths.Tanh((Math.Abs(input.Y) - threshold.Y) / (1 - threshold.Y))));
		}

		// force clamp in case we spill over
		input = Vector2.Clamp(input, -Vector2.One, Vector2.One);

		_output.Value = input;
	}
}

