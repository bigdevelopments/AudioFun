namespace AudioComponents;

public class Splitter : ComponentBase
{
	// inputs and outputs
	private readonly Input _input;
	private readonly Output _output1;
	private readonly Output _output2;

	public Splitter()
	{
		_input = AddInput("in");
		_output1 = AddOutput("out-1");
		_output2 = AddOutput("out-2");
	}

	public override void Tick()
	{
		_output1.Value = new System.Numerics.Vector2(_input.Value.X, _input.Value.X);
		_output2.Value = new System.Numerics.Vector2(_input.Value.Y, _input.Value.Y);
	}
}
