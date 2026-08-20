using System;
using UnityEngine;
using UnityEngine.UI;

public class UISpellFly : MonoBehaviour
{
	public Canvas spellCanvas;

	public Image image_Icon;

	public Image image_Spelllevel2Star;

	public Image image_Spelllevel3Star;

	private Action OnFlyFinish;

	private UISlotBag flyToSlotBag;

	private UISlotWand flyToSlotWand;

	private Vector3 lastFrameTargetPosition;

	private int spellID;

	private float flySpeed;

	private void Update()
	{
		if (flyToSlotBag != null)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, flyToSlotBag.transform.position, flySpeed * Time.unscaledDeltaTime);
			if (base.transform.position == flyToSlotBag.transform.position)
			{
				flyToSlotBag.image_Icon.gameObject.SetActive(value: true);
				if (SpellConfig.dic[spellID].level >= 2)
				{
					flyToSlotBag.image_Star1.gameObject.SetActive(value: true);
				}
				if (SpellConfig.dic[spellID].level >= 3)
				{
					flyToSlotBag.image_Star2.gameObject.SetActive(value: true);
				}
				OnFlyFinish?.Invoke();
				OnFlyFinish = null;
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
		}
		else if (flyToSlotWand != null && flyToSlotWand.gameObject.activeInHierarchy)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, flyToSlotWand.transform.position, flySpeed * Time.unscaledDeltaTime);
			lastFrameTargetPosition = flyToSlotWand.transform.position;
			if (base.transform.position == flyToSlotWand.transform.position)
			{
				UIPlayerDataMgr.Inst.WandUpdate(flyToSlotWand.WandIndex);
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
		}
		else if (lastFrameTargetPosition != Vector3.zero)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, lastFrameTargetPosition, flySpeed * Time.unscaledDeltaTime);
			if (base.transform.position == lastFrameTargetPosition)
			{
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
		}
		else
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}

	public void Initialize(int spellID, UISlotBag flyToSlotBag)
	{
		lastFrameTargetPosition = Vector3.zero;
		this.flyToSlotBag = flyToSlotBag;
		flyToSlotBag.HideIcon();
		OnFlyFinish = flyToSlotBag.ShowIcon;
		flySpeed = Vector3.Distance(base.transform.position, flyToSlotBag.transform.position) / 0.1f;
		flyToSlotWand = null;
		FinalInitialize(spellID);
	}

	public void Initialize(int spellID, UISlotWand flyToSlotWand, Vector3? targetPos = null)
	{
		flyToSlotBag = null;
		this.flyToSlotWand = flyToSlotWand;
		flyToSlotWand.HideIcon();
		lastFrameTargetPosition = targetPos ?? flyToSlotWand.transform.position;
		flySpeed = Vector3.Distance(base.transform.position, lastFrameTargetPosition) / 0.1f;
		FinalInitialize(spellID);
	}

	private void FinalInitialize(int spellID)
	{
		this.spellID = spellID;
		SpellConfig spellConfig = SpellConfig.dic[spellID];
		image_Icon.sprite = ABResources.LoadAsset<Sprite>(spellConfig.GetIconPath());
		if (spellConfig.level >= 2)
		{
			image_Spelllevel2Star.gameObject.SetActive(value: true);
		}
		else
		{
			image_Spelllevel2Star.gameObject.SetActive(value: false);
		}
		if (spellConfig.level >= 3)
		{
			image_Spelllevel3Star.gameObject.SetActive(value: true);
		}
		else
		{
			image_Spelllevel3Star.gameObject.SetActive(value: false);
		}
		SEMgr.Inst.uiFly.PlaySE();
	}
}
