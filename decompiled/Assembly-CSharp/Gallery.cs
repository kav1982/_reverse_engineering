using UnityEngine;

public class Gallery : InteractiveObj
{
	[Space(50f)]
	public GameObject go_Outline;

	public override void Select()
	{
		go_Outline.SetActive(value: true);
	}

	public override void Unselect()
	{
		go_Outline.SetActive(value: false);
	}

	public override void Interact()
	{
		if (UICampMgr.Inst.uiGallery == null)
		{
			GameUISingletonPrefab gameUISingletonPrefab = (GameUISingletonPrefab)typeof(UIGallery).GetCustomAttributes(typeof(GameUISingletonPrefab), inherit: false)[0];
			UICampMgr.Inst.uiGallery = GameMgr.Inst.LoadUIObj(gameUISingletonPrefab.prefabPath).GetComponent<UIGallery>();
			UICampMgr.Inst.uiGallery.ShowInitFromIneract();
		}
		else
		{
			UICampMgr.Inst.uiGallery.ShowInitFromIneract();
		}
	}
}
