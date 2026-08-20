using System;
using JetBrains.Annotations;
using SpriteEffectSystem;
using UnityEngine;

[Serializable]
public class SpriteEffectAnimaByColorType : GetObjectBySpellColorType<SpriteEffectAnima>
{
	public SpriteEffectAnima[] Player;

	public SpriteEffectAnima[] Monster;

	public SpriteEffectAnima[] Frozen;

	public SpriteEffectAnima[] Venom;

	public SpriteEffectAnima[] Mucus;

	public SpriteEffectAnima[] Fire;

	public SpriteEffectAnima[] Thunder;

	public SpriteEffectAnima[] Void;

	public SpriteEffectAnima Get(SpellColorType colorType)
	{
		SpriteEffectAnima[] array = GetArray(colorType);
		if (array == null)
		{
			return null;
		}
		return array[UnityEngine.Random.Range(0, array.Length)];
	}

	public int GetCount(SpellColorType colorType)
	{
		SpriteEffectAnima[] array = GetArray(colorType);
		if (array == null)
		{
			return 0;
		}
		return array.Length;
	}

	[CanBeNull]
	public SpriteEffectAnima[] GetArray(SpellColorType colorType)
	{
		return colorType switch
		{
			SpellColorType.Frozen => Frozen, 
			SpellColorType.Monster => Monster, 
			SpellColorType.Mucus => Mucus, 
			SpellColorType.Player => Player, 
			SpellColorType.Venom => Venom, 
			SpellColorType.Fire => Fire, 
			SpellColorType.Thunder => Thunder, 
			SpellColorType.Void => Void, 
			_ => null, 
		};
	}
}
