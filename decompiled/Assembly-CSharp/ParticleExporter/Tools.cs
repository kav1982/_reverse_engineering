using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace ParticleExporter;

internal static class Tools
{
	public static void SavePngs(byte[][] pngs, string inAssetDir, string filePrefix)
	{
		Directory.CreateDirectory(Path.Join(Application.dataPath, inAssetDir));
		string text = Path.Join(Application.dataPath, inAssetDir, filePrefix);
		for (int i = 0; i < pngs.Length; i++)
		{
			File.WriteAllBytes(text + "_" + i + ".png", pngs[i]);
		}
	}

	public static void RenderAnimaAndSave(this Camera camera, AnimaExportSettings settings)
	{
		RenderTexture renderTexture2 = (camera.targetTexture = new RenderTexture(settings.PngSize, settings.PngSize, GraphicsFormat.R8G8B8A8_UNorm, GraphicsFormat.D32_SFloat_S8_UInt));
		SavePngs(camera.RenderAnimaPngs(settings.rootAnima, settings.clipName, settings.Fps, settings.PngSize, settings.Scale), settings.ExportPath, settings.FilePrefix);
	}

	public static void RenderParticleAndSave(this Camera camera, ExportSettings settings)
	{
		RenderTexture renderTexture2 = (camera.targetTexture = new RenderTexture(settings.PngSize, settings.PngSize, GraphicsFormat.R8G8B8A8_UNorm, GraphicsFormat.D32_SFloat_S8_UInt));
		SavePngs(camera.RenderPngs(settings.rootPs, settings.Fps, settings.PngSize, settings.Scale), settings.ExportPath, settings.FilePrefix);
	}

	public static byte[][] RenderPngs(this Camera camera, ParticleSystem rootParticleSystem, int fps, int pngSize, float scale, float startTime = 0.03f)
	{
		camera.orthographicSize = rootParticleSystem.CheckCameraOrthographicSize(fps) * scale;
		List<ParticleSystem> list = rootParticleSystem.GetComponentsInChildren<ParticleSystem>().ToList();
		list.Add(rootParticleSystem);
		float num = 0f;
		float num2 = 1f / (float)fps;
		List<byte[]> list2 = new List<byte[]>();
		rootParticleSystem.Simulate(startTime, withChildren: true, restart: true);
		list2.Add(camera.RenderPng(pngSize));
		for (; num < 0.5f || list.Sum((ParticleSystem e) => e.particleCount) > 0; num += num2)
		{
			if (num > 10f)
			{
				Debug.LogError("TO LONG");
				break;
			}
			rootParticleSystem.Simulate(num2, withChildren: true, restart: false);
			list2.Add(camera.RenderPng(pngSize));
		}
		return list2.ToArray();
	}

	public static byte[][] RenderAnimaPngs(this Camera camera, Animator rootAnima, string clipName, int fps, int pngSize, float scale, float startTime = 0f)
	{
		camera.orthographicSize = 1f * scale;
		_ = 1f / (float)fps;
		List<byte[]> list = new List<byte[]>();
		rootAnima.speed = 0f;
		_ = rootAnima.GetCurrentAnimatorClipInfo(0)[0].clip.length;
		return list.ToArray();
	}

	public static byte[] RenderPng(this Camera camera, int pngSize)
	{
		if (!camera.targetTexture)
		{
			Debug.LogWarning("渲染 PNG 需要相机有合适的 RenderTexture");
		}
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = camera.targetTexture;
		camera.Render();
		Texture2D texture2D = new Texture2D(pngSize, pngSize, TextureFormat.ARGB32, mipChain: false);
		texture2D.ReadPixels(new Rect(0f, 0f, pngSize, pngSize), 0, 0);
		byte[] result = texture2D.EncodeToPNG();
		RenderTexture.active = active;
		return result;
	}

	public static float CheckCameraOrthographicSize(this ParticleSystem rootParticleSystem, int checkFps = 20)
	{
		Bounds bounds = rootParticleSystem.CheckBounds(1f / (float)checkFps);
		Vector3 vector = bounds.max - bounds.min;
		return Mathf.Max(vector.x, vector.y) / 2f;
	}

	public static Bounds CheckBounds(this ParticleSystem rootParticleSystem, float checkTimeInterval)
	{
		Bounds result = default(Bounds);
		List<ParticleSystem> list = rootParticleSystem.GetComponentsInChildren<ParticleSystem>().ToList();
		list.Add(rootParticleSystem);
		ParticleSystemRenderer[] renders = list.Select((ParticleSystem e) => e.GetComponent<ParticleSystemRenderer>()).ToArray();
		for (float num = 0f; num < 0.5f || list.Sum((ParticleSystem e) => e.particleCount) > 0; num += checkTimeInterval)
		{
			if (num > 2f)
			{
				Debug.LogError("TO LONG");
				break;
			}
			Bounds bounds = CheckBoundsInFrame(rootParticleSystem, renders, num);
			result.Encapsulate(bounds);
		}
		return result;
	}

	private static Bounds CheckBoundsInFrame(ParticleSystem rootParticleSystem, ParticleSystemRenderer[] renders, float time, int checkLoop = 3)
	{
		Bounds result = new Bounds(Vector3.zero, Vector3.one);
		for (int i = 0; i < checkLoop; i++)
		{
			rootParticleSystem.Simulate(time, withChildren: true, restart: true);
			for (int j = 0; j < renders.Length; j++)
			{
				Bounds bounds = renders[j].bounds;
				result.Encapsulate(bounds);
			}
		}
		return result;
	}
}
