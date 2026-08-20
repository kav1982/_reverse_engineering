using Unity.Entities;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Brittleness1 : UnitBase, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Space(50f)]
	public SpriteRenderer[] srs;

	public Sprite[] sprite_Deads;

	public Brittleness1DeadEFType[] deadEFType;

	public VariableInt copyCount;

	public VariableInt mobileCopyCount;

	public VariableFloat copyOffset;

	public SpriteSkin[] allSpriteSkins;

	[Header("戴夫水草阴影")]
	public bool isDave;

	public SpriteRenderer[] srDaveShadows;

	public Material mt_NODR;

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
					SyncDotsPosition();
				}
				else
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + myPpt.unitCfg.id, point).GetComponent<Brittleness1>().MarkCreateByOther();
				}
			}
		}
		index = Random.Range(0, srs.Length);
		for (int j = 0; j < srs.Length; j++)
		{
			srs[j].gameObject.SetActive(j == index);
			if (isDave)
			{
				srDaveShadows[j].gameObject.SetActive(j == index);
			}
		}
	}

	public override void Frame2InitialCallback()
	{
		base.enabled = false;
		myPpt.enabled = false;
	}

	public override void BeforeTakeDamage_Dots(ref TakeDamageInfo_Dots info)
	{
		info.ignoreFloatText = true;
		info.immuneDamage = immuneDamage;
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		switch (deadEFType[index])
		{
		case Brittleness1DeadEFType.Grass:
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_Grass", base.transform.position, 2f);
			break;
		case Brittleness1DeadEFType.Flower:
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_Flower", base.transform.position, 2f);
			break;
		case Brittleness1DeadEFType.DullGrass:
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_DullGrass", base.transform.position, 2f);
			break;
		case Brittleness1DeadEFType.SeaWeed:
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_SeaWeed", base.transform.position, 2f);
			break;
		case Brittleness1DeadEFType.Coral:
			ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Dead_Coral", base.transform.position, 2f);
			break;
		default:
			Debug.LogError(deadEFType);
			break;
		}
		SpriteRenderer spriteRenderer = Object.Instantiate(srs[index], base.transform.position, Quaternion.identity, LevelMgr.Inst.CurrentRoomT);
		spriteRenderer.sprite = sprite_Deads[index];
		spriteRenderer.transform.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Tile9_AboveAO);
		if (isDave)
		{
			Object.Destroy(spriteRenderer.material);
			spriteRenderer.material = mt_NODR;
		}
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

	public void OnTriggerEnter_Dots(Entity other)
	{
		if (UnitDotsSyncSystem.HasComponent<UnitProperty_Dots>(other))
		{
			base.Anima.enabled = true;
			for (int i = 0; i < allSpriteSkins.Length; i++)
			{
				allSpriteSkins[i].enabled = true;
			}
			base.Anima.SetTrigger("Shake");
		}
	}

	public void OnTriggerStay_Dots(Entity other)
	{
	}

	public void OnTriggerExit_Dots(Entity other)
	{
	}
}
