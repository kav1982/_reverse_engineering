using Spine.Unity;
using UnityEngine;

public class Boss5_WaveMaker : MonoBehaviour
{
	public SkeletonAnimation Sanima;

	public bool towardsRight;

	public float waveDelay;

	private float delayTimer;

	private bool waveMaked;

	public Animator anima;

	public ShockParam knockShake;

	public SpriteRenderer thisRenderer;

	private Vector3 waveCreatePoint;

	public void Initialize(Vector3 waveCreatePoint, bool towardsRight)
	{
		if (towardsRight)
		{
			thisRenderer.flipX = false;
			Vector3 localPosition = Sanima.transform.localPosition;
			localPosition.x = 0f - Mathf.Abs(localPosition.x);
			Sanima.transform.localPosition = localPosition;
			Sanima.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else
		{
			Vector3 localPosition2 = Sanima.transform.localPosition;
			localPosition2.x = Mathf.Abs(localPosition2.x);
			Sanima.transform.localPosition = localPosition2;
			thisRenderer.flipX = true;
			Sanima.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		this.waveCreatePoint = waveCreatePoint;
		this.towardsRight = towardsRight;
		anima.Play("Boss5_WaveMaker");
		waveMaked = false;
		delayTimer = 0f;
		Sanima.AnimationState.SetAnimation(0, "attack2", loop: false);
	}

	private void Update()
	{
		delayTimer += Time.deltaTime;
		if (delayTimer > waveDelay && !waveMaked)
		{
			CamController.Inst.SetShock(knockShake);
			waveMaked = true;
			if ((bool)Boss5.Inst && !Boss5.Inst.myPpt.AlreadyDead)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss5_Wave", waveCreatePoint, 6f).GetComponent<Boss5_Wave>().Initialize(towardsRight ? FourDir.Right : FourDir.Left);
			}
		}
	}
}
