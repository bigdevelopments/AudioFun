//using Architecture;

//using MidiInterop.Events;

//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace MidiInterop
//{
//	/// <summary>
//	/// A thing that takes midi events and converts into continuous control streams
//	/// </summary>
//	public class PolyphonicBridge : Component
//	{
//		// remember the channel to filter events on
//		private readonly int _channel;

//		// we'll keep a queue of available voices that aren't doing anything
//		// to round-robin free voices for new note on messages
//		private readonly Queue<int> _freeQueue;
		
//		// and we need to store all the outputs so we can calculate them once and return
//		// them as needed
//		private readonly MidiEvent[][] _outputValues;

//		// we need state information about each voice - what key its attached to...
//		private readonly int[] _voiceStates;

//		// reusable empty output so we don't need to keep newing up arrays
//		private readonly MidiEvent[] _null = new MidiEvent[0];

//		// and of course we need to prevent double-ticking
//		private int _clock;

//		public PolyphonicBridge(string name, int channel, int polyphony) : base(name)
//		{
//			_channel = channel;
//			_freeQueue = new Queue<int>(polyphony);
//			_outputValues = new MidiEvent[polyphony][];
//			_voiceStates = new int[polyphony];

//			for (int index = 0; index < polyphony; index++)
//			{
//				_freeQueue.Enqueue(index);
//				_outputValues[index] = new MidiEvent[0];
//				_voiceStates[index] = -1;
//			}
//		}

//		public int Voices => _outputValues.Length;

//		public Func<MidiEvent[]> MidiSource { get; set; }

//		public Func<MidiEvent[]> this[int index] => () =>
//		{
//			return _outputValues[index];
//		};

//		public override void OnCalculate(int count)
//		{
//			// set the output values to nothing which they pretty much will be all the time
//			for (int index = 0; index < _outputValues.Length; index++) _outputValues[index] = _null;

//			// go to the main source
//			MidiEvent[] midiEvents = MidiSource();

//			// if there is nothing on this tick (which will be the usual case) we are done;
//			if (midiEvents == null || midiEvents.Length == 0) return;

//			// we have some events!
//			foreach (MidiEvent midiEvent in midiEvents)
//			{
//				NoteOn noteOn = midiEvent as NoteOn;
//				if (noteOn != null)
//				{
//					// we need to find a free voice for it
//					if (_freeQueue.Count > 0)
//					{
//						int voice = _freeQueue.Dequeue();
//						_voiceStates[voice] = noteOn.Key;
//						_outputValues[voice] = _outputValues[voice].Concat(new[] { midiEvent }).ToArray();
//					}
//				}
//				else
//				{
//					NoteOff noteOff = midiEvent as NoteOff;
//					if (noteOff != null)
//					{
//						// we now need to find the voice to which this attached
//						for (int voice = 0; voice < _voiceStates.Length; voice++)
//						{
//							if (_voiceStates[voice] == noteOff.Key)
//							{
//								_voiceStates[voice] = -1;
//								_freeQueue.Enqueue(voice);
//								_outputValues[voice] = _outputValues[voice].Concat(new[] { midiEvent }).ToArray();
//								break;
//							}
//						}
//					}
//				}
//			}
//		}
//	}
//}