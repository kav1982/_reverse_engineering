using UnityEngine;

public class Relic_SpellDestroyer_Hit : LayerCorrect
{
	[Space(50f)]
	public GameObject go_Node;

	public GameObject go_Hit;

	public LineRenderer lr_Laser;

	public LineRenderer lr_LaserShadow;

	public int countOfBulletPerMeter;

	public void Initialize(Vector3 startWorldPoint, Vector3 endWorldPoint)
	{
		lr_Laser.SetPosition(0, Tool2D.GetLayerPoint(startWorldPoint));
		lr_Laser.SetPosition(1, Tool2D.GetLayerPoint(endWorldPoint));
		lr_LaserShadow.SetPosition(0, Tool2D.IgnoreZPoint(startWorldPoint, 1.05f));
		lr_LaserShadow.SetPosition(1, Tool2D.IgnoreZPoint(endWorldPoint, 1.05f));
		go_Node.transform.position = Tool2D.GetLayerPoint(startWorldPoint);
		go_Hit.transform.position = Tool2D.GetLayerPoint(endWorldPoint);
	}
}
