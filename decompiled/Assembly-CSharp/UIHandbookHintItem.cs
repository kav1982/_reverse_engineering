using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIHandbookHintItem : MonoBehaviour
{
	public Text textTitle;

	public Text textDescription;

	private UIHandbookVideoTextCtrl videoTextCtrl;

	public RawImage rawImage_Demo;

	public GameObject go_Demo;

	public GameObject go_Texture;

	public VideoPlayer vp;

	private RenderTexture rt;

	private HandbookConfig thisHandbookConfig;

	public void Init(int id)
	{
		thisHandbookConfig = HandbookConfig.dic[id];
		textTitle.text = "◆ " + thisHandbookConfig.GetTitle() + " ◆";
		textDescription.text = GeneralTool.FormatTextIfPublishTest(textDescription, thisHandbookConfig.GetDesc());
		go_Demo.SetActive(thisHandbookConfig.demoType == HandbookDemoType.Mp4);
		go_Texture.SetActive(thisHandbookConfig.demoType == HandbookDemoType.Texture);
		switch (thisHandbookConfig.demoType)
		{
		case HandbookDemoType.Texture:
		{
			for (int i = 0; i < go_Texture.transform.childCount; i++)
			{
				Object.Destroy(go_Texture.transform.GetChild(i).gameObject);
			}
			Object.Instantiate(ABResources.LoadAsset<GameObject>("Handbook/" + thisHandbookConfig.id), go_Texture.transform).transform.localScale = new Vector3(1f, 1f, 1f);
			break;
		}
		default:
			Debug.LogError(thisHandbookConfig.demoType);
			break;
		case HandbookDemoType.None:
		case HandbookDemoType.Mp4:
			break;
		}
	}

	public void OnSelect()
	{
		switch (thisHandbookConfig.demoType)
		{
		case HandbookDemoType.Mp4:
			videoTextCtrl.ClearState();
			vp.Stop();
			vp.Play();
			break;
		case HandbookDemoType.None:
		case HandbookDemoType.Texture:
			break;
		}
	}

	public void OnDisSelect()
	{
		switch (thisHandbookConfig.demoType)
		{
		case HandbookDemoType.Mp4:
			videoTextCtrl.ClearState();
			vp.Stop();
			break;
		case HandbookDemoType.None:
		case HandbookDemoType.Texture:
			break;
		}
	}
}
