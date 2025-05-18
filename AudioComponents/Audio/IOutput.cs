using System.Numerics;

namespace AudioComponents.Audio;

public interface IOutput
{
	string Name { get; }
}

public interface IOutputWriter : IOutput
{
	Vector2 Value { set; }
}


public interface IInput
{
	string Name { get; }
	void ConnectTo(IOutput output);
}

public interface IInputReader
{
	Vector2 Value { get; }
}

