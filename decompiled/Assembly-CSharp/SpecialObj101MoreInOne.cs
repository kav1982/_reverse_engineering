using UnityEngine;

public class SpecialObj101MoreInOne : InteractiveObj
{
	[Space(50f)]
	public GameObject go_HighLight;

	public SpriteRenderer sr;

	public GameObject go_Symbol2;

	public void UseOnce()
	{
		base.tag = "Untagged";
		Object.Destroy(go_Symbol2);
	}

	public override void Select()
	{
		go_HighLight.SetActive(value: true);
	}

	public override void Unselect()
	{
		go_HighLight.SetActive(value: false);
	}

	public override void Interact()
	{
		GameUISingletonMono<UIMoreInOne>.ShowInit(this);
	}
}
