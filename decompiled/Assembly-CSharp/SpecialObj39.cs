using UnityEngine;

public class SpecialObj39 : LayerCorrect, IRoomCtrller
{
	[Space(50f)]
	public bool flipWithRoom;

	public Sprite[] sprites;

	public SpriteRenderer sr;

	public float offset;

	public SpriteRenderer sr_Editor;

	public override void OnEnable()
	{
		base.transform.position += Tool2D.GetDir() * Random.Range(0f, offset);
		sr.sprite = sprites[Random.Range(0, sprites.Length)];
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
