using System.Collections.Generic;
using UnityEngine;

public class Boss6_LongChildBody : UnitBase
{
	public List<SpellBase> hitList;

	public Boss6_LongChild master;

	public Transform tsf_Body;

	public Transform tsf_BodyShadow;

	public float bodyHeight;

	public Transform tsf_LeftLeg;

	public Transform tsf_RightLeg;

	public Transform tsf_LeftLegShadow;

	public Transform tsf_RightLegShadow;

	public List<SpriteRenderer> SRs = new List<SpriteRenderer>();

	public List<SpriteRenderer> SRs_Shadow = new List<SpriteRenderer>();

	public Color shadowColor;

	public Sprite bodySprite;

	public Sprite tailSprite;

	public override void SingleInitialCallback()
	{
		for (int i = 0; i < SRs.Count; i++)
		{
			myPpt.RemoveSRFromArray(SRs[i]);
		}
		for (int j = 0; j < SRs_Shadow.Count; j++)
		{
			myPpt.RemoveSRFromArray(SRs_Shadow[j]);
			SRs_Shadow[j].color = shadowColor;
			myPpt.RemoveSRFromArray(SRs_Shadow[j]);
		}
	}

	public void SetTail(bool isTail)
	{
		if (isTail)
		{
			tsf_LeftLeg.gameObject.SetActive(value: false);
			tsf_LeftLeg.gameObject.SetActive(value: false);
			tsf_LeftLegShadow.gameObject.SetActive(value: false);
			tsf_LeftLegShadow.gameObject.SetActive(value: false);
			SRs[0].sprite = tailSprite;
			SRs_Shadow[0].sprite = tailSprite;
		}
		else
		{
			tsf_LeftLegShadow.gameObject.SetActive(value: true);
			tsf_LeftLegShadow.gameObject.SetActive(value: true);
			SRs_Shadow[0].sprite = bodySprite;
		}
	}

	public void SetColor(Color color)
	{
		if (SRs[0].color != color)
		{
			for (int i = 0; i < SRs.Count; i++)
			{
				SRs[i].color = color;
			}
		}
	}

	public override void EveryInitialCallback()
	{
		tsf_Body.transform.localPosition = new Vector3(0f, bodyHeight, 0f - bodyHeight);
	}

	public void SetPositionAndDir(Vector3 position, Vector3 dir)
	{
		base.transform.position = position;
		tsf_Body.up = dir;
	}

	public void SetHandDir(Vector3 dir, float rotateAngle)
	{
		tsf_BodyShadow.transform.position = Tool2D.GetLayerPoint(base.transform.position, LayerCorrectType.Shadow);
		tsf_LeftLeg.localEulerAngles = new Vector3(0f, 0f, rotateAngle);
		tsf_RightLeg.localEulerAngles = new Vector3(0f, 0f, 0f - rotateAngle);
		tsf_LeftLegShadow.localEulerAngles = new Vector3(0f, 0f, rotateAngle);
		tsf_RightLegShadow.localEulerAngles = new Vector3(0f, 0f, 0f - rotateAngle);
	}

	public override void Update()
	{
		myPpt.unitCfg.currentHP = myPpt.unitCfg.maxHP;
		base.Update();
	}

	public override void BeforeTakeDamage(TakeDamageInfo info)
	{
		base.BeforeTakeDamage(info);
		if (info.spellBase != null)
		{
			if (!hitList.Contains(info.spellBase))
			{
				myPpt.TakeBeHit(info.spellBase.Direction);
				info.beHitShake = false;
				master.myPpt.TakeDamage(info.spellBase, info);
				hitList.Add(info.spellBase);
			}
			else
			{
				info.immuneDamage = true;
			}
		}
	}
}
