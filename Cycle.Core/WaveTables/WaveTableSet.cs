namespace Cycle.Core.WaveTables;

/// <summary>
/// Represents a set of same-sized wave tables, each containing a set of samples.
/// Fully managed implementation for now
/// </summary>
public class WaveTableSet
{
	private readonly int _width;
	private readonly int _count;
	private readonly float[] _data;

	public WaveTableSet(int width, int count)
	{
		_width = width;
		_count = count;
		_data = new float[width * count];
	}

	private WaveTableSet(int width, int count, float[] data)
	{
		_width = width;
		_count = count;
		_data = data;
	}


	public int Width => _width;
	public int Count => _count;
	public int Size => _width * _count;

	public ReadOnlySpan<float> GetTable(int index)
	{
		if (index < 0 || index >= _count) throw new IndexOutOfRangeException(nameof(index));
		return _data.AsSpan(index * _width, _width);
	}

	public void SetTable(int index, ReadOnlySpan<float> samples)
	{
		if (index < 0 || index >= _count) throw new IndexOutOfRangeException(nameof(index));
		if (samples.Length != _width) throw new ArgumentException($"Width of supplied table does not match the set", nameof(samples));
		samples.CopyTo(_data.AsSpan(index * _width, _width));
	}

	public Vector2 Sample(int index, float phase)
	{
		phase *= Width;
		int sample = (int)phase;
		int wave = index * Width;
		float leftSample = _data[wave + sample];
		float rightSample = _data[wave + (sample + 1) % Width];
		float interpolated = leftSample + (rightSample - leftSample) * (phase - sample);
		return new Vector2(interpolated);

	}

	public void Save(string path)
	{
		if (string.IsNullOrEmpty(path)) throw new ArgumentException("Invalid path", nameof(path));
		using var writer = new BinaryWriter(File.Create(path));
		writer.Write(_width);
		writer.Write(_count);
		for (int index = 0; index < Size; index++) writer.Write(_data[index]);
	}

	public static WaveTableSet Load(string path)
	{
		if (string.IsNullOrEmpty(path)) throw new ArgumentException("Invalid path", nameof(path));
		using var reader = new BinaryReader(File.OpenRead(path));
		int width = reader.ReadInt32();
		int count = reader.ReadInt32();
		float[] data = new float[width * count];
		for (int index = 0; index < width * count; index++) data[index] = reader.ReadSingle();
		return new WaveTableSet(width, count, data);
	}
}
