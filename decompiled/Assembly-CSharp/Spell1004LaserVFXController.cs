using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

public class Spell1004LaserVFXController : MonoBehaviour
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
		ettMgr.CreateSingletonBuffer<Spell1004LaserTrailBED>();
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
		using EntityQuery entityQuery = ettMgr.CreateEntityQuery(typeof(Spell1004LaserTrailBED));
		DynamicBuffer<Spell1004LaserTrailBED> singletonBuffer = entityQuery.GetSingletonBuffer<Spell1004LaserTrailBED>();
		bool isChAge14_Static = GameMgr.IsChAge14_Static;
		if (singletonBuffer.Length <= 0)
		{
			return;
		}
		int num = math.min(singletonBuffer.Length, maxNumberGraphicsBufferCount);
		Vector4[] array = new Vector4[num];
		for (int i = 0; i < num; i++)
		{
			if (math.abs(singletonBuffer[i].posAndColorType.w - 2f) < 0.1f && isChAge14_Static)
			{
				array[i] = new float4(singletonBuffer[i].posAndColorType.xyz, 8f);
			}
			else
			{
				array[i] = singletonBuffer[i].posAndColorType;
			}
		}
		posAndColorTypeBuffer.SetData(array);
		vfx.SetInt(countID, num);
		vfx.Play();
		singletonBuffer.Clear();
	}
}
