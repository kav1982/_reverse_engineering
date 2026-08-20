using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ParticleExporter;

[RequireComponent(typeof(Camera))]
public class ParticleExporterCamera : MonoBehaviour
{
	public int PngSize = 512;

	public int Fps = 30;

	public float Scale = 1f;

	public string ExportPath = "ParticleSystemExport/";

	private Camera _camera;

	private Camera particleCamera
	{
		get
		{
			if (!_camera)
			{
				return _camera = GetComponent<Camera>();
			}
			return _camera;
		}
	}

	[MethodButton]
	public void RenderAndSave()
	{
		ExportSettings[] exportObjects = GetExportObjects();
		foreach (ExportSettings settings in exportObjects)
		{
			particleCamera.RenderParticleAndSave(settings);
		}
	}

	private ExportSettings[] GetExportObjects()
	{
		List<ExportSettings> list = new List<ExportSettings>();
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			if (child.gameObject.activeSelf)
			{
				ExportSettingsOverwrite component = child.GetComponent<ExportSettingsOverwrite>();
				ParticleSystem component2 = child.GetComponent<ParticleSystem>();
				if ((bool)component2)
				{
					ExportSettings item = MakeSettings(component2, component);
					list.Add(item);
				}
			}
		}
		return list.ToArray();
	}

	private ExportSettings MakeSettings(ParticleSystem system, ExportSettingsOverwrite overwrite = null)
	{
		ExportSettings exportSettings = default(ExportSettings);
		exportSettings.Fps = Fps;
		exportSettings.ExportPath = ExportPath;
		exportSettings.FilePrefix = system.name;
		exportSettings.PngSize = PngSize;
		exportSettings.rootPs = system;
		exportSettings.Scale = Scale;
		ExportSettings result = exportSettings;
		if ((bool)overwrite)
		{
			if (overwrite.Size.Overwrite)
			{
				result.PngSize = overwrite.Size.Value;
			}
			if (overwrite.Fps.Overwrite)
			{
				result.Fps = overwrite.Fps.Value;
			}
			if (overwrite.FilePrefix.Overwrite)
			{
				result.FilePrefix = overwrite.FilePrefix.Value;
			}
			if (overwrite.ExportPath.Overwrite)
			{
				result.ExportPath = overwrite.ExportPath.Value;
			}
			if (overwrite.SubDir.Overwrite)
			{
				result.ExportPath = Path.Join(result.ExportPath, overwrite.SubDir.Value);
			}
			result.Scale = overwrite.Scale;
		}
		return result;
	}
}
