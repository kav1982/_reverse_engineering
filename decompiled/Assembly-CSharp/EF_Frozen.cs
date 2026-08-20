using UnityEngine;

public class EF_Frozen : LayerCorrect
{
	[Space(50f)]
	public ParticleSystem ps;

	public Vector2 oneScale;

	private UnitProperty ownerPpt;

	public void Initialize(UnitProperty ownerPpt)
	{
		this.ownerPpt = ownerPpt;
		CapsuleCollider component = ownerPpt.GetComponent<CapsuleCollider>();
		ParticleSystem.ShapeModule shape = ps.shape;
		if (component != null)
		{
			shape.scale = new Vector3(component.radius / oneScale.x, component.height / oneScale.y, 1f);
			shape.position = new Vector3(0f, shape.scale.y / 2f, 0f);
		}
		else
		{
			shape.scale = Vector3.one;
		}
	}

	private void Update()
	{
		if (ownerPpt == null)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
		else if (!ownerPpt.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: false);
		}
		else
		{
			base.transform.position = ownerPpt.transform.position;
		}
	}
}
