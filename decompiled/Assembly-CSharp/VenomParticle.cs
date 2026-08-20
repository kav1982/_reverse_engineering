using System;
using UnityEngine;

public class VenomParticle : MonoBehaviour
{
	[Header("Bubble")]
	public ParticleSystem ps_Bubble;

	public VariableFloat ratePerSquareMeter;

	public float bubbleRadiusRatio;

	private void Start()
	{
		ps_Bubble.transform.position = Tool2D.IgnoreZPoint(base.transform, 1.14f);
	}

	public void UpdateBubbleCircle(float radius)
	{
		ParticleSystem.ShapeModule shape = ps_Bubble.shape;
		ParticleSystem.EmissionModule emission = ps_Bubble.emission;
		shape.radius = radius * bubbleRadiusRatio;
		shape.scale = Vector3.one;
		emission.rateOverTime = MathF.PI * shape.radius * shape.radius * ratePerSquareMeter.result;
	}

	public void UpdateBubbleRectangle(float radius, float rectangleDistance)
	{
		ParticleSystem.ShapeModule shape = ps_Bubble.shape;
		shape.scale = new Vector3(radius * bubbleRadiusRatio, rectangleDistance, 1f);
		ParticleSystem.EmissionModule emission = ps_Bubble.emission;
		emission.rateOverTime = shape.scale.x * shape.scale.y * ratePerSquareMeter.result;
	}

	public void Initialize(float radius)
	{
		ps_Bubble.transform.position = Tool2D.IgnoreZPoint(base.transform, 1.14f);
		ratePerSquareMeter.RandomResult();
		UpdateBubbleCircle(radius);
	}

	public void Initialize(Vector3 point1, Vector3 point2, float radius)
	{
		base.transform.position = (point1 + point2) / 2f;
		float rectangleDistance = Vector3.Distance(point1, point2);
		ps_Bubble.transform.position = Tool2D.IgnoreZPoint(base.transform, 1.14f);
		ps_Bubble.transform.up = point1 - point2;
		ratePerSquareMeter.RandomResult();
		UpdateBubbleRectangle(radius, rectangleDistance);
	}
}
