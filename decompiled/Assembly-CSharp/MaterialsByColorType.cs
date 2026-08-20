using System;
using UnityEngine;

[Serializable]
public class MaterialsByColorType : GetObjectBySpellColorType<Material>
{
	public Material Player;

	public Material Monster;

	public Material Frozen;

	public Material Mucus;

	public Material Venom;

	public Material Fire;

	public Material Thunder;

	public Material Void;

	public Material Get(SpellColorType color)
	{
		return color switch
		{
			SpellColorType.Frozen => Frozen, 
			SpellColorType.Monster => Monster, 
			SpellColorType.Mucus => Mucus, 
			SpellColorType.Player => Player, 
			SpellColorType.Venom => Venom, 
			SpellColorType.Fire => Fire, 
			SpellColorType.Thunder => Thunder, 
			SpellColorType.Void => Void, 
			_ => throw new ArgumentOutOfRangeException("color", color, null), 
		};
	}
}
