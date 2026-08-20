using DG.Tweening;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

[GameUISingletonPrefab("UIProcessInOne_Controller")]
public class UIProcessInOne_Controller : GameUISingletonMono<UIProcessInOne_Controller>
{
	public enum UIProcessInOneType
	{
		Compound,
		Reroll,
		MoreInOne,
		RerollRelic,
		Sell
	}

	public GameObject uiRoot;

	public CanvasGroup canvasGroup;

	public UIProcessInOneType currentControllerType;

	public UIMobileReturnAndRess uiReturnAndRess;

	public UIProcessInOne_Catergorys catergorys;

	public UIProcessInOne_ItemContainer itemContainer;

	public UIProcessInOne_Process processer;

	public Entity currentEntity = Entity.Null;

	[Header("选择相关")]
	public UIInfoSpell uiinfoSpell;

	public UIInfoRelic uiinfoRelic;

	public UIInfoPotion uiinfoPotion;

	[Header("模糊")]
	public int blurHeight = 512;

	public SpriteBlurCore blurCore = new SpriteBlurCore();

	private RenderTexture originalRT;

	private RenderTexture blurredRT;

	public RawImage blurBG;

	public int rerollReolcCounter { get; set; }

	public UIProcessInOne_Item currentSelectedItemSlot { get; set; }

	public override void Hide()
	{
		if (!processer.Processing)
		{
			base.Hide();
		}
	}

	protected override void OnShow(object obj = null)
	{
		if (obj is Entity entity)
		{
			InteractiveObj_Dots componentData = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<InteractiveObj_Dots>(entity);
			currentEntity = entity;
			switch (componentData.type)
			{
			case InteractiveObjType.SO101MoreInOne:
				SetProcessInOne(UIProcessInOneType.MoreInOne);
				break;
			case InteractiveObjType.SO101Reroll:
				SetProcessInOne(UIProcessInOneType.Reroll);
				break;
			case InteractiveObjType.SO101Compound:
				SetProcessInOne(UIProcessInOneType.Compound);
				break;
			case InteractiveObjType.SpecialObj21:
				SetProcessInOne(UIProcessInOneType.Sell);
				break;
			}
		}
		else if (obj is UIProcessInOneType processInOne)
		{
			SetProcessInOne(processInOne);
		}
		base.OnShow(obj);
		BlurBG();
		canvasGroup.alpha = 0f;
		canvasGroup.DOFade(1f, 0.3f).SetUpdate(isIndependentUpdate: true);
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		Time.timeScale = 0f;
	}

	public void BlurBG()
	{
		int num = blurHeight;
		float num2 = (float)Screen.width / (float)Screen.height;
		int width = Mathf.CeilToInt((float)num * num2);
		if (!blurCore.init)
		{
			blurCore.Init(width, num);
			originalRT = new RenderTexture(width, num, 16, RenderTextureFormat.ARGB32);
			originalRT.Create();
			blurredRT = new RenderTexture(width, num, 0, RenderTextureFormat.ARGB32);
			blurredRT.Create();
		}
		Camera.main.targetTexture = originalRT;
		Camera.main.Render();
		blurCore.ApplyBlur(originalRT, blurredRT);
		Camera.main.targetTexture = null;
		blurBG.texture = blurredRT;
	}

	public void SetProcessInOne(UIProcessInOneType type)
	{
		Debug.Log(type);
		currentControllerType = type;
		processer.DisSelectAll();
		uiRoot.SetActive(value: true);
		catergorys.CreatCatergorys();
		switch (GameUISingletonMono<UIProcessInOne_Controller>.Inst.currentControllerType)
		{
		case UIProcessInOneType.Compound:
			itemContainer.numLimit = null;
			break;
		case UIProcessInOneType.Reroll:
			itemContainer.numLimit = 1;
			break;
		case UIProcessInOneType.MoreInOne:
			itemContainer.numLimit = 4;
			break;
		case UIProcessInOneType.RerollRelic:
			itemContainer.numLimit = 1;
			rerollReolcCounter = 0;
			break;
		case UIProcessInOneType.Sell:
			itemContainer.numLimit = null;
			break;
		}
		uiReturnAndRess.Show(this);
		processer.Init();
	}

	protected override void OnHide()
	{
		Debug.Log("OnHide");
		Time.timeScale = 1f;
		PlayerMgr.Inst.PlayerCtrller.StartMotion();
		uiRoot.SetActive(value: false);
		uiReturnAndRess.Hide();
		processer.DisSelectAll();
		itemContainer.allItemsData.Clear();
		itemContainer.numLimit = null;
		itemContainer.idOnly = null;
		HideInfoPanels();
	}

	public override void OnDestroy()
	{
		originalRT?.Release();
		blurredRT?.Release();
		blurCore.ReleaseWeight();
		base.OnDestroy();
	}

	protected override void RegistarWhenInit()
	{
	}

	protected override void RegistarOnlyWhenOpen()
	{
	}

	protected override void UnRegistarOnlyWhenHide()
	{
	}

	protected override void UnRegistarWhenDestroy()
	{
	}

	public void UpdateShowAll()
	{
		GameUISingletonMono<UIProcessInOne_Controller>.Inst.catergorys.UpdateCategoryShow();
		GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.UpdateCurrentSlots();
		GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.GenerateCanCompoundList();
		GameUISingletonMono<UIProcessInOne_Controller>.Inst.processer.UpdateSelectedItems();
		GameUISingletonMono<UIProcessInOne_Controller>.Inst.itemContainer.UpdateSlots();
	}

	public void HideInfoPanels()
	{
		uiinfoSpell.gameObject.SetActive(value: false);
		uiinfoPotion.gameObject.SetActive(value: false);
		uiinfoRelic.gameObject.SetActive(value: false);
		currentSelectedItemSlot = null;
	}
}
