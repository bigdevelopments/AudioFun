using AudioComponents;

using NAudio.Wave;

using NAudioTest;

class Program
{
	[STAThread]
	public static void Main(string[] args)
	{
		Base bse = new Base();

		var audioOut = bse.Add("audio-out", new AudioOutput());

		bse.Add("440hz", new Constant(440f));
		bse.Add("442hz", new Constant(442f));
		bse.Add("amplitude", new Constant(0.5f));
		bse.Add("sine-osc", new SineOscillator());
		bse.Add("sine-osc2", new SineOscillator());
		bse.Add("combiner", new Combiner());

		bse.Connect("440hz", "out", "sine-osc", "frequency");
		bse.Connect("442hz", "out", "sine-osc2", "frequency");
		bse.Connect("amplitude", "out", "sine-osc", "amplitude");
		bse.Connect("amplitude", "out", "sine-osc2", "amplitude");
		bse.Connect("sine-osc", "out", "combiner", "in-1");
		bse.Connect("sine-osc2", "out", "combiner", "in-2");
		bse.Connect("combiner", "out", "audio-out", "in");

		bse.Initialise(48000, 1000);

		var audioLink = new AudioLink(bse, audioOut);	
		using var outputDevice = new WasapiOut(NAudio.CoreAudioApi.AudioClientShareMode.Exclusive, 10);

		outputDevice.Init(audioLink);
		outputDevice.Play();
		while (outputDevice.PlaybackState == PlaybackState.Playing) Thread.Sleep(1000);
	}
}
