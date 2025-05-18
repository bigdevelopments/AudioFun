using AudioComponents.Core;

namespace AudioComponents.Audio;

public class Combiner : Component
{
	// inputs and outputs
	private readonly IInputReader _input1;
	private readonly IInputReader _input2;
	private readonly IOutputWriter _output;

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
