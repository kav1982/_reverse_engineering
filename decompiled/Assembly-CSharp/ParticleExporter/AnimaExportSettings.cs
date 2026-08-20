using UnityEngine;

namespace ParticleExporter;

internal struct AnimaExportSettings
{
	public int PngSize;

	public int Fps;

	public string clipName;

	public string ExportPath;

	public string FilePrefix;

	public float Scale;

	public Animator rootAnima;
}
