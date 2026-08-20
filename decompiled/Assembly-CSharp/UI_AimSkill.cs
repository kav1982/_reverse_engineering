using UnityEngine;
using UnityEngine.EventSystems;

public class UI_AimSkill : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler, IPointerUpHandler
{
	public GameObject dirObj;

	public float maxDistance = 0.002f;

	private Vector2 startPosition;

	public Vector3 aimDir;

	public bool useSkillDir;

	public float finalDistance;

	public bool skillCancle;

	public GameObject skillCancleObj;

	public bool isButtonDown;

	public float aimStartDuration;

	private float pressDuration;

	public bool isAiming;

	private void Update()
	{
		if (isButtonDown)
		{
			pressDuration += Time.deltaTime;
			if (pressDuration > aimStartDuration && !isAiming)
			{
				isAiming = true;
				useSkillDir = true;
				skillCancleObj.SetActive(value: true);
			}
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!UIMgr.Inst.uiSetting.customMobileControl.activeInHierarchy)
		{
			aimDir = eventData.position - startPosition;
			dirObj.transform.position = base.transform.position + aimDir * maxDistance;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!UIMgr.Inst.uiSetting.customMobileControl.activeInHierarchy)
		{
			startPosition = eventData.position;
			dirObj.transform.position = base.transform.position;
			ControlMgr.Inst.SprintPerformed();
			isButtonDown = true;
			isAiming = false;
			pressDuration = 0f;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (UIMgr.Inst.uiSetting.customMobileControl.activeInHierarchy)
		{
			return;
		}
		if (isAiming)
		{
			if (PlayerMgr.Inst.ItemCtrller.GetRelicConfig(938) == null)
			{
				useSkillDir = false;
			}
			skillCancleObj.SetActive(value: false);
		}
		dirObj.transform.position = base.transform.position;
		ControlMgr.Inst.SprintCanceld();
		finalDistance = Tool2D.IgnoreZDistanceSqr(eventData.position, startPosition);
		isButtonDown = false;
	}
}
