using UnityEngine;

public class Spell9010BounceBullet : SpellBase
{
	[Space(50f)]
	public GameObject Model;

	public GameObject Shadow;

	public override void InitializeCallback()
	{
		tsf_Layer.gameObject.SetActive(value: false);
		if (base.spellAroundOwnerRadius != 0f)
		{
			rigid.linearVelocity = Vector3.zero;
		}
		else
		{
			rigid.linearVelocity = base.Direction * base.CurrentSpeed;
		}
		base.rebounceTime = 1;
	}

	public override void OnFirstFrame()
	{
		base.OnFirstFrame();
		tsf_Layer.gameObject.SetActive(value: true);
	}

	public override void Update()
	{
		base.Update();
		Model.transform.right = Tool2D.IgnoreZPoint(rigid.linearVelocity);
		Shadow.transform.position = Tool2D.GetLayerPoint(Tool2D.IgnoreZPoint(base.transform.position), LayerCorrectType.Shadow);
		Shadow.transform.right = Tool2D.IgnoreZPoint(rigid.linearVelocity);
		base.DurationTimer += Time.deltaTime;
		if (base.DurationTimer > base.spellCfg.duration)
		{
			if (!base.isFlyFinish)
			{
				base.isFlyFinish = true;
				rigid.linearVelocity = Vector3.zero;
				base.CurrentSpeed = 0f;
			}
			tsf_Layer.localScale = Vector3.one * (tsf_Layer.localScale.x - 5f * Time.deltaTime);
			if (tsf_Layer.localScale.x <= 0f)
			{
				PoolRecycle();
			}
		}
	}
}
