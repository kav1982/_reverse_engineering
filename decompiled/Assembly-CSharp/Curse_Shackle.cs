using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class Curse_Shackle : MonoBehaviour
{
	public Transform tsf_IronBall;

	public Transform tsf_Chain1;

	public Transform tsf_Chain2;

	public int chainCount;

	public float chainLength;

	public float ikIterationTime;

	public float threshold;

	public float slowMoveRatio;

	private Transform[] tsf_Chains;

	private Vector3[] points;

	private float totalLength;

	private EntityManager ettMgr;

	private Entity ett_IronBall;

	public float MoveRatio
	{
		get
		{
			float3 point = PlayerMgr.Inst.PlayerPointIgnoreZ;
			float3 @float = IronBallPoint;
			if (DTool.IgnoreZDistanceSqr(in point, in @float) < totalLength * totalLength)
			{
				return 1f;
			}
			return slowMoveRatio;
		}
	}

	private Vector3 IronBallPoint
	{
		get
		{
			return points[points.Length - 1];
		}
		set
		{
			points[points.Length - 1] = value;
		}
	}

	private void Awake()
	{
		ettMgr = World.DefaultGameObjectInjectionWorld.EntityManager;
		ett_IronBall = QuickCreateSystem.Inst.CreateMixedEtt("Curse_Shackle_IronBall", PlayerMgr.Inst.PlayerPointIgnoreZ);
		totalLength = (float)chainCount * chainLength;
		tsf_Chains = new Transform[chainCount + 1];
		points = new Vector3[chainCount + 1];
		tsf_Chains[0] = tsf_Chain1;
		tsf_Chains[1] = tsf_Chain2;
		for (int i = 2; i < chainCount + 1; i++)
		{
			if (i == chainCount)
			{
				tsf_Chains[i] = tsf_IronBall;
			}
			else if (i % 2 == 0)
			{
				tsf_Chains[i] = Object.Instantiate(tsf_Chain1, base.transform.position, Quaternion.identity, base.transform);
			}
			else
			{
				tsf_Chains[i] = Object.Instantiate(tsf_Chain2, base.transform.position, Quaternion.identity, base.transform);
			}
		}
		for (int j = 0; j < chainCount + 1; j++)
		{
			points[j] = PlayerMgr.Inst.PlayerPointIgnoreZ;
		}
	}

	private void LateUpdate()
	{
		LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(ett_IronBall);
		IronBallPoint = componentData.Position;
		if ((PlayerMgr.Inst.PlayerPointIgnoreZ - IronBallPoint).sqrMagnitude < totalLength * totalLength)
		{
			for (int i = 0; (float)i < ikIterationTime; i++)
			{
				points[0] = PlayerMgr.Inst.PlayerPointIgnoreZ;
				for (int j = 1; j < points.Length - 1; j++)
				{
					points[j] = points[j - 1] + (points[j] - points[j - 1]).normalized * chainLength;
				}
				for (int num = points.Length - 2; num >= 0; num--)
				{
					points[num] = points[num + 1] + (points[num] - points[num + 1]).normalized * chainLength;
				}
				if ((PlayerMgr.Inst.PlayerPointIgnoreZ - points[0]).sqrMagnitude < threshold * threshold)
				{
					break;
				}
			}
		}
		else
		{
			Vector3 normalized = (IronBallPoint - PlayerMgr.Inst.PlayerPointIgnoreZ).normalized;
			points[0] = PlayerMgr.Inst.PlayerPointIgnoreZ;
			for (int k = 1; k < points.Length; k++)
			{
				points[k] = points[k - 1] + normalized * chainLength;
			}
		}
		for (int l = 0; l < tsf_Chains.Length - 1; l++)
		{
			tsf_Chains[l].position = Tool2D.IgnoreZPoint(points[l], 1.06f);
			tsf_Chains[l].right = Tool2D.IgnoreZV2ToV1Normal(points[l + 1], points[l]);
		}
		componentData.Position = IronBallPoint;
		ettMgr.SetComponentData(ett_IronBall, componentData);
	}

	public void PointerToPlayer()
	{
		for (int i = 0; i < points.Length; i++)
		{
			points[i] = PlayerMgr.Inst.PlayerPointIgnoreZ;
		}
		tsf_IronBall.position = IronBallPoint;
		LocalTransform componentData = ettMgr.GetComponentData<LocalTransform>(ett_IronBall);
		componentData.Position = IronBallPoint;
		ettMgr.SetComponentData(ett_IronBall, componentData);
	}

	private void OnDestroy()
	{
		ettMgr.DestroyEntity(ett_IronBall);
	}
}
