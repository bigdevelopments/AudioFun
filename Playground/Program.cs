using Cycle.Core.AudioComponents;
using Cycle.Core.Core;
using Cycle.Core.Factory;
using Cycle.Midi;
using Cycle.Wasapi.NAudio;

using NAudio.Wave;

using System.Diagnostics;
using System.Runtime;

// some prioritisation to try and keep latency low
Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

//// attempt pre-load of all assemblies in the current directory
//foreach (string assembly in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll"))
//{
//	try { Assembly.LoadFrom(assembly); } catch { }
//}

// this thing create components for us
ComponentFactory componentFactory = new ComponentFactory();

// add unit specifications from file
componentFactory.AddUnitSpec("Units\\test.unit");
componentFactory.AddUnitSpec("Units\\sine_synth.unit");
componentFactory.AddUnitSpec("Units\\adsr_lane.unit");
componentFactory.AddUnitSpec("Units\\test_synth.unit");

// create the host
Host host = new Host();

// add in midi devices
MidiInterface midiInterface = new MidiInterface();
foreach (var midiInputInterface in midiInterface.Inputs) host.Add(midiInputInterface.Name.ToLower(), midiInputInterface);

// add in the audio output
var audioOut = host.Add("audio_out", componentFactory.Create("audio_out"));

// just add a test unit and connect it the audio output
host.Add("synth", componentFactory.Create("test_synth"));
host.Connect("synth", "out", "audio_out", "in");
host.Connect(midiInterface.Inputs[0].Name.ToLower(), "out", "synth", "midi_in");
host.Connect(midiInterface.Inputs[0].Name.ToLower(), "out", "synth", "midi_in2");

// set sample rate
host.Initialise(48000);

// we now need an 'audio link' which handles buffer events calls the host to tick the events and captures audio output
var audioLink = new AudioLink(host, audioOut as AudioOutput);
using var outputDevice = new WasapiOut(NAudio.CoreAudioApi.AudioClientShareMode.Exclusive, 10);

outputDevice.Init(audioLink);

// drive
outputDevice.Play();

// keep going, well forever at the moment
while (outputDevice.PlaybackState == PlaybackState.Playing) Thread.Sleep(1000);