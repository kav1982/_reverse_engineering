using UnityEngine;

public class Boss6_DirtGenerator : MonoBehaviour
{
	[Header("出生粒子，随时间发射")]
	public bool isBornParticle;

	public float bornParticleDurationTime;

	public AnimationCurve bornParticleCountCurve;

	public float singleSecGenerateCount;

	private float bornGenerateInterval;

	private float bornTimer;

	private float bornTimeCounter;

	[Header("移动粒子，距离发射")]
	public float singleDistanceGenerateCount;

	private float generateDistance;

	public VariableFloat generateRadius;

	public bool isSpike;

	public bool isLargeDirt;

	public bool isQuick;

	public float recycleTime;

	private float movesDistance;

	private Vector3 lastFramePos;

	private void OnEnable()
	{
		generateDistance = 1f / singleDistanceGenerateCount;
		movesDistance = 0f;
		lastFramePos = base.transform.position;
		bornGenerateInterval = 1f / singleSecGenerateCount;
		bornTimer = 0f;
	}

	private void Update()
	{
		if (isBornParticle)
		{
			bornTimer += Time.deltaTime;
			bornTimeCounter += Time.deltaTime * bornParticleCountCurve.Evaluate(bornTimer / bornParticleDurationTime) * singleSecGenerateCount;
			if (bornTimer > bornParticleDurationTime)
			{
				base.enabled = false;
				return;
			}
			while (bornTimeCounter > bornGenerateInterval)
			{
				bornTimeCounter -= bornGenerateInterval;
				if (isQuick)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_SingleDirtLargeQuick" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position + Tool2D.GetDir() * generateRadius.RandomResult(), recycleTime);
				}
				else if (isLargeDirt)
				{
					if (isSpike)
					{
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_SingleDirtSpikeLarge" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position + Tool2D.GetDir() * generateRadius.RandomResult(), recycleTime);
					}
					else
					{
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_SingleDirtLarge" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position + Tool2D.GetDir() * generateRadius.RandomResult(), recycleTime);
					}
				}
				else if (isSpike)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_SingleDirtSpike" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position + Tool2D.GetDir() * generateRadius.RandomResult(), recycleTime);
				}
				else
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_SingleDirt" + (GameMgr.IsChAge14_Static ? " H" : ""), base.transform.position + Tool2D.GetDir() * generateRadius.RandomResult(), recycleTime);
				}
			}
			return;
		}
		movesDistance += (base.transform.position - lastFramePos).magnitude;
		Vector3 normalized = (base.transform.position - lastFramePos).normalized;
		while (movesDistance > generateDistance)
		{
			movesDistance -= generateDistance;
			lastFramePos += normalized * generateDistance;
			if (isLargeDirt)
			{
				if (isSpike)
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_SingleDirtSpikeLarge" + (GameMgr.IsChAge14_Static ? " H" : ""), lastFramePos + Tool2D.GetDir() * generateRadius.RandomResult(), recycleTime);
				}
				else
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_SingleDirtLarge" + (GameMgr.IsChAge14_Static ? " H" : ""), lastFramePos + Tool2D.GetDir() * generateRadius.RandomResult(), recycleTime);
				}
			}
			else if (isSpike)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_SingleDirtSpike" + (GameMgr.IsChAge14_Static ? " H" : ""), lastFramePos + Tool2D.GetDir() * generateRadius.RandomResult(), recycleTime);
			}
			else
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss6_SingleDirt" + (GameMgr.IsChAge14_Static ? " H" : ""), lastFramePos + Tool2D.GetDir() * generateRadius.RandomResult(), recycleTime);
			}
		}
		lastFramePos = base.transform.position;
	}
}
