namespace AudioComponents;

public class Combiner : ComponentBase
{
	// inputs and outputs
	private readonly Input _input1;
	private readonly Input _input2;
	private readonly Output _output;

	public Combiner()
	{
		_input1 = AddInput("in-1");
		_input2 = AddInput("in-2");
		_output = AddOutput("out");
	}

	public override void Tick()
	{
		_output.Value = new System.Numerics.Vector2(_input1.Value.X, _input2.Value.Y);
	}
}
