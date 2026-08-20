using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public class Monster22 : UnitBase
{
	[Space(50f)]
	public Shadow shadow;

	public int recoverHPPerSencond;

	public float minScale;

	public float attackInterval;

	public Transform tsf_Scale;

	public AIPattern pattern;

	[Header("Pattern2")]
	public MeshRenderer mr2;

	public int summonID;

	public int summonMaxCount;

	public float summonFlyForce;

	public GameObject pfb_Connection;

	public int connectionNodeCount;

	public float connectionHeight;

	public float connectionMiddleHeight;

	[Header("Spell")]
	public float spellHeight;

	public float spellSpeed;

	public float spellVerticalSpeed;

	public float spellDuration;

	public int spellDamage;

	[Header("Invincible")]
	public bool canInvincible;

	public float checkInvincibleInterval;

	public float invincibleDistance;

	public ParticleSystem ps_Vincible;

	public MeshRenderer mr1;

	public Sprite sprite_SR1_Normal;

	public Sprite sprite_SR1_Invincible;

	public Sprite sprite_SR2_Normal;

	public Sprite sprite_SR2_Invincible;

	private float finalSummonMaxCount;

	private float shadowInitialScale;

	private float originalColliderRadius;

	private float recoverHPTimer;

	private float attackIntervalTimer = 999f;

	private List<Monster21> summons = new List<Monster21>();

	private List<LineRenderer> connections = new List<LineRenderer>();

	private int summonCounter;

	private float checkInvincibleIntervalTimer;

	private float originalFrozenTimeRatio;

	private bool isInvincible;

	private SpellSpawnParams ssp;

	private bool behit;

	public override void SingleInitialCallback()
	{
		shadow.CreateShadow();
		shadowInitialScale = shadow.ShadowGO.transform.localScale.x;
		originalColliderRadius = base.CC_Self.radius;
		if (GameMgr.IsMobile_Static)
		{
			spellSpeed *= 0.8f;
			spellVerticalSpeed *= 0.8f;
			if (pattern == AIPattern.Pattern1)
			{
				attackInterval *= 1.5f;
			}
		}
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90241);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Damage = spellDamage;
		sSPModifier.Speed = Mathf.Sqrt(spellSpeed * spellSpeed + spellVerticalSpeed * spellVerticalSpeed);
		sSPModifier.ApplyToSSP(ref ssp);
		originalFrozenTimeRatio = myPpt.unitCfg.frozenTimeRatio;
		finalSummonMaxCount = (GameMgr.IsMobile_Static ? 0.6f : 1f) * (float)summonMaxCount;
	}

	public override void EveryInitialCallback()
	{
		recoverHPTimer = 0f;
		attackIntervalTimer = 999f;
		CorrectScale();
		if (pattern == AIPattern.Pattern2)
		{
			mr2.gameObject.SetActive(value: true);
			summons.Clear();
			connections.Clear();
			summonCounter = 0;
		}
		if (canInvincible)
		{
			checkInvincibleIntervalTimer = 0f;
			mr1.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_SR1_Normal.texture);
			mr2.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_SR2_Normal.texture);
			base.gameObject.tag = "Monster";
			base.gameObject.layer = LayerMask.NameToLayer("Monster_Ghost");
			SetDotsLayer(8192u);
			isInvincible = false;
		}
	}

	public override void Update()
	{
		if (pattern == AIPattern.Pattern2)
		{
			CorrectConnection();
		}
		attackIntervalTimer += Time.deltaTime;
		CorrectScale();
		if (canInvincible)
		{
			checkInvincibleIntervalTimer += Time.deltaTime;
			if (checkInvincibleIntervalTimer >= checkInvincibleInterval)
			{
				checkInvincibleIntervalTimer = 0f;
				bool flag = false;
				if (Tool2D.IgnoreZDistanceSqr(base.transform.position, PlayerMgr.Inst.PlayerPoint) < invincibleDistance * invincibleDistance)
				{
					flag = true;
				}
				bool flag2 = false;
				if (!flag && UnitDotsSyncSystem.HaveCollider(base.transform.position, invincibleDistance, GameConst.Filter_Friendly))
				{
					flag2 = true;
				}
				if (flag2 || flag)
				{
					if (isInvincible)
					{
						isInvincible = false;
						mr1.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_SR1_Normal.texture);
						mr2.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_SR2_Normal.texture);
						ps_Vincible.Play();
						UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
						componentData.unitCfg.frozenTimeRatio = originalFrozenTimeRatio;
						componentData.InvincibleUnregister();
						SetComponentData(componentData);
						base.gameObject.tag = "Monster";
						base.gameObject.layer = LayerMask.NameToLayer("Monster");
						SetDotsLayer(2048u);
					}
				}
				else if (!isInvincible)
				{
					isInvincible = true;
					mr1.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_SR1_Invincible.texture);
					mr2.material.SetTexture(GameConstManaged.shaderTextureIndex, sprite_SR2_Invincible.texture);
					UnitProperty_Dots componentData2 = GetComponentData<UnitProperty_Dots>();
					componentData2.unitCfg.frozenTimeRatio = 0f;
					componentData2.InvincibleRegister();
					SetComponentData(componentData2);
					base.gameObject.tag = "Untagged";
					base.gameObject.layer = LayerMask.NameToLayer("Item");
					SetDotsLayer(262144u);
				}
			}
		}
		base.Update();
		if (!base.IsLocked)
		{
			recoverHPTimer += Time.deltaTime;
			if (recoverHPTimer >= 1f)
			{
				recoverHPTimer = 0f;
				UnitDotsSyncSystem.UnitRecoveryHP(myPpt.myEntity, recoverHPPerSencond, World.DefaultGameObjectInjectionWorld.EntityManager);
			}
		}
	}

	public override void AfterTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (attackIntervalTimer >= attackInterval)
		{
			attackIntervalTimer = 0f;
			switch (pattern)
			{
			case AIPattern.Pattern1:
			{
				base.Anima.SetTrigger("Action");
				bool flag = ((Random.Range(0, 2) == 0) ? true : false);
				Vector3 dir = Tool2D.GetDir();
				UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
				sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				sSPModifier.Float1 = spellVerticalSpeed / sSPModifier.Speed * (float)(flag ? 1 : (-1));
				sSPModifier.Float2 = spellSpeed / sSPModifier.Speed;
				sSPModifier.Direction = dir;
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
				sSPModifier.Direction = -dir;
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
				break;
			}
			case AIPattern.Pattern2:
				behit = true;
				break;
			default:
				Debug.LogError(pattern);
				break;
			}
		}
	}

	public void LateUpdate()
	{
		if (behit && (float)summonCounter < finalSummonMaxCount)
		{
			summonCounter++;
			if ((float)summonCounter >= finalSummonMaxCount)
			{
				mr2.gameObject.SetActive(value: false);
			}
			base.Anima.SetTrigger("Action");
			Monster21 component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + summonID, base.transform.position).GetComponent<Monster21>();
			component.SummonFly(Tool2D.GetDir() * summonFlyForce);
			summons.Add(component);
			LineRenderer component2 = Object.Instantiate(pfb_Connection, LevelMgr.Inst.CurrentRoomT).GetComponent<LineRenderer>();
			connections.Add(component2);
			component2.positionCount = connectionNodeCount;
			CorrectConnection();
		}
		behit = false;
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		for (int num = summons.Count - 1; num >= 0; num--)
		{
			summons[num].DotsAnnouncedDeath();
		}
		for (int num2 = connections.Count - 1; num2 >= 0; num2--)
		{
			Object.Destroy(connections[num2].gameObject);
		}
	}

	private unsafe void CorrectScale()
	{
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		float num = Mathf.Lerp(minScale, 1f, componentData.unitCfg.currentHP / componentData.unitCfg.maxHP);
		tsf_Scale.localScale = Vector3.one * num;
		base.CC_Self.radius = originalColliderRadius * num;
		PhysicsCollider componentData2 = GetComponentData<PhysicsCollider>();
		Unity.Physics.CapsuleCollider* colliderPtr = (Unity.Physics.CapsuleCollider*)componentData2.ColliderPtr;
		CapsuleGeometry geometry = colliderPtr->Geometry;
		geometry.Radius = myPpt.CC_Self.radius;
		colliderPtr->Geometry = geometry;
		SetComponentData(componentData2);
		shadow.ShadowGO.transform.localScale = Vector3.one * shadowInitialScale * num;
	}

	private void CorrectConnection()
	{
		for (int num = summons.Count - 1; num >= 0; num--)
		{
			if ((summons[num] != null) & summons[num].gameObject.activeSelf)
			{
				Vector3 vector = base.transform.position + new Vector3(0f, 0f, 0f - connectionHeight);
				Vector3 vector2 = summons[num].transform.position + new Vector3(0f, 0f, 0f - connectionHeight);
				Vector3 v = (vector + vector2) / 2f + new Vector3(0f, 0f, 0f - connectionMiddleHeight);
				for (int i = 0; i < connectionNodeCount; i++)
				{
					Vector3 rootPoint = GeneralTool.QuadraticBezierCurve(vector, v, vector2, (float)i / ((float)connectionNodeCount - 1f));
					connections[num].SetPosition(i, Tool2D.GetLayerPoint(rootPoint));
				}
			}
			else
			{
				summons.RemoveAt(num);
				Object.Destroy(connections[num].gameObject);
				connections.RemoveAt(num);
			}
		}
	}
}
