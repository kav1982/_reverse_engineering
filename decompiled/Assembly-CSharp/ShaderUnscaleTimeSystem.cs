using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

[CompilerGenerated]
public class ShaderUnscaleTimeSystem : SystemBase
{
	private static readonly int UnscaleTime = Shader.PropertyToID("_UnscaleTime");

	private static readonly int UnscaleCosineTime = Shader.PropertyToID("_UnscaleCosineTime");

	private static readonly int UnscaleSineTime = Shader.PropertyToID("_UnscaleSineTime");

	[Preserve]
	protected override void OnUpdate()
	{
		Shader.SetGlobalFloat(UnscaleTime, UnityEngine.Time.unscaledTime);
		Shader.SetGlobalFloat(UnscaleCosineTime, Mathf.Cos(UnityEngine.Time.unscaledTime));
		Shader.SetGlobalFloat(UnscaleSineTime, Mathf.Sin(UnityEngine.Time.unscaledTime));
	}

	[Preserve]
	public ShaderUnscaleTimeSystem()
	{
	}
}
