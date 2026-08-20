using System;

public class GetSummonExplodePrefabPathByColorType : GetObjectBySpellColorType<string>
{
	public string Get(SpellColorType colorType)
	{
		return colorType switch
		{
			SpellColorType.Player => "Prefabs/Spell/31181/31181" + Enum.GetName(typeof(SpellColorType), SpellColorType.Player), 
			SpellColorType.Monster => "Prefabs/Spell/31181/31181" + Enum.GetName(typeof(SpellColorType), SpellColorType.Monster), 
			SpellColorType.Mucus => "Prefabs/Spell/31181/31181" + Enum.GetName(typeof(SpellColorType), SpellColorType.Mucus), 
			SpellColorType.Venom => "Prefabs/Spell/31181/31181" + Enum.GetName(typeof(SpellColorType), SpellColorType.Venom), 
			SpellColorType.Fire => "Prefabs/Spell/31181/31181" + Enum.GetName(typeof(SpellColorType), SpellColorType.Fire), 
			SpellColorType.Thunder => "Prefabs/Spell/31181/31181" + Enum.GetName(typeof(SpellColorType), SpellColorType.Thunder), 
			SpellColorType.Frozen => "Prefabs/Spell/31181/31181" + Enum.GetName(typeof(SpellColorType), SpellColorType.Frozen), 
			SpellColorType.Void => "Prefabs/Spell/31181/31181" + Enum.GetName(typeof(SpellColorType), SpellColorType.Void), 
			_ => "Prefabs/Spell/31181/31181" + Enum.GetName(typeof(SpellColorType), SpellColorType.Player), 
		};
	}
}
