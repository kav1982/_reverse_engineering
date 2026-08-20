using System.Collections.Generic;
using UnityEngine;

public class Elite7_Trap : MonoBehaviour
{
	private enum BallState
	{
		Fly,
		Land,
		Disappear
	}

	public Transform tsf_Layer;

	public Transform tsf_LandRT;

	public Transform tsf_Bubble;

	public GameObject go_FlySpell;

	public GameObject go_LandEF;

	public GameObject go_Shadow;

	public GameObject go_Bubble;

	public ParticleSystem ps_Trail;

	public float biggerSpeed;

	public float heightToChangeLayer;

	[Header("Damage")]
	public LayerMask damageLayer;

	public int damage;

	public float damageInterval;

	private BallState state;

	private UnitProperty elite7Ppt;

	private float radius;

	private float gravity;

	private bool midwayChangeLayer;

	private Vector3 flyVelocity;

	private float zSpeed;

	private float damageIntervalTimer;

	private List<UnitDotsSyncSystem.DistanceHitResult> results = new List<UnitDotsSyncSystem.DistanceHitResult>();

	private void Update()
	{
		switch (state)
		{
		case BallState.Fly:
			zSpeed += gravity * Time.deltaTime;
			base.transform.position += new Vector3(0f, 0f, 0f - zSpeed) * Time.deltaTime;
			base.transform.position += flyVelocity * Time.deltaTime;
			if (midwayChangeLayer && zSpeed < 0f && base.transform.gameObject.layer != LayerMask.NameToLayer("Default") && base.transform.position.z >= 0f - heightToChangeLayer)
			{
				go_FlySpell.transform.localPosition = Vector3.zero;
				base.transform.ChangeAllLayer("Default");
			}
			tsf_Layer.position = Tool2D.GetLayerPoint(base.transform);
			tsf_LandRT.position = Tool2D.IgnoreZPoint(base.transform, -160f);
			go_Shadow.transform.position = Tool2D.IgnoreZPoint(base.transform, 1.05f);
			if (base.transform.position.z >= 0f)
			{
				state = BallState.Land;
				SEMgr.Inst.elite7VoidExplode.PlaySE();
				go_FlySpell.SetActive(value: false);
				go_Shadow.SetActive(value: false);
				go_LandEF.SetActive(value: true);
				tsf_LandRT.gameObject.SetActive(value: true);
				tsf_LandRT.ChangeLayer("Invisible");
				go_Bubble.SetActive(value: true);
				tsf_Layer.position = Tool2D.IgnoreZPoint(base.transform, 1.08f);
				go_Bubble.transform.position = Tool2D.IgnoreZPoint(base.transform, 1.11f);
				ps_Trail.Stop();
			}
			break;
		case BallState.Land:
			if (tsf_LandRT.localScale.x != radius * 2f)
			{
				float num2 = Mathf.MoveTowards(tsf_LandRT.localScale.x, radius * 2f, biggerSpeed * Time.deltaTime);
				tsf_LandRT.localScale = Vector3.one * num2;
				tsf_Bubble.localScale = Vector3.one * num2;
			}
			damageIntervalTimer += Time.deltaTime;
			if (damageIntervalTimer > damageInterval)
			{
				damageIntervalTimer = 0f;
				TakeDamageInfo_Dots info2 = TakeDamageInfo_Dots.NewInfo(Elite7.Inst.myPpt.myEntity);
				info2.damage = damage;
				UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, tsf_LandRT.localScale.x, GameConst.Filter_Friendly, results);
				for (int j = 0; j < results.Count; j++)
				{
					UnitDotsSyncSystem.AddTakeDamageRequest(results[j].entity, info2);
				}
			}
			break;
		case BallState.Disappear:
			if (tsf_LandRT.localScale.x != 0f)
			{
				float num = Mathf.MoveTowards(tsf_LandRT.localScale.x, 0f, biggerSpeed * Time.deltaTime);
				tsf_LandRT.localScale = Vector3.one * num;
				tsf_Bubble.localScale = Vector3.one * num;
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
			damageIntervalTimer += Time.deltaTime;
			if (damageIntervalTimer > damageInterval)
			{
				damageIntervalTimer = 0f;
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Elite7.Inst.myPpt.myEntity);
				info.damage = damage;
				UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, tsf_LandRT.localScale.x, GameConst.Filter_Friendly, results);
				for (int i = 0; i < results.Count; i++)
				{
					UnitDotsSyncSystem.AddTakeDamageRequest(results[i].entity, info);
				}
			}
			break;
		default:
			Debug.LogError(state);
			break;
		}
	}

	public void Initialize(UnitProperty elite7Ppt, Vector3 landPoint, float upSpeed, float gravity, float radius, bool midwayChangeLayer)
	{
		this.elite7Ppt = elite7Ppt;
		this.gravity = gravity;
		this.radius = radius;
		this.midwayChangeLayer = midwayChangeLayer;
		zSpeed = upSpeed;
		go_FlySpell.transform.localScale *= radius;
		go_Shadow.transform.localScale *= radius;
		ps_Trail.transform.localScale *= radius;
		float horizontalDistance = Tool2D.IgnoreZDistance(landPoint, base.transform.position);
		float num = GeneralTool.CannonSpeed(upSpeed, 0f - base.transform.position.z, gravity, horizontalDistance);
		flyVelocity = Tool2D.IgnoreZV2ToV1Normal(landPoint, base.transform.position) * num;
		if (!midwayChangeLayer)
		{
			go_FlySpell.transform.localPosition = Vector3.zero;
			base.transform.ChangeAllLayer("Default");
		}
	}

	public void Disappear()
	{
		state = BallState.Disappear;
	}

	private void _DestroySelf()
	{
		Object.Destroy(base.gameObject);
	}
}
