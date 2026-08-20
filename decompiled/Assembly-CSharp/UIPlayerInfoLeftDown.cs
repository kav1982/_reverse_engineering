using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfoLeftDown : MonoBehaviour
{
	public enum UIType
	{
		Normal,
		Dave
	}

	public UIType type;

	[Header("HP")]
	public RectTransform rtsf_HP1;

	public Image hpFillDave;

	public RectTransform rtsf_HP2;

	public Slider slider_HP1;

	public Slider slider_HP2;

	public TextMeshProUGUI tmp_HP;

	public float perHPWidth;

	public float hpWidthMoveSpeed;

	public float hpRatioMoveSpeed;

	public float maxHPAndShieldWidth;

	public float minHPSHieldWidth = 10f;

	[Header("Shield")]
	public RectTransform rtsf_Shield;

	public TextMeshProUGUI tmp_Shield;

	public RectTransform rtsf_ShieldTemp;

	public TextMeshProUGUI tmp_ShieldTemp;

	public float shieldSpaceWithHP;

	public float shieldMoveSpeed;

	[Header("MP")]
	public GameObject panel_MP;

	public RectTransform rtsf_MP1;

	public RectTransform rtsf_MP2;

	public RectTransform rtsf_MPWarning;

	public Animator anima_MPWarning;

	public Slider slider_MP1;

	public Slider slider_MP2;

	public TextMeshProUGUI tmp_MP;

	public float mobileSizeMultiply = 0.8f;

	public UnitConfig PlayerCfg;

	public void HPShieldCheck()
	{
		float num = PlayerCfg.maxHP * perHPWidth;
		float num2 = PlayerCfg.shield * perHPWidth;
		float num3 = PlayerCfg.shieldTemp * perHPWidth;
		float num4 = num + num2 + num3;
		float num5 = maxHPAndShieldWidth;
		if (num2 > 0f)
		{
			num5 -= shieldSpaceWithHP;
		}
		if (num3 > 0f)
		{
			num5 -= shieldSpaceWithHP;
		}
		if (num4 > num5)
		{
			num = num / num4 * num5;
			num2 = num2 / num4 * num5;
			num3 = num3 / num4 * num5;
		}
		float num6 = 0f;
		if (num > 0f && num < minHPSHieldWidth)
		{
			num6 += minHPSHieldWidth - num;
			num = minHPSHieldWidth;
		}
		if (num2 > 0f && num2 < minHPSHieldWidth)
		{
			num6 += minHPSHieldWidth - num2;
			num2 = minHPSHieldWidth;
		}
		if (num3 > 0f && num3 < minHPSHieldWidth)
		{
			num6 += minHPSHieldWidth - num3;
			num3 = minHPSHieldWidth;
		}
		if (num6 > 0f && num4 > num5)
		{
			float num7 = 0f;
			if (num > minHPSHieldWidth)
			{
				num7 += num - minHPSHieldWidth;
			}
			if (num2 > minHPSHieldWidth)
			{
				num7 += num2 - minHPSHieldWidth;
			}
			if (num3 > minHPSHieldWidth)
			{
				num7 += num3 - minHPSHieldWidth;
			}
			if (num7 > 0f)
			{
				if (num > minHPSHieldWidth)
				{
					float num8 = (num - minHPSHieldWidth) / num7 * num6;
					num -= num8;
				}
				if (num2 > minHPSHieldWidth)
				{
					float num9 = (num2 - minHPSHieldWidth) / num7 * num6;
					num2 -= num9;
				}
				if (num3 > minHPSHieldWidth)
				{
					float num10 = (num3 - minHPSHieldWidth) / num7 * num6;
					num3 -= num10;
				}
			}
		}
		switch (type)
		{
		case UIType.Normal:
		{
			if (!Mathf.Approximately(rtsf_HP1.sizeDelta.x, num))
			{
				float x = Mathf.MoveTowards(rtsf_HP1.sizeDelta.x, num, hpWidthMoveSpeed * Time.unscaledDeltaTime);
				rtsf_HP1.sizeDelta = new Vector2(x, rtsf_HP1.sizeDelta.y);
				rtsf_HP2.sizeDelta = rtsf_HP1.sizeDelta;
			}
			float num11 = PlayerCfg.currentHP / PlayerCfg.maxHP;
			if (!Mathf.Approximately(slider_HP1.value, num11))
			{
				slider_HP1.value = Mathf.MoveTowards(slider_HP1.value, num11, hpRatioMoveSpeed * Time.unscaledDeltaTime);
			}
			if (!Mathf.Approximately(slider_HP2.value, num11))
			{
				slider_HP2.value = num11;
			}
			break;
		}
		case UIType.Dave:
			hpFillDave.material.SetFloat("_Percent", PlayerCfg.currentHP / PlayerCfg.maxHP);
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		if (PlayerCfg.shield > 0f)
		{
			rtsf_Shield.gameObject.SetActive(value: true);
			float num12 = rtsf_HP1.anchoredPosition.x + (rtsf_HP1.sizeDelta.x + shieldSpaceWithHP) * (GameMgr.IsMobile_Static ? mobileSizeMultiply : 1f);
			if (!Mathf.Approximately(rtsf_Shield.anchoredPosition.x, num12))
			{
				rtsf_Shield.anchoredPosition = new Vector2(num12, rtsf_Shield.anchoredPosition.y);
			}
			if (!Mathf.Approximately(rtsf_Shield.sizeDelta.x, num2))
			{
				float x2 = Mathf.MoveTowards(rtsf_Shield.sizeDelta.x, num2, shieldMoveSpeed * Time.unscaledDeltaTime);
				rtsf_Shield.sizeDelta = new Vector2(x2, rtsf_Shield.sizeDelta.y);
			}
		}
		else if (PlayerCfg.shield == 0f && rtsf_Shield.gameObject.activeSelf)
		{
			rtsf_Shield.gameObject.SetActive(value: false);
			rtsf_Shield.sizeDelta = new Vector2(0f, rtsf_Shield.sizeDelta.y);
		}
		if (PlayerCfg.shieldTemp > 0f)
		{
			rtsf_ShieldTemp.gameObject.SetActive(value: true);
			float num13 = rtsf_HP1.anchoredPosition.x + (rtsf_HP1.sizeDelta.x + shieldSpaceWithHP) * (GameMgr.IsMobile_Static ? mobileSizeMultiply : 1f);
			if (rtsf_Shield.gameObject.activeSelf)
			{
				num13 += (rtsf_Shield.sizeDelta.x + shieldSpaceWithHP) * (GameMgr.IsMobile_Static ? mobileSizeMultiply : 1f);
			}
			if (!Mathf.Approximately(rtsf_ShieldTemp.anchoredPosition.x, num13))
			{
				rtsf_ShieldTemp.anchoredPosition = new Vector2(num13, rtsf_ShieldTemp.anchoredPosition.y);
			}
			if (!Mathf.Approximately(rtsf_ShieldTemp.sizeDelta.x, num3))
			{
				float x3 = Mathf.MoveTowards(rtsf_ShieldTemp.sizeDelta.x, num3, shieldMoveSpeed * Time.unscaledDeltaTime);
				rtsf_ShieldTemp.sizeDelta = new Vector2(x3, rtsf_ShieldTemp.sizeDelta.y);
			}
		}
		else if (PlayerCfg.shieldTemp == 0f && rtsf_ShieldTemp.gameObject.activeSelf)
		{
			rtsf_ShieldTemp.gameObject.SetActive(value: false);
			rtsf_ShieldTemp.sizeDelta = new Vector2(0f, rtsf_ShieldTemp.sizeDelta.y);
		}
	}

	public void MPCheck()
	{
		if (PlayerMgr.Inst.SelectedWandCfg == null)
		{
			if (panel_MP.activeSelf)
			{
				panel_MP.SetActive(value: false);
			}
		}
		else
		{
			if (PlayerMgr.Inst.SelectedWand == null)
			{
				return;
			}
			if (PlayerMgr.Inst.SelectedWand.MaxMP <= 0f)
			{
				panel_MP.SetActive(value: false);
				return;
			}
			if (!panel_MP.activeSelf)
			{
				panel_MP.SetActive(value: true);
			}
			float num = PlayerMgr.Inst.SelectedWand.MaxMP * perHPWidth;
			if (num > maxHPAndShieldWidth)
			{
				num = maxHPAndShieldWidth;
			}
			if (!Mathf.Approximately(rtsf_MP1.sizeDelta.x, num))
			{
				rtsf_MP1.sizeDelta = new Vector2(num, rtsf_MP1.sizeDelta.y);
				rtsf_MP2.sizeDelta = rtsf_MP1.sizeDelta;
			}
			float currentManaPercent = PlayerMgr.Inst.SelectedWand.GetCurrentManaPercent();
			if (!Mathf.Approximately(slider_MP1.value, currentManaPercent))
			{
				slider_MP1.value = Mathf.MoveTowards(slider_MP1.value, currentManaPercent, hpRatioMoveSpeed * Time.unscaledDeltaTime);
			}
			if (!Mathf.Approximately(slider_MP2.value, currentManaPercent))
			{
				slider_MP2.value = currentManaPercent;
			}
			if (UIPlayerDataMgr.Inst.isHoverMP)
			{
				tmp_MP.text = ((int)PlayerMgr.Inst.SelectedWand.CurrentMP).ToString("F0") + "/" + ((int)PlayerMgr.Inst.SelectedWand.MaxMP).ToString("F0");
			}
			else
			{
				tmp_MP.text = ((int)PlayerMgr.Inst.SelectedWand.CurrentMP).ToString("F0");
			}
		}
	}

	public void RecorrectHPMPShieldWidthDirect()
	{
		if (PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt))
		{
			PlayerCfg = playerPpt.unitCfg;
			float num = hpWidthMoveSpeed;
			float num2 = hpRatioMoveSpeed;
			float num3 = shieldMoveSpeed;
			hpWidthMoveSpeed = 100000000f;
			hpRatioMoveSpeed = 100000000f;
			shieldMoveSpeed = 100000000f;
			HPShieldCheck();
			hpWidthMoveSpeed = num;
			hpRatioMoveSpeed = num2;
			shieldMoveSpeed = num3;
		}
	}

	public void UpdateHP()
	{
		if (UIPlayerDataMgr.Inst.isHoverHP || GameMgr.IsMobile_Static)
		{
			tmp_HP.text = PlayerCfg.currentHP.ToStringHP() + "/" + PlayerCfg.maxHP;
		}
		else
		{
			tmp_HP.text = PlayerCfg.currentHP.ToStringHP();
		}
	}

	public void UpdateMP()
	{
		float currentManaPercent = PlayerMgr.Inst.SelectedWand.GetCurrentManaPercent();
		slider_MP1.value = currentManaPercent;
		slider_MP2.value = currentManaPercent;
	}

	public void MPWarning()
	{
		anima_MPWarning.SetTrigger("start");
	}

	public void UpdateShield()
	{
		if (PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt))
		{
			tmp_Shield.text = playerPpt.unitCfg.shield.ToString("F0");
		}
	}

	public void UpdateShieldTemp()
	{
		if (PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt))
		{
			tmp_ShieldTemp.text = playerPpt.unitCfg.shieldTemp.ToString("F0");
		}
	}
}
