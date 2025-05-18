using AudioComponents.Core;

namespace AudioComponents;

public class MidiDecoder : Component
{
	public MidiDecoder(int polyphony)
	{
		if (polyphony < 0 || polyphony > 63) throw new Exception("Polyphony must be between 0 and 63.");
	}


	public override void Tick()
	{
		throw new NotImplementedException();
	}
}




public class Ads
{
	

}