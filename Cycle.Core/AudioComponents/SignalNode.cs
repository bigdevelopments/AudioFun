namespace Cycle.Core.AudioComponents;

[Primitive("signal_node", "Straight through component, provides a place for multiple input taps - usually when exposed")]
public class SignalNode : Component
{
	// inputs and outputs
	private readonly SignalInput _input;
	private readonly SignalOutput _output;

	public SignalNode()
	{
		_input = AddSignalInput("in");
		_output = AddSignalOutput("out");
	}

	public override void Tick()
	{
		_output.Value = _input.Value;
	}
}
