using Unity.Entities;
using UnityEngine;

public class SpecialObj217_Handle : InteractiveObj
{
	public BoxCollider thisCollider;

	public SpecialObj217 SpecialObj217;

	public GameObject Outlline;

	public Entity thisEntity;

	private void Start()
	{
		thisEntity = RegisterDotsInteractiveObj(thisCollider, InteractiveObjType.SpecialObj217_Handle);
	}

	private void Update()
	{
	}

	public override void Interact()
	{
		SpecialObj217.InteractHandle();
	}

	public override void Select()
	{
		base.Select();
		Outlline.SetActive(value: true);
	}

	public override void Unselect()
	{
		base.Unselect();
		Outlline.SetActive(value: false);
	}
}
