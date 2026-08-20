using UnityEngine;

public class Spell1017Trace : EffectController
{
	public float disapearSpeed;

	public float initialProgress;

	private SpriteRenderer targetSprite;

	public float traceDuration;

	public float traceFadeOutStartTime;

	private float traceTimer;

	public override void OnEnable()
	{
		base.OnEnable();
		targetSprite = null;
		traceTimer = 0f;
	}

	private void Update()
	{
		if (targetSprite != null && targetSprite.gameObject.activeInHierarchy && targetSprite.material.GetFloat("_Progress") > 0f)
		{
			targetSprite.material.SetFloat("_Progress", targetSprite.material.GetFloat("_Progress") - disapearSpeed * Time.deltaTime);
			targetSprite.transform.localPosition = new Vector3(0f, 0f, initialProgress - targetSprite.material.GetFloat("_Progress"));
		}
		traceTimer += Time.deltaTime;
		if (traceTimer >= traceFadeOutStartTime)
		{
			targetSprite.material.SetFloat("_Transparency", Mathf.Max(0f, (traceDuration - traceTimer) / (traceDuration - traceFadeOutStartTime)));
		}
	}

	public void SetAll(SpellColorType type, float EffectScale)
	{
		ECChangeColor(type);
		targetSprite = ECGetCurrentEffect().GetComponentInChildren<SpriteRenderer>();
		targetSprite.material = Object.Instantiate(targetSprite.material);
		targetSprite.material.SetFloat("_Progress", initialProgress);
		targetSprite.material.SetFloat("_Transparency", 1f);
		targetSprite.material.SetFloat("_BaseRotate", Random.Range(0f, 6.5f));
		targetSprite.material.SetFloat("_ExplodeRotate", Random.Range(0f, 6.5f));
		tsf_Layer.localScale = Vector3.one * EffectScale;
	}
}
