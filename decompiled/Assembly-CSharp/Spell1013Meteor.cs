using System;
using System.Collections.Generic;
using UnityEngine;

public class Spell1013Meteor : SpellBase
{
	[Space(50f)]
	public VariableFloat horizontalOffset;

	public float height;

	public ShockParam shockParam;

	public AudioSource as_Duration;

	private static HashSet<string> explosionAttackTags = new HashSet<string> { "Monster", "Destructible", "Player", "Teammate", "SolidObj", "Spell", "RollBall", "Butterfly", "Brittleness" };

	private static HashSet<string> hoverAttackTags = new HashSet<string> { "Player", "Teammate", "Monster", "RollBall", "Butterfly" };

	private static Collider[] colliderBuffer = new Collider[256];

	private float realHeight;

	private int subMeteorSerialNumber;

	public float tinyMeteorDamageRatio;

	public float tinyMeteorSizeRatio;

	public float tinyMeteorShakeStrengh;

	public float tinyMeteorHeightShift;

	public VariableFloat tinyMeteorLandPointRandomness;

	public float tinyMeteorLandPointMinDistanceRatio;

	private Vector3 _landPoint = Vector3.zero;

	private Vector3 landingDir;

	public float mouseLerpSpeedRatio;

	private Vector3 velocity;

	public float minHoverHeight;

	public float hoverCheckRadiu;

	public float hoverCheckInterval;

	private float hoverCheckTimer;

	private Vector3 hoverStopPoint = Vector3.zero;

	public float gravity;

	private bool rebounceStart;

	public float distanceBetweenCalculateRatio;

	private bool hoverStart;

	public float rebounceFollowMouseLerpRatio;

	public float chaseTargetSpeedRatio;

	private bool IsSubMeteor => base.SIP.tags.Contains(SpellTag.SubMeteor);

	public override void InitializeCallback()
	{
		_landPoint = Vector3.zero;
		hoverStopPoint = Vector3.zero;
		rebounceStart = false;
		hoverStart = false;
		hoverCheckTimer = 0f;
		if (base.SIP.spellIsFall)
		{
			base.spellCfg.radius = GetFallingGroundDamageRadius();
		}
		realHeight = height;
		if (IsSubMeteor)
		{
			realHeight += (subMeteorSerialNumber + 1) * 2;
		}
		InitByShootSpatialInfo();
	}

	public override void OnFirstFrame()
	{
		base.OnFirstFrame();
		if (!base.spellCfg.isSplitSpell && !IsSubMeteor)
		{
			CreateSubMeteors();
		}
	}

	public override void OnEnable()
	{
		base.OnEnable();
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		tsf_Layer.localPosition = new Vector3(0f, height, 0f);
		SoundVolumeChange();
	}

	public void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	public override void Update()
	{
		base.Update();
		if (base.transform.position.z >= 0f && (base.SpellHoverTimer == 0f || base.SpellHoverTimer >= base.SpellHoverTime))
		{
			ExplosionEffect();
			GameObject[] hitTarget = ExplosionDamage();
			if (lastOutputDamageFrame != Time.frameCount || !TrySpecial1013Refract(hitTarget))
			{
				if (base.rebounceTime <= 0 && base.SpellHoverTimer >= base.SpellHoverTime)
				{
					base.isFlyFinish = true;
					PoolRecycle();
				}
				else
				{
					base.transform.position = Tool2D.IgnoreZPoint(base.transform.position, -0.1f);
					velocity = new Vector3(velocity.x, velocity.y, 0f - velocity.z);
					rebounceStart = true;
					base.rebounceTime--;
				}
			}
		}
		if (base.rebounceTime == 0 && velocity.z > 0f)
		{
			base.rebounceTime = -1;
		}
		if (base.rebounceTime < 0 && base.SpellHoverTimer <= base.SpellHoverTime && base.transform.position.z >= minHoverHeight)
		{
			base.SpellHoverTimer += Time.deltaTime;
			hoverCheckTimer += Time.deltaTime;
			hoverStart = true;
			if (!(hoverCheckTimer >= hoverCheckInterval))
			{
				return;
			}
			hoverCheckTimer -= hoverCheckInterval;
			bool flag = false;
			int collidersNonAlloc = GeneralTool.GetCollidersNonAlloc(base.transform.position, hoverCheckRadiu, colliderBuffer, hoverAttackTags);
			for (int i = 0; i < collidersNonAlloc; i++)
			{
				if (colliderBuffer[i].CompareTag("Butterfly"))
				{
					((Spell1003Butterfly)colliderBuffer[i].GetComponentInParent<SpellBase>()).HitEFAndRecycle();
				}
				else
				{
					flag = true;
				}
			}
			if (flag)
			{
				base.SpellHoverTimer = base.SpellHoverTime;
			}
		}
		else
		{
			if (rebounceStart)
			{
				velocity += new Vector3(0f, 0f, 1f) * (gravity * Time.deltaTime);
			}
			if (!hoverStart || base.SpellHoverTimer >= base.SpellHoverTime)
			{
				base.transform.position += velocity * Time.deltaTime;
			}
		}
	}

