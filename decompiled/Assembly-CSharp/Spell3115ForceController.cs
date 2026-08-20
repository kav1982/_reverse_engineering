using UnityEngine;

public class Spell3115ForceController : MonoBehaviour
{
	public Transform startTrans;

	public Transform endTrans;

	public ParticleSystemForceField pForceField;

	public ParticleSystem forceParticle;

	public float baseEmitDuration;

	private Vector3 endPoint;

	public float speedPerDistance;

	public void Initialize(Transform starttrans, Vector3 endPoint)
	{
		this.endPoint = endPoint;
		base.transform.position = starttrans.position;
		ObjPoolMgr.Inst.RecycleGO(base.gameObject, baseEmitDuration);
		ParticleSystem.MainModule main = forceParticle.main;
		main.startLifetime = 0.7f;
		forceParticle.Play();
	}

	public void Initialize(Vector3 startpos, Vector3 endPoint)
	{
		this.endPoint = endPoint;
		base.transform.position = startpos;
		ObjPoolMgr.Inst.RecycleGO(base.gameObject, baseEmitDuration);
		ParticleSystem.MainModule main = forceParticle.main;
		main.startLifetime = 0.7f;
		forceParticle.Play();
	}

	public void UpdateFuseParticleEffect(Vector3 newPos)
	{
		startTrans.position = Tool2D.IgnoreZPoint(newPos);
		endTrans.position = Tool2D.IgnoreZPoint(endPoint);
		ParticleSystem.MinMaxCurve gravity = pForceField.gravity;
		gravity.curveMultiplier = Mathf.Max(5f, Vector3.Distance(startTrans.position, endTrans.position) * speedPerDistance);
		pForceField.gravity = gravity;
	}
}
