using FunWithAudio.Common.Audio;
using FunWithAudio.Common.Core;

namespace FunWithAudio.Wasapi.NAudio;

/// <summary>
/// Just collects the input values and makes them available to the sound card.
/// </summary>
internal class AudioOutput : Component
{
	// only inputs, the output are your speakers
	private readonly Input _input;
	
	public AudioOutput()
	{
		_input = AddInput("in");
	}

	public override void Tick()
	{
		// nothing here!
	}

	// expose the input as properties so the 
	internal float Left => _input.Value.X;
	internal float Right => _input.Value.Y;
}
