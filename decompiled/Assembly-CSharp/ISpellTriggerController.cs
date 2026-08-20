using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public interface ISpellTriggerController
{
	void InitAndEnable(SlotData[] triggers, SpellShootGroup group);

	void Disable();

	void TryTriggerOnStart();

	bool HasOnStartTrigger();

	bool HasOnOverTrigger();

	bool TryTriggerOnOver(Vector3 position, Vector3 direction, [CanBeNull] out IEnumerable<SpellBase> newSpells);

	bool HasOnMoveTrigger();

	bool UpdateMoveTrigger(float moveDistance, Vector3 position, Vector3 direction, [CanBeNull] out IEnumerable<SpellBase> newSpells);

	bool HasOnHitTrigger();

	void AddHitTriggerPoint(Vector3 point);

	bool TryTriggerOnHit([CanBeNull] out IEnumerable<SpellBase> newSpells);
}
