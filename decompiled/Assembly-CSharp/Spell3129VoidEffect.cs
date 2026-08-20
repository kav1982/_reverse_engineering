using UnityEngine;

public class Spell3129VoidEffect : LayerCorrect
{
	[Space(50f)]
	public ParticleSystem ps;

	public ParticleSystem ps2;

	public Vector2 oneScale;

	private UnitProperty ownerPpt;

	public Transform VoidTrans;

	public void Initialize(UnitProperty ownerPpt)
	{
		this.ownerPpt = ownerPpt;
		CapsuleCollider component = ownerPpt.GetComponent<CapsuleCollider>();
		ParticleSystem.ShapeModule shape = ps.shape;
		ParticleSystem.ShapeModule shape2 = ps2.shape;
		VoidTrans.localScale = Vector3.one * ownerPpt.gameObject.transform.localScale.x * ownerPpt.tsf_Layer.localScale.x;
		if (component != null)
		{
			shape.scale = new Vector3(component.radius / oneScale.x, component.height / oneScale.y, 1f);
			shape.position = new Vector3(0f, shape.scale.y / 2f, 0f);
			shape2.scale = shape.scale;
			shape2.position = shape.position;
		}
		else
		{
			shape.scale = Vector3.one;
			shape2.scale = shape.scale;
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

	public void StartParticle()
	{
		ps.Play();
		ps2.Play();
	}
}
