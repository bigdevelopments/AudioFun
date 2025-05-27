namespace Cycle.Core.AudioComponents;

[Primitive("multiplier", "Multiplies two inputs together")]
public class Multiplier : Component
{
	// inputs and outputs
	private readonly SignalInput[] _inputs;
	private readonly SignalOutput _output;

	public Multiplier(params string[] parameters)
	{
		if (parameters?.Length != 1 || !int.TryParse(parameters[0], out var count))
		{
			throw new ArgumentException("Multiplier requires exactly one parameter: the number of inputs", nameof(parameters));
		}

		if (count < 2 || count > 64)
		{
			throw new ArgumentOutOfRangeException(nameof(count), "Multiplier supports between 2 and 64 inputs");
		}

		_inputs = new SignalInput[count];
		for (int i = 0; i < count; i++) _inputs[i] = AddSignalInput($"in_{i + 1}");
		_output = AddSignalOutput("out");
	}

	public override void Tick()
	{
		var result = _inputs[0].Value;
		for (int i = 1; i < _inputs.Length; i++)
		{
			result *= _inputs[i].Value;
		}
		_output.Value = result;
	}
}
