using System.Numerics;

namespace Cycle.AdditiveSynthesis;

internal class AdditiveOscillator
{
	// state - 16 float phase
	private Sixteen _phases = new Sixteen();

	private float OneOverSampleRate = 1/48000f;

	public Vector2 Step(float fundamental, Sixteen frequencies, Sixteen amplitudes, Sixteen pans, Sixteen phases)
	{
		// apply the fundamental to the frequencies
		frequencies = frequencies * fundamental;

		// now change the frequencies to phase deltas
		var phaseDeltas = frequencies * OneOverSampleRate;

		// now add the phase deltas to the phases
		_phases += phaseDeltas;

		// now calculate the outputs, scaled to amplitudes and pans
		var outputs = Sixteen.Sin(_phases) * amplitudes;

		var leftAdjustment = (1 - pans) * 0.5f;
		var rightAdjustment = (1 + pans) * 0.5f;

		leftAdjustment *= outputs;
		rightAdjustment *= outputs;

		return new Vector2(rightAdjustment.Sum(), leftAdjustment.Sum());
	}
}
