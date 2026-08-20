using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.VFX;

public class Elite8_SingleChain : MonoBehaviour
{
	public bool dummy;

	public bool isRed;

	public ParticleSystem particle1;

	public ParticleSystem particle2;

	public Transform point1;

	public Transform point2;

	public Elite8_Child star1;

	public Elite8_Child star2;

	public bool single;

	public float speed;

	public Vector3 diration;

	public float duartion;

	public GameObject startEffect;

	public GameObject endEffect;

	public LayerMask attackLayer;

	public LineRenderer lr_Laser;

	public LineRenderer lr_Shadow;

	public VisualEffect ve_Trail;

	public int damage;

	public float chainHeight;

	public float trailRatePerMeter;

	public float changeRateInterval;

	public List<float> unitsGetHurtedTime = new List<float>();

	public List<Entity> unitsGetHurted = new List<Entity>();

	public float hurtInterval;

	public float existTime;

	private float existTimer;

	public Animator anima;

	public bool chainCanHurt;

	public bool workOnce;

	public bool reportHurt;

	public bool hasStopped;

	private float changeRateIntervalTimer;

	private float rayCheckTimer;

	private UnitDotsSyncSystem.RayCastHitResult[] hitInfos;

	[Header("和谐模式")]
	public Material MT_harmony;

	public VisualEffect ve_Trail_H;

	private void Start()
	{
		anima.GetComponent<AnimaEvent>().DoAction = AnimaAction;
		chainCanHurt = false;
		if (single)
		{
			point1.position = base.transform.position - Tool2D.GetDir(diration, 90f).normalized * 30f;
			point2.position = base.transform.position + Tool2D.GetDir(diration, 90f).normalized * 30f;
		}
		if (GameMgr.IsHarmony_Static && isRed)
		{
			Object.Destroy(lr_Laser.material);
			lr_Laser.material = MT_harmony;
			ve_Trail.gameObject.SetActive(value: false);
			ve_Trail = ve_Trail_H;
		}
		if (dummy && !isRed)
		{
			startEffect.SetActive(value: false);
			endEffect.SetActive(value: false);
		}
	}

