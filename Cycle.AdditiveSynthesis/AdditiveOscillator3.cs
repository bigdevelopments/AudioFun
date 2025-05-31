using System.Numerics;


namespace Cycle.AdditiveSynthesis;

internal unsafe class AdditiveOscillator3
{
	// state - 16 float phase
	private Vector4[] _phases = new Vector4[4];

	private float OneOverSampleRate = 1 / 48000f;

	public Vector2 Step(float fundamental, Vector4* frequencies, Vector4* amplitudes, Vector4* pans, Vector4* phases)
	{
		Vector2 result = Vector2.Zero;
		var fundamentalPhase = fundamental * OneOverSampleRate;

		// now add the phase deltas to the phases
		_phases[0] += frequencies[0] * fundamentalPhase;
		_phases[1] += frequencies[1] * fundamentalPhase;
		_phases[2] += frequencies[2] * fundamentalPhase;
		_phases[3] += frequencies[3] * fundamentalPhase;

		// now calculate the outputs, scaled to amplitudes and pans
		var output0 = Vector4.Sin(phases[0] + _phases[0]) * amplitudes[0];
		var output1 = Vector4.Sin(phases[1] + _phases[1]) * amplitudes[1];
		var output2 = Vector4.Sin(phases[2] + _phases[2]) * amplitudes[2];
		var output3 = Vector4.Sin(phases[3] + _phases[3]) * amplitudes[3];

		// adjustments
		var leftAdjustment0 = (Vector4.One - pans[0]) * 0.5f;
		var rightAdjustment0 = Vector4.One - leftAdjustment0;
		var leftAdjustment1 = (Vector4.One - pans[1]) * 0.5f;
		var rightAdjustment1 = Vector4.One - leftAdjustment1;
		var leftAdjustment2 = (Vector4.One - pans[2]) * 0.5f;
		var rightAdjustment2 = Vector4.One - leftAdjustment2;
		var leftAdjustment3 = (Vector4.One - pans[3]) * 0.5f;
		var rightAdjustment3 = Vector4.One - leftAdjustment3;

		leftAdjustment0 *= output0;
		rightAdjustment0 *= output0;
		leftAdjustment1 *= output1;
		rightAdjustment1 *= output1;
		leftAdjustment2 *= output2;
		rightAdjustment2 *= output2;
		leftAdjustment3 *= output3;
		rightAdjustment3 *= output3;

		result = new Vector2(
			Vector4.Dot(leftAdjustment0 + leftAdjustment1 + leftAdjustment2 + leftAdjustment3, Vector4.One),
			Vector4.Dot(rightAdjustment0 + rightAdjustment1 + rightAdjustment2 + rightAdjustment3, Vector4.One));


		return result;
	}
}
