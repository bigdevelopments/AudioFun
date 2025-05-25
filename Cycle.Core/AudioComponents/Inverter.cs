namespace Cycle.Core.AudioComponents;

[Primitive("inverter", "Combines the X signal of input 1 with the Y signal of input 2")]
public class Inverter : Component
{
	private readonly SignalInput _input;
	private readonly SignalOutput _output;

	public Inverter() 
	{
		_input = AddSignalInput("in");
		_output = AddSignalOutput("out");
	}

	public override void Tick()
	{
		_output.Value = -_input.Value;
	}
}
