using System.Collections.Generic;
using System.Numerics;

public class DamageRecorder
{
	public readonly Dictionary<int, BigInteger> DamagePreSpell = new Dictionary<int, BigInteger>();

	public BigInteger TotalDamage = 0;

	public void Record(int id, double damage)
	{
		BigInteger bigInteger = (BigInteger)damage;
		if (!DamagePreSpell.TryAdd(id, bigInteger))
		{
			DamagePreSpell[id] += (BigInteger)(long)bigInteger;
		}
		TotalDamage += bigInteger;
	}

	public void Clear()
	{
		DamagePreSpell.Clear();
		TotalDamage = 0;
	}
}
