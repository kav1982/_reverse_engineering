using UnityEngine;

namespace ParticleExporter;

public class ExportSettingsOverwrite : MonoBehaviour
{
	public CanOverWriteValue<int> Size = new CanOverWriteValue<int>(512);

	public CanOverWriteValue<int> Fps = new CanOverWriteValue<int>(30);

	public CanOverWriteValue<string> FilePrefix = new CanOverWriteValue<string>("");

	public CanOverWriteValue<string> SubDir = new CanOverWriteValue<string>("");

	public CanOverWriteValue<string> ExportPath = new CanOverWriteValue<string>("ParticleSystemExport/");

	public float Scale = 1f;
}
