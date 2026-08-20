using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIBLiveTool : MonoBehaviour
{
	public GameObject connectedUI;

	public GameObject disconnectedUI;

	public Text pasteButtonText;

	public Text connectButtonText;

	public Button connectButton;

	public Text disconnectButtonText;

	public Button disconnectButton;

	public Text errorTest;

	private string _pastedCode = "";

	private Sequence disconnectButtonTextSequence;

	public static UIBLiveTool Inst { get; private set; }

	private void Awake()
	{
		Inst = this;
		if (BLiveMgr.Inst == null)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	private void OnEnable()
	{
		disconnectedUI.SetActive(BLiveMgr.Inst.Connected);
		connectedUI.SetActive(!BLiveMgr.Inst.Connected);
	}

	public void pastButton_OnClick()
	{
		errorTest.text = "";
		if (GUIUtility.systemCopyBuffer.Length <= 200)
		{
			if (string.IsNullOrEmpty(_pastedCode))
			{
				_pastedCode = GUIUtility.systemCopyBuffer;
				pasteButtonText.text = new string('*', _pastedCode.Length);
			}
			else
			{
				_pastedCode = "";
				pasteButtonText.text = "点击粘贴身份码";
			}
		}
	}

	public async void connectButton_OnClick()
	{
		errorTest.text = "";
		if (string.IsNullOrEmpty(_pastedCode))
		{
			errorTest.text = "先粘贴身份码再连接";
			return;
		}
		try
		{
			connectButton.interactable = false;
			connectButtonText.text = "连接中...";
			await BLiveMgr.Inst.Connect(_pastedCode);
			disconnectedUI.SetActive(value: true);
			connectedUI.SetActive(value: false);
		}
		catch (Exception ex)
		{
			errorTest.text = $"连接错误\n{ex.GetType()}:\n${ex.Message}";
		}
		finally
		{
			connectButton.interactable = true;
			connectButtonText.text = "连接直播间";
		}
	}

	public async void disconnectButton_OnClick()
	{
		errorTest.text = "";
		if (disconnectButtonText.text == "断开连接")
		{
			disconnectButtonText.text = "确定断开？";
			disconnectButtonTextSequence = DOTween.Sequence(this).AppendInterval(1f).AppendCallback(delegate
			{
				disconnectButtonText.text = "断开连接";
			})
				.SetUpdate(isIndependentUpdate: true)
				.Play();
			return;
		}
		disconnectButtonTextSequence.Complete(withCallbacks: true);
		errorTest.text = "";
		disconnectButton.interactable = false;
		await BLiveMgr.Inst.Disconnect();
		disconnectButton.interactable = true;
		disconnectedUI.SetActive(value: false);
		connectedUI.SetActive(value: true);
	}
}
