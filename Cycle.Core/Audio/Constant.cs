namespace Cycle.Core.Audio;

[Primitive("constant", "Represents an immutable value")]
public class Constant : Component
{
	private readonly SignalOutput _output;

	public Constant(params string[] parameters) 
	{
		_output = AddSignalOutput("out");
		if (parameters?.Length < 1 || parameters?.Length> 2)
		{
			throw new ArgumentException("A constant requires one or two parameters requires one parameter: the number of inputs", nameof(parameters));
		}

		if (!float.TryParse(parameters[0], out float valueX)) throw new ArgumentException("The first parameter must be a 32 bit float", nameof(parameters));

		if (parameters?.Length == 1)
		{
			_output.Value = new Vector2(valueX, valueX);
		}
		else
		{
			if (!float.TryParse(parameters[1], out float valueY)) throw new ArgumentException("The second parameter must be a 32 bit float", nameof(parameters));
			_output.Value = new Vector2(valueX, valueY);
		}
	}

	public void Set(float value)
	{
		_output.Value = new Vector2(value, value);
	}

	public void Set(Vector2 value)
	{
		_output.Value = value;
	}

	public override void Tick()
	{
		// nothing to do here - the value never changes	
	}
}
