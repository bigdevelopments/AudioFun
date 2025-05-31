using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Cycle.AdditiveSynthesis;

public class AdditiveTest
{
	// all tests on single thread on i9-13900K

	public void RunManagedTest()
	{
		// interesting the JIT hit here. Over 1 second audio, the time taken was about 230ms, meaning that that just 4 of these oscillators
		// would occupy a whole thread. But over 1000 seconds, the time taken is 1.8 seconds, meaning just 2ms per oscillator per second, so 500 odd oscillators
		// per thread - liveable with. Presume some on the fly optimisations are happening, but perhaps we can do better, using Spans or going
		// unsafe

		// Making Sixteen a class means passing by ref, but is also creating garbage all over the shop which we don't want and seems to take twice as long because of it
		// so it remains a struct

		AdditiveOscillator oscillator = new AdditiveOscillator();
		var frequencies = new Sixteen(1f);
		var amplitudes = new Sixteen(0f);
		amplitudes.Band1 = new Vector4(1f, 0, 0, 0);
		var pans = new Sixteen(0f);
		var phases = new Sixteen(0f);

		Vector2[] outputs = new Vector2[48000000];

		Stopwatch stopwatch = Stopwatch.StartNew();
		for (int i = 0; i < outputs.Length; i++)
		{
			outputs[i] = oscillator.Step(46.875f, frequencies, amplitudes, pans, phases);
		}
		Console.WriteLine(stopwatch.ElapsedMilliseconds);
		Console.ReadLine();
	}

	public void RunUsingSpans()
	{
		// this represents an improvement on the above - 1.2 seconds processing for 1000 seconds
		// not sure if the stackallocs are realistic in the real world though, as they are contained
		// to the scope of the method

		AdditiveOscillator2 oscillator = new AdditiveOscillator2();

		Span<Vector4> frequencies = stackalloc Vector4[4];

		for (int i = 0; i < frequencies.Length; i++)
		{
			frequencies[i] = new Vector4(1f);
		}

		Span<Vector4> amplitudes = stackalloc Vector4[4];
		amplitudes[0] = new Vector4(1f, 0, 0, 0);

		Span<Vector4> pans = stackalloc Vector4[4];
		Span<Vector4> phases = stackalloc Vector4[4];
		Vector2[] outputs = new Vector2[48000000];

		Stopwatch stopwatch = Stopwatch.StartNew();
		for (int i = 0; i < outputs.Length; i++)
		{
			outputs[i] = oscillator.Step(46.875f, frequencies, amplitudes, pans, phases);
		}
		Console.WriteLine(stopwatch.ElapsedMilliseconds);
		Console.ReadLine();
	}

	public unsafe void RunUsingUnsafeCode()
	{
		AdditiveOscillator3 oscillator = new AdditiveOscillator3();

		Vector4* frequencies = (Vector4*)Marshal.AllocHGlobal(64);
		Vector4* amplitudes = (Vector4*)Marshal.AllocHGlobal(64);
		Vector4* pans = (Vector4*)Marshal.AllocHGlobal(64);
		Vector4* phases = (Vector4*)Marshal.AllocHGlobal(64);

		for (int i = 0; i < 4; i++)
		{
			frequencies[i] = new Vector4(1f);
			amplitudes[i] = new Vector4(0f);
			pans[i] = new Vector4(0f);
			phases[i] = new Vector4(0f);
		}

		amplitudes[0] = new Vector4(1f, 0, 0, 0);

		Vector2[] outputs = new Vector2[48000000];

		Stopwatch stopwatch = Stopwatch.StartNew();
		for (int i = 0; i < outputs.Length; i++)
		{
			outputs[i] = oscillator.Step(46.875f, frequencies, amplitudes, pans, phases);
		}
		Console.WriteLine(stopwatch.ElapsedMilliseconds);
		Console.ReadLine();
	}
}
