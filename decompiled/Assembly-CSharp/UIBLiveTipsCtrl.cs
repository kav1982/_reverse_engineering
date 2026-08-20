using UnityEngine;

public class UIBLiveTipsCtrl : MonoBehaviour
{
	public GameObject Tips;

	public GameObject ShowTipsButton;

	private void Start()
	{
		if (BLiveMgr.Inst == null)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void Show()
	{
		Tips.SetActive(value: true);
		ShowTipsButton.SetActive(value: false);
	}

	public void Hide()
	{
		Tips.SetActive(value: false);
		ShowTipsButton.SetActive(value: true);
	}
}
