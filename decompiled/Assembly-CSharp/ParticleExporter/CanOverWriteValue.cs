using System;

namespace ParticleExporter;

[Serializable]
public class CanOverWriteValue<T>
{
	public bool Overwrite;

	public T Value;

	public CanOverWriteValue(T defaultValue)
	{
		Value = defaultValue;
	}
}
