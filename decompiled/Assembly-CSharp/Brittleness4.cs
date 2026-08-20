using Unity.Entities;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Brittleness4 : UnitBase, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Space(50f)]
	public SpriteRenderer[] srs;

	public SpriteRenderer[] sr_NeedFlips;

	public Sprite[] sprite_Deads;

	public VariableInt copyCount;

	public VariableInt mobileCopyCount;

	public VariableFloat copyOffset;

	public SpriteSkin[] allSpriteSkins;

	[Header("和谐")]
	public bool needHarmonize;

	public Sprite[] sprite_H;

	public Sprite[] sprite_Deads_H;

	private int index;

	private bool createByOther;

	private bool immuneDamage;

	public Entity thisEntity { get; set; }

	public override void EveryInitialCallback()
	{
		base.Anima.enabled = false;
		for (int i = 0; i < allSpriteSkins.Length; i++)
		{
			allSpriteSkins[i].enabled = false;
		}
		createByOther = false;
		immuneDamage = false;
		if (needHarmonize && GameMgr.IsHarmony_Static)
		{
			for (int j = 0; j < srs.Length; j++)
			{
				srs[j].sprite = sprite_H[j];
			}
			for (int k = 0; k < sprite_Deads_H.Length; k++)
			{
				sprite_Deads[k] = sprite_Deads_H[k];
			}
		}
	}

	public override void Frame1InitialCallback()
	{
		if (!createByOther)
		{
			copyCount.RandomResult();
			mobileCopyCount.RandomResult();
			int num = (GameMgr.IsMobile_Static ? mobileCopyCount.result : copyCount.result);
			for (int i = 0; i < num; i++)
			{
				Vector3 point = base.transform.position + Tool2D.GetDir(360f / (float)num * (float)i) * copyOffset.RandomResult();
				if (i == num - 1)
				{
					base.transform.position += Tool2D.GetDir() * copyOffset.RandomResult();
					myPpt.CorrectLayerOnce();
				}
				else
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + myPpt.unitCfg.id, point).GetComponent<Brittleness4>().MarkCreateByOther();
				}
			}
		}
		index = Random.Range(0, srs.Length);
		for (int j = 0; j < srs.Length; j++)
		{
			srs[j].gameObject.SetActive(j == index);
		}
		for (int k = 0; k < sr_NeedFlips.Length; k++)
		{
			SetSingleFlip(sr_NeedFlips[k], Random.Range(0, 2));
		}
	}

	public override void Frame2InitialCallback()
	{
		base.enabled = false;
		myPpt.enabled = false;
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		info.immuneDamage = immuneDamage;
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		SpriteRenderer spriteRenderer = Object.Instantiate(srs[index], base.transform.position, Quaternion.identity, LevelMgr.Inst.CurrentRoomT);
		spriteRenderer.sprite = sprite_Deads[index];
		spriteRenderer.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Tile9_AboveAO);
	}

	public void MarkCreateByOther()
	{
		createByOther = true;
	}

	public void MarkImmuneDamage()
	{
		immuneDamage = true;
	}

	public override void AnimaAction(string animaName)
	{
		if (animaName == "ShakeDone")
		{
			base.Anima.enabled = false;
			for (int i = 0; i < allSpriteSkins.Length; i++)
			{
				allSpriteSkins[i].enabled = false;
			}
		}
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		base.Anima.enabled = true;
		for (int i = 0; i < allSpriteSkins.Length; i++)
		{
			allSpriteSkins[i].enabled = true;
		}
		base.Anima.SetTrigger("Shake");
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}
}
