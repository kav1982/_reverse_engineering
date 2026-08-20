using Unity.Entities;
using UnityEngine;

public class Brittleness3 : UnitBase, IDotsTriggerReceiver, IDotsPhysicsReciever
{
	[Space(50f)]
	public MeshRenderer[] mr;

	private int shaderFlipIndex = Shader.PropertyToID("_FlipX");

	public VariableFloat scale;

	public VariableInt copyCount;

	public VariableFloat copyOffset;

	private bool createByOther;

	public Entity thisEntity { get; set; }

	public override void EveryInitialCallback()
	{
		createByOther = false;
	}

	public override void Frame1InitialCallback()
	{
		if (!createByOther)
		{
			copyCount.RandomResult();
			for (int i = 0; i < copyCount.result; i++)
			{
				Vector3 point = base.transform.position + Tool2D.GetDir(360f / (float)copyCount.result * (float)i) * copyOffset.RandomResult();
				if (i == copyCount.result - 1)
				{
					base.transform.position += Tool2D.GetDir() * copyOffset.RandomResult();
					myPpt.CorrectLayerOnce();
				}
				else
				{
					ObjPoolMgr.Inst.GetGO("Prefabs/Units/" + myPpt.unitCfg.id, point).GetComponent<Brittleness3>().MarkCreateByOther();
				}
			}
		}
		int num = Random.Range(0, mr.Length);
		for (int j = 0; j < mr.Length; j++)
		{
			if (j == num)
			{
				mr[j].gameObject.SetActive(value: true);
				mr[j].transform.localScale = new Vector3(mr[j].transform.localScale.x * (float)(Random.Range(0, 2) * 2 - 1), mr[j].transform.localScale.y, 1f);
			}
			else
			{
				mr[j].gameObject.SetActive(value: false);
			}
		}
		myPpt.tsf_Layer.localScale = Vector3.one * scale.RandomResult();
	}

	public override void Frame2InitialCallback()
	{
		base.enabled = false;
		myPpt.enabled = false;
	}

	public void MarkCreateByOther()
	{
		createByOther = true;
	}

	void IDotsTriggerReceiver.OnTriggerEnter_Dots(Entity other)
	{
		DotsAnnouncedDeath();
	}

	void IDotsTriggerReceiver.OnTriggerStay_Dots(Entity other)
	{
	}

	void IDotsTriggerReceiver.OnTriggerExit_Dots(Entity other)
	{
	}
}
