namespace Cycle.Core.Audio;

public class Splitter : Component
{
	// inputs and outputs
	private readonly SignalInput _input;
	private readonly SignalOutput _output1;
	private readonly SignalOutput _output2;

	public Splitter() 
	{
		_input = AddSignalInput("in");
		_output1 = AddSignalOutput("out-1");
		_output2 = AddSignalOutput("out-2");
	}

	public override void Tick()
	{
		_output1.Value = new Vector2(_input.Value.X, _input.Value.X);
		_output2.Value = new Vector2(_input.Value.Y, _input.Value.Y);
	}
}
