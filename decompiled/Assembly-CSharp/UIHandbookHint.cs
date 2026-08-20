using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[GameUISingletonPrefab("UIHandbookHint")]
public class UIHandbookHint : GameUISingletonMono<UIHandbookHint>
{
	public Text titleText;

	public Button buttonPrevious;

	public Button buttonNext;

	public GameObject uiRoot;

	public GameObject closeButton;

	public Text Text;

	public CanvasGroup canvasGroup;

	public List<UIHandbookHintItem> currentShowItems;

	public int currentShowingIndex;

	public RectTransform loadRoot;

	public UIHandbookHintItem itemPrefab;

	public Sprite markFull;

	public Sprite markEmpty;

	public Transform markTrans;

	protected override void OnShow(object obj = null)
	{
		titleText.text = 1006701.GetText();
		base.OnShow(obj);
		if ((bool)PlayerMgr.Inst.PlayerCtrller)
		{
			PlayerMgr.Inst.PlayerCtrller.StopMotion();
		}
		Time.timeScale = 0f;
		uiRoot.SetActive(value: true);
		canvasGroup.alpha = 0f;
		canvasGroup.DOFade(1f, 0.5f).SetUpdate(isIndependentUpdate: true);
		Vector2 anchoredPosition = loadRoot.anchoredPosition;
		anchoredPosition.x = 0f;
		loadRoot.anchoredPosition = anchoredPosition;
		if (obj is List<int>)
		{
			while (currentShowItems.Count > 0)
			{
				Object.DestroyImmediate(currentShowItems[0].gameObject);
				currentShowItems.RemoveAt(0);
			}
			InitializeHints(obj as List<int>);
			currentShowingIndex = 0;
			LayoutRebuilder.ForceRebuildLayoutImmediate(loadRoot);
			currentShowItems[currentShowingIndex].OnSelect();
			UpdateMark();
		}
		else
		{
			Debug.LogWarning("\ufffd\ufffd\u05a7\ufffd\u05b5\ufffd\ufffd\ufffd\ufffd\ufffd");
		}
	}

	protected override void OnHide()
	{
		uiRoot.SetActive(value: false);
		Time.timeScale = 1f;
		if ((bool)PlayerMgr.Inst.PlayerCtrller)
		{
			PlayerMgr.Inst.PlayerCtrller.StartMotion();
		}
	}

	private void InitializeHints(List<int> ids)
	{
		loadRoot.DestroyAllChildImmediate();
		ids.ForEach(delegate(int index)
		{
			currentShowItems.Add(Object.Instantiate(itemPrefab, loadRoot));
			List<UIHandbookHintItem> list = currentShowItems;
			list[list.Count - 1].Init(index);
		});
	}

	private void Update()
	{
		buttonNext.interactable = currentShowingIndex != currentShowItems.Count - 1;
		buttonPrevious.interactable = currentShowingIndex != 0;
	}

	protected override void RegistarWhenInit()
	{
	}

	protected override void RegistarOnlyWhenOpen()
	{
		base.inputActions.Player.GamepadDirect.performed += GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed += GamepadDirectPerformed_Stick;
		base.inputActions.Player.GamepadEast.performed += GamepadBack;
		base.inputActions.Player.WASD.performed += GamepadDirectPerformed;
	}

	protected override void UnRegistarOnlyWhenHide()
	{
		base.inputActions.Player.GamepadDirect.performed -= GamepadDirectPerformed;
		base.inputActions.Player.LeftStick.performed -= GamepadDirectPerformed_Stick;
		base.inputActions.Player.GamepadEast.performed -= GamepadBack;
		base.inputActions.Player.WASD.performed -= GamepadDirectPerformed;
	}

	protected override void UnRegistarWhenDestroy()
	{
	}

	private void GamepadDirectPerformed_Stick(InputAction.CallbackContext context)
	{
		if (base.IsOpen)
		{
			Vector2 vector = context.ReadValue<Vector2>();
			vector = ControlMgr.Inst.RampVector2(vector);
			MoveDirect(vector);
		}
	}

	private void GamepadDirectPerformed(InputAction.CallbackContext context)
	{
		if (base.IsOpen)
		{
			Vector2 direct = context.ReadValue<Vector2>();
			MoveDirect(direct);
		}
	}

	private void MoveDirect(Vector2 direct)
	{
		if (direct == Vector2.left)
		{
			ShowPrevious();
		}
		else if (direct == Vector2.right)
		{
			ShowNext();
		}
	}

	private void GamepadBack(InputAction.CallbackContext obj)
	{
		_Close();
	}

	private void UpdatePosition()
	{
		float endValue = loadRoot.position.x - currentShowItems[currentShowingIndex].transform.position.x;
		float duration = 0.3f;
		loadRoot.DOMoveX(endValue, duration).SetEase(Ease.Linear).SetUpdate(isIndependentUpdate: true);
	}

	public void ShowPrevious()
	{
		if (currentShowingIndex != 0)
		{
			currentShowItems[currentShowingIndex].OnDisSelect();
			currentShowingIndex--;
			currentShowItems[currentShowingIndex].OnSelect();
			UpdatePosition();
			UpdateMark();
		}
	}

	public void ShowNext()
	{
		if (currentShowingIndex != currentShowItems.Count - 1)
		{
			currentShowItems[currentShowingIndex].OnDisSelect();
			currentShowingIndex++;
			currentShowItems[currentShowingIndex].OnSelect();
			UpdatePosition();
			UpdateMark();
		}
	}

	private void UpdateMark()
	{
		markTrans.DestroyAllChildImmediate();
		currentShowItems.ForEach(delegate
		{
			GameObject obj = new GameObject();
			obj.transform.SetParent(markTrans);
			obj.transform.localScale = Vector3.one;
			Image image = obj.AddComponent<Image>();
			image.sprite = ((markTrans.childCount == currentShowingIndex + 1) ? markFull : markEmpty);
			image.SetNativeSize();
		});
	}
}
