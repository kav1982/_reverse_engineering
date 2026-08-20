using System;
using UnityEngine;

public class Venom : MonoBehaviour
{
	public Transform tsf_Collider;

	public Transform tsf_SR;

	public float becomeSmllerSpeed = 1f;

	[Header("Bubble")]
	public ParticleSystem ps_Bubble;

	public VariableFloat ratePerSquareMeter;

	public float bubbleRadiusRatio;

	private float radius;

	private float duration;

	private float durationTimer;

	private bool durationFinish;

	private bool isRectangle;

	private float rectangleDistance;

	private float particleUpdateTimer;

	private void Start()
	{
		tsf_SR.position = Tool2D.IgnoreZPoint(base.transform, -130f);
		ps_Bubble.transform.position = Tool2D.IgnoreZPoint(base.transform, 1.14f);
	}

	private void Update()
	{
		if (durationFinish)
		{
			radius -= Time.deltaTime * becomeSmllerSpeed;
			if (radius > 0f)
			{
				if (isRectangle)
				{
					tsf_Collider.localScale = new Vector3(radius * 2f, tsf_Collider.localScale.y, 1f);
					UpdateBubbleRectangle();
				}
				else
				{
					tsf_Collider.localScale = Vector3.one * radius * 2f;
					UpdateBubbleCircle();
				}
				tsf_SR.localScale = tsf_Collider.localScale;
			}
			else
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
		}
		else
		{
			durationTimer += Time.deltaTime;
			if (durationTimer >= duration)
			{
				durationFinish = true;
			}
		}
	}

	private void UpdateBubbleCircle()
	{
		ParticleSystem.ShapeModule shape = ps_Bubble.shape;
		ParticleSystem.EmissionModule emission = ps_Bubble.emission;
		shape.radius = radius * bubbleRadiusRatio;
		emission.rateOverTime = MathF.PI * shape.radius * shape.radius * ratePerSquareMeter.result;
	}

	private void UpdateBubbleRectangle()
	{
		ParticleSystem.ShapeModule shape = ps_Bubble.shape;
		shape.scale = new Vector3(radius * bubbleRadiusRatio, rectangleDistance, 1f);
		ParticleSystem.EmissionModule emission = ps_Bubble.emission;
		emission.rateOverTime = shape.scale.x * shape.scale.y * ratePerSquareMeter.result;
	}

	public void Initialize(float radius, float duration)
	{
		durationFinish = false;
		isRectangle = false;
		durationTimer = 0f;
		this.radius = radius;
		this.duration = duration;
		tsf_Collider.localScale = Vector3.one * radius * 2f;
		tsf_SR.localScale = tsf_Collider.localScale;
		ratePerSquareMeter.RandomResult();
		UpdateBubbleCircle();
	}

	public void Initialize(Vector3 point1, Vector3 point2, float radius, float duration)
	{
		durationFinish = false;
		isRectangle = true;
		durationTimer = 0f;
		this.radius = radius;
		this.duration = duration;
		base.transform.position = (point1 + point2) / 2f;
		rectangleDistance = Vector3.Distance(point1, point2);
		tsf_Collider.localScale = new Vector3(radius * 2f, rectangleDistance * 1.1f, 1f);
		tsf_SR.localScale = tsf_Collider.localScale;
		tsf_Collider.up = point1 - point2;
		tsf_SR.up = tsf_Collider.up;
		ps_Bubble.transform.up = tsf_Collider.up;
		ratePerSquareMeter.RandomResult();
		UpdateBubbleRectangle();
	}
}
