namespace Cycle.Core.AudioComponents;

[Primitive("junction", "Straight through component, provides a place for multiple input taps")]
public class Junction : Component
{
	// inputs and outputs
	private readonly SignalInput _input;
	private readonly SignalOutput _output;

	public Junction()
	{
		_input = AddSignalInput("in");
		_output = AddSignalOutput("out");
	}

	public override void Tick()
	{
		_output.Value = _input.Value;
	}
}
