namespace AudioComponents;

public class Inverter : ComponentBase
{
	private readonly Input _input;
	private readonly Output _output;

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
