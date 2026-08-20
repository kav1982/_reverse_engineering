using UnityEngine;

public class Boss5_Tentacle : MonoBehaviour
{
	public VariableInt nodeCount;

	public LineRenderer thisLineRenderer;

	public float tentacleLength;

	public VariableFloat lengthMinus;

	public VariableFloat nodeInterval;

	public float smoothTime;

	public float strengthLerp;

	public VariableFloat swingFrequency;

	public VariableFloat startSwing;

	public VariableFloat swingAmplitude;

	public bool reversed;

	public VariableFloat tentacleThick;

	public VariableFloat positionOffset;

	public Animator anima;

	private void OnEnable()
	{
		anima.Play("Boss5_GroundTentacle");
	}

	private void Update()
	{
		thisLineRenderer.SetPosition(0, base.transform.position);
		thisLineRenderer.SetPosition(1, base.transform.position + new Vector3(0f, 2f, 0f));
	}
}