	private bool TrySpecial1013Refract(params GameObject[] hitTarget)
	{
		if (base.currentSpellMovement == SpellSpecialMovementType.Rotation)
		{
			if (base.remainRefractCount <= 0)
			{
				return false;
			}
			base.remainRefractCount--;
			velocity.z *= -1f;
			rebounceStart = true;
			return true;
		}
		UnitProperty unitProperty = TryRefract(hitTarget);
		if ((bool)unitProperty)
		{
			base.transform.position = Tool2D.IgnoreZPoint(base.transform.position, -0.1f);
			float num = Tool2D.IgnoreZDistance(unitProperty.transform.position, base.transform.position);
			float num2 = velocity.magnitude * 2f / base.CurrentSpeed * num;
			float z = velocity.z;
			velocity = num2 * base.Direction;
			velocity.z = 0f - z;
			rebounceStart = true;
		}
		return unitProperty;
	}

	private void SoundVolumeChange()
	{
		as_Duration.volume = DataMgr.settingData.GetFinalSound();
	}

	private void CreateSubMeteors()
	{
		int num = (GeneralTool.IsLowFpsOptimizeActive(40f) ? Mathf.FloorToInt((float)base.spellCfg.int1 * GameMgr.Inst.GetFps() / 40f) : base.spellCfg.int1);
		float num2 = (float)base.spellCfg.int1 / (float)num;
		for (int i = 0; i < num; i++)
		{
			SpellConfig spellConfig = SpellConfig.dic[base.spellCfg.id];
			Spell1013Meteor spell1013Meteor = (Spell1013Meteor)ObjPoolMgr.Inst.GetGO("Prefabs/Spell/" + spellConfig.prefab, base.transform.position).GetComponent<SpellBase>();
			SpellInitialParameter spellInitialParameter = base.InitialParameter.Copy();
			spellInitialParameter.finalDamageRatio *= tinyMeteorDamageRatio * num2;
			spellInitialParameter.finalSizeRatio *= tinyMeteorSizeRatio;
			spellInitialParameter.tags.Add(SpellTag.SubMeteor);
			spell1013Meteor.subMeteorSerialNumber = i;
			ShootSpellSpatialInfo finalShootSpatialInfo = spellInitialParameter.finalShootSpatialInfo;
			if (finalShootSpatialInfo != null && finalShootSpatialInfo.Target.HasValue)
			{
				ShootSpellSpatialInfo finalShootSpatialInfo2 = spellInitialParameter.finalShootSpatialInfo;
				finalShootSpatialInfo2.Target += Tool2D.GetDir() * (base.currentLevelPureCfg.radius * tinyMeteorLandPointMinDistanceRatio) * tinyMeteorLandPointRandomness.RandomResult();
			}
			spell1013Meteor.Initialize(spellInitialParameter);
			spell1013Meteor.wandChargeData = base.wandChargeData;
			if (spell1013Meteor.enableAroundPlayer)
			{
				float degree = UnityEngine.Random.Range(0f, 360f);
				spell1013Meteor.transform.position += Tool2D.GetDir(degree) * spell1013Meteor.spellAroundOwnerRadius;
				spell1013Meteor.spellAroundOwnerCurrentAngle = degree;
			}
			if (base.OwnerSpell == null)
			{
				spell1013Meteor.OwnerTsf = ownerPpt.transform;
			}
			else if (base.OwnerPoint != Vector3.zero)
			{
				spell1013Meteor.OwnerTsf = base.OwnerSpell.transform;
			}
			spell1013Meteor.OwnerTsf = base.OwnerTsf;
			spell1013Meteor.OwnerPoint = base.OwnerPoint;
			spell1013Meteor.undifferDamageRatio = base.undifferDamageRatio;
			spell1013Meteor.indirectShootByPlayer = true;
			spell1013Meteor.shouldCameraShock = false;
			spell1013Meteor.spellSplitCount = 0;
			spell1013Meteor.realHeight += (float)(i + 2) * tinyMeteorHeightShift;
			spell1013Meteor.landingDir = landingDir;
		}
	}

