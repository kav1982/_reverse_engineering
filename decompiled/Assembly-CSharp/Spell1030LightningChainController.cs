using System.Collections.Generic;
using UnityEngine;

public class Spell1030LightningChainController : MonoBehaviour
{
	private List<Spell1030HarpoonsLightingchain> chainList = new List<Spell1030HarpoonsLightingchain>();

	private void Update()
	{
		UpdateAllChainState();
	}

	private void UpdateAllChainState()
	{
		_ = chainList.Count;
		_ = 0;
	}

	private void AddnewHarpoonsLightningChain(UnitProperty ownerPpt, UnitProperty targetPpt, float chainDamage, float detectRange, float conductNewTargetDamageRatio, Wand targetWand = null, SpellBase targetSpell = null)
	{
	}
}
