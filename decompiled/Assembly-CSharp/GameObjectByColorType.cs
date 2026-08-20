using System;
using UnityEngine;

[Serializable]
public class GameObjectByColorType : GetObjectBySpellColorType<GameObject>
{
	public GameObject Player;

	public GameObject Monster;

	public GameObject Frozen;

	public GameObject Mucus;

	public GameObject Venom;

	public GameObject Fire;

	public GameObject Thunder;

	public GameObject Void;

	public GameObject Get(SpellColorType color)
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

	public GameObject[] GetAll()
	{
		return new GameObject[8] { Player, Monster, Frozen, Mucus, Venom, Fire, Thunder, Void };
	}
}
