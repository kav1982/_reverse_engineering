using UnityEngine;

public class VenomController : MonoBehaviour
{
	public ParticleSystem globalVenomParticle;

	public RoomController belongRoomController;

	public void CreateVenom(Vector3 point, float radius, float duration)
	{
		if (!GameMgr.IsMobile_Static)
		{
			VenomSystem.CreateVenom(point, radius, duration);
		}
	}

	public void CreateVenom(Vector3 point1, Vector3 point2, float radius, float duration)
	{
		if (!GameMgr.IsMobile_Static)
		{
			VenomSystem.CreateVenom(point1, point2, radius, duration);
		}
	}

	public void Initialize(RoomController roomController)
	{
		if (GameMgr.IsMobile_Static)
		{
			ParticleSystem.EmissionModule emission = globalVenomParticle.emission;
			emission.enabled = false;
			return;
		}
		belongRoomController = roomController;
		ParticleSystem.ShapeModule shape = globalVenomParticle.shape;
		Vector3 vector2 = (shape.scale = belongRoomController.RoomScale);
		globalVenomParticle.transform.position = Tool2D.IgnoreZPoint(roomController.CenterPoint, 1.14f);
		ParticleSystem.MainModule main = globalVenomParticle.main;
		main.maxParticles = 2000;
	}

	public void RecycleAllVenom()
	{
		ObjPoolMgr.Inst.RecycleSpecify("Prefabs/EF/EF_VenomSphere");
		ObjPoolMgr.Inst.RecycleSpecify("Prefabs/EF/EF_VenomRectangle");
		VenomSystem.Inst.Clear();
	}
}
