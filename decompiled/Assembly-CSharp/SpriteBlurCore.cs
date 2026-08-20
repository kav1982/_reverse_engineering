using System;
using UnityEngine;

[Serializable]
public class SpriteBlurCore
{
	[Range(0f, 10f)]
	public float sigema;

	protected const int gpuMemoryBlockSizeBlur = 1024;

	protected const int maxRadius = 92;

	[Range(0.01f, 1f)]
	public float screenScaling = 1f;

	[Range(0f, 92f)]
	public float radius = 1f;

	public ComputeShader blurShader;

	protected int texWidthVisibleSize;

	protected int texHeightVisibleSize;

	public ComputeBuffer weightsBuffer;

	public ComputeBuffer weightsSum;

	private RenderTexture verBlurOutput;

	private RenderTexture horBlurOutput;

	private RenderTexture tempSource;

	private int blurHorID;

	private int blurVerID;

	private int sumsResetID;

	private int weightsCalculatorID;

	private int weightsNormalizerID;

	public bool init { get; set; }

	public void Init(int width, int height)
	{
		InitComputeShaderSetting();
		SetTexVisibleSize((int)((float)width * screenScaling), (int)((float)height * screenScaling));
		init = true;
	}

	public void ApplyBlur(RenderTexture source, RenderTexture destination)
	{
		if (radius < 0.5f || blurShader == null)
		{
			Graphics.Blit(source, destination);
			return;
		}
		blurShader.SetInt("blurRadius", (int)radius);
		blurShader.SetFloat("sigma", (sigema == 0f) ? sigema : GetSigma(radius));
		blurShader.Dispatch(sumsResetID, 1, 1, 1);
		blurShader.Dispatch(weightsCalculatorID, (int)radius + 1, 1, 1);
		blurShader.Dispatch(weightsNormalizerID, (int)radius * 2 + 1, 1, 1);
		DispatchWithSource(ref source, ref destination);
		static float GetSigma(float r)
		{
			return r / 3f;
		}
	}

	public void ReleaseWeight()
	{
		weightsBuffer?.Release();
		weightsSum?.Release();
	}

	private void InitComputeShaderSetting()
	{
		if (!SystemInfo.supportsComputeShaders)
		{
			Debug.LogError(" It seems your target Hardware does not support Compute Shaders.");
			return;
		}
		if (!blurShader)
		{
			Debug.LogError("No BlurShader");
			return;
		}
		blurHorID = blurShader.FindKernel("HorzBlurCs");
		blurVerID = blurShader.FindKernel("VertBlurCs");
		weightsCalculatorID = blurShader.FindKernel("WeightCalculatorCs");
		weightsNormalizerID = blurShader.FindKernel("WeightNormalizerCs");
		sumsResetID = blurShader.FindKernel("SumsDeleteCS");
		weightsSum = new ComputeBuffer(1, 4);
		weightsBuffer = new ComputeBuffer(Mathf.Min(185, 512), 4);
		blurShader.SetBuffer(blurHorID, "gWeights", weightsBuffer);
		blurShader.SetBuffer(blurVerID, "gWeights", weightsBuffer);
		blurShader.SetBuffer(weightsCalculatorID, "gWeights", weightsBuffer);
		blurShader.SetBuffer(weightsNormalizerID, "gWeights", weightsBuffer);
		blurShader.SetBuffer(sumsResetID, "weightsSum", weightsSum);
		blurShader.SetBuffer(weightsCalculatorID, "weightsSum", weightsSum);
		blurShader.SetBuffer(weightsNormalizerID, "weightsSum", weightsSum);
	}

	private void SetTexVisibleSize(int width, int height)
	{
		if (texWidthVisibleSize != width || texHeightVisibleSize != height)
		{
			texWidthVisibleSize = width;
			texHeightVisibleSize = height;
			WarmUpTextures();
		}
	}

	private void WarmUpTextures()
	{
		CreateTextue(ref verBlurOutput);
		CreateTextue(ref horBlurOutput);
		CreateTextue(ref tempSource);
		blurShader.SetTexture(blurHorID, "source", tempSource);
		blurShader.SetTexture(blurHorID, "horBlurOutput", horBlurOutput);
		blurShader.SetTexture(blurVerID, "horBlurOutput", horBlurOutput);
		blurShader.SetTexture(blurVerID, "verBlurOutput", verBlurOutput);
		void CreateTextue(ref RenderTexture textureToMake)
		{
			textureToMake = new RenderTexture(texWidthVisibleSize, texHeightVisibleSize, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
			textureToMake.enableRandomWrite = true;
			textureToMake.wrapMode = TextureWrapMode.Clamp;
			textureToMake.Create();
		}
	}

	private void DispatchWithSource(ref RenderTexture source, ref RenderTexture destination, Material postProcessMat = null)
	{
		if (init)
		{
			int threadGroupsX = Mathf.CeilToInt((float)texWidthVisibleSize / 1024f);
			int threadGroupsY = Mathf.CeilToInt((float)texHeightVisibleSize / 1024f);
			if (postProcessMat == null)
			{
				Graphics.Blit(source, tempSource);
			}
			else
			{
				Graphics.Blit(source, tempSource, postProcessMat);
			}
			blurShader.Dispatch(blurHorID, threadGroupsX, texHeightVisibleSize, 1);
			blurShader.Dispatch(blurVerID, texWidthVisibleSize, threadGroupsY, 1);
			if (postProcessMat == null)
			{
				Graphics.Blit(verBlurOutput, destination);
			}
			else
			{
				Graphics.Blit(verBlurOutput, destination, postProcessMat);
			}
		}
	}
}
