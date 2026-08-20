using UnityEngine;

public class Spell1006HoverTorch : SpellBase
{
	[Space(50f)]
	public float slowdownLerp;

	public float minSpeed;

	public float chaseMouseSpeedUp;

	private Vector3 pullForce;
}
