namespace Cycle.Core.AudioComponents;

[Primitive("multiplier", "Multiplies two inputs together")]
public class Multiplier : Component
{
	// inputs and outputs
	private readonly SignalInput _input1;
	private readonly SignalInput _input2;
	private readonly SignalOutput _output;

	public Multiplier() 
	{
		_input1 = AddSignalInput("in_1");
		_input2 = AddSignalInput("in_2");
		_output = AddSignalOutput("out");
	}

	public override void Tick()
	{
		_output.Value = _input1.Value * _input2.Value;
	}
}
