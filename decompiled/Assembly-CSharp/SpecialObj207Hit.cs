using UnityEngine;

public class SpecialObj207Hit : LayerCorrect
{
	[Space(50f)]
	public Animator anima;

	public SpriteRenderer sr;

	public Sprite[] symble;

	private int cls;

	private bool canblink;

	private bool faileblink;

	public float blinktime = 1f;

	public float failetime = 1f;

	private float starttime;

	public void Initialize(int cl)
	{
		cls = cl;
		switch (cls)
		{
		case 1:
			sr.sprite = symble[0];
			break;
		case 2:
			sr.sprite = symble[1];
			break;
		case 3:
			sr.sprite = symble[2];
			break;
		case 4:
			sr.sprite = symble[3];
			break;
		}
	}

	private void Update()
	{
		if (canblink)
		{
			starttime += Time.deltaTime;
			if (starttime >= blinktime)
			{
				anima.SetTrigger("Off");
				starttime = 0f;
				canblink = false;
			}
		}
		if (faileblink)
		{
			starttime += Time.deltaTime;
			if (starttime >= failetime)
			{
				anima.SetTrigger("Off");
				starttime = 0f;
				faileblink = false;
			}
		}
	}

	public void Correct()
	{
		anima.SetTrigger("On");
	}

	public void Off()
	{
		anima.SetTrigger("Off");
	}

	public void Blink()
	{
		starttime = 0f;
		canblink = true;
	}

	public void Failed()
	{
		anima.SetTrigger("Failed");
		starttime = 0f;
		faileblink = true;
	}
}
