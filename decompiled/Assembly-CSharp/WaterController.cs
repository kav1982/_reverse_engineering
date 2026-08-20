using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WaterController : MonoBehaviour
{
	private const int MaxPointCount = 2000;

	public MeshCollider mc;

	public MeshFilter rt_MF;

	public float combineInterval;

	private List<Liquid> liquids = new List<Liquid>();

	private float combineIntervalTimer;

	public int pointCount { get; private set; }

	private void Start()
	{
		mc.enabled = false;
		pointCount = 0;
		mc.sharedMesh = new Mesh();
		mc.transform.position = Vector3.zero;
		rt_MF.transform.position = Tool2D.IgnoreZPoint(Vector3.zero, -110f);
		mc.tag = "Water";
	}

	private void Update()
	{
		combineIntervalTimer += Time.deltaTime;
		if (!(combineIntervalTimer >= combineInterval))
		{
			return;
		}
		combineIntervalTimer = 0f;
		if (liquids.Count > 0)
		{
			CombineInstance[] array = new CombineInstance[liquids.Count + 1];
			for (int i = 0; i < liquids.Count; i++)
			{
				array[i].mesh = liquids[i].mc.sharedMesh;
				array[i].transform = liquids[i].mc.transform.localToWorldMatrix;
				ObjPoolMgr.Inst.RecycleGO(liquids[i].gameObject);
			}
			array[liquids.Count].mesh = mc.sharedMesh;
			array[liquids.Count].transform = mc.transform.localToWorldMatrix;
			Mesh mesh = new Mesh();
			mesh.indexFormat = IndexFormat.UInt32;
			mesh.CombineMeshes(array);
			if ((bool)mc.sharedMesh)
			{
				Object.Destroy(mc.sharedMesh);
			}
			mc.sharedMesh = mesh;
			if ((bool)rt_MF.mesh)
			{
				Object.Destroy(rt_MF.mesh);
			}
			rt_MF.mesh = mesh;
			LevelMgr.Inst.CurrentRoomCtrller.CamWaterRenderOnce();
			liquids.Clear();
		}
	}

	public void CountWater()
	{
		pointCount++;
	}

	public void CreateWater(Vector3 point, float radius)
	{
		if (!GameMgr.IsMobile_Static && pointCount <= 2000)
		{
			pointCount++;
			Liquid component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_WaterSphere", point).GetComponent<Liquid>();
			component.Initialize(radius);
			liquids.Add(component);
			LevelMgr.Inst.CurrentRoomCtrller.CamWaterRenderOnce();
		}
	}

	public void CreateWater(Vector3 point1, Vector3 point2, float radius)
	{
		if (!GameMgr.IsMobile_Static && pointCount <= 2000)
		{
			pointCount++;
			Liquid component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_WaterRectangle", (point1 + point2) / 2f).GetComponent<Liquid>();
			component.Initialize(radius, Vector3.Distance(point1, point2), point1 - point2);
			liquids.Add(component);
			LevelMgr.Inst.CurrentRoomCtrller.CamWaterRenderOnce();
		}
	}

	public void Clear()
	{
		Mesh mesh = new Mesh();
		if ((bool)mc.sharedMesh)
		{
			Object.Destroy(mc.sharedMesh);
		}
		mc.sharedMesh = mesh;
		if ((bool)rt_MF.sharedMesh)
		{
			Object.Destroy(rt_MF.sharedMesh);
		}
		rt_MF.mesh = mesh;
		pointCount = 0;
		LevelMgr.Inst.CurrentRoomCtrller.CamWaterRenderOnce();
		liquids.Clear();
		WaterSystem.Inst.Clear();
	}
}
