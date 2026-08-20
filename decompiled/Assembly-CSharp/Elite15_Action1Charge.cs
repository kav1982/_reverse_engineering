using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Elite15_Action1Charge : MonoBehaviour
{
	private enum EFState
	{
		Charge,
		Fly,
		FlyFinish
	}

	public GameObject go_Charge;

	public GameObject go_ChargeFinish;

	public ParticleSystem[] ps_Missiles;

	public GameObject go_Explosion;

	public Transform tsf_Layer;

	public Transform tsf_LayerShadow;

	public float recycleDelay;

	[Header("Explosion")]
	public int explosionDamage;

	public ShockParam explosionShock;

	[Header("Action1Bullet")]
	public Vector3 action1FlyDir;

	public float action1OulletOffset;

	public int action1BulletCount;

	public float action1BulletHeight;

	public float action1BulletSpeed;

	public float action1BulletUpSpeed;

	public float action1BulletGravity;

	public float action1BulletBounceRatio;

	[Header("Action4Bullet")]
	public float action4BulletOffset;

	public float action4BulletHeight;

	public int action4BulletDirCount;

	public int action4BulletCountPerDir;

	public VariableFloat action4BulletSpeed;

	public float action4BulletDecelerationLerp;

	public float action4BulletDuration;

	private MiniObjPool masterPool;

	private EFState state;

	private Elite15ActionType actionType;

	private Elite15 elite15;

	private Vector3 flyDir;

	private float flySpeed;

	private float recycleDelayTimer;

	private List<UnitDotsSyncSystem.DistanceHitResult> results = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public void Initialize(Elite15 elite15, Elite15ActionType actionType, MiniObjPool masterPool)
	{
		this.masterPool = masterPool;
		this.elite15 = elite15;
		this.actionType = actionType;
		action1FlyDir.Normalize();
		state = EFState.Charge;
		go_Charge.SetActive(value: true);
		go_ChargeFinish.SetActive(value: false);
		for (int i = 0; i < ps_Missiles.Length; i++)
		{
			ps_Missiles[i].Stop();
		}
		go_Explosion.SetActive(value: false);
		tsf_LayerShadow.gameObject.SetActive(value: true);
		recycleDelayTimer = 0f;
		tsf_Layer.position = Tool2D.GetLayerPoint(base.transform);
		tsf_LayerShadow.IgnoreZPoint(1.05f);
		SEMgr.Inst.elite15Charge.PlaySE();
	}

	private void Update()
	{
		switch (state)
		{
		case EFState.Charge:
			base.transform.position = ((actionType == Elite15ActionType.Action1) ? elite15.GetAction1ShootRealPoint() : elite15.GetAction4ShootRealPoint());
			tsf_Layer.position = Tool2D.GetLayerPoint(base.transform);
			tsf_LayerShadow.IgnoreZPoint(1.05f);
			break;
		case EFState.Fly:
		{
			base.transform.position += flyDir * flySpeed * Time.deltaTime;
			tsf_Layer.position = Tool2D.GetLayerPoint(base.transform);
			tsf_LayerShadow.IgnoreZPoint(1.05f);
			if (!(base.transform.position.z > 0f))
			{
				break;
			}
			base.transform.IgnoreZPoint();
			state = EFState.FlyFinish;
			for (int i = 0; i < ps_Missiles.Length; i++)
			{
				ps_Missiles[i].Stop();
			}
			go_Explosion.SetActive(value: true);
			tsf_LayerShadow.gameObject.SetActive(value: false);
			CamController.Inst.SetShock(explosionShock);
			UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, elite15.action4WarningRadius, GameConst.Filter_MonsterAoe, results);
			for (int j = 0; j < results.Count; j++)
			{
				UnitDotsSyncSystem.DistanceHitResult distanceHitResult = results[j];
				Entity entity = distanceHitResult.entity;
				switch (UnitDotsSyncSystem.GetLayer(entity))
				{
				case 16777216u:
				{
					UnitDotsSyncSystem.ProcessHitSpell(entity, explosionDamage, out var _);
					break;
				}
				case 512u:
				case 32768u:
				case 131072u:
				case 2097152u:
				{
					if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(distanceHitResult.entity, out var result))
					{
						TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite15.Inst.myPpt.myEntity);
						info.damage = explosionDamage;
						if (result.unitCfg.unitType == UnitType.NotAttack)
						{
							info.damage = 999999f;
							info.ignoreFloatText = true;
						}
						info.teammateTakeDamageRatio = 4f;
						UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
					}
					break;
				}
				}
			}
			switch (actionType)
			{
			case Elite15ActionType.Action1:
			{
				for (int m = 0; m < action1BulletCount; m++)
				{
					Vector3 dir2 = Tool2D.GetDir(360f / (float)action1BulletCount * (float)m);
					ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite15_Bullet" + (GameMgr.IsHarmony_Static ? " H" : ""), base.transform.position + new Vector3(0f, 0f, 0f - action1BulletHeight) + dir2 * action1OulletOffset).GetComponent<Elite15_Bullet>().Initialize(dir2 * action1BulletSpeed, action1BulletUpSpeed, action1BulletGravity, action1BulletBounceRatio);
				}
				break;
			}
			case Elite15ActionType.Action4:
			{
				float num = Random.Range(0, 360);
				for (int k = 0; k < action4BulletDirCount; k++)
				{
					Vector3 dir = Tool2D.GetDir((float)(360 / action4BulletDirCount * k) + num);
					for (int l = 0; l < action4BulletCountPerDir; l++)
					{
						ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Elite15_Bullet" + (GameMgr.IsHarmony_Static ? " H" : ""), base.transform.position + new Vector3(0f, 0f, 0f - action4BulletHeight) + dir * action4BulletOffset).GetComponent<Elite15_Bullet>().Initialize(dir * action4BulletSpeed.RandomResult(), action4BulletDuration, action4BulletDecelerationLerp);
					}
				}
				break;
			}
			default:
				Debug.LogError(actionType);
				break;
			}
			SEMgr.Inst.elite15ChargeShootHit.PlaySE();
			break;
		}
		case EFState.FlyFinish:
			recycleDelayTimer += Time.deltaTime;
			if (recycleDelayTimer >= recycleDelay)
			{
				masterPool.RecycleGO(base.gameObject);
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	public void Mute()
	{
		go_Charge.SetActive(value: false);
		masterPool.RecycleGO(base.gameObject);
	}

	public void Shoot(float flySpeed)
	{
		this.flySpeed = flySpeed;
		state = EFState.Fly;
		go_Charge.SetActive(value: false);
		go_ChargeFinish.SetActive(value: true);
		for (int i = 0; i < ps_Missiles.Length; i++)
		{
			ps_Missiles[i].Play();
		}
		flyDir = ((elite15.tsf_Model.localScale.x > 0f) ? action1FlyDir : new Vector3(0f - action1FlyDir.x, action1FlyDir.y, action1FlyDir.z));
		SEMgr.Inst.elite15ChargeShoot.PlaySE();
	}

	public void Shoot(float flySpeed, Vector3 landPoint)
	{
		this.flySpeed = flySpeed;
		state = EFState.Fly;
		go_Charge.SetActive(value: false);
		go_ChargeFinish.SetActive(value: true);
		for (int i = 0; i < ps_Missiles.Length; i++)
		{
			ps_Missiles[i].Play();
		}
		flyDir = (landPoint - base.transform.position).normalized;
		SEMgr.Inst.elite15ChargeShoot.PlaySE();
	}
}
