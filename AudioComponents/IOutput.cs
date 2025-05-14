using System.Numerics;

namespace AudioComponents;

public interface IOutput
{
	string Name { get; }
}

public interface IInput
{
	string Name { get; }
	void FeedFrom(IOutput output);
}

public interface IOutputWriter
{
	Vector2 Value { get; }
}

public interface IInputReader
{
	Vector2 Value { get; }
}

