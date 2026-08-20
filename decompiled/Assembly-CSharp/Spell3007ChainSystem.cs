using System.Linq;

public static class Spell3007ChainSystem
{
	public static SpellBase lastChainSpell;

	public static void CreateChains(SpellBase[] spells, Wand targetWand)
	{
		SpellBase[] array = spells.Where((SpellBase e) => e.lightningChainDamage > 0f).ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			if (lastChainSpell != null && lastChainSpell.gameObject.activeInHierarchy)
			{
				CreateChain(lastChainSpell, array[i], targetWand, PlayerMgr.Inst.PlayerPpt);
			}
			lastChainSpell = array[i];
		}
	}

	private static void CreateChain(SpellBase s1, SpellBase s2, Wand wand, UnitProperty unitPpt)
	{
		float chainDamage = (s1.lightningChainDamage + s2.lightningChainDamage) / 2f;
		ObjPoolMgr.Inst.GetGO("Prefabs/Spell/30071").GetComponent<Spell3007LightningChain>().Iniatialize(unitPpt, s1.transform, s2.transform, chainDamage, wand, s1, s2);
	}
}
