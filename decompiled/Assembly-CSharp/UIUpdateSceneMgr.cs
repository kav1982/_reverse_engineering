using UnityEngine;
using UnityEngine.UI;

public class UIUpdateSceneMgr : MonoBehaviour
{
	public Text logText;

	public static UIUpdateSceneMgr Inst { get; private set; }

	private void Awake()
	{
		Inst = this;
	}

	public void ServerConnect()
	{
		logText.text = "连接服务器";
	}

	public void LoginStartQ()
	{
		logText.text = "开始排队";
	}

	public void LoginGetResult()
	{
		logText.text = "获取登录验证";
	}

	public void LoginGetResultAgain()
	{
		logText.text = "准备重新获取";
	}

	public void StartQ()
	{
		logText.text = "开始排队";
	}

	public void LoginErrorHasNetworkError()
	{
		logText.text = "网络连接错误";
	}

	public void LoginErrorNoVerifyInfo()
	{
		logText.text = "账号或密码错误";
	}

	public void LoginErrorDisabledAccount()
	{
		logText.text = "账户出现问题";
	}

	public void LoginErrorTooOften()
	{
		logText.text = "登录过于频繁,请稍后再试";
	}

	public void LoginSuccess()
	{
		logText.text = "登陆成功";
	}

	public void LoginSuccessTester()
	{
		logText.text = "测试账号登陆成功";
	}
}
