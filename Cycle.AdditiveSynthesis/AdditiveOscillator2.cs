using System.Numerics;


namespace Cycle.AdditiveSynthesis;

internal class AdditiveOscillator2
{
	// state - 16 float phase
	private Vector4[] _phases = new Vector4[4];

	private float OneOverSampleRate = 1/48000f;

	public Vector2 Step(float fundamental, Span<Vector4> frequencies, Span<Vector4> amplitudes, Span<Vector4> pans, Span<Vector4> phases)
	{
		Vector2 result = Vector2.Zero;
		var fundamentalPhase = fundamental * OneOverSampleRate;

		// now add the phase deltas to the phases
		_phases[0] += phases[0] + frequencies[0] * fundamentalPhase;
		_phases[1] += phases[1] + frequencies[1] * fundamentalPhase;
		_phases[2] += phases[2] + frequencies[2] * fundamentalPhase;
		_phases[3] += phases[3] + frequencies[3] * fundamentalPhase;

		// now calculate the outputs, scaled to amplitudes and pans
		var output0 = Vector4.Sin(_phases[0]) * amplitudes[0];
		var output1 = Vector4.Sin(_phases[1]) * amplitudes[1];
		var output2 = Vector4.Sin(_phases[2]) * amplitudes[2];
		var output3 = Vector4.Sin(_phases[3]) * amplitudes[3];

		// adjustments
		var leftAdjustment0 = (Vector4.One - pans[0]) * 0.5f;
		var rightAdjustment0 = (Vector4.One + pans[0]) * 0.5f;
		var leftAdjustment1 = (Vector4.One - pans[1]) * 0.5f;
		var rightAdjustment1 = (Vector4.One + pans[1]) * 0.5f;
		var leftAdjustment2 = (Vector4.One - pans[2]) * 0.5f;
		var rightAdjustment2 = (Vector4.One + pans[2]) * 0.5f;
		var leftAdjustment3 = (Vector4.One - pans[3]) * 0.5f;
		var rightAdjustment3 = (Vector4.One + pans[3]) * 0.5f;

		leftAdjustment0 *= output0;
		rightAdjustment0 *= output0;
		leftAdjustment1 *= output0;
		rightAdjustment1 *= output0;
		leftAdjustment2 *= output0;
		rightAdjustment2 *= output0;
		leftAdjustment3 *= output0;
		rightAdjustment3 *= output0;

		result.X += leftAdjustment0.X + leftAdjustment0.Y + leftAdjustment0.Z + leftAdjustment0.W;
		result.Y += rightAdjustment0.X + rightAdjustment0.Y + rightAdjustment0.Z + rightAdjustment0.W;
		result.X += leftAdjustment1.X + leftAdjustment1.Y + leftAdjustment1.Z + leftAdjustment1.W;
		result.Y += rightAdjustment1.X + rightAdjustment1.Y + rightAdjustment1.Z + rightAdjustment1.W;
		result.X += leftAdjustment2.X + leftAdjustment2.Y + leftAdjustment2.Z + leftAdjustment2.W;
		result.Y += rightAdjustment2.X + rightAdjustment2.Y + rightAdjustment2.Z + rightAdjustment2.W;
		result.X += leftAdjustment3.X + leftAdjustment3.Y + leftAdjustment3.Z + leftAdjustment3.W;
		result.Y += rightAdjustment3.X + rightAdjustment3.Y + rightAdjustment3.Z + rightAdjustment3.W;

		return result;
	}
}
