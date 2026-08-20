using UnityEngine;

public class SpecialObj101Compound : InteractiveObj
{
	[Space(50f)]
	public GameObject go_HighLight;

	public Transform tsf_Carpet;

	private void Start()
	{
		tsf_Carpet.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.Tile9_AboveAO);
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
		GameUISingletonMono<UICompound>.ShowInit(this);
	}
}
