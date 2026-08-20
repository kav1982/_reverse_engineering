using DG.Tweening;
using UnityEngine;

public class Spell2006DrainEffectController : MonoBehaviour
{
	public Transform spriteTrail;

	public float TrailDefaultLength;

	public float TrailDefaultWidth;

	public float TrailLerpTime;

	private UnitProperty targetOwner;

	private float resizeTimer;

	public float resizeTime;

	private UnitProperty targetPpt;

	public Transform rotateTrans;

	public Transform DrainCenterTrans;

	public ParticleSystem[] trailParticles;

	private void OnEnable()
	{
		spriteTrail.localScale = new Vector3(0f, 0f, 1f);
		StopEffect();
		StartPlayEffect();
	}

	private void Update()
	{
		if ((bool)targetOwner)
		{
			resizeTimer += Time.deltaTime;
			if (resizeTimer >= resizeTime)
			{
				resizeTimer = 0f;
				float num = Mathf.Min(TrailDefaultLength, Tool2D.IgnoreZDistance(base.transform.position, targetOwner.transform.position)) / Mathf.Sqrt(base.transform.lossyScale.x);
				spriteTrail.localScale = new Vector3(num, spriteTrail.localScale.y, 1f);
				spriteTrail.localPosition = new Vector3(num / 2f, 0f, 0f);
			}
			rotateTrans.right = (targetOwner.gameObject.transform.position - targetPpt.gameObject.transform.position).normalized;
			DrainCenterTrans.position = targetOwner.transform.position;
		}
		if ((bool)targetPpt)
		{
			base.transform.position = targetPpt.transform.position;
		}
	}

	public void StartPlayEffect()
	{
		ParticleSystem[] array = trailParticles;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Play();
		}
		spriteTrail.DOScaleY(TrailDefaultWidth, 2.5f);
	}

	public void StopEffect()
	{
		ParticleSystem[] array = trailParticles;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Stop();
		}
		spriteTrail.DOScaleY(0f, TrailLerpTime);
	}

	public void SetFollowTarget(UnitProperty owner, UnitProperty target)
	{
		targetOwner = owner;
		targetPpt = target;
	}
}
