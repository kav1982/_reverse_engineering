using UnityEngine;

public class Boss6_SingleQuake : MonoBehaviour
{
	[Header("表现")]
	public ParticleSystem explodeParticle;

	public float particleTime;

	public float delayTime;

	private bool exploded;

	private float existTime;

	public void Initialize()
	{
		existTime = 0f;
		exploded = false;
		explodeParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position);
	}

	private void Update()
	{
		existTime += Time.deltaTime;
		if (existTime > particleTime && !explodeParticle.isPlaying && !exploded)
		{
			explodeParticle.Play();
		}
		if (existTime > delayTime && !exploded)
		{
			exploded = true;
		}
		if (existTime > delayTime + 2f)
		{
			explodeParticle.Clear();
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
		explodeParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position);
	}
}
