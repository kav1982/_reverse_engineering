using UnityEngine;
using UnityEngine.UI;

public class UIEndlessEliteHpBar : MonoBehaviour
{
	public Image HpBar;

	public Text hpText;

	public UnitBase targetUnit;

	public Transform targetModel;

	public void Initialize(UnitBase target)
	{
		targetUnit = target;
		targetModel = targetUnit.myPpt.Tsf_BeHit.GetChild(0);
		if (PlayerMgr.Inst.ItemCtrller.relicCfg_ShowUnitHPUI == null || PlayerMgr.Inst.ItemCtrller.relicCfg_ShowUnitHPUI.level < 2)
		{
			hpText.gameObject.SetActive(value: false);
		}
		else
		{
			hpText.gameObject.SetActive(value: true);
		}
	}

	private void Update()
	{
		base.transform.position = Tool2D.GetLayerPoint(targetUnit.transform.position + Vector3.back * (targetModel.localPosition.y + targetUnit.myPpt.unitCfg.relicShowHPUIHight)) + Vector3.back * 0.05f;
		if (!targetUnit.deadStayed)
		{
			UnitConfig unitCfg = targetUnit.GetComponentData<UnitProperty_Dots>().unitCfg;
			hpText.text = ((float)Mathf.FloorToInt(Mathf.Max(0f, unitCfg.currentHP))).FormatWithUnit() + "/" + unitCfg.maxHP.FormatWithUnit();
			HpBar.fillAmount = unitCfg.currentHP / unitCfg.maxHP;
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