	private void Update()
	{
		if (point1 == null || point2 == null)
		{
			return;
		}
		if (single)
		{
			base.transform.position += Time.deltaTime * speed * diration;
			existTimer += Time.deltaTime;
			if (existTimer > existTime)
			{
				StopChain();
				existTimer = 0f;
			}
		}
		lr_Laser.SetPosition(0, Tool2D.GetLayerPoint(point1.position + new Vector3(0f, 0f, 0f - chainHeight)));
		lr_Laser.SetPosition(1, Tool2D.GetLayerPoint(point2.position + new Vector3(0f, 0f, 0f - chainHeight)));
		startEffect.transform.position = Tool2D.GetLayerPoint(point1.position + new Vector3(0f, 0f, 0f - chainHeight));
		endEffect.transform.position = Tool2D.GetLayerPoint(point2.position + new Vector3(0f, 0f, 0f - chainHeight));
		lr_Shadow.SetPosition(0, Tool2D.IgnoreZPoint(point1, 1.05f));
		lr_Shadow.SetPosition(1, Tool2D.IgnoreZPoint(point2, 1.05f));
		ve_Trail.SetVector3("Point1", Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(point1.position - base.transform.position + new Vector3(0f, 0f, 0f - chainHeight)), 1.12f));
		ve_Trail.SetVector3("Point2", Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(point2.position - base.transform.position + new Vector3(0f, 0f, 0f - chainHeight)), 1.12f));
		changeRateIntervalTimer += Time.deltaTime;
		if (changeRateIntervalTimer >= changeRateInterval)
		{
			changeRateIntervalTimer = 0f;
			ve_Trail.SetFloat("Rate", Vector3.Distance(point1.position, point2.position) * trailRatePerMeter);
		}
		if (!point1.gameObject.activeSelf || !point2.gameObject.activeSelf)
		{
			return;
		}
		if (chainCanHurt)
		{
			rayCheckTimer += Time.deltaTime;
			if (!(rayCheckTimer > 0.05f))
			{
				return;
			}
			rayCheckTimer = 0f;
			hitInfos = UnitDotsSyncSystem.RayCastAll(point1.position, point2.position - point1.position, (point2.position - point1.position).magnitude, GameConst.Filter_Friendly);
			if (hitInfos.Length != 0)
			{
				for (int i = 0; i < hitInfos.Length; i++)
				{
					Entity entity = hitInfos[i].entity;
					if (!unitsGetHurted.Contains(entity))
					{
						TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite8.Inst.myPpt.myEntity);
						info.damage = damage;
						info.teammateTakeDamageRatio = 2f;
						UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
						unitsGetHurted.Add(entity);
						unitsGetHurtedTime.Add(hurtInterval);
						SEMgr.Inst.elite8ChainHit.PlaySE();
						if (GameMgr.IsHarmony_Static)
						{
							if (!isRed)
							{
								Elite8.MiniPool.GetGO("Prefabs/EF/EF_Elite8_ChainHit", hitInfos[i].point, 1f);
							}
							else
							{
								Elite8.MiniPool.GetGO("Prefabs/EF/EF_Elite8_ChainHitRed_H", hitInfos[i].point, 1f);
							}
						}
						else if (!isRed)
						{
							Elite8.MiniPool.GetGO("Prefabs/EF/EF_Elite8_ChainHit", hitInfos[i].point, 1f);
						}
						else
						{
							Elite8.MiniPool.GetGO("Prefabs/EF/EF_Elite8_ChainHitRed", hitInfos[i].point, 1f);
						}
					}
					if (workOnce)
					{
						chainCanHurt = false;
						reportHurt = true;
						return;
					}
				}
			}
		}
		for (int num = unitsGetHurtedTime.Count - 1; num >= 0; num--)
		{
			unitsGetHurtedTime[num] -= Time.deltaTime;
			if (unitsGetHurtedTime[num] < 0f)
			{
				unitsGetHurtedTime.RemoveAt(num);
				unitsGetHurted.RemoveAt(num);
			}
		}
	}

	private void OnEnable()
	{
		chainCanHurt = false;
		hasStopped = false;
		if (GameMgr.IsMobile_Static)
		{
			ve_Trail.enabled = false;
		}
		else
		{
			ve_Trail.Play();
		}
		anima.Play("Elite8_ChainShow");
		if (!isRed)
		{
			particle1.Play();
			particle2.Play();
		}
		if (single)
		{
			point1.position = base.transform.position - Tool2D.GetDir(diration, 90f).normalized * 30f;
			point2.position = base.transform.position + Tool2D.GetDir(diration, 90f).normalized * 30f;
		}
	}

	private void RecycleSelf()
	{
		ve_Trail.Stop();
		lr_Laser.SetPosition(0, Vector3.zero);
		lr_Laser.SetPosition(1, Vector3.zero);
		lr_Shadow.SetPosition(0, Vector3.zero);
		lr_Shadow.SetPosition(1, Vector3.zero);
		ve_Trail.SetVector3("Point1", Vector3.zero);
		ve_Trail.SetVector3("Point2", Vector3.zero);
		base.gameObject.SetActive(value: false);
	}

	public void StopChain()
	{
		if (!hasStopped)
		{
			anima.Play("Elite8_ChainFade");
			hasStopped = true;
			if (!isRed)
			{
				particle1.Stop();
				particle2.Stop();
			}
		}
	}

	public void AnimaAction(string animaName)
	{
		switch (animaName)
		{
		case "start":
			chainCanHurt = true;
			break;
		case "end":
			chainCanHurt = false;
			ve_Trail.Stop();
			break;
		case "disable":
			base.gameObject.SetActive(value: false);
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}
}
