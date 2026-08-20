using UnityEngine;

public class Oil : LayerCorrect
{
	private enum MucusState
	{
		Largen,
		Idle
	}

	public Transform tsf_Model;

	public Transform tsf_Collider;

	public float becomeBigerSpeed = 1f;

	public float becomeSmllerSpeed = 1f;

	private MucusState state;

	private float radius;

	private void Update()
	{
		switch (state)
		{
		case MucusState.Largen:
		{
			float num = tsf_Collider.localScale.x / 2f + Time.deltaTime * becomeBigerSpeed;
			if (num > radius)
			{
				num = radius;
				state = MucusState.Idle;
			}
			tsf_Collider.localScale = Vector3.one * num * 2f;
			tsf_Model.localScale = Vector3.one * num * 2f;
			break;
		}
		default:
			Debug.LogError(state);
			break;
		case MucusState.Idle:
			break;
		}
	}

	public void Initialize(float radius)
	{
		state = MucusState.Largen;
		this.radius = radius;
		tsf_Collider.localScale = Vector3.zero;
		tsf_Model.localScale = Vector3.zero;
	}
}
