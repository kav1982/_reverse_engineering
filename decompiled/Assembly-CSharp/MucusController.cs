using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.Rendering;

public class MucusController : MonoBehaviour
{
	public UnityEngine.MeshCollider mc;

	public MeshFilter rt_MF;

	public float combineInterval;

	public List<Liquid> liquids = new List<Liquid>();

	public List<Entity> uncombinedEntityList = new List<Entity>();

	public List<Entity> uncombinedCollidersList = new List<Entity>();

	public List<BlobAssetReference<Unity.Physics.Collider>> combinedAssetList = new List<BlobAssetReference<Unity.Physics.Collider>>();

	public List<Entity> combinedEntityList = new List<Entity>();

	private float combineIntervalTimer;

	public int meshPointCountLimit = 500;

	public List<Mesh> meshes = new List<Mesh>();

	public int pointCount { get; private set; }

	public int meshPointCount { get; private set; }

	public Mesh CombinedMesh { get; private set; }

	private void Start()
	{
		pointCount = 0;
		meshPointCount = 0;
		mc.tag = "Mucus";
		mc.sharedMesh = new Mesh();
		mc.transform.position = Vector3.zero;
		rt_MF.transform.position = Tool2D.IgnoreZPoint(Vector3.zero, -120f);
	}

	private void FixedUpdate()
	{
		combineIntervalTimer += Time.deltaTime;
		if (!(combineIntervalTimer >= combineInterval))
		{
			return;
		}
		combineIntervalTimer = 0f;
		if (liquids.Count > 0 && meshPointCount > meshPointCountLimit)
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
			CombinedMesh = mesh;
			MucusSystem.Inst.Combine(this);
			liquids.Clear();
			meshPointCount = 0;
			mc.sharedMesh = new Mesh();
		}
	}

	public void CreateMucus(Vector3 point, float radius)
	{
		if (!GameMgr.IsMobile_Static && pointCount <= 2000)
		{
			pointCount++;
			meshPointCount++;
			Liquid component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MucusSphere", point).GetComponent<Liquid>();
			component.Initialize(radius);
			liquids.Add(component);
		}
	}

	public void CreateMucus(Vector3 point1, Vector3 point2, float radius)
	{
		if (!GameMgr.IsMobile_Static && pointCount <= 2000)
		{
			pointCount++;
			meshPointCount++;
			Liquid component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_MucusRectangle", (point1 + point2) / 2f).GetComponent<Liquid>();
			component.Initialize(radius, Vector3.Distance(point1, point2), point1 - point2);
			liquids.Add(component);
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
		if ((bool)rt_MF.mesh)
		{
			Object.Destroy(rt_MF.mesh);
		}
		rt_MF.mesh = mesh;
		pointCount = 0;
		LevelMgr.Inst.CurrentRoomCtrller.CamMucusRenderOnce();
		for (int i = 0; i < liquids.Count; i++)
		{
			ObjPoolMgr.Inst.RecycleGO(liquids[i].gameObject);
		}
		liquids.Clear();
		MucusSystem.Inst.Clear(this, clearAsset: false);
	}

	public void OnDestroy()
	{
		MucusSystem.Inst.Clear(this, clearAsset: true);
	}
}
