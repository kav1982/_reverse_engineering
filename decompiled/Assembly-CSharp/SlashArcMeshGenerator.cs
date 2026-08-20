using System;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SlashArcMeshGenerator : MonoBehaviour
{
	public float radiusInner = 0.15f;

	public float radiusOuter = 2.4f;

	public float angle = 110f;

	public int segments = 32;

	public AnimationCurve fadeCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.15f, 1f), new Keyframe(1f, 0f));

	public AnimationCurve emissionCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.2f, 1f), new Keyframe(1f, 0f));

	public AnimationCurve distortionCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));

	public float duration = 0.28f;

	public float maxEmission = 4f;

	public float maxDistortion = 0.1f;

	private static readonly int TransparencyId = Shader.PropertyToID("_Transparency");

	private static readonly int AlphaId = Shader.PropertyToID("_Alpha");

	private static readonly int EmissionId = Shader.PropertyToID("_Emission");

	private static readonly int EmissionStrengthId = Shader.PropertyToID("_EmissionStrength");

	private static readonly int DistortionId = Shader.PropertyToID("_Distortion");

	private MeshFilter meshFilter;

	private MeshRenderer meshRenderer;

	private MaterialPropertyBlock propertyBlock;

	private float timer;

	private void Awake()
	{
		CacheComponents();
		RebuildMesh();
	}

	private void OnEnable()
	{
		timer = 0f;
		CacheComponents();
		RebuildMesh();
		ApplyMaterialProperties(0f);
	}

	private void Update()
	{
		if (!(duration <= 0f))
		{
			timer += Time.deltaTime;
			ApplyMaterialProperties(Mathf.Clamp01(timer / duration));
		}
	}

	private void OnValidate()
	{
		segments = Mathf.Max(1, segments);
		radiusInner = Mathf.Max(0f, radiusInner);
		radiusOuter = Mathf.Max(radiusInner, radiusOuter);
		if (!Application.isPlaying)
		{
			CacheComponents();
			RebuildMesh();
			ApplyMaterialProperties(0f);
		}
	}

	private void CacheComponents()
	{
		if (!meshFilter)
		{
			meshFilter = GetComponent<MeshFilter>();
		}
		if (!meshRenderer)
		{
			meshRenderer = GetComponent<MeshRenderer>();
		}
		if (propertyBlock == null)
		{
			propertyBlock = new MaterialPropertyBlock();
		}
	}

	private void RebuildMesh()
	{
		if ((bool)meshFilter)
		{
			int num = (segments + 1) * 2;
			Vector3[] array = new Vector3[num];
			Vector2[] array2 = new Vector2[num];
			Color[] array3 = new Color[num];
			int[] array4 = new int[segments * 6];
			float num2 = (0f - angle) * 0.5f;
			float num3 = angle / (float)segments;
			for (int i = 0; i <= segments; i++)
			{
				float num4 = (float)i / (float)segments;
				float f = (num2 + num3 * (float)i) * (MathF.PI / 180f);
				Vector3 vector = new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0f);
				int num5 = i * 2;
				int num6 = num5 + 1;
				array[num5] = vector * radiusInner;
				array[num6] = vector * radiusOuter;
				array2[num5] = new Vector2(num4, 0f);
				array2[num6] = new Vector2(num4, 1f);
				float a = ((fadeCurve != null) ? Mathf.Clamp01(fadeCurve.Evaluate(num4)) : 1f);
				array3[num5] = new Color(1f, 1f, 1f, a);
				array3[num6] = new Color(1f, 1f, 1f, a);
			}
			for (int j = 0; j < segments; j++)
			{
				int num7 = j * 2;
				int num8 = j * 6;
				array4[num8] = num7;
				array4[num8 + 1] = num7 + 1;
				array4[num8 + 2] = num7 + 3;
				array4[num8 + 3] = num7;
				array4[num8 + 4] = num7 + 3;
				array4[num8 + 5] = num7 + 2;
			}
			Mesh mesh = meshFilter.sharedMesh;
			if (mesh == null)
			{
				mesh = new Mesh
				{
					name = "SlashArcMesh"
				};
				meshFilter.sharedMesh = mesh;
			}
			else
			{
				mesh.Clear();
			}
			mesh.vertices = array;
			mesh.uv = array2;
			mesh.colors = array3;
			mesh.triangles = array4;
			mesh.RecalculateBounds();
		}
	}

	private void ApplyMaterialProperties(float normalizedTime)
	{
		if ((bool)meshRenderer)
		{
			float value = ((fadeCurve != null) ? Mathf.Clamp01(fadeCurve.Evaluate(normalizedTime)) : 1f);
			float value2 = ((emissionCurve != null) ? (emissionCurve.Evaluate(normalizedTime) * maxEmission) : 0f);
			float value3 = ((distortionCurve != null) ? (distortionCurve.Evaluate(normalizedTime) * maxDistortion) : 0f);
			meshRenderer.GetPropertyBlock(propertyBlock);
			propertyBlock.SetFloat(TransparencyId, value);
			propertyBlock.SetFloat(AlphaId, value);
			propertyBlock.SetFloat(EmissionId, value2);
			propertyBlock.SetFloat(EmissionStrengthId, value2);
			propertyBlock.SetFloat(DistortionId, value3);
			meshRenderer.SetPropertyBlock(propertyBlock);
		}
	}
}
