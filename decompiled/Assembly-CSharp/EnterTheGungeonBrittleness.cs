using UnityEngine;

public class EnterTheGungeonBrittleness : UnitBase, IRoomCtrller
{
	[Space(50f)]
	private RoomController belongCtrller;

	public MeshRenderer mr;

	public Sprite[] sprites;

	public override void EveryInitialCallback()
	{
		mr.material.SetTexture(GameConstManaged.shaderTextureIndex, sprites[Random.Range(0, sprites.Length)].texture);
		myPpt.CorrectLayerOnce();
	}

	public override void Update()
	{
		base.Update();
	}

	public override void AfterDead(ref TakeDamageInfo_Dots info)
	{
		base.AfterDead(ref info);
	}

	public override void AfterTakeDamage(TakeDamageInfo info)
	{
		base.AfterTakeDamage(info);
		myPpt.AnnouncedDeath();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") || other.CompareTag("Monster") || other.CompareTag("Teammate"))
		{
			myPpt.AnnouncedDeath();
		}
	}

	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		belongCtrller = roomCtrller;
	}
}
