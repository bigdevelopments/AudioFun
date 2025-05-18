namespace AudioComponents.Audio;

public class Splitter : Component
{
	// inputs and outputs
	private readonly IInputReader _input;
	private readonly IOutputWriter _output1;
	private readonly IOutputWriter _output2;

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
