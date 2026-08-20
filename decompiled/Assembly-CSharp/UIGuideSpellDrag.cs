using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIGuideSpellDrag : MonoBehaviour
{
	private enum UIState
	{
		HideDirect,
		Show,
		Move,
		Hide
	}

	public Image mobilePointerClick;

	public Sprite clickDown;

	public Sprite clickUp;

	public float mobileStopClickTime = 0.2f;

	public float mobilePointerDisappearTime = 0.2f;

	private float _mobilePointerDisappearTime;

	private Sequence Sequence;

	private float _mobileStopClickTime;

	public Animator anima;

	public float moveSpeedMobile = 3f;

	public float moveSpeedPC = 1.5f;

	public GameObject go_GuideDrag_Gamepad;

	public GameObject go_GuideDrag_Keyboard;

	public GameObject go_GuideDrag_Mobile;

	private UIState state;

	private bool isMove2;

	private float moveSpeed
	{
		get
		{
			if (!GameMgr.IsMobile_Static)
			{
				return moveSpeedPC;
			}
			return moveSpeedMobile;
		}
	}

	public void OnEnable()
	{
		anima.SetTrigger("Show");
		UpdatePlatform();
	}

	private void Update()
	{
		switch (state)
		{
		case UIState.Move:
		{
			if (PlayerMgr.Inst.SelectedWand == null)
			{
				break;
			}
			Vector3 zero = Vector3.zero;
			if (GameMgr.IsMobile_Static && _mobileStopClickTime >= 0f)
			{
				_mobileStopClickTime -= Time.unscaledDeltaTime;
				break;
			}
			zero = ((!isMove2) ? UIPlayerDataMgr.Inst.GetWandUISlotWands(0)[0].transform.position : UIPlayerDataMgr.Inst.GetWandUISlotWands(0)[1].transform.position);
			if (ScriptableObjMgr.Inst.testCtrller.isBW && PlayerMgr.Inst.BaData.wandCfgs.Count > 0)
			{
				int num = 0;
				num = PlayerMgr.Inst.BaData.wandCfgs[0].normalSlots.Bag_GetFirstNullSlotIndex();
				zero = UIPlayerDataMgr.Inst.uiWands[0].GetUIAllUISlot()[num].transform.position;
			}
			base.transform.position = Vector3.MoveTowards(base.transform.position, zero, moveSpeed * Time.fixedUnscaledDeltaTime);
			if (GameMgr.IsMobile_Static)
			{
				if (base.transform.position == zero)
				{
					_mobileStopClickTime = mobileStopClickTime;
					SetClickAnimation(0.2f, delegate
					{
						state = UIState.Hide;
						anima.SetTrigger("Hide");
					});
				}
			}
			else if (base.transform.position == zero)
			{
				state = UIState.Hide;
				anima.SetTrigger("Hide");
			}
			break;
		}
		default:
			Debug.LogError(state);
			break;
		case UIState.HideDirect:
		case UIState.Show:
		case UIState.Hide:
			break;
		}
	}

	public void StartAnima(bool isMove2 = false)
	{
		this.isMove2 = isMove2;
		state = UIState.Show;
		anima.SetTrigger("Show");
		base.transform.SetParent(UIMgr.Inst.canvas_10Scaler.transform);
		if (isMove2)
		{
			for (int i = 0; i < PlayerMgr.Inst.BaData.bagSpellDatas.Count; i++)
			{
				if (PlayerMgr.Inst.BaData.bagSpellDatas[0] != null && (PlayerMgr.Inst.BaData.bagSpellDatas[0].id == 30121 || PlayerMgr.Inst.BaData.bagSpellDatas[0].id == 31031))
				{
					base.transform.position = UIPlayerDataMgr.Inst.GetUISlotBag(i).transform.position;
					break;
				}
			}
		}
		else
		{
			base.transform.position = UIPlayerDataMgr.Inst.GetUISlotBag(0).transform.position;
		}
		if (GameMgr.IsMobile_Static)
		{
			SetClickAnimation(0.4f);
		}
	}

	private void SetClickAnimation(float firstwait, Action afterClick = null)
	{
		_mobileStopClickTime = mobileStopClickTime;
		Sequence.Kill();
		Sequence = DOTween.Sequence().AppendInterval(firstwait).AppendCallback(delegate
		{
			mobilePointerClick.sprite = clickDown;
		})
			.AppendInterval(0.15f)
			.AppendCallback(delegate
			{
				mobilePointerClick.sprite = clickUp;
				afterClick?.Invoke();
			})
			.SetUpdate(isIndependentUpdate: true);
	}

	private void _ShowFinish()
	{
		state = UIState.Move;
	}

	private void _HideFinish()
	{
		state = UIState.Show;
		anima.SetTrigger("Show");
		if (isMove2)
		{
			for (int i = 0; i < PlayerMgr.Inst.BaData.bagSpellDatas.Count; i++)
			{
				if (PlayerMgr.Inst.BaData.bagSpellDatas[0] != null && (PlayerMgr.Inst.BaData.bagSpellDatas[0].id == 30121 || PlayerMgr.Inst.BaData.bagSpellDatas[0].id == 31031))
				{
					base.transform.position = UIPlayerDataMgr.Inst.GetUISlotBag(i).transform.position;
					break;
				}
			}
		}
		else
		{
			base.transform.position = UIPlayerDataMgr.Inst.GetUISlotBag(0).transform.position;
		}
		if (GameMgr.IsMobile_Static)
		{
			SetClickAnimation(0.4f);
		}
	}

	public void UpdatePlatform()
	{
		if (GameMgr.IsMobile_Static)
		{
			if ((bool)go_GuideDrag_Mobile)
			{
				go_GuideDrag_Mobile.SetActive(!MobileMgr.inst.gamepadPlugged);
			}
			if ((bool)go_GuideDrag_Gamepad)
			{
				go_GuideDrag_Gamepad.SetActive(MobileMgr.inst.gamepadPlugged);
			}
			if ((bool)go_GuideDrag_Keyboard)
			{
				go_GuideDrag_Keyboard.SetActive(value: false);
			}
		}
		else
		{
			if ((bool)go_GuideDrag_Mobile)
			{
				go_GuideDrag_Mobile.SetActive(value: false);
			}
			if ((bool)go_GuideDrag_Gamepad)
			{
				go_GuideDrag_Gamepad.SetActive(UIMgr.Inst.InputType == PlayerInputType.Gamepad);
			}
			if ((bool)go_GuideDrag_Keyboard)
			{
				go_GuideDrag_Keyboard.SetActive(UIMgr.Inst.InputType == PlayerInputType.Keyboard);
			}
		}
	}
}
