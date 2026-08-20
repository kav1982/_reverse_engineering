using System;
using UnityEngine;

public class Boss5_Child : UnitBase
{
	public enum MonsterState
	{
		BornMove,
		Move,
		Die
	}

	public Boss5 master;

	public Vector3 moveDiration;

	[Header("状态机")]
	public MonsterState _state;

	private bool stateQuit;

	private bool changedState;

	private float stateExistTime;

	[Header("死亡爆子弹")]
	public VariableInt deadBulletCount;

	public VariableFloat bulletForwardSpeed;

	public VariableFloat bulletUpSpeed;

	public float bulletGravity;

	public int spellDamage;

	public float spellDuration;

	private SpellInitialParameter sipBullet = new SpellInitialParameter();

	public VariableFloat dieDelay;

	[Header("模式")]
	public AIPattern pattern;

	public float splitChance;

	public float knockWallForceKillTime;

	public float knockWallForceKillCounter;

	private Vector3 roomCenterPoint;

	private float roomWidth;

	private float roomHeight;

	public ShockParam knockShake;

	private bool forceKill;

	private bool dying;

	private Vector3 lastRecordPoint;

	private bool isFrame1;

	public AudioSource as_Tornado;

	public AudioSource as_TornadoWave;

	private SpellSpawnParams ssp;

	public MonsterState state
	{
		get
		{
			return _state;
		}
		set
		{
			stateExistTime = 0f;
			stateQuit = true;
			_state = value;
		}
	}

	private void OnEnable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Combine(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
		SoundVolumeChange();
	}

	private void OnDisable()
	{
		EventMgr.SoundVolumeChange = (Action)Delegate.Remove(EventMgr.SoundVolumeChange, new Action(SoundVolumeChange));
	}

	private void SoundVolumeChange()
	{
		as_Tornado.volume = DataMgr.settingData.GetFinalSound();
	}

