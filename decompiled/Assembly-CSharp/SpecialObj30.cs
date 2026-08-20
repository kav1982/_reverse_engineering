using UnityEngine;

public class SpecialObj30 : LayerCorrect, IRoomCtrller
{
	[Space(50f)]
	public bool flipWithRoom;

	public Sprite[] sprites;

	public SpriteRenderer sr;

	public SpriteRenderer sr_Editor;

	[Header("和谐")]
	public bool needHarmonize;

	public Sprite[] sprite_H;

	public override void OnEnable()
	{
		if (needHarmonize && GameMgr.IsHarmony_Static)
		{
			sr.sprite = sprite_H[Random.Range(0, sprite_H.Length)];
		}
		else
		{
			sr.sprite = sprites[Random.Range(0, sprites.Length)];
		}
		Object.Destroy(sr_Editor.gameObject);
		base.OnEnable();
	}

	public void SetRoomCtrlller(RoomController roomCtrller)
	{
		if (roomCtrller.roomCfg.isFlipped && flipWithRoom)
		{
			base.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
	}
}
