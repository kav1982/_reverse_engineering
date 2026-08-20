using UnityEngine;

[ExecuteAlways]
public class blitToRT : MonoBehaviour
{
	public RenderTexture rt;

	public Material material;

	public float interval = 0.1f;

	private RoomController roomController;

	private float timer;

	public void Start()
	{
		roomController = GetComponentInParent<RoomController>();
	}

	public void Update()
	{
		if (roomController == null || roomController == LevelMgr.Inst.CurrentRoomCtrller)
		{
			timer += Time.deltaTime;
			if (timer >= interval)
			{
				Blit();
				timer = 0f;
			}
		}
	}

	public void Blit()
	{
		Graphics.Blit(null, rt, material);
	}
}
