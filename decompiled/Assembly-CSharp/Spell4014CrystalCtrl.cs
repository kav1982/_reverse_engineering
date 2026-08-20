using UnityEngine;

public class Spell4014CrystalCtrl : MonoBehaviour
{
	public TrailRenderer[] trail;

	public void SetTrailWidth(float width)
	{
		TrailRenderer[] array = trail;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].widthMultiplier = width;
		}
	}
}
