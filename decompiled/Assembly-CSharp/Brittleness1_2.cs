using UnityEngine;

public class Brittleness1_2 : UnitBase
{
	[Space(50f)]
	public MeshRenderer mr;

	public Texture[] textures;

	public Texture[] texture_Deads;

	public float offset;

	private int index;

	public override void EveryInitialCallback()
	{
		index = Random.Range(0, textures.Length);
		mr.material.SetTexture(GameConstManaged.baseMapIndex, textures[index]);
		if (Random.Range(0, 2) == 0)
		{
			mr.transform.localScale = new Vector3(0f - mr.transform.localScale.x, mr.transform.localScale.y, mr.transform.localScale.z);
		}
		base.transform.position += Tool2D.GetDir() * Random.Range(0f, offset);
		myPpt.CorrectLayerOnce();
		SyncDotsPosition();
	}

	public override void Frame2InitialCallback()
	{
		base.enabled = false;
		myPpt.enabled = false;
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
		MeshRenderer meshRenderer = Object.Instantiate(mr, LevelMgr.Inst.CurrentRoomT);
		meshRenderer.material.SetTexture(GameConstManaged.shaderTextureIndex, texture_Deads[index]);
		meshRenderer.transform.position = mr.transform.position;
		meshRenderer.transform.localScale = mr.transform.localScale;
	}
}
