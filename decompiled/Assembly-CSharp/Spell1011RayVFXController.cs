using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

public class Spell1011RayVFXController : MonoBehaviour
{
	public VisualEffect vfx;

	[ColorUsage(true, true)]
	public Color[] colors;

	public int maxNumberGraphicsBufferCount;

	private GraphicsBuffer posAndColorTypeBuffer;

	private GraphicsBuffer colorBuffer;

	private EntityManager ettMgr;

	private int countID;

	private void Awake()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		ettMgr.CreateSingletonBuffer<Spell1011RayTrailBED>();
		countID = Shader.PropertyToID("Count");
		posAndColorTypeBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, maxNumberGraphicsBufferCount, 16);
		colorBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, colors.Length, 16);
		colorBuffer.SetData(colors);
		vfx.SetGraphicsBuffer("PosAndColorTypeBuffer", posAndColorTypeBuffer);
		vfx.SetGraphicsBuffer("ColorBuffer", colorBuffer);
	}

	private void OnDestroy()
	{
		posAndColorTypeBuffer?.Release();
		colorBuffer?.Release();
	}

	private void Update()
	{
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(Spell1011RayTrailBED));
		DynamicBuffer<Spell1011RayTrailBED> singletonBuffer = entityQuery.GetSingletonBuffer<Spell1011RayTrailBED>();
		if (singletonBuffer.Length > 0)
		{
			int num = math.min(singletonBuffer.Length, maxNumberGraphicsBufferCount);
			Vector4[] array = new Vector4[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = singletonBuffer[i].posAndColorType;
			}
			posAndColorTypeBuffer.SetData(array);
			vfx.SetInt(countID, num);
			vfx.Play();
			singletonBuffer.Clear();
		}
	}
}
