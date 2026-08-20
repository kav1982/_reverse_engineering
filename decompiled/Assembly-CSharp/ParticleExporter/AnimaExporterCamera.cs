using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ParticleExporter;

[RequireComponent(typeof(Camera))]
public class AnimaExporterCamera : MonoBehaviour
{
	public int PngSize = 512;

	public int Fps = 30;

	public float Scale = 1f;

	public string ClipName = "";

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
		AnimaExportSettings[] exportObjects = GetExportObjects();
		for (int i = 0; i < exportObjects.Length; i++)
		{
			AnimaExportSettings settings = exportObjects[i];
			if (!settings.rootAnima.HasState(0, Animator.StringToHash(settings.clipName)))
			{
				Debug.LogError("没有指定的动画名！");
			}
			else
			{
				particleCamera.RenderAnimaAndSave(settings);
			}
		}
	}

	private AnimaExportSettings[] GetExportObjects()
	{
		List<AnimaExportSettings> list = new List<AnimaExportSettings>();
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			if (child.gameObject.activeSelf)
			{
				ExportSettingsOverwrite component = child.GetComponent<ExportSettingsOverwrite>();
				Animator component2 = child.GetComponent<Animator>();
				if ((bool)component2)
				{
					AnimaExportSettings item = MakeSettings(component2, component);
					list.Add(item);
				}
			}
		}
		return list.ToArray();
	}

	private AnimaExportSettings MakeSettings(Animator anima, ExportSettingsOverwrite overwrite = null)
	{
		AnimaExportSettings animaExportSettings = default(AnimaExportSettings);
		animaExportSettings.Fps = Fps;
		animaExportSettings.ExportPath = ExportPath;
		animaExportSettings.FilePrefix = anima.name;
		animaExportSettings.PngSize = PngSize;
		animaExportSettings.rootAnima = anima;
		animaExportSettings.Scale = Scale;
		animaExportSettings.clipName = ClipName;
		AnimaExportSettings result = animaExportSettings;
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
