using System.Numerics;

namespace AudioComponents;

public class Constant : ComponentBase
{
	private readonly Output _output;

	public Constant(Vector2 value = default)
	{
		_output = AddOutput("out");
		_output.Value = value;
	}

	public Constant(float value = default): this (new Vector2(value, value))
	{
		// nothing more
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
		// nothing to do here
	}
}