	private void InitByShootSpatialInfo()
	{
		ShootSpellSpatialInfo finalShootSpatialInfo = base.SIP.finalShootSpatialInfo;
		if (finalShootSpatialInfo == null || !finalShootSpatialInfo.Target.HasValue)
		{
			Debug.LogError("没有攻击点的信息");
			return;
		}
		_landPoint = base.SIP.finalShootSpatialInfo.Target.Value;
		Vector3 vector = -base.Direction;
		float num = Mathf.Clamp(Tool2D.IgnoreZDistance(_landPoint, GetAroundTargetBasePoint()) * distanceBetweenCalculateRatio, horizontalOffset.value1, horizontalOffset.value2);
		base.transform.position = _landPoint + vector * num + new Vector3(0f, 0f, 0f - realHeight);
		landingDir = (vector * num + new Vector3(0f, 0f, 0f - realHeight)) / realHeight;
		velocity = (_landPoint - base.transform.position) / (Vector3.Distance(base.transform.position, _landPoint) / base.spellCfg.speed);
	}

	public override void SpellAroundPlayer()
	{
		if (base.rebounceTime <= 0 && base.SpellHoverTimer < base.SpellHoverTime && base.transform.position.z >= minHoverHeight)
		{
			if (hoverStopPoint == Vector3.zero)
			{
				hoverStopPoint = Tool2D.IgnoreZPoint(GetAroundTargetBasePoint());
			}
		}
		else
		{
			base.spellAroundOwnerCurrentAngle += 360f / (MathF.PI * 2f * base.spellAroundOwnerRadius / base.CurrentSpeed) * Time.deltaTime;
			Vector3 vector = ((hoverStopPoint != Vector3.zero) ? (hoverStopPoint + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius) : (GetAroundTargetBasePoint() + Tool2D.GetDir(base.spellAroundOwnerCurrentAngle) * base.spellAroundOwnerRadius));
			base.Direction = ToPointDir(vector);
			base.transform.position = Tool2D.IgnoreZPoint(vector, base.transform.position.z);
		}
	}

