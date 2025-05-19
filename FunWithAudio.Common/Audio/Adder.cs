namespace FunWithAudio.Common.Audio;

public class Adder : Component
{
	// inputs and outputs
	private readonly IInputReader _input1;
	private readonly IInputReader _input2;
	private readonly IOutputWriter _output;

	public Adder()
	{
		_input1 = AddInput("input-1");
		_input2 = AddInput("input-2");
		_output = AddOutput("output");
	}

	public override void Tick()
	{
		_output.Value = _input1.Value + _input2.Value;
	}
}
