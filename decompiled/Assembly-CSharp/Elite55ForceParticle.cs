using UnityEngine;

public class Elite55ForceParticle : MonoBehaviour
{
	public Transform startTrans;

	public Transform endTrans;

	public ParticleSystemForceField pForceField;

	public ParticleSystem forceParticle;

	public float baseEmitDuration;

	private Vector3 endPoint;

	public float speedPerDistance;

	private void OnEnable()
	{
		forceParticle.Stop();
	}

	public void Initialize()
	{
		forceParticle.Play();
	}

	public void UpdateFuseParticleEffect(Vector3 startPos, Vector3 targetPos)
	{
		startTrans.position = Tool2D.IgnoreZPoint(startPos);
		endTrans.position = Tool2D.IgnoreZPoint(targetPos);
	}
}
