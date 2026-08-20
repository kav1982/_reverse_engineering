using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

[GameUISingletonPrefab("UIQuickPanel")]
public class UIQuickPanel : GameUISingletonMono<UIQuickPanel>
{
	public UIDamageRecorde damageRecorde;

	public List<UpdatButtonShow> UpdateButtons = new List<UpdatButtonShow>();

	public RectTransform window;

	public CanvasGroup windowGroup;

	public Vector3 closeScale = new Vector3(0.8f, 0.8f, 0.8f);

	public float animatorDuration = 0.15f;

	private Sequence windowSequence;

	public void Switch()
	{
		if (base.IsOpen)
		{
			Hide();
		}
		else
		{
			Show();
		}
	}

	protected override void OnShow(object obj = null)
	{
		SEMgr.Inst.UIDamageRecordBoard.PlaySE(SEPlayMode.Replay, 3, 0.1f);
		windowSequence?.Complete(withCallbacks: true);
		windowGroup.alpha = 0f;
		window.localScale = closeScale;
		window.gameObject.SetActive(value: true);
		windowSequence = DOTween.Sequence(base.gameObject).SetUpdate(isIndependentUpdate: true).Append(windowGroup.DOFade(1f, animatorDuration))
			.Join(window.DOScale(Vector3.one, animatorDuration))
			.AppendCallback(delegate
			{
				windowSequence = null;
			});
		if (GameMgr.IsMobile_Static)
		{
			PlayerMgr.Inst.PlayerCtrller.StopMotion();
			TimeScaleMgr.Inst.Pause();
		}
	}

	protected override void OnHide()
	{
		SEMgr.Inst.UIDamageRecordBoard.PlaySE(SEPlayMode.Replay, 3, 0.1f);
		windowSequence?.Complete(withCallbacks: true);
		windowSequence = DOTween.Sequence(base.gameObject).SetUpdate(isIndependentUpdate: true).Append(windowGroup.DOFade(0f, animatorDuration))
			.Join(window.DOScale(closeScale, animatorDuration))
			.AppendCallback(delegate
			{
				window.gameObject.SetActive(value: false);
				windowSequence = null;
			});
		if (GameMgr.IsMobile_Static)
		{
			PlayerMgr.Inst.PlayerCtrller.StartMotion();
			TimeScaleMgr.Inst.Recovery();
		}
	}

	private void UpdateControllerSHow()
	{
		foreach (UpdatButtonShow updateButton in UpdateButtons)
		{
			updateButton.UpdateButton();
		}
	}

	protected override void RegistarWhenInit()
	{
	}

	protected override void RegistarOnlyWhenOpen()
	{
		EventMgr.InputChange = (Action)Delegate.Combine(EventMgr.InputChange, new Action(UpdateControllerSHow));
		EventMgr.ControlChange = (Action)Delegate.Combine(EventMgr.ControlChange, new Action(UpdateControllerSHow));
		base.inputActions.Player.GamepadLB.performed += GamepadLBPerformed;
		base.inputActions.Player.GamepadRB.performed += GamepadRBPerformed;
		base.inputActions.Player.GamepadWest.performed += GamepadDirectMoveObjPerformed;
	}

	private void GamepadDirectMoveObjPerformed(InputAction.CallbackContext obj)
	{
		damageRecorde.ResetCurrentRoomDamage();
	}

	private void GamepadRBPerformed(InputAction.CallbackContext obj)
	{
		damageRecorde.SwitchToTotalDamageChart();
	}

	private void GamepadLBPerformed(InputAction.CallbackContext obj)
	{
		damageRecorde.SwitchToCurrentRoomDamageChart();
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		EventMgr.InputChange = (Action)Delegate.Remove(EventMgr.InputChange, new Action(UpdateControllerSHow));
		EventMgr.ControlChange = (Action)Delegate.Remove(EventMgr.ControlChange, new Action(UpdateControllerSHow));
		base.inputActions.Player.GamepadLB.performed += GamepadLBPerformed;
		base.inputActions.Player.GamepadRB.performed += GamepadRBPerformed;
		base.inputActions.Player.GamepadWest.performed += GamepadDirectMoveObjPerformed;
	}

	protected override void UnRegistarWhenDestroy()
	{
	}
}
