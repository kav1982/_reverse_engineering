using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class SpecialObj8_T6TongueCluster : LayerCorrect
{
	private enum StateMachine
	{
		Idle,
		Out,
		Back,
		Eating,
		EatingScaleShort,
		EatingScaleHigh,
		EatingRecovery,
		Disappear
	}

	[Header("GenerateTongue")]
	[Space(50f)]
	public Transform tsf_Mask;

	public SpriteRenderer sr_Tongue;

	public int tongueCount;

	public float tongueGenerateRadius;

	public float tongueGenerateCenterOffsetY;

	public float tongueGenerateCircleWidthScale;

	public VariableFloat tongueWaveTimeOffset;

	public VariableFloat tongueWaveSpeed;

	[Header("FindSpell")]
	public LineRenderer lr_TongueLong;

	public Transform tsf_TongueLongHead;

	public int tongueLongNodeCount;

	public float checkSpellInterval;

	public float checkSpellRadius;

	public float bezierPoint2Hight;

	public float bezierPoint3Hight;

	public float outSpeed;

	public float backSpeed;

	public float itemExtraHight;

	[Header("SpellIcon")]
	public Transform tsf_SpellCenter;

	public SpriteRenderer sr_FindedSpellIcon;

	public GameObject go_Star1;

	public GameObject go_Star2;

	[Header("EatSpell")]
	public VariableFloat eatSpellWaveSpeed;

	public float eatScaleShortSpeed;

	public float eatScaleHighSpeed;

	public float eatScaleRecoverySpeed;

	public float eatScaleValue;

	public RollRewardFly pfb_RollRewardFly;

	public float disappearSpeed;

	private StateMachine sm;

	private List<SpriteRenderer> sr_Tongues = new List<SpriteRenderer>();

	private Vector3[] bezierPoints = new Vector3[4];

	private float checkSpellTimer;

	private Entity findedItemEtt;

	private float longTongueLerp;

	private Vector3 eatSpellPoint;

	private float spitScale;

	private float currentOffset;

	private void Update()
	{
		switch (sm)
		{
		case StateMachine.Idle:
		{
			checkSpellTimer += Time.deltaTime;
			if (!(checkSpellTimer >= checkSpellInterval))
			{
				break;
			}
			checkSpellTimer = 0f;
			CollisionFilter collisionFilter = default(CollisionFilter);
			collisionFilter.BelongsTo = 1073741824u;
			collisionFilter.CollidesWith = 262144u;
			collisionFilter.GroupIndex = 0;
			CollisionFilter filter = collisionFilter;
			List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
			UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, checkSpellRadius, filter, list);
			Entity entity = Entity.Null;
			float num = checkSpellRadius + 1f;
			for (int j = 0; j < list.Count; j++)
			{
				Entity entity2 = list[j].entity;
				float distance = list[j].distance;
				if (UnitDotsSyncSystem.TryGetComponent<Item>(entity2, out var result) && result.info.type == ItemType.Spell && SpellConfig.dic[result.info.id].dropType != ItemDropType.Special && num > distance)
				{
					num = distance;
					entity = entity2;
				}
			}
			if (entity != Entity.Null)
			{
				sm = StateMachine.Out;
				findedItemEtt = entity;
				tsf_TongueLongHead.gameObject.SetActive(value: true);
			}
			break;
		}
		case StateMachine.Out:
		{
			if (!UnitDotsSyncSystem.EntityIsValid(findedItemEtt))
			{
				sm = StateMachine.Back;
				break;
			}
			Vector3 vector = UnitDotsSyncSystem.GetComponentData<LocalTransform>(findedItemEtt).Position;
			if (Vector3.SqrMagnitude(base.transform.position - vector) > checkSpellRadius * checkSpellRadius + 1f)
			{
				sm = StateMachine.Back;
				break;
			}
			bezierPoints[2] = vector + new Vector3(0f, 0f, 0f - bezierPoint3Hight);
			bezierPoints[3] = vector + new Vector3(0f, 0f, 0f - itemExtraHight);
			longTongueLerp = Mathf.MoveTowards(longTongueLerp, 1f, outSpeed * Time.deltaTime);
			for (int m = 0; m < tongueLongNodeCount; m++)
			{
				Vector3 rootPoint = GeneralTool.FreeBezierCurve((float)m / ((float)tongueLongNodeCount - 1f) * longTongueLerp, bezierPoints);
				lr_TongueLong.SetPosition(m, Tool2D.GetLayerPoint(rootPoint));
				if (m == tongueLongNodeCount - 1)
				{
					tsf_TongueLongHead.position = Tool2D.GetLayerPoint(rootPoint);
				}
			}
			if (longTongueLerp == 1f)
			{
				Item componentData = UnitDotsSyncSystem.GetComponentData<Item>(findedItemEtt);
				int id = componentData.info.id;
				sm = StateMachine.Back;
				tsf_SpellCenter.gameObject.SetActive(value: true);
				tsf_SpellCenter.position = lr_TongueLong.GetPosition(tongueLongNodeCount - 1);
				sr_FindedSpellIcon.sprite = ABResources.LoadAsset<Sprite>(SpellConfig.dic[id].GetIconPath());
				go_Star1.SetActive(SpellConfig.dic[id].level > 1);
				go_Star2.SetActive(SpellConfig.dic[id].level > 2);
				eatSpellPoint = vector;
				componentData.BackPool();
				componentData.Pickup(playSE: false);
				UnitDotsSyncSystem.SetComponentData(componentData, findedItemEtt);
			}
			break;
		}
		case StateMachine.Back:
		{
			longTongueLerp = Mathf.MoveTowards(longTongueLerp, 0f, backSpeed * Time.deltaTime);
			for (int num3 = 0; num3 < tongueLongNodeCount; num3++)
			{
				Vector3 rootPoint2 = GeneralTool.FreeBezierCurve((float)num3 / ((float)tongueLongNodeCount - 1f) * longTongueLerp, bezierPoints);
				lr_TongueLong.SetPosition(num3, Tool2D.GetLayerPoint(rootPoint2));
				if (num3 == tongueLongNodeCount - 1)
				{
					tsf_TongueLongHead.position = Tool2D.GetLayerPoint(rootPoint2);
					if (tsf_SpellCenter.gameObject.activeSelf)
					{
						tsf_SpellCenter.position = Tool2D.GetLayerPoint(rootPoint2);
					}
				}
			}
			if (longTongueLerp != 0f)
			{
				break;
			}
			tsf_TongueLongHead.gameObject.SetActive(value: false);
			if (tsf_SpellCenter.gameObject.activeSelf)
			{
				sm = StateMachine.EatingScaleShort;
				tsf_SpellCenter.gameObject.SetActive(value: false);
				for (int num4 = 0; num4 < sr_Tongues.Count; num4++)
				{
					sr_Tongues[num4].material.SetFloat("_WaveSpeed", eatSpellWaveSpeed.RandomResult());
				}
			}
			else
			{
				sm = StateMachine.Idle;
			}
			break;
		}
		case StateMachine.EatingScaleShort:
		{
			spitScale = Mathf.MoveTowards(spitScale, eatScaleValue, eatScaleShortSpeed * Time.deltaTime);
			for (int num2 = 0; num2 < sr_Tongues.Count; num2++)
			{
				sr_Tongues[num2].transform.localScale = new Vector3(1f + spitScale, 1f - spitScale, 1f);
			}
			if (spitScale == eatScaleValue)
			{
				sm = StateMachine.EatingScaleHigh;
			}
			break;
		}
		case StateMachine.EatingScaleHigh:
		{
			spitScale = Mathf.MoveTowards(spitScale, 0f - eatScaleValue, eatScaleHighSpeed * Time.deltaTime);
			for (int k = 0; k < sr_Tongues.Count; k++)
			{
				sr_Tongues[k].transform.localScale = new Vector3(1f + spitScale, 1f - spitScale, 1f);
			}
			if (spitScale == 0f - eatScaleValue)
			{
				sm = StateMachine.EatingRecovery;
				for (int l = 0; l < sr_Tongues.Count; l++)
				{
					sr_Tongues[l].material.SetFloat("_WaveSpeed", tongueWaveSpeed.RandomResult());
				}
				int spellFromPool = PlayerMgr.Inst.BaData.GetSpellFromPool(1, ItemDropType.Special, 10201);
				PlayerMgr.Inst.ItemCtrller.RewardDropFly(spellFromPool, SpecialObj217.rewardType.SpellSpecial, base.transform.position, eatSpellPoint);
			}
			break;
		}
		case StateMachine.EatingRecovery:
		{
			spitScale = Mathf.MoveTowards(spitScale, 0f, eatScaleRecoverySpeed * Time.deltaTime);
			for (int n = 0; n < sr_Tongues.Count; n++)
			{
				sr_Tongues[n].transform.localScale = new Vector3(1f + spitScale, 1f - spitScale, 1f);
			}
			if (spitScale == 0f)
			{
				sm = StateMachine.Disappear;
			}
			break;
		}
		case StateMachine.Disappear:
		{
			currentOffset = Mathf.MoveTowards(currentOffset, 1f, disappearSpeed * Time.deltaTime);
			for (int i = 0; i < sr_Tongues.Count; i++)
			{
				sr_Tongues[i].material.SetFloat("_Offset", currentOffset);
			}
			if (currentOffset == 1f)
			{
				Object.Destroy(base.gameObject);
			}
			break;
		}
		default:
			Debug.LogError(sm);
			break;
		}
	}

	private Vector3 GetGeneratePoint()
	{
		Vector3 dir = Tool2D.GetDir();
		dir.x *= tongueGenerateCircleWidthScale;
		return Tool2D.GetLayerPoint(base.transform.position + new Vector3(0f, tongueGenerateCenterOffsetY, 0f) + dir * Random.Range(0f, tongueGenerateRadius));
	}

	public void Initialize(bool isSO8Flip)
	{
		if (isSO8Flip)
		{
			tsf_Mask.localScale = new Vector3(-1f, 1f, 1f);
		}
		sr_Tongue.transform.position = GetGeneratePoint();
		sr_Tongues.Add(sr_Tongue);
		for (int i = 0; i < tongueCount; i++)
		{
			SpriteRenderer spriteRenderer = Object.Instantiate(sr_Tongue, tsf_Mask);
			spriteRenderer.transform.position = GetGeneratePoint();
			spriteRenderer.material.SetFloat("_TimeOffset", tongueWaveTimeOffset.RandomResult());
			spriteRenderer.material.SetFloat("_WaveSpeed", tongueWaveSpeed.RandomResult());
			sr_Tongues.Add(spriteRenderer);
		}
		lr_TongueLong.positionCount = tongueLongNodeCount;
		for (int j = 0; j < tongueLongNodeCount; j++)
		{
			lr_TongueLong.SetPosition(j, Vector3.zero);
		}
		longTongueLerp = 0f;
		tsf_TongueLongHead.gameObject.SetActive(value: false);
		tsf_SpellCenter.gameObject.SetActive(value: false);
		bezierPoints[0] = base.transform.position;
		bezierPoints[1] = base.transform.position + new Vector3(0f, 0f, 0f - bezierPoint2Hight);
	}
}
