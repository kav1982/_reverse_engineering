using UnityEngine;

public class Spell1022TrailController : EffectController
{
	public TrailRenderer[] trails;

	public void ClearTrail()
	{
		TrailRenderer[] array = trails;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Clear();
		}
	}
}