	public override void SingleInitialCallback()
	{
		if (GameMgr.IsMobile_Static)
		{
			deadBulletCount.value1 = Mathf.CeilToInt((float)deadBulletCount.value1 * 0.66f);
			deadBulletCount.value2 = Mathf.CeilToInt((float)deadBulletCount.value2 * 0.66f);
			bulletForwardSpeed.value1 *= 0.8f;
			bulletForwardSpeed.value2 *= 0.8f;
		}
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90011);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Gravity = 0f - bulletGravity;
		sSPModifier.Damage = spellDamage;
		sSPModifier.ColorType = SpellColorType.Frozen;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
		ssp.ElementComponentData.FrozenDuration = 2f;
	}

	public override void EveryInitialCallback()
	{
		roomCenterPoint = LevelMgr.Inst.CurrentRoomCtrller.CenterPoint;
		roomWidth = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Width;
		roomHeight = LevelMgr.Inst.CurrentRoomCtrller.roomCfg.theme8Height;
		forceKill = false;
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = false;
		SetDotsCCEnable(isOpen: false);
		state = MonsterState.BornMove;
		dying = false;
		isFrame1 = true;
		knockWallForceKillCounter = 0f;
	}

	private FourDir CloseToWitchCliff()
	{
		if (base.transform.position.y + myPpt.CC_Self.radius - roomCenterPoint.y > roomHeight / 2f - 0.1f)
		{
			return FourDir.Up;
		}
		if (base.transform.position.y - myPpt.CC_Self.radius - roomCenterPoint.y < (0f - roomHeight) / 2f + 0.1f)
		{
			return FourDir.Down;
		}
		if (base.transform.position.x + myPpt.CC_Self.radius - roomCenterPoint.x > roomWidth / 2f - 0.1f)
		{
			return FourDir.Right;
		}
		return FourDir.Left;
	}

	public override void Update()
	{
		if (isFrame1)
		{
			isFrame1 = false;
			lastRecordPoint = base.transform.position;
		}
		else
		{
			WaterSystem.CreateWater(Tool2D.IgnoreZPoint(base.transform.position), Tool2D.IgnoreZPoint(lastRecordPoint), myPpt.CC_Self.radius);
			lastRecordPoint = base.transform.position;
		}
		base.Update();
		if (base.IsLocked)
		{
			return;
		}
		if (stateQuit)
		{
			stateQuit = false;
			changedState = true;
		}
		else
		{
			changedState = false;
		}
		stateExistTime += Time.deltaTime;
		switch (state)
		{
		case MonsterState.BornMove:
		{
			if (changedState)
			{
				base.Anima.Play("Boss5_Child_Spawn", 0, 0f);
				SEMgr.Inst.boss5_TornadoDie.PlaySE();
			}
			SetMove(moveDiration * base.MoveSpeed);
			Vector3 vector2 = base.transform.position - roomCenterPoint;
			if (Mathf.Abs(vector2.x) + myPpt.CC_Self.radius > roomWidth / 2f - 0.1f || Mathf.Abs(vector2.y) + myPpt.CC_Self.radius > roomHeight / 2f - 0.1f)
			{
				state = MonsterState.Move;
			}
			break;
		}
		case MonsterState.Move:
		{
			if (changedState)
			{
				base.CC_Self.enabled = true;
				SetDotsCCEnable(isOpen: true);
				UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
				componentData.CanTouch = true;
				SetDotsCCEnable(isOpen: true);
				base.Anima.Play("Boss5_Child_Move");
				if (!dying)
				{
					KnockWall();
				}
				Vector3 zero = Vector3.zero;
				FourDir fourDir = CloseToWitchCliff();
				if (fourDir == FourDir.Up || fourDir == FourDir.Down)
				{
					if (fourDir == FourDir.Up)
					{
						zero.y = -1f;
					}
					else
					{
						zero.y = 1f;
					}
					zero.x = Mathf.Sign(moveDiration.x);
				}
				else
				{
					if (fourDir == FourDir.Right)
					{
						zero.x = -1f;
					}
					else
					{
						zero.x = 1f;
					}
					zero.y = Mathf.Sign(moveDiration.y);
				}
				moveDiration = Tool2D.IgnoreZPoint(zero).normalized;
			}
			SetMove(moveDiration * base.MoveSpeed);
			Vector3 vector = base.transform.position - roomCenterPoint;
			if (Mathf.Abs(vector.x) + myPpt.CC_Self.radius > roomWidth / 2f - 0.1f || Mathf.Abs(vector.y) + myPpt.CC_Self.radius > roomHeight / 2f - 0.1f)
			{
				FourDir fourDir2 = CloseToWitchCliff();
				if ((fourDir2 != FourDir.Up || !(moveDiration.y < 0f)) && (fourDir2 != FourDir.Down || !(moveDiration.y > 0f)) && (fourDir2 != FourDir.Left || !(moveDiration.x > 0f)) && (fourDir2 != FourDir.Right || !(moveDiration.x < 0f)))
				{
					state = MonsterState.Move;
				}
			}
			break;
		}
		}
	}

	public void KnockWall()
	{
		if (pattern == AIPattern.Pattern1)
		{
			knockWallForceKillCounter += 1f;
			if (knockWallForceKillCounter >= knockWallForceKillTime)
			{
				ForceKill();
			}
			return;
		}
		CamController.Inst.SetShock(knockShake);
		if (master.enabled && UnityEngine.Random.Range(0f, 1f) < splitChance)
		{
			GetNearestTargetPlayerFirst();
			Boss5_Child component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + 500552, base.transform.position).GetComponent<Boss5_Child>();
			component.moveDiration = Tool2D.IgnoreZPoint(Tool2D.GetDir(moveDiration, (UnityEngine.Random.Range(0f, 1f) < 0.5f) ? 90 : (-90))).normalized;
			if (UnityEngine.Random.Range(0f, 1f) < 0.5f && base.HaveTarget)
			{
				component.moveDiration = ToTargetDir().normalized;
			}
			master.allChildren.Add(component);
		}
	}

	public void ForceKill()
	{
		if (!myPpt.AlreadyDead)
		{
			dying = true;
			forceKill = true;
			DotsAnnouncedDeath();
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		if (pattern == AIPattern.Pattern1)
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss5_TornadoDie", base.transform.position, 3f).GetComponent<Animator>().Play("Boss5_Child_Die");
		}
		else
		{
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss5_TornadoDieLarge", base.transform.position, 3f).GetComponent<Animator>().Play("Boss5_Child_Die");
		}
		SEMgr.Inst.boss5_WaveHit.PlaySE();
		if (forceKill)
		{
			int num = deadBulletCount.RandomResult();
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			for (int i = 0; i < num; i++)
			{
				sSPModifier.Speed = bulletForwardSpeed.RandomResult();
				sSPModifier.Direction = Tool2D.GetDir();
				sSPModifier.SpawnPosition = Tool2D.IgnoreZPoint(base.transform.position, -0.1f);
				sSPModifier.CurrentFallSpeed = 0f - bulletUpSpeed.RandomResult();
				sSPModifier.ApplyToSSP(ref ssp);
				ssp.ConfigComponentData.ColorType = SpellColorType.Frozen;
				ssp.ElementComponentData.FrozenDuration = 1.5f;
				ShootSpell(ssp);
			}
		}
		if (pattern == AIPattern.Pattern2 && !forceKill)
		{
			for (int j = 0; j < 2; j++)
			{
				Boss5_Child component = ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + 500552, base.transform.position).GetComponent<Boss5_Child>();
				component.moveDiration = Tool2D.IgnoreZPoint(Tool2D.GetDir(moveDiration, (j == 0) ? 90 : (-90))).normalized;
				master.allChildren.Add(component);
				component.master = master;
			}
		}
		base.AfterDead(ref info);
		base.transform.position = roomCenterPoint - new Vector3(0f, 0f, 2f);
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "SpawnFinish"))
		{
			if (animaName == "RealDeath")
			{
				DotsAnnouncedDeath();
			}
			return;
		}
		base.Anima.Play("Boss5_Child_Move");
		base.CC_Self.enabled = true;
		SetDotsCCEnable(isOpen: true);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = true;
		SetDotsCCEnable(isOpen: true);
	}
}
