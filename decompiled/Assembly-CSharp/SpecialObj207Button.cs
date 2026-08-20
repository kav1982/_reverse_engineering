using UnityEngine;

public class SpecialObj207Button : LayerCorrect
{
	[Space(50f)]
	public Animator anima;

	public MeshRenderer mr;

	private SpecialObj207 so207;

	private bool isCorrect;

	private bool canblink;

	private bool longblink = true;

	public float blinktime = 1f;

	private float starttime;

	private int cls;

	public Sprite[] sprite_Symbols_Original;

	public Sprite[] sprite_Symbols_H;

	public bool IsOn { get; private set; }

	private Sprite[] symble
	{
		get
		{
			if (GameMgr.IsHarmony_Static)
			{
				return sprite_Symbols_H;
			}
			return sprite_Symbols_Original;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!isCorrect && !canblink && other.IsPlayerTrigger())
		{
			so207.ButtonEntry(this);
		}
	}

	public void Initialize(SpecialObj207 so207, int c)
	{
		this.so207 = so207;
		cls = c;
		switch (cls)
		{
		case 1:
			mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, symble[0].texture);
			break;
		case 2:
			mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, symble[1].texture);
			break;
		case 3:
			mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, symble[2].texture);
			break;
		case 4:
			mr.material.SetTexture(GameConstManaged.shaderBaseMapIndex, symble[3].texture);
			break;
		}
		IsOn = false;
		if (IsOn)
		{
			anima.SetTrigger("On");
		}
		else
		{
			anima.SetTrigger("Off");
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
		if (longblink)
		{
			starttime += Time.deltaTime;
			if (starttime >= 3f * blinktime)
			{
				anima.SetTrigger("Off");
				starttime = 0f;
				longblink = false;
			}
		}
	}

	public void Change()
	{
		if (IsOn)
		{
			IsOn = false;
			anima.SetTrigger("Off");
		}
		else
		{
			IsOn = true;
			anima.SetTrigger("On");
		}
	}

	public void Correct()
	{
		isCorrect = true;
	}

	public void Failed()
	{
		IsOn = false;
		anima.SetTrigger("Failed");
		starttime = 0f;
		canblink = true;
	}

	public void LongBlink()
	{
		anima.SetTrigger("On");
		starttime = 0f;
		longblink = true;
	}

	public int GetCls()
	{
		return cls;
	}
}
