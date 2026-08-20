using UnityEngine;

public class Monster51_GroundSpear : MonoBehaviour
{
	private Vector3 diration;

	private Vector3 flyDiration;

	public float flySpeed;

	private float flyTime;

	public float waveSpeed;

	private float flyTimer;

	private Vector3 startPoint;

	private Vector3 targetPoint;

	private bool flyFinish;

	public Monster51 master;

	public SpriteRenderer mainRenderer;

	public SpriteRenderer shadowRenderer;

	public ParticleSystem trailParticle;

	public ParticleSystem explodeParticle;

	public void Initialize(Vector3 targetPoint, Monster51 master)
	{
		this.master = master;
		startPoint = base.transform.position;
		this.targetPoint = targetPoint;
		diration = Tool2D.IgnoreZPoint(targetPoint - base.transform.position).normalized;
		flyDiration = Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(targetPoint) - Tool2D.GetLayerPoint(base.transform.position)).normalized;
		flyTime = (startPoint - targetPoint).magnitude / flySpeed;
		flyTimer = 0f;
		mainRenderer.transform.position = Tool2D.GetLayerPoint(base.transform.position);
		shadowRenderer.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position), LayerCorrectType.Shadow);
		mainRenderer.transform.up = flyDiration;
		shadowRenderer.transform.up = diration;
		mainRenderer.enabled = true;
		shadowRenderer.enabled = true;
		trailParticle.Play();
		flyFinish = false;
	}

	private void Update()
	{
		mainRenderer.transform.position = Tool2D.GetLayerPoint(base.transform.position);
		shadowRenderer.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position), LayerCorrectType.Shadow);
		flyTimer += Time.deltaTime;
		if (flyTimer < flyTime)
		{
			base.transform.position = Vector3.Lerp(startPoint, targetPoint, flyTimer / flyTime);
			return;
		}
		if (!flyFinish)
		{
			SEMgr.Inst.monster51_SpearEnd.PlaySE();
			explodeParticle.transform.position = Tool2D.GetLayerPoint(base.transform.position);
			explodeParticle.Play();
			trailParticle.Stop();
			mainRenderer.enabled = false;
			shadowRenderer.enabled = false;
			string text = "EF_Monster52_BladeWaveSlow";
			if (GameMgr.IsChAge14_Static)
			{
				text = "EF_Monster52_BladeWaveSlow_H";
			}
			for (int i = 0; i < 4; i++)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/" + text, base.transform.position).GetComponent<Monster52_BladeWave>().Initialize(Tool2D.GetDir(Vector3.up, 45 + (i - 1) * 360 / 4), waveSpeed, master.myPpt);
			}
			if (master != null && master.enabled)
			{
				master.aimTracking = false;
				master.aimRenderer.enabled = false;
			}
		}
		flyFinish = true;
	}
}
