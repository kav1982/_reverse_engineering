using System;
using UnityEngine;

[Serializable]
public class ColorByColorType : GetObjectBySpellColorType<Color>
{
	[ColorUsage(true, true)]
	public Color Player;

	[ColorUsage(true, true)]
	public Color Monster;

	[ColorUsage(true, true)]
	public Color Frozen;

	[ColorUsage(true, true)]
	public Color Mucus;

	[ColorUsage(true, true)]
	public Color Venom;

	[ColorUsage(true, true)]
	public Color Fire;

	[ColorUsage(true, true)]
	public Color Thunder;

	[ColorUsage(true, true)]
	public Color Void;

	public Color Get(SpellColorType color)
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
