using UnityEngine;

public class ResourceChanger : InteractiveObj
{
	[Space(50f)]
	public GameObject go_Outline;

	public override void Select()
	{
		go_Outline.SetActive(value: true);
	}

	public override void Unselect()
	{
		go_Outline.SetActive(value: false);
	}

	public override void Interact()
	{
		GameUISingletonMono<UIResourceChanger>.ShowInit();
	}
}
