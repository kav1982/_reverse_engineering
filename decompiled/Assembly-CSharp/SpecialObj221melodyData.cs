using UnityEngine;

public class SpecialObj221melodyData
{
	public static readonly SpecialObj221melody[] melodys = new SpecialObj221melody[4]
	{
		new SpecialObj221melody(150, "60350230120301", "五声音阶1"),
		new SpecialObj221melody(180, "1210005321", "五声音阶1"),
		new SpecialObj221melody(180, "121056500321", "五声音阶1"),
		new SpecialObj221melody(180, "6536005321", "五声音阶1")
	};

	public static SpecialObj221melody GetRandomMelody()
	{
		return melodys[Random.Range(0, melodys.Length)];
	}

	public static SpecialObj221melody GetRandomMelody(int index)
	{
		return melodys[index];
	}
}
