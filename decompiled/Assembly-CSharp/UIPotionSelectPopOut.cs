using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIPotionSelectPopOut : GameUI
{
	private static readonly int Angle = Shader.PropertyToID("_Angle");

	public float radios;

	public Vector2 centerPoint;

	public float angleStart;

	public float angleEnd;

	public GameObject UIObj;

	public GameObject potionDropObj;

	public GameObject UIPotionSelect;

	public GameObject UIPotionLineRect;

	public GameObject UIPotionLinePrefab;

	public GameObject layout;

	public Image popOutImage;

	public Image popOutImage2;

	public float outlineAngleAdd = 4f;

	protected override void OnShow(object obj = null)
	{
		TimeScaleMgr.Inst.Pause();
		RefreshPotionPopOut();
		UIObj.SetActive(value: true);
		potionDropObj.SetActive(value: true);
	}

	public void RefreshPotionPopOut()
	{
		layout.transform.DestroyAllChild();
		UIPotionLineRect.transform.DestroyAllChild();
		float num = (angleEnd - angleStart) / (float)PlayerMgr.Inst.BaData.potionMaxCount;
		popOutImage.material.SetFloat(Angle, num - outlineAngleAdd);
		popOutImage2.material.SetFloat(Angle, num);
		for (int i = 0; i < PlayerMgr.Inst.BaData.potionMaxCount; i++)
		{
			GameObject obj = UnityEngine.Object.Instantiate(UIPotionSelect, layout.transform);
			if (i != 0)
			{
				GameObject obj2 = UnityEngine.Object.Instantiate(UIPotionLinePrefab, UIPotionLineRect.transform);
				obj2.transform.localEulerAngles = new Vector3(0f, 0f, angleStart + num * (float)i);
				obj2.transform.localScale = new Vector3(1f, 1f, 1f);
				obj2.SetActive(value: true);
			}
			obj.SetActive(value: true);
			int num2 = PlayerMgr.Inst.BaData.potionIDs[i];
			Image[] componentsInChildren = obj.GetComponentsInChildren<Image>(includeInactive: true);
			float num3 = angleStart + num * (float)i + num / 2f;
			obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(radios * Mathf.Cos(num3 * (MathF.PI / 180f)), radios * Mathf.Sin(num3 * (MathF.PI / 180f))) + centerPoint;
			if (num2 != 0)
			{
				Sprite sprite = ABResources.LoadAsset<Sprite>(PotionConfig.dic[num2].GetIconPath());
				componentsInChildren.ToList().ForEach(delegate(Image x)
				{
					x.enabled = true;
					x.sprite = sprite;
				});
			}
			else
			{
				componentsInChildren.ToList().ForEach(delegate(Image x)
				{
					x.enabled = false;
				});
			}
		}
	}

	public void UpdateSelectedPotion()
	{
		float num = (angleEnd - angleStart) / (float)PlayerMgr.Inst.BaData.potionMaxCount;
		popOutImage.transform.localEulerAngles = new Vector3(0f, 0f, angleStart + num * (float)(PlayerMgr.Inst.ItemCtrller.SelectedPotionIndex + 1) + 180f - outlineAngleAdd / 2f);
		popOutImage2.transform.localEulerAngles = new Vector3(0f, 0f, angleStart + num * (float)(PlayerMgr.Inst.ItemCtrller.SelectedPotionIndex + 1) + 180f);
	}

	protected override void OnHide()
	{
		TimeScaleMgr.Inst.Recovery();
		UIObj.SetActive(value: false);
		potionDropObj.SetActive(value: false);
		popOutImage.material.SetFloat(Angle, 0f);
		popOutImage2.material.SetFloat(Angle, 0f);
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

	private void Update()
	{
		if (GameMgr.IsMobile_Static && PlayerMgr.Inst != null && PlayerMgr.Inst.BaData != null)
		{
			UpdateSelectedPotion();
		}
	}
}
