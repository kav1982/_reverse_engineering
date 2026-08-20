using UnityEngine;

public class Elite15_Child : UnitBase
{
	[Space(50f)]
	public Elite15Child_Tentacle pfb_Tentacle;

	public Transform tsf_Motion;

	public int tentacleCount;

	public float deadSpellCount;

	public float spellHeight;

	[Header("Explosion")]
	public float explosionDelayTime;

	private Elite15Child_Tentacle[] tentacles;

	private SpellInitialParameter sip = new SpellInitialParameter();

	private Elite15 elite15;

	private bool willExlosion;

	private float explosionDelayTimer;

	private bool isDeadNow;

	private SpellSpawnParams ssp;

	public override void SingleInitialCallback()
	{
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90261);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.ApplyToSSP(ref ssp);
		tentacles = new Elite15Child_Tentacle[tentacleCount];
		for (int i = 0; i < tentacles.Length; i++)
		{
			tentacles[i] = Object.Instantiate(pfb_Tentacle, tsf_Motion);
			tentacles[i].SingleInitial(this);
		}
	}

	public override void EveryInitialCallback()
	{
		base.CC_Self.enabled = false;
		SetDotsCCEnable(isOpen: false);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.CanTouch = false;
		SetComponentData(componentData);
		isDeadNow = false;
	}

	public override void Update()
	{
		base.Update();
		if (!base.IsLocked && willExlosion)
		{
			explosionDelayTimer += Time.deltaTime;
			if (explosionDelayTimer >= explosionDelayTime)
			{
				explosionDelayTimer = 0f;
				DotsAnnouncedDeath();
			}
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (animaName == "BornFinish")
		{
			base.CC_Self.enabled = true;
			SetDotsCCEnable(isOpen: true);
			UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
			componentData.CanTouch = true;
			SetComponentData(componentData);
			if (willExlosion)
			{
				base.Anima.Play("Explosion", 0, 0f);
			}
		}
		else
		{
			Debug.LogError(animaName);
		}
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		if (!isDeadNow)
		{
			UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
			for (int i = 0; (float)i < deadSpellCount; i++)
			{
				sSPModifier.Direction = Tool2D.GetDir(360f / deadSpellCount * (float)i);
				sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - spellHeight);
				sSPModifier.ApplyToSSP(ref ssp);
				ShootSpell(ssp);
			}
		}
		elite15.ChildUnregister(this);
	}

	public void SetMother(Elite15 elite15)
	{
		this.elite15 = elite15;
		willExlosion = false;
		base.Anima.Play("Idle", 0, 0f);
	}

	public void SetExlosion()
	{
		willExlosion = true;
		explosionDelayTimer = 0f;
	}

	public void DeadNow()
	{
		isDeadNow = true;
		DotsAnnouncedDeath();
	}
}
