using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[ExecuteAlways]
[RequireComponent(typeof(MeshRenderer))]
public class Boss56SlashLine3SliceController : MonoBehaviour
{
	public Material slashMaterial;

	public float slashLength = 4f;

	public float slashWidth = 0.6f;

	public float tipLength = 0.45f;

	[Range(0f, 1f)]
	public float progress = 1f;

	public bool overrideMaterialProgress;

	public bool playOnEnable = true;

	public bool driveProgressWhenPlaying;

	public float playDuration = 0.18f;

	public AnimationCurve alphaCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	private static readonly int SlashLengthId = Shader.PropertyToID("_SlashLength");

	private static readonly int TipLengthId = Shader.PropertyToID("_TipLength");

	private static readonly int AlphaId = Shader.PropertyToID("_Alpha");

	private static readonly int ProgressId = Shader.PropertyToID("_Progress");

	private MeshFilter meshFilter;

	private MeshRenderer meshRenderer;

	private MaterialPropertyBlock propertyBlock;

	private Mesh mesh;

	private float playTimer;

	private void OnEnable()
	{
		EnsureComponents();
		RebuildMesh();
		playTimer = 0f;
		ApplyProperties();
	}

	private void OnValidate()
	{
		EnsureComponents();
		RebuildMesh();
		ApplyProperties();
	}

	private void Update()
	{
		if (!Application.isPlaying || !playOnEnable)
		{
			ApplyProperties();
			return;
		}
		playTimer += Time.deltaTime;
		ApplyProperties();
	}

	public void Play(float length)
	{
		slashLength = Mathf.Max(0.01f, length);
		playTimer = 0f;
		RebuildMesh();
		ApplyProperties();
	}

	private void EnsureComponents()
	{
		if (meshFilter == null)
		{
			meshFilter = GetComponent<MeshFilter>();
		}
		if (meshRenderer == null)
		{
			meshRenderer = GetComponent<MeshRenderer>();
		}
		if (propertyBlock == null)
		{
			propertyBlock = new MaterialPropertyBlock();
		}
		if (mesh == null)
		{
			mesh = new Mesh
			{
				name = "SlashLine3SliceMesh"
			};
			mesh.hideFlags = HideFlags.DontSave;
		}
		if (meshFilter != null && meshFilter.sharedMesh != mesh)
		{
			meshFilter.sharedMesh = mesh;
		}
		if (meshRenderer != null && slashMaterial != null)
		{
			meshRenderer.sharedMaterial = slashMaterial;
		}
	}

	private void RebuildMesh()
	{
		if (!(mesh == null))
		{
			slashLength = Mathf.Max(0.01f, slashLength);
			slashWidth = Mathf.Max(0.01f, slashWidth);
			tipLength = Mathf.Clamp(tipLength, 0.001f, slashLength * 0.5f);
			float num = slashLength * 0.5f;
			float num2 = slashWidth * 0.5f;
			float x = 0f - num + tipLength;
			float x2 = num - tipLength;
			float num3 = tipLength / slashLength;
			float x3 = 1f - num3;
			Vector3[] vertices = new Vector3[8]
			{
				new Vector3(0f - num, 0f - num2, 0f),
				new Vector3(0f - num, num2, 0f),
				new Vector3(x, 0f - num2, 0f),
				new Vector3(x, num2, 0f),
				new Vector3(x2, 0f - num2, 0f),
				new Vector3(x2, num2, 0f),
				new Vector3(num, 0f - num2, 0f),
				new Vector3(num, num2, 0f)
			};
			Vector2[] uv = new Vector2[8]
			{
				new Vector2(0f, 0f),
				new Vector2(0f, 1f),
				new Vector2(num3, 0f),
				new Vector2(num3, 1f),
				new Vector2(x3, 0f),
				new Vector2(x3, 1f),
				new Vector2(1f, 0f),
				new Vector2(1f, 1f)
			};
			int[] triangles = new int[18]
			{
				0, 1, 2, 2, 1, 3, 2, 3, 4, 4,
				3, 5, 4, 5, 6, 6, 5, 7
			};
			mesh.Clear();
			mesh.vertices = vertices;
			mesh.uv = uv;
			mesh.colors = new Color[8]
			{
				Color.white,
				Color.white,
				Color.white,
				Color.white,
				Color.white,
				Color.white,
				Color.white,
				Color.white
			};
			mesh.triangles = triangles;
			mesh.RecalculateBounds();
		}
	}

	private void ApplyProperties()
	{
		if (meshRenderer == null)
		{
			return;
		}
		propertyBlock.Clear();
		propertyBlock.SetFloat(SlashLengthId, Mathf.Max(0.01f, slashLength));
		propertyBlock.SetFloat(TipLengthId, Mathf.Max(0.001f, tipLength));
		float value = 1f;
		bool flag = overrideMaterialProgress;
		float value2 = Mathf.Clamp01(progress);
		if (Application.isPlaying && playOnEnable && playDuration > 0f)
		{
			float num = Mathf.Clamp01(playTimer / playDuration);
			value = alphaCurve.Evaluate(num);
			if (driveProgressWhenPlaying)
			{
				value2 = num;
				flag = true;
			}
		}
		propertyBlock.SetFloat(AlphaId, value);
		if (flag)
		{
			propertyBlock.SetFloat(ProgressId, value2);
		}
		meshRenderer.SetPropertyBlock(propertyBlock);
	}
}
