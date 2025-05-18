using AudioComponents;

using NAudio.Wave;
using NAudioTest;

class Program
{
	public static void Main(string[] args)
	{
		ComponentSurface surface = new ComponentSurface();

		var audioOut = surface.Add("audio-out", new AudioOutput());

		surface.Add("440hz", new Constant(440f));
		surface.Add("442hz", new Constant(442f));
		surface.Add("amplitude", new Constant(0.5f));
		surface.Add("sine-osc", new SineOscillator());
		surface.Add("combiner", new Combiner());

		var hybrid = surface.Add("hybrid", new HybridComponent());
		hybrid.Add("osc", new SineOscillator());
		hybrid.Add("inv", new Inverter());
		hybrid.ExposeInput("osc", "frequency", "frequency");
		hybrid.ExposeInput("osc", "amplitude", "amplitude");
		hybrid.ExposeOutput("osc", "out", "out");

		surface.Connect("440hz", "out", "sine-osc", "frequency");
		surface.Connect("442hz", "out", "hybrid", "frequency");
		surface.Connect("amplitude", "out", "sine-osc", "amplitude");
		surface.Connect("amplitude", "out", "hybrid", "amplitude");
		surface.Connect("sine-osc", "out", "combiner", "in-1");
		surface.Connect("hybrid", "out", "combiner", "in-2");
		surface.Connect("combiner", "out", "audio-out", "in");


		surface.Initialise(48000, 1000);

		var audioLink = new AudioLink(surface, audioOut);	
		using var outputDevice = new WasapiOut(NAudio.CoreAudioApi.AudioClientShareMode.Exclusive, 10);

		outputDevice.Init(audioLink);
		outputDevice.Play();
		while (outputDevice.PlaybackState == PlaybackState.Playing) Thread.Sleep(1000);
	}
}
