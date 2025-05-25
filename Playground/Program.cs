using Cycle.Core.Audio;
using Cycle.Core.Core;
using Cycle.Core.Factory;
using Cycle.Wasapi.NAudio;

using NAudio.Wave;

using System.Diagnostics;
using System.Reflection;
using System.Runtime;

// some prioritisation to try and keep latency low
Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

// attempt pre-load of all assemblies in the current directory
foreach (string assembly in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll"))
{
	try { Assembly.LoadFrom(assembly); } catch { }
}


// this thing create components for us
ComponentFactory componentFactory = new ComponentFactory();

// add unit specifications from file
componentFactory.AddUnitSpec("Units\\test.unit");

ComponentSurface surface = new ComponentSurface();
var audioOut = surface.Add("audio-out", componentFactory.Create("audio_out"));

surface.Add("unit", componentFactory.Create("test_unit"));
surface.Connect("audio-out", "in", "unit", "out");
surface.Initialise(48000);


var audioLink = new AudioLink(surface, audioOut as AudioOutput);
using var outputDevice = new WasapiOut(NAudio.CoreAudioApi.AudioClientShareMode.Exclusive, 10);

outputDevice.Init(audioLink);
outputDevice.Play();
while (outputDevice.PlaybackState == PlaybackState.Playing) Thread.Sleep(1000);
//midi.Inputs[0].Stop();
Console.ReadLine();



//ComponentSurface surface = new ComponentSurface();
//var driver = surface.Add("midi-driver", new PolyphonyDriver(4));

//var midi = new MidiDevices();
//if (midi.Inputs.Length > 0)
//{
//	midi.Inputs[0].Start(driver);
//}
//else
//{
//	Console.WriteLine("No MIDI devices found.");
//}

//var audioOut = surface.Add("audio-out", new AudioOutput());

//surface.Add("440hz", new Constant("440"));
//surface.Add("442hz", new Constant("442"));
//surface.Add("amplitude", new Constant("0.15"));
//surface.Add("sine-osc", new SineOscillator());
//surface.Add("combiner", new Combiner());

//var hybrid = surface.Add("hybrid", new Unit());
//hybrid.Add("osc", new SineOscillator());
//hybrid.Add("inv", new Inverter());
//hybrid.ExposeInput("osc", "frequency", "frequency");
//hybrid.ExposeInput("osc", "amplitude", "amplitude");
//hybrid.ExposeOutput("osc", "out", "out");

//if (midi.Inputs.Length == 0)
//{
//	surface.Connect("440hz", "out", "sine-osc", "frequency");
//	surface.Connect("442hz", "out", "hybrid", "frequency");
//	surface.Connect("amplitude", "out", "sine-osc", "amplitude");
//	surface.Connect("amplitude", "out", "hybrid", "amplitude");
//}
//else
//{
//	surface.Connect("midi-driver", "frequency-0", "sine-osc", "frequency");
//	surface.Connect("midi-driver", "amplitude-0", "sine-osc", "amplitude");
//	surface.Connect("midi-driver", "frequency-1", "hybrid", "frequency");
//	surface.Connect("midi-driver", "amplitude-1", "hybrid", "amplitude");
//}
//surface.Connect("sine-osc", "out", "combiner", "in-1");
//surface.Connect("hybrid", "out", "combiner", "in-2");
//surface.Connect("combiner", "out", "audio-out", "in");

//surface.Initialise(48000, 1000);

//var audioLink = new AudioLink(surface, audioOut);
//using var outputDevice = new WasapiOut(NAudio.CoreAudioApi.AudioClientShareMode.Exclusive, 10);

//outputDevice.Init(audioLink);
//outputDevice.Play();
//while (outputDevice.PlaybackState == PlaybackState.Playing) Thread.Sleep(1000);
//midi.Inputs[0].Stop();
