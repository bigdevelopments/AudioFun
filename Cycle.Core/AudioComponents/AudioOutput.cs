namespace Cycle.Core.AudioComponents;

[Primitive("audio_out", "Simple stereo exit point for delivering stereo samples")]
public class AudioOutput : Component
{
	// only inputs, the output are your speakers
	private readonly SignalInput _input;

	public AudioOutput()
	{
		_input = AddSignalInput("in");
	}

	public override void Tick()
	{
		// nothing here!
	}

	// expose the input as properties so the 
	public float Left => _input.Value.X;
	public float Right => _input.Value.Y;
}
