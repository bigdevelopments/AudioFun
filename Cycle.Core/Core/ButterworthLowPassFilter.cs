namespace Cycle.Core.Core;

// for the most part created by copilot this, with a bit of tweaking, not tested, might be cobblers

public class ButterworthLowPass
{
	private float a0, a1, a2, b1, b2;
	private float prevInput1, prevInput2;
	private float prevOutput1, prevOutput2;

	public ButterworthLowPass(int sampleRate)
	{
		// set the cut off frequency to 95% of Nyquist frequency
		double cutOff = 0.95d * (sampleRate / 2);

		// Calculate normalized frequency
		double omega = 2 * Math.PI * cutOff / (double)sampleRate;
		double tanOmega = Math.Tan(omega / 2);
		float tanOmegaSquared = (float)(tanOmega * tanOmega);

		// Compute Butterworth filter coefficients
		double norm = 1d / (1d + Math.Sqrt(2) * tanOmega + tanOmega * tanOmega);
		a0 = (float)norm;
		a1 = 2.0f * a0;
		a2 = a0;
		b1 = 2.0f * (tanOmegaSquared - 1.0f) * (float)norm;
		b2 = (1.0f - MathF.Sqrt(2) * (float)tanOmega + tanOmegaSquared) * (float)norm;
	}

	public float Process(float input)
	{
		float output = a0 * input + a1 * prevInput1 + a2 * prevInput2 - b1 * prevOutput1 - b2 * prevOutput2;

		// Shift history buffers
		prevInput2 = prevInput1;
		prevInput1 = input;
		prevOutput2 = prevOutput1;
		prevOutput1 = output;

		return output;
	}
}
