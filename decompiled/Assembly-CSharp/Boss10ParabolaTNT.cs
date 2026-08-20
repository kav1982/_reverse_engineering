using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Boss10ParabolaTNT : MonoBehaviour
{
	private Vector3 moveDir;

	private float forwardSpeed;

	public float damage;

	public float damageRadius;

	public float knockBack;

	public float moveSpeed;

	public float gravity;

	public float rockGravity;

	public ShockParam shockParam;

	public int bulletCount = 15;

	public bool shootBullet;

	public bool isTnt;

	private float verticalSpeed;

	[Header("图片替换")]
	public SpriteRenderer spriteRenderer;

	public MeshRenderer meshRenderer;

	public Sprite rockSprite;

	public Sprite[] tntSprites;

	public Shadow shadow;

	public ParticleSystem trailParticle;

	[Header("旋转")]
	public float originAngle;

	public float rotateSpeed;

	private bool frame1;

	private bool dropped;

	private List<UnitDotsSyncSystem.DistanceHitResult> distanceHits = new List<UnitDotsSyncSystem.DistanceHitResult>();

	public void Update()
	{
		if (frame1)
		{
			trailParticle.Play();
			frame1 = false;
		}
		originAngle += rotateSpeed * Time.deltaTime;
		if (originAngle > 360f)
		{
			originAngle -= 360f;
		}
		base.transform.eulerAngles = new Vector3(0f, 0f, originAngle);
		verticalSpeed += gravity * Time.deltaTime;
		base.transform.position -= new Vector3(0f, 0f, verticalSpeed * Time.deltaTime);
		base.transform.position += moveDir * forwardSpeed * Time.deltaTime;
		if (base.transform.position.z > 0f && verticalSpeed < 0f && !dropped)
		{
			dropped = true;
			Drop();
		}
	}

	private void Drop()
	{
		UnitDotsSyncSystem.GetCollidersInRange(base.transform.position, damageRadius, GameConst.Filter_MonsterAoeUndiffer, distanceHits);
		for (int i = 0; i < distanceHits.Count; i++)
		{
			Entity entity = distanceHits[i].entity;
			uint layer = UnitDotsSyncSystem.GetLayer(entity);
			switch (layer)
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, damage, out var _);
				break;
			}
			case 512u:
			case 2048u:
			case 2097152u:
			{
				TakeDamageInfo_Dots info2 = TakeDamageInfo_Dots.NewInfo(Boss10.Inst.myPpt.myEntity);
				info2.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHits[i].point, base.transform.position) * knockBack;
				info2.damage = damage;
				info2.isUndifferDamage = true;
				if (layer == 131072)
				{
					info2.ignoreFloatText = true;
				}
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info2);
				break;
			}
			case 32768u:
			case 131072u:
			{
				TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(Boss10.Inst.myPpt.myEntity);
				info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHits[i].point, base.transform.position) * knockBack;
				info.damage = damage * 2f;
				info.isUndifferDamage = true;
				if (layer == 131072)
				{
					info.ignoreFloatText = true;
				}
				UnitDotsSyncSystem.AddTakeDamageRequest(entity, info);
				break;
			}
			}
		}
		base.transform.position = Tool2D.IgnoreZPoint(base.transform);
		if (isTnt)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss13Explosion", base.transform.position).transform.localScale = new Vector3(damageRadius / 2f, damageRadius / 2f, 1f);
			SEMgr.Inst.boss10_Explosion.PlaySE();
		}
		else
		{
			Boss10.Inst.DoubleArcBullet(bulletCount, base.transform.position, 0.5f);
			SEMgr.Inst.boss10_FallRock.PlaySE();
			SEMgr.Inst.boss10_Explosion.PlaySE();
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss10_BigKnock", base.transform.position);
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss10SmashTrace", Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.GroundEffect), 10f).transform.localScale = new Vector3(3f, 3f, 3f);
		}
		CamController.Inst.SetShock(shockParam);
		spriteRenderer.enabled = false;
		trailParticle.Stop();
		shadow.Hide();
		ObjPoolMgr.Inst.RecycleGO(base.gameObject, 3f);
	}

	public void Initialize(Vector3 landPoint, float upForce, bool isTnt)
	{
		if (!isTnt)
		{
			moveDir = Tool2D.IgnoreZV2ToV1Normal(landPoint, base.transform.position);
			float horizontalDistance = Vector3.Distance(base.transform.position, landPoint);
			gravity = rockGravity;
			forwardSpeed = GeneralTool.CannonSpeed(upForce, 0f, gravity, horizontalDistance);
		}
		else
		{
			moveDir = Vector3.zero;
			spriteRenderer.sprite = tntSprites[Random.Range(0, tntSprites.Length)];
			meshRenderer.material.SetTexture(GameConstManaged.shaderTextureIndex, tntSprites[Random.Range(0, tntSprites.Length)].texture);
		}
		frame1 = true;
		base.transform.eulerAngles = new Vector3(0f, 0f, originAngle);
		verticalSpeed = upForce;
		dropped = false;
		spriteRenderer.enabled = true;
		trailParticle.Clear();
		shadow.Show();
		float duration = GeneralTool.CannonLandTime(upForce, 0f - base.transform.position.z, gravity);
		if (isTnt)
		{
			this.isTnt = isTnt;
			ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle_Boss10" + (GameMgr.IsHarmony_Static ? "_H" : ""), landPoint).GetComponent<WarningArea>().Initialize(damageRadius, duration);
		}
		else
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/Mixed/WarningArea_Circle_Boss10" + (GameMgr.IsHarmony_Static ? "_H" : ""), landPoint).GetComponent<WarningArea>().Initialize(damageRadius, duration);
		}
	}
}
