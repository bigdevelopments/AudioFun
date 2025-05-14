using System.Numerics;

namespace AudioComponents;

public class Input : IInput, IInputReader
{
	public string Name { get; init; }
	public Output SourceOutput { get; set; }

	public Input(string name)
	{
		Name = name;
	}

	public void FeedFrom(IOutput output)
	{
		if (!(output is Output o)) throw new ArgumentException($"Output {output.Name} is not a valid output.");
		SourceOutput = o;
	}

	public Vector2 Value
	{
		get => SourceOutput?.Value ?? Vector2.Zero;
	}
}
