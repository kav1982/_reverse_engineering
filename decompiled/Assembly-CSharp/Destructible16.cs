using Unity.Collections;
using UnityEngine;

public class Destructible16 : UnitBase, ITrap
{
	private enum UnitState
	{
		Idle,
		Shooting,
		ShootWait
	}

	[Space(50f)]
	public float shootHeight;

	public int shootCount;

	public float shootFinishWaitTime;

	[Header("Spell")]
	public float spellSpeed;

	public float spellDuration;

	public int spellDamage;

	[Header("Dead")]
	public MeshRenderer mr_1;

	public MeshRenderer mr_2;

	public Sprite sprite_SR2Normal;

	public Sprite sprite_SR2Dead;

	private UnitState state;

	private bool isStop;

	private float shootFinishWaitTimer;

	private SpellInitialParameter sip = new SpellInitialParameter();

	private SpellSpawnParams ssp;

	public override void SingleInitialCallback()
	{
		ssp = UnitDotsSyncSystem.GetSpellPrototype(90151);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.Shooter = myPpt.myEntity;
		sSPModifier.Damage = spellDamage;
		sSPModifier.Duration = spellDuration;
		sSPModifier.Speed = spellSpeed;
		sSPModifier.ApplyToSSP(ref ssp);
		ssp.ElementComponentData.FrozenDuration = 2f;
		ssp.ConfigComponentData.ShooterType = UnitType.NotAttack;
	}

	public override void EveryInitialCallback()
	{
		state = UnitState.Idle;
		isStop = false;
		base.Anima.Play("Idle");
		mr_1.gameObject.SetActive(value: true);
		mr_2.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_SR2Normal.texture);
		UnitProperty_Dots componentData = GetComponentData<UnitProperty_Dots>();
		componentData.InvincibleRegister();
		SetComponentData(componentData);
	}

	public override void Update()
	{
		base.Update();
		switch (state)
		{
		case UnitState.ShootWait:
			shootFinishWaitTimer += Time.deltaTime;
			if (shootFinishWaitTimer >= shootFinishWaitTime)
			{
				shootFinishWaitTimer = 0f;
				state = UnitState.Idle;
			}
			break;
		default:
			Debug.LogError(state);
			break;
		case UnitState.Idle:
		case UnitState.Shooting:
			break;
		}
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		if (info.isUndifferDamage)
		{
			SetTrapInvalid();
		}
		if (!isStop && state == UnitState.Idle)
		{
			state = UnitState.Shooting;
			base.Anima.Play("Shoot");
		}
	}

	public override void AnimaAction(string animaName)
	{
		if (!(animaName == "Shoot"))
		{
			if (animaName == "ShootFinish")
			{
				base.Anima.Play("Idle");
				state = UnitState.ShootWait;
			}
			else
			{
				Debug.LogError(animaName);
			}
			return;
		}
		float num = Random.Range(0f, 360f);
		UnitSpellModifier sSPModifier = UnitBase.GetSSPModifier(in ssp);
		sSPModifier.SpawnPosition = base.transform.position + new Vector3(0f, 0f, 0f - shootHeight);
		for (int i = 0; i < shootCount; i++)
		{
			sSPModifier.Direction = Tool2D.GetDir((float)(360 / shootCount * i) + num);
			sSPModifier.ApplyToSSP(ref ssp);
			ShootSpell(ssp);
		}
	}

	public void SetTrapInvalid()
	{
		if (!isStop)
		{
			isStop = true;
			mr_1.gameObject.SetActive(value: false);
			mr_2.material.SetTexture(GameConstManaged.shaderBaseMapIndex, sprite_SR2Dead.texture);
			myPpt.unitCfg.deadSEs.Value.ToArray()[0].PlaySE();
			ObjPoolMgr inst = ObjPoolMgr.Inst;
			FixedString128Bytes deadEF = myPpt.unitCfg.deadEF;
			inst.GetGO("Prefabs/EF/" + deadEF.ToString(), base.transform.position, 2f);
		}
	}
}