	private void ExplosionEffect()
	{
		PlaySE("Explosion");
		EffectBase.CreateSpriteEffect("Explosion");
		if (!IsSubMeteor)
		{
			CamController.Inst.SetShock(this.shockParam);
		}
		else
		{
			ShockParam shockParam = this.shockParam;
			shockParam.speed *= tinyMeteorShakeStrengh;
			shockParam.radius *= tinyMeteorShakeStrengh;
			CamController.Inst.SetShock(shockParam);
		}
		if (!SpellEffectBase.FullTransparency)
		{
			float size = base.spellCfg.radius * 2f;
			string typePostfix = null;
			SpellColorType colorType = base.ColorType;
			if (colorType == SpellColorType.Fire || colorType == SpellColorType.Thunder)
			{
				typePostfix = base.ColorType.ToString();
			}
			float recycleTime = (GeneralTool.IsLowFpsOptimizeActive(60f) ? (5f * GameMgr.Inst.GetFps() / 60f) : 5f);
			if (GeneralTool.IsLowFpsOptimizeActive(10f) || UnityEngine.Random.Range(0f, 1f) <= 0.2f)
			{
				SpellTraceTools.CreateTrace(10131, base.transform.position, size, typePostfix, 0.07f, recycleTime);
			}
		}
	}

	private GameObject[] ExplosionDamage()
	{
		List<GameObject> list = new List<GameObject>();
		int collidersNonAlloc = GeneralTool.GetCollidersNonAlloc(base.transform.position, base.spellCfg.radius, colliderBuffer, explosionAttackTags);
		for (int i = 0; i < collidersNonAlloc; i++)
		{
			Collider collider = colliderBuffer[i];
			switch (collider.tag)
			{
			case "Spell":
			case "RollBall":
			case "Butterfly":
			{
				if (!collider.gameObject.activeInHierarchy)
				{
					break;
				}
				SpellBase componentInParent = collider.GetComponentInParent<SpellBase>();
				if (componentInParent.spellCfg.abilityType == SpellAbilityType.FireBall)
				{
					if (collider.transform.parent != base.transform)
					{
						Spell1012TrickMine componentInParent2 = collider.GetComponentInParent<Spell1012TrickMine>();
						if (componentInParent2 != null)
						{
							componentInParent2.Detonated();
						}
					}
				}
				else if (componentInParent.spellCfg.abilityType == SpellAbilityType.Meteor)
				{
					Spell1013Meteor componentInParent3 = collider.GetComponentInParent<Spell1013Meteor>();
					if (componentInParent3 != null)
					{
						componentInParent3.SpellHoverTimer = componentInParent3.SpellHoverTime;
					}
				}
				else if (componentInParent.spellCfg.abilityType == SpellAbilityType.Rollball)
				{
					((Spell1002RollBall)componentInParent).TakeDamage(base.spellCfg.damage);
				}
				else if (componentInParent.spellCfg.abilityType == SpellAbilityType.Butterfly)
				{
					((Spell1003Butterfly)componentInParent).HitEFAndRecycle();
				}
				break;
			}
			default:
			{
				TakeDamageInfo takeDamageInfo = new TakeDamageInfo();
				takeDamageInfo.canRebound = false;
				takeDamageInfo.knockbackForce = Tool2D.IgnoreZV2ToV1Normal(collider.transform, base.transform) * base.spellCfg.knockback;
				takeDamageInfo.isUndifferDamage = true;
				OutputDamage(collider.gameObject, takeDamageInfo);
				list.Add(collider.gameObject);
				break;
			}
			}
		}
		if (spellMucusTime > 0f)
		{
			LevelMgr.Inst.CurrentRoomCtrller.mucusCtrller.CreateMucus(Tool2D.IgnoreZPoint(base.transform), base.spellCfg.radius);
		}
		if (spellVenomTime > 0f)
		{
			LevelMgr.Inst.CurrentRoomCtrller.venomCtrller.CreateVenom(Tool2D.IgnoreZPoint(base.transform), base.spellCfg.radius, spellVenomTime * 2f);
		}
		if (spellFrozenTime > 0f)
		{
			LevelMgr.Inst.CurrentRoomCtrller.waterCtrller.CreateWater(Tool2D.IgnoreZPoint(base.transform), base.spellCfg.radius);
		}
		return list.ToArray();
	}

