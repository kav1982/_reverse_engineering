using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class SpellTriggerController : ISpellTriggerController
{
	private readonly SpellBase _ownerSpell;

	private WandConfig _wandCfg;

	private SlotData[] _triggers;

	private SpellShootGroup _subGroup;

	private float _subGroupMpCost;

	private float _moveDisCounter;

	private float _moveTriggerNeedDis;

	private static bool _moveTriggerDirFlag;

	private bool _localMoveTriggerDirFlag;

	private SlotData _lastMoveTrigger;

	private readonly Dictionary<SlotData, float> _onHitTriggerCD = new Dictionary<SlotData, float>();

	private readonly List<Vector3> _hitTargets = new List<Vector3>();

	[CanBeNull]
	private Wand ShooterWand
	{
		get
		{
			if (_wandCfg == null)
			{
				return null;
			}
			if ((object)_ownerSpell.shooterWand == null)
			{
				return null;
			}
			return PlayerMgr.Inst.Wands.FirstOrDefault((Wand e) => e.WandCfg == _wandCfg);
		}
	}

	public SpellTriggerController([NotNull] SpellBase ownerSpell)
	{
		_ownerSpell = ownerSpell;
	}

	public void InitAndEnable([NotNull] SlotData[] triggers, [NotNull] SpellShootGroup group)
	{
		_triggers = triggers.ToArray();
		_subGroup = group;
		_wandCfg = _ownerSpell.shooterWand.WandCfg;
		_subGroupMpCost = GetSubGroupManaCostIgnoreParentGroup();
		InitStartTrigger();
		InitOverTrigger();
		InitMoveTrigger();
		InitHitTrigger();
	}

	public void Disable()
	{
		_triggers = null;
		_subGroup = null;
	}

	private void InitStartTrigger()
	{
	}

	public bool HasOnStartTrigger()
	{
		return HasAbility(_triggers, SpellAbilityType.OnStartRotationTrigger);
	}

	public void TryTriggerOnStart()
	{
		SpellConfig[] source = (from e in _triggers
			select e.GetFinalConfig() into e
			where e.abilityType == SpellAbilityType.OnStartRotationTrigger
			select e).ToArray();
		int num = source.Sum((SpellConfig e) => e.int2);
		float aroundDis = source.Max((SpellConfig e) => e.float1);
		float extSpeed = source.Max((SpellConfig e) => e.float2);
		float twineTrrigerRatio = (GeneralTool.IsLowFpsOptimizeActive(30f) ? (1f + 9f * (1f - GameMgr.Inst.GetFps() / 30f)) : 1f);
		float num2 = Mathf.Max(2, Mathf.FloorToInt((float)num / twineTrrigerRatio));
		twineTrrigerRatio = (float)num / num2;
		List<SpellBase> list = new List<SpellBase>();
		for (int i = 0; (float)i < num2; i++)
		{
			SpellInitialParameter.Builder builder = CreateLaunchParameterBuilder();
			builder.OnBuildAfter += delegate(SpellInitialParameter.Builder _, SpellInitialParameter parameter)
			{
				parameter.aroundCasterRadiuRatio = aroundDis / 3f;
				parameter.tags.Add(SpellTag.Twine);
				parameter.finalMovementType = SpellSpecialMovementType.Rotation;
				parameter.bounsSpeed += extSpeed;
				parameter.finalDamageRatio *= twineTrrigerRatio;
			};
			Vector3 position = _ownerSpell.transform.position;
			if (_ownerSpell.SIP.spellIsFall)
			{
				SpellInitialParameter sIP = _ownerSpell.SIP;
				if (sIP != null)
				{
					ShootSpellSpatialInfo finalShootSpatialInfo = sIP.finalShootSpatialInfo;
					if (finalShootSpatialInfo != null && finalShootSpatialInfo.Target.HasValue)
					{
						position = _ownerSpell.SIP.finalShootSpatialInfo.Target.Value;
					}
				}
			}
			list.AddRange(Launch(position, _ownerSpell.Direction, builder, _triggers.First((SlotData e) => e.GetFinalConfig().abilityType == SpellAbilityType.OnStartRotationTrigger), createLightChain: true));
		}
		float num3 = UnityEngine.Random.Range(0f, 360f);
		for (int j = 0; j < list.Count; j++)
		{
			list[j].spellAroundOwnerCurrentAngle = 360f / (float)list.Count * (float)j + num3;
		}
		if (!(ShooterWand != null))
		{
			return;
		}
		foreach (SpellBase item in list)
		{
			Spell3007ChainSystem.CreateChains(new SpellBase[2] { item, _ownerSpell }, ShooterWand);
		}
		Spell3007ChainSystem.CreateChains(list.ToArray(), ShooterWand);
	}

	private void InitOverTrigger()
	{
	}

	public bool HasOnOverTrigger()
	{
		return HasAbility(_triggers, SpellAbilityType.OnOverTrigger, SpellAbilityType.OnOverSplitTrigger);
	}

	public bool TryTriggerOnOver(Vector3 position, Vector3 direction, out IEnumerable<SpellBase> newSpells)
	{
		newSpells = null;
		if (_triggers == null || _subGroup == null)
		{
			return false;
		}
		List<SpellBase> list = new List<SpellBase>();
		SlotData[] array = _triggers.Where((SlotData e) => e.GetFinalConfig().abilityType == SpellAbilityType.OnOverSplitTrigger).ToArray();
		if (array.Length != 0)
		{
			list.AddRange(LaunchForOverSplitTrigger(array, position));
			SpawnOnOverSplitEffect(position);
		}
		SlotData[] array2 = _triggers.Where((SlotData e) => e.GetFinalConfig().abilityType == SpellAbilityType.OnOverTrigger).ToArray();
		SlotData[] array3 = array2;
		foreach (SlotData slotData in array3)
		{
			float num = (float)slotData.GetFinalConfig().int2 / 100f;
			int extraDamage = Mathf.CeilToInt(_ownerSpell.spellCfg.damage * num);
			list.AddRange(LaunchForOverTrigger(slotData, extraDamage, position, direction));
		}
		if (array2.Length != 0)
		{
			SpawnOnOverEffect(position, direction);
		}
		newSpells = list;
		return true;
	}

	private void SpawnOnOverEffect(Vector3 defaultPosition, Vector3 shootDirection)
	{
		Vector3 position = (_ownerSpell.tsf_Layer ? _ownerSpell.tsf_Layer.position : defaultPosition);
		SpellSpriteEffectController.Inst.PlayOnOverTriggerEffect(position, shootDirection);
	}

	private void SpawnOnOverSplitEffect(Vector3 defaultPosition)
	{
		Vector3 position = (_ownerSpell.tsf_Layer ? _ownerSpell.tsf_Layer.position : defaultPosition);
		SpellSpriteEffectController.Inst.PlayOnOverSplitTriggerEffect(position);
	}

	private IEnumerable<SpellBase> LaunchForOverTrigger(SlotData trigger, int extraDamage, Vector3 position, Vector3 direction)
	{
		SpellInitialParameter.Builder builder = CreateLaunchParameterBuilder();
		builder.OnBuildAfter += delegate(SpellInitialParameter.Builder self, SpellInitialParameter parameter)
		{
			parameter.finalDamageExtra += extraDamage;
		};
		return Launch(position, direction, builder, trigger, createLightChain: true);
	}

	private IEnumerable<SpellBase> LaunchForOverSplitTrigger(SlotData[] triggers, Vector3 position)
	{
		List<SpellBase> list = new List<SpellBase>();
		float num = UnityEngine.Random.Range(0f, 360f);
		foreach (SlotData trigger in triggers)
		{
			list.AddRange(LaunchForOneOverSplitTrigger(trigger, num, position));
			num += 90f / (float)triggers.Length;
		}
		CreateLightChainForOverSplitTrigger(list.ToArray());
		return list;
	}

	private IEnumerable<SpellBase> LaunchForOneOverSplitTrigger(SlotData trigger, float baseDir, Vector3 position)
	{
		SpellInitialParameter.Builder builder = CreateLaunchParameterBuilder();
		builder.OnBuildAfter += delegate(SpellInitialParameter.Builder self, SpellInitialParameter parameter)
		{
			float num = (float)trigger.GetFinalConfig().int3 / 100f;
			parameter.finalDamageRatio *= num;
			parameter.finalKnockBackRatio *= num;
			parameter.lightningChainDamage = Mathf.CeilToInt(parameter.lightningChainDamage * num);
		};
		List<SpellBase> list = new List<SpellBase>();
		foreach (Vector3 item in CalculateSplitTriggerDirections(trigger.GetFinalConfig().int2, Tool2D.GetDir(baseDir)))
		{
			Vector3 position2 = position + item * 0.5f;
			SpellBase[] collection = Launch(position2, item, builder, trigger, createLightChain: false).ToArray();
			list.AddRange(collection);
		}
		return list;
	}

	private void CreateLightChainForOverSplitTrigger(SpellBase[] spells)
	{
		if ((object)ShooterWand != null)
		{
			Spell3007ChainSystem.CreateChains(spells, ShooterWand);
		}
	}

	private static IEnumerable<Vector3> CalculateSplitTriggerDirections(int count, Vector3 originDirection)
	{
		float num = 360f / (float)count;
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < count; i++)
		{
			list.Add(Tool2D.GetDir(originDirection, (float)i * num));
		}
		return list;
	}

	private void InitMoveTrigger()
	{
		_lastMoveTrigger = _triggers.LastOrDefault((SlotData e) => e.GetFinalConfig().abilityType == SpellAbilityType.OnMoveTrigger);
		_moveDisCounter = 0f;
		_localMoveTriggerDirFlag = _moveTriggerDirFlag;
		_moveTriggerDirFlag = !_moveTriggerDirFlag;
		float num = GetSubGroupManaCostIgnoreParentGroup();
		if (num > 15f)
		{
			num = 15f;
		}
		float num2 = (from e in _triggers
			select e.GetFinalConfig() into e
			where e.abilityType == SpellAbilityType.OnMoveTrigger
			select e).Sum((SpellConfig e) => e.float1);
		_moveTriggerNeedDis = num / num2;
	}

	public bool HasOnMoveTrigger()
	{
		return HasAbility(_triggers, SpellAbilityType.OnMoveTrigger);
	}

	public bool UpdateMoveTrigger(float moveDistance, Vector3 position, Vector3 direction, out IEnumerable<SpellBase> newSpells)
	{
		newSpells = null;
		if (_triggers == null || _subGroup == null || _lastMoveTrigger == null)
		{
			return false;
		}
		_moveDisCounter += moveDistance;
		float num = (GeneralTool.IsLowFpsOptimizeActive(30f) ? (_moveTriggerNeedDis * (1f + 9f * (1f - GameMgr.Inst.GetFps() / 30f))) : _moveTriggerNeedDis);
		if (_moveDisCounter < num)
		{
			return false;
		}
		_moveDisCounter = 0f;
		newSpells = LaunchForMoveTrigger(_lastMoveTrigger, position, direction, out var shootDirection);
		if (newSpells.ToArray().Length != 0)
		{
			SpawnOnMoveEffect(position, shootDirection);
		}
		return true;
	}

	private void SpawnOnMoveEffect(Vector3 defaultPosition, Vector3 direction)
	{
		Vector3 position = (_ownerSpell.tsf_Layer ? _ownerSpell.tsf_Layer.position : defaultPosition);
		SpellSpriteEffectController.Inst.PlayOnMoveTriggerEffect(position, direction);
	}

	private IEnumerable<SpellBase> LaunchForMoveTrigger(SlotData trigger, Vector3 position, Vector3 direction, out Vector3 shootDirection)
	{
		shootDirection = default(Vector3);
		if ((object)ShooterWand == null)
		{
			return Array.Empty<SpellBase>();
		}
		float targetMoveTrrigerRatio = (GeneralTool.IsLowFpsOptimizeActive(30f) ? (1f + 9f * (1f - GameMgr.Inst.GetFps() / 30f)) : 1f);
		float num = _subGroupMpCost * GetMoveTriggerManaRatio();
		if (!ShooterWand.CheckCurrentMpEnough(num))
		{
			return Array.Empty<SpellBase>();
		}
		ShooterWand.CostMp(num);
		int num2 = (_localMoveTriggerDirFlag ? 90 : (-90));
		_localMoveTriggerDirFlag = !_localMoveTriggerDirFlag;
		shootDirection = Tool2D.GetDir(direction, num2);
		Vector3 position2 = position + shootDirection * (0.2f + _ownerSpell.spellCfg.radius * 0.9f);
		SpellInitialParameter.Builder builder = CreateLaunchParameterBuilder();
		builder.OnBuildAfter += delegate(SpellInitialParameter.Builder self, SpellInitialParameter parameter)
		{
			parameter.finalSpeedRatio *= 0.7f;
		};
		builder.OnBuildAfter += delegate(SpellInitialParameter.Builder self, SpellInitialParameter parameter)
		{
			parameter.finalDamageRatio *= targetMoveTrrigerRatio;
		};
		SpellBase[] array = Launch(position2, shootDirection, builder, trigger, createLightChain: false).ToArray();
		CreateLightChainForMoveTrigger(array);
		return array;
	}

	private void CreateLightChainForMoveTrigger(SpellBase[] spells)
	{
		if ((object)ShooterWand != null)
		{
			List<SpellBase> list = spells.ToList();
			list.Add(_ownerSpell);
			Spell3007ChainSystem.CreateChains(list.ToArray(), ShooterWand);
		}
	}

	private float GetMoveTriggerManaRatio()
	{
		return (float)(from e in _triggers
			select e.GetFinalConfig() into e
			where e.abilityType == SpellAbilityType.OnMoveTrigger
			select e.int1).Min() / 100f;
	}

	private void InitHitTrigger()
	{
		_onHitTriggerCD.Clear();
		_hitTargets.Clear();
		foreach (SlotData item in _triggers.Where((SlotData e) => e.GetFinalConfig().abilityType == SpellAbilityType.OnHitTrigger))
		{
			_onHitTriggerCD.Add(item, 0f);
		}
	}

	public bool HasOnHitTrigger()
	{
		return HasAbility(_triggers, SpellAbilityType.OnHitTrigger);
	}

	public void AddHitTriggerPoint(Vector3 point)
	{
		_hitTargets.Add(point);
	}

	public bool TryTriggerOnHit(out IEnumerable<SpellBase> newSpells)
	{
		newSpells = null;
		if (_triggers == null || _subGroup == null)
		{
			return false;
		}
		if (_hitTargets.Count == 0)
		{
			return false;
		}
		List<SpellBase> list = new List<SpellBase>();
		float time = Time.time;
		float num = (GeneralTool.IsLowFpsOptimizeActive(30f) ? (1f + 9f * (1f - GameMgr.Inst.GetFps() / 30f)) : 1f);
		List<Vector3> list2 = new List<Vector3>();
		foreach (SlotData item in _triggers.Where((SlotData e) => e.GetFinalConfig().abilityType == SpellAbilityType.OnHitTrigger))
		{
			float num2 = 1f / (float)item.GetFinalConfig().int2 * num;
			if (!(time - _onHitTriggerCD[item] < num2))
			{
				Vector3 vector;
				if (_hitTargets.Count > 0)
				{
					int index = UnityEngine.Random.Range(0, _hitTargets.Count);
					vector = _hitTargets[index];
					_hitTargets.RemoveAt(index);
				}
				else
				{
					vector = list2[UnityEngine.Random.Range(0, list2.Count)];
				}
				list.AddRange(LaunchForHitTrigger(item, vector));
				list2.Add(vector);
				_onHitTriggerCD[item] = time;
			}
		}
		_hitTargets.Clear();
		newSpells = list;
		return true;
	}

	private IEnumerable<SpellBase> LaunchForHitTrigger(SlotData trigger, Vector3 position)
	{
		if ((object)ShooterWand == null)
		{
			return Array.Empty<SpellBase>();
		}
		float hitIntervalRatio = (GeneralTool.IsLowFpsOptimizeActive(30f) ? (1f + 9f * (1f - GameMgr.Inst.GetFps() / 30f)) : 1f);
		float num = _subGroupMpCost * GetHitTriggerManaRatio() * hitIntervalRatio;
		if (!ShooterWand.CheckCurrentMpEnough(num))
		{
			return Array.Empty<SpellBase>();
		}
		ShooterWand.CostMp(num);
		Vector3 dir = Tool2D.GetDir();
		SpellInitialParameter.Builder builder = CreateLaunchParameterBuilder();
		builder.OnBuildAfter += delegate(SpellInitialParameter.Builder self, SpellInitialParameter parameter)
		{
			parameter.finalDamageRatio *= hitIntervalRatio;
		};
		return Launch(position, dir, builder, trigger, createLightChain: true);
	}

	private float GetHitTriggerManaRatio()
	{
		return (float)(from e in _triggers
			select e.GetFinalConfig() into e
			where e.abilityType == SpellAbilityType.OnHitTrigger
			select e.int1).Min() / 100f;
	}

	private IEnumerable<SpellBase> Launch(Vector3 position, Vector3 direction, [NotNull] SpellInitialParameter.Builder builder, [NotNull] SlotData trigger, bool createLightChain)
	{
		ShootSpellSpatialInfo.ToPoint(position, position + UnityEngine.Random.insideUnitSphere.IgnoreZ() * 0.1f, direction);
		return Array.Empty<SpellBase>();
	}

	private SpellInitialParameter.Builder CreateLaunchParameterBuilder()
	{
		SpellInitialParameter.Builder builder = new SpellInitialParameter.Builder();
		if ((object)ShooterWand != null)
		{
			builder.ApplyWandEffect(ShooterWand, _ownerSpell.wandChargeData);
		}
		builder.ApplyOwnerTriggerSpellEffect(_ownerSpell);
		return builder;
	}

	private float GetSubGroupManaCostIgnoreParentGroup()
	{
		SpellShootData ownerShootData = _subGroup.OwnerShootData;
		_subGroup.OwnerShootData = null;
		float result = (ShooterWand ? _subGroup.GetGroupManaCost_FinalPlayerValue(ShooterWand) : _subGroup.GetGroupManaCost(1f));
		_subGroup.OwnerShootData = ownerShootData;
		return result;
	}

	private static bool HasAbility(SlotData[] slots, params SpellAbilityType[] abilitys)
	{
		return slots?.Any((SlotData e) => abilitys.Contains(e.GetFinalConfig().abilityType)) ?? false;
	}
}
