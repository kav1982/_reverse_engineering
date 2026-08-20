using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIWandSelectPopOut : GameUI
{
	private static readonly int Angle = Shader.PropertyToID("_Angle");

	public float radios;

	public Vector2 centerPoint;

	public float angleStart;

	public float angleEnd;

	public GameObject UIObj;

	public GameObject UIWandPrefab;

	public GameObject UIPotionLineRect;

	public GameObject UIPotionLinePrefab;

	public GameObject layout;

	public Image popOutImage;

	public Image popOutImage2;

	public float outlineAngleAdd = 4f;

	private float dir => (angleEnd > angleStart) ? 1 : (-1);

	protected override void OnShow(object obj = null)
	{
		TimeScaleMgr.Inst.Pause();
		layout.transform.DestroyAllChild();
		UIPotionLineRect.transform.DestroyAllChild();
		float num = Mathf.Abs(angleEnd - angleStart) / (float)PlayerMgr.Inst.BaData.wandMaxCount;
		popOutImage.material.SetFloat(Angle, num - outlineAngleAdd);
		popOutImage2.material.SetFloat(Angle, num);
		for (int i = 0; i < PlayerMgr.Inst.BaData.wandMaxCount; i++)
		{
			WandConfig wandConfig = DataMgr.selectedWorldData.battleData9.wandCfgs[i];
			GameObject obj2 = UnityEngine.Object.Instantiate(UIPotionLinePrefab, UIPotionLineRect.transform);
			obj2.transform.localEulerAngles = new Vector3(0f, 0f, angleStart + num * (float)i * dir);
			obj2.transform.localScale = new Vector3(1f, 1f, 1f);
			obj2.SetActive(value: true);
			GameObject gameObject = UnityEngine.Object.Instantiate(UIWandPrefab, layout.transform);
			if (wandConfig == null)
			{
				gameObject.SetActive(value: false);
				continue;
			}
			gameObject.SetActive(value: true);
			int id = wandConfig.id;
			Image[] componentsInChildren = gameObject.GetComponentsInChildren<Image>(includeInactive: true);
			float num2 = angleStart + num * (float)i * dir + num / 2f * dir;
			gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(radios * Mathf.Cos(num2 * (MathF.PI / 180f)), radios * Mathf.Sin(num2 * (MathF.PI / 180f))) + centerPoint;
			if (id != 0)
			{
				Sprite sprite = ABResources.LoadAsset<Sprite>(WandConfig.dic[id].GetIconPath());
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
		UIObj.SetActive(value: true);
	}

	public void UpdateSelectedWand()
	{
		float num = (angleEnd - angleStart) / (float)PlayerMgr.Inst.BaData.wandMaxCount;
		popOutImage.transform.localEulerAngles = new Vector3(0f, 0f, angleStart + 180f + dir * num * (float)(PlayerMgr.Inst.SelectedWandIndex + 1) - outlineAngleAdd / 2f);
		popOutImage2.transform.localEulerAngles = new Vector3(0f, 0f, angleStart + 180f + dir * num * (float)(PlayerMgr.Inst.SelectedWandIndex + 1));
	}

	protected override void OnHide()
	{
		TimeScaleMgr.Inst.Recovery();
		UIObj.SetActive(value: false);
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
			UpdateSelectedWand();
		}
	}
}
