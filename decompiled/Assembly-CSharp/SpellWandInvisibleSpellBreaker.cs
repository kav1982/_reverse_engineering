using UnityEngine;

public class SpellWandInvisibleSpellBreaker : SpellBase
{
	public BoxCollider hitBox;

	public float ZaxisHeight;

	public float hitBoxWidth;

	private Transform followingTransform;

	private int effectLevel;

	private bool canRebound;

	public void InitialHitBoxData(float boxLength, Transform followTransfrom, SpellInitialParameter targetSip, int spellLevel, float transRotate)
	{
		FinalInitialize(targetSip);
		base.CanBeCapture = false;
		ownerPpt = PlayerMgr.Inst.PlayerPpt;
		followingTransform = followTransfrom;
		hitBox.size = new Vector3(1f, hitBoxWidth, ZaxisHeight);
		hitBox.center = new Vector3(boxLength, 0f, hitBox.transform.localPosition.z - 0.3f);
		effectLevel = spellLevel;
		hitBox.transform.localEulerAngles = new Vector3(0f, 0f, transRotate);
	}

	public void HitBoxToggle(bool toggle)
	{
		canRebound = toggle;
	}

	public override void Update()
	{
		base.Update();
		if ((bool)followingTransform)
		{
			base.transform.position = Tool2D.IgnoreZPoint(followingTransform.position);
			base.transform.right = followingTransform.right;
		}
	}

	private void tryRecycleSpell(SpellBase _spellBase)
	{
		if (!_spellBase.IsSameCamp(ownerPpt.unitCfg.unitType) && _spellBase.spellCfg.abilityType != SpellAbilityType.Dash)
		{
			_spellBase.PoolRecycle();
		}
	}

	public override void TriggerIn(Collider other)
	{
		if (!canRebound)
		{
			return;
		}
		switch (other.tag)
		{
		case "Spell":
			TryReflectTargetSpell(other.GetComponentInParent<SpellBase>());
			break;
		case "RollBall":
		{
			Spell1002RollBall componentInParent2 = other.GetComponentInParent<Spell1002RollBall>();
			if (!IsSameCamp(componentInParent2))
			{
				componentInParent2.TakeDamage(100f);
				SpawnRecycleEffect(other.transform.position);
			}
			break;
		}
		case "Butterfly":
		{
			Spell1003Butterfly componentInParent = other.GetComponentInParent<Spell1003Butterfly>();
			if (!IsSameCamp(componentInParent))
			{
				componentInParent.Break();
				SpawnRecycleEffect(other.transform.position);
			}
			break;
		}
		}
	}

	private void SpawnRecycleEffect(Vector3 pos)
	{
		ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_LongWandClearSpell", pos, 0.5f);
	}

	private void TryReflectTargetSpell(SpellBase _spellBase)
	{
		if (IsSameCamp(_spellBase) || !_spellBase.CanChangeTeam)
		{
			return;
		}
		SpawnRecycleEffect(_spellBase.transform.position);
		if (effectLevel == 1 || !SpellConfig.dic.ContainsKey(_spellBase.spellCfg.id))
		{
			tryRecycleSpell(_spellBase);
			return;
		}
		UnitProperty unitProperty = _spellBase.ownerPpt;
		if (ownerPpt.CompareTag("Monster"))
		{
			_spellBase.ChangeTeamToMonster(ownerPpt);
		}
		else
		{
			if (!ownerPpt.CompareTag("Player") && !ownerPpt.CompareTag("Teammate"))
			{
				tryRecycleSpell(_spellBase);
				return;
			}
			_spellBase.ChangeTeamToPlayer();
		}
		float num = Mathf.Max(_spellBase.rigid.linearVelocity.magnitude, SpellConfig.dic[_spellBase.spellCfg.id].speed) * 1.2f;
		Vector3 normalized = (unitProperty.transform.position - _spellBase.transform.position).normalized;
		_spellBase.rigid.linearVelocity = normalized * num;
		_spellBase.Direction = _spellBase.rigid.linearVelocity.normalized;
	}
}
