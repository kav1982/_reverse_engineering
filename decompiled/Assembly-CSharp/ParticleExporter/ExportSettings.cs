using UnityEngine;

namespace ParticleExporter;

internal struct ExportSettings
{
	public int PngSize;

	public int Fps;

	public string ExportPath;

	public string FilePrefix;

	public float Scale;

	public ParticleSystem rootPs;
}
