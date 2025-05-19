namespace AudioComponents.Audio;

public class Inverter : Component
{
	private readonly IInputReader _input;
	private readonly IOutputWriter _output;

	public Inverter()
	{
		_input = AddInput("input");
		_output = AddOutput("output");
	}

	public override void Tick()
	{
		_output.Value = -_input.Value;
	}
}
