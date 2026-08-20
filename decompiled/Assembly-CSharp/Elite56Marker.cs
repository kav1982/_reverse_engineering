using UnityEngine;

public class Elite56Marker : MonoBehaviour
{
	private static readonly int End = Animator.StringToHash("End");

	private static readonly int Start = Animator.StringToHash("Start");

	public Animator Anima;

	public Transform ModelTransform;

	private float timer;

	private bool chaseTarget;

	private bool isStart;

	public ParticleSystem WaveParticles;

	private float particlePlayTime;

	private float markStopAt;

	private int stage;

	public float PopSePlayInterval;

	private float PopSeTimer;

	private bool isFollowPlayer = true;

	private void OnEnable()
	{
		ModelTransform.localScale = Vector3.zero;
		timer = 0f;
		chaseTarget = true;
		isStart = false;
		stage = 0;
		PopSeTimer = 0f;
	}

	public void MarkStart(float markerDuration, float particleduration, bool isfollowPlayer = true)
	{
		Anima.Play("StartLock");
		markStopAt = markerDuration;
		particlePlayTime = particleduration;
		isStart = true;
		timer = 0f;
		isFollowPlayer = isfollowPlayer;
	}

	public void MarkEnd()
	{
		stage = 1;
		Anima.Play("LockEnd");
	}

	public void EffectEnd()
	{
		stage = 2;
		WaveParticles.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmitting);
		ObjPoolMgr.Inst.RecycleGO(base.gameObject, 0.8f);
	}

	public void StopChaseTarget()
	{
		chaseTarget = false;
	}

	private void Update()
	{
		if (isStart)
		{
			timer += Time.deltaTime;
			if (timer >= markStopAt && stage < 1)
			{
				MarkEnd();
			}
			if (timer >= particlePlayTime + markStopAt && stage < 2)
			{
				EffectEnd();
			}
			PopSeTimer += Time.deltaTime;
			if (PopSeTimer >= PopSePlayInterval && stage < 2)
			{
				PopSeTimer -= PopSePlayInterval;
				SEMgr.Inst.elite56PopWave.PlaySE();
			}
		}
	}

	private void LateUpdate()
	{
		if (chaseTarget && isFollowPlayer)
		{
			base.transform.position = PlayerMgr.Inst.PlayerPoint;
		}
	}
}
