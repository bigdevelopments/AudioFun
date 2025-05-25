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

// create the host
Host host = new Host();

// add in midi devices
MidiInterface midiInterface = new MidiInterface();
foreach (var midiInput in midiInterface.Inputs) host.Add(midiInput.Name, midiInput);

// add in the audio output
var audioOut = host.Add("audio-out", componentFactory.Create("audio_out"));

// just add a test unit and connect it the audio output
host.Add("unit", componentFactory.Create("sine_synth"));
host.Connect("unit", "out", "audio-out", "in");
host.Connect("LPK25", "out", "unit", "midi_in");


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