	protected override void SpellFollowMouse()
	{
		if (base.rebounceTime <= 0 && base.SpellHoverTimer < base.SpellHoverTime && base.transform.position.z >= minHoverHeight)
		{
			return;
		}
		Vector3 mousePoint = PlayerMgr.Inst.GetMousePoint(base.transform.position.z);
		if (!rebounceStart)
		{
			float num = base.CurrentSpeed * Time.deltaTime * spellFollowMouseLerp;
			float num2 = Tool2D.IgnoreZDistance(mousePoint, base.transform.position);
			if (num2 < 3f)
			{
				num *= 4f - num2;
			}
			if (rebounceStart)
			{
				num *= rebounceFollowMouseLerpRatio;
			}
			base.Direction = (mousePoint - base.transform.position).normalized;
			base.transform.position = Vector3.Lerp(base.transform.position, mousePoint, num * mouseLerpSpeedRatio * 2f);
		}
		else
		{
			velocity = Tool2D.IgnoreZPoint(Vector3.Lerp(velocity, ToPointDir(mousePoint) * base.CurrentSpeed, base.CurrentSpeed * Time.deltaTime * spellFollowMouseLerp * mouseLerpSpeedRatio)) + new Vector3(0f, 0f, velocity.z);
			base.Direction = velocity.normalized;
		}
	}

	protected override void SpellFollowTarget()
	{
		if (base.rebounceTime > 0 || !(base.SpellHoverTimer < base.SpellHoverTime) || !(base.transform.position.z >= minHoverHeight))
		{
			if (!base.SpellFollowHaveTarget)
			{
				spellFollowTargetPpt = GetMiniMalAngleTargetablePpt();
			}
			if (base.SpellFollowHaveTarget)
			{
				base.Direction = Tool2D.DirMoveTowards(base.Direction, ToPointDir(spellFollowTargetPpt.transform), base.CurrentSpeed * spellFollowTargetRotateSpeed * Time.deltaTime * chaseTargetSpeedRatio);
				base.transform.position += base.Direction * base.CurrentSpeed * Time.deltaTime;
			}
		}
	}

	protected override void SpellFollowOwner()
	{
		if (base.rebounceTime > 0 || !(base.SpellHoverTimer < base.SpellHoverTime) || !(base.transform.position.z >= minHoverHeight))
		{
			Transform targetT = ((base.OwnerTsf != null) ? base.OwnerTsf : ownerPpt.transform);
			base.Direction = Tool2D.DirMoveTowards(base.Direction, ToPointDir(targetT), base.CurrentSpeed * spellFollowTargetRotateSpeed * Time.deltaTime * chaseTargetSpeedRatio);
			base.transform.position += base.Direction * base.CurrentSpeed * Time.deltaTime;
		}
	}

	public void SkipHover()
	{
		base.SpellHoverTimer = base.SpellHoverTime;
	}

	protected override SpellBase CreateSplitSpell(SpellConfig splitCfg, float baseAngle, int index)
	{
		Spell1013Meteor spell1013Meteor = (Spell1013Meteor)base.CreateSplitSpell(splitCfg, baseAngle, index);
		spell1013Meteor.gameObject.transform.position = Tool2D.IgnoreZPoint(base.transform.position) + new Vector3(0f, 0f, 0f - minHoverHeight - 1f);
		spell1013Meteor.OwnerTsf = base.OwnerTsf;
		spell1013Meteor.velocity = new Vector3(0f - spell1013Meteor.velocity.x, 0f - spell1013Meteor.velocity.y, 0f - velocity.z);
		spell1013Meteor.rebounceStart = true;
		return spell1013Meteor;
	}

	protected override IEnumerable<SpellBase> TrySplit()
	{
		if (!IsSubMeteor)
		{
			return base.TrySplit();
		}
		return Array.Empty<SpellBase>();
	}

	public override void TriggerIn(Collider other)
	{
	}

	protected override void InitSpeedAndPosition()
	{
		float fallExplosionRadius = base.SIP.fallExplosionRadius;
		base.SIP.fallExplosionRadius = 0f;
		base.InitSpeedAndPosition();
		base.SIP.fallExplosionRadius = fallExplosionRadius;
	}
}
