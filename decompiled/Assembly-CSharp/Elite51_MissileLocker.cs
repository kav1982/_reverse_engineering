using System.Collections.Generic;
using UnityEngine;

public class Elite51_MissileLocker : MonoBehaviour
{
	private static readonly int Show = Animator.StringToHash("Show");

	private static readonly int Hide = Animator.StringToHash("Hide");

	private static readonly int Progress = Shader.PropertyToID("_Progress");

	private static readonly int HasEffect = Shader.PropertyToID("_HasEffect");

	public Transform CornerTransform;

	public Transform CenterTransform;

	public Vector3 BaseOffset;

	public Animator Anima;

	public List<SpriteRenderer> CornerSprites;

	public Vector2 WarningSEInterval;

	private float seTimer;

	private float lockProgress;

	private bool lockingTarget;

	private void OnEnable()
	{
		seTimer = 0f;
		lockProgress = 0f;
		lockingTarget = false;
	}

	private void Update()
	{
		if (Progress <= 0)
		{
			seTimer = 0f;
		}
		else if (lockingTarget)
		{
			seTimer += Time.deltaTime;
			if (!(seTimer < WarningSEInterval.x + (WarningSEInterval.y - WarningSEInterval.x) * (1f - lockProgress)))
			{
				seTimer = 0f;
				SEMgr.Inst.elite51Lock.PlaySE();
			}
		}
	}

	public void UpdateTransform(Vector3 centerPos)
	{
		CornerTransform.position = PlayerMgr.Inst.PlayerPoint + BaseOffset;
		CenterTransform.position = centerPos + BaseOffset;
	}

	public void UpdateLockProgress(float progress, bool isLocked)
	{
		foreach (SpriteRenderer cornerSprite in CornerSprites)
		{
			cornerSprite.material.SetFloat(Progress, progress);
			cornerSprite.material.SetFloat(HasEffect, isLocked ? 1 : 0);
		}
		lockProgress = progress;
	}

	public void LockStart()
	{
		Anima.SetTrigger(Show);
		lockingTarget = true;
	}

	public void LockEnd()
	{
		Anima.SetTrigger(Hide);
		lockingTarget = false;
	}
}
