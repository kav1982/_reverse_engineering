using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class Monster16_Explosion : MonoBehaviour
{
	[Header("表现")]
	public SpriteRenderer thisRenderer;

	public ShockParam shock;

	[Header("伤害")]
	public float damageRadius;

	public int damage;

	public float playerDamageRatio;

	public float damageDelay;

	public float knockback;

	private Vector2 berlinSeed;

	private Vector3 originModelLocalPosition;

	private Vector3 originModelScale;

	private Color originColor;

	private float explodeTimer;

	private bool exploded;

	public void Initialize()
	{
		SEMgr.Inst.monster16Charge.PlaySE();
		explodeTimer = 0f;
		exploded = false;
		berlinSeed = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
		originModelLocalPosition = Vector3.zero;
		originModelScale = Vector3.one;
		thisRenderer.enabled = true;
		originColor = thisRenderer.material.GetColor("_GlowColor");
		thisRenderer.material.SetColor("_GlowColor", new Color(originColor.r, originColor.g, originColor.b, 0f));
	}

	private void Update()
	{
		thisRenderer.material.SetColor("_GlowColor", new Color(originColor.r, originColor.g, originColor.b, explodeTimer / damageDelay));
		Vector2 vector = berlinSeed * explodeTimer * 32f;
		float x = Mathf.PerlinNoise(vector.x, vector.y) - 0.5f;
		float y = Mathf.PerlinNoise(vector.y, vector.x) - 0.5f;
		if (explodeTimer < damageDelay)
		{
			thisRenderer.transform.localPosition = originModelLocalPosition + new Vector3(x, y, 0f) * 0.5f * explodeTimer / damageDelay;
			thisRenderer.transform.localScale = originModelScale * Mathf.Lerp(1f, 1.2f, explodeTimer / damageDelay);
		}
		explodeTimer += Time.deltaTime;
		if (explodeTimer > damageDelay && !exploded)
		{
			CamController.Inst.SetShock(shock);
			thisRenderer.enabled = false;
			SEMgr.Inst.monster16Explode.PlaySE();
			exploded = true;
			ExplodeOnce(base.transform.position);
		}
	}

	private void ExplodeOnce(Vector3 explodePoint)
	{
		List<UnitDotsSyncSystem.DistanceHitResult> list = new List<UnitDotsSyncSystem.DistanceHitResult>();
		UnitDotsSyncSystem.GetCollidersInRange(explodePoint, damageRadius, GameConst.Filter_MonsterAoeUndiffer, list);
		for (int i = 0; i < list.Count; i++)
		{
			UnitDotsSyncSystem.DistanceHitResult distanceHitResult = list[i];
			Entity entity = distanceHitResult.entity;
			switch (UnitDotsSyncSystem.GetLayer(entity))
			{
			case 16777216u:
			{
				UnitDotsSyncSystem.ProcessHitSpell(entity, damage, out var _);
				break;
			}
			case 512u:
			case 32768u:
			case 131072u:
			case 2097152u:
			{
				if (UnitDotsSyncSystem.TryGetComponent<UnitProperty_Dots>(distanceHitResult.entity, out var _))
				{
					TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(AttackerType.NothingSpecial);
					info.damage = damage;
					info.playerTakeDamageRatio = playerDamageRatio;
					info.isUndifferDamage = true;
					info.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(distanceHitResult.point, explodePoint) * knockback;
					UnitDotsSyncSystem.AddTakeDamageRequest(distanceHitResult.entity, info);
				}
				break;
			}
			}
		}
	}
}
