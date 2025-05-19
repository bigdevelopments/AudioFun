
//using MidiInterop.Events;


//namespace MidiInterop;

//public unsafe class MidiController

//{}

//	// state
//	rivate float _level;

//	public MidiController(string name) : base(name)
//	{
//		_output = CreateTerminal("Output");
//		Min = 0f;
//		Max = 1f;
//		Controller = 1;
//	}

//	public int Controller { get; set; }
//	public float Min { get; set; }
//	public float Max { get; set; }

//	public Terminal Output => _output;

//	public Func<MidiEvent[]> MidiSource { get; set; }

//	public override void OnCalculate(int count)
//	{
//		MidiEvent[] events = MidiSource();

//		foreach (MidiEvent midiEvent in events)
//		{
//			ControllerChange controllerChange = midiEvent as ControllerChange;
//			if (controllerChange != null && controllerChange.Controller == Controller)
//			{
//				_level = Min + (Max - Min) * (controllerChange.Value / 127f);
//			}
//		}

//		float* pOutput = _output.FloatWriteBuffer;
//		float level = _level;
//		for (int index = 0; index < count; index++) *pOutput++ = level;
//	}
//}