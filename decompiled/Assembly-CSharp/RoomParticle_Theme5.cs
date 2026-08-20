using UnityEngine;

public class RoomParticle_Theme5 : AbsRoomSpecialize
{
	public ParticleSystem ps1;

	public ParticleSystem ps2;

	public Material matHolloween1;

	public Material matHolloween2;

	public Material matSpring1;

	public Material matSpring2;

	public Material matSummer1;

	public Material matSummer2;

	public Material matChristmas1;

	public Material matChristmas2;

	public float centiareRateLow;

	public float centiareRateHigh;

	public override void RoomSpecializeInitialize(RoomController roomCtrller)
	{
		Material material = null;
		Material material2 = null;
		switch (GameMgr.CampSkinType)
		{
		case CampSkinType.Halloween:
			material = matHolloween1;
			material2 = matHolloween2;
			break;
		case CampSkinType.Spring:
			material = matSpring1;
			material2 = matSpring2;
			break;
		case CampSkinType.Summer:
			material = matSummer1;
			material2 = matSummer2;
			break;
		case CampSkinType.Christmas:
			material = matChristmas1;
			material2 = matChristmas2;
			break;
		}
		if (material != null)
		{
			Renderer component = ps1.GetComponent<Renderer>();
			Object.Destroy(component.material);
			component.material = material;
		}
		if (material2 != null)
		{
			Renderer component2 = ps2.GetComponent<Renderer>();
			Object.Destroy(component2.material);
			component2.material = material2;
		}
		base.transform.position = roomCtrller.CenterPoint;
		ParticleSystem.EmissionModule emission = ps1.emission;
		ParticleSystem.EmissionModule emission2 = ps2.emission;
		ParticleSystem.ShapeModule shape = ps1.shape;
		ParticleSystem.ShapeModule shape2 = ps2.shape;
		emission.rateOverTime = roomCtrller.RoomScale.x * roomCtrller.RoomScale.y * centiareRateLow;
		emission2.rateOverTime = roomCtrller.RoomScale.x * roomCtrller.RoomScale.y * centiareRateHigh;
		shape.scale = roomCtrller.RoomScale;
		shape2.scale = roomCtrller.RoomScale;
		Object.Destroy(this);
	}
}
