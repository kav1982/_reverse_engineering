using UnityEngine;

public class SpecialObj214MainButton : SpecialObj205
{
	[Space(50f)]
	public float outTime;

	private Color roomDefaultColor;

	private float outLerpSpeed;

	private float currentLerp = 1f;

	private bool isFinished;

	public Color grey;

	private new void Start()
	{
		base.Start();
		outLerpSpeed = 1f / outTime;
		roomDefaultColor = new Color(0.79607844f, 0.79607844f, 0.79607844f);
		for (int i = 0; i < belongRoom.traps.Count; i++)
		{
			if (belongRoom.traps[i] is SpecialObj214LightButton)
			{
				(belongRoom.traps[i] as SpecialObj214LightButton).SetMainButton(this);
			}
		}
	}

	private new void OnDestroy()
	{
		base.OnDestroy();
	}

	private void Update()
	{
		if (!isFinished && LevelMgr.Inst.CurrentRoomCtrller == belongRoom)
		{
			currentLerp = Mathf.MoveTowards(currentLerp, 1f, outLerpSpeed * Time.deltaTime);
			LevelMgr.Inst.globalLight.color = Color.Lerp(roomDefaultColor, grey, currentLerp);
		}
	}

	public void PlayerEnter()
	{
		currentLerp = 0f;
	}

	public override void SO205PlayerEntered()
	{
		base.SO205PlayerEntered();
		isFinished = true;
		LevelMgr.Inst.globalLight.color = roomDefaultColor;
	}
}
