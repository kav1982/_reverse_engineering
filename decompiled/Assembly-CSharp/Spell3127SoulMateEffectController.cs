using DG.Tweening;
using UnityEngine;

public class Spell3127SoulMateEffectController : MonoBehaviour
{
	public Transform spriteTrail;

	public float TrailDefaultLength;

	public float TrailDefaultWidth;

	public float TrailLerpTime;

	private UnitProperty targetOwner;

	private float resizeTimer;

	public float resizeTime;

	private float timer;

	public SpriteRenderer trailSprite;

	private void OnEnable()
	{
		spriteTrail.localScale = new Vector3(0f, 0f, 1f);
		spriteTrail.DOScaleY(TrailDefaultWidth, TrailLerpTime);
		timer = 0f;
	}

	private void Update()
	{
		if ((bool)targetOwner)
		{
			timer += Time.deltaTime;
			resizeTimer += Time.deltaTime;
			if (resizeTimer >= resizeTime)
			{
				resizeTimer = 0f;
				float num = Mathf.Min(TrailDefaultLength, Tool2D.IgnoreZDistance(base.transform.position, targetOwner.transform.position)) / Mathf.Sqrt(base.transform.lossyScale.x) * Mathf.Min(timer, TrailLerpTime);
				spriteTrail.DOScaleX(num, 0.1f);
				spriteTrail.localPosition = new Vector3(num / 2f, 0f, 0f);
			}
		}
		trailSprite.color = new Color(1f, 1f, 1f, DataMgr.settingData.SummonTransparent);
	}

	public void SetFollowTarget(UnitProperty owner)
	{
		targetOwner = owner;
	}
}
