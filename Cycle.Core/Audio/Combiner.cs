namespace Cycle.Core.Audio;

[Primitive("combiner", "Combines the X signal of input 1 with the Y signal of input 2")]
public class Combiner : Component
{
	// inputs and outputs
	private readonly SignalInput _input1;
	private readonly SignalInput _input2;
	private readonly SignalOutput _output;

	public Combiner() 
	{
		_input1 = AddSignalInput("in-1");
		_input2 = AddSignalInput("in-2");
		_output = AddSignalOutput("out");
	}

	public override void Tick()
	{
		_output.Value = new Vector2(_input1.Value.X, _input2.Value.Y);
	}
}
