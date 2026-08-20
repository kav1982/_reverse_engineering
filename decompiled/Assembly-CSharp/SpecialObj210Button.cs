using UnityEngine;

public class SpecialObj210Button : LayerCorrect
{
	[Space(50f)]
	public Animator anima;

	public SpriteRenderer sr;

	public GameObject symble;

	public GameObject mid;

	public int cla;

	private SpecialObj210 so210;

	private bool isCorrect;

	private bool canblink;

	public float blinktime = 1f;

	private float starttime;

	public bool IsOn { get; private set; }

	private void Update()
	{
		if (!canblink)
		{
			return;
		}
		starttime += Time.deltaTime;
		if (starttime >= blinktime)
		{
			if (cla == 0)
			{
				anima.SetTrigger("MidOff");
			}
			else
			{
				anima.SetTrigger("Off");
			}
			starttime = 0f;
			canblink = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!isCorrect && !so210.IsPlaying && !canblink && !so210.IsRight && other.IsPlayerTrigger())
		{
			if (cla == 0)
			{
				anima.SetTrigger("MidOn");
			}
			else
			{
				anima.SetTrigger("On");
			}
			so210.ButtonEntry(this);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (!isCorrect && !so210.IsPlaying && !canblink && !so210.IsRight && other.IsPlayerTrigger())
		{
			if (cla == 0)
			{
				anima.SetTrigger("MidOff");
			}
			else
			{
				anima.SetTrigger("Off");
			}
		}
	}

	public void Initialize(SpecialObj210 so210, int cla)
	{
		this.so210 = so210;
		this.cla = cla;
		if (cla == 0)
		{
			symble.SetActive(value: false);
		}
		else
		{
			mid.SetActive(value: false);
		}
		IsOn = false;
		if (IsOn)
		{
			if (cla == 0)
			{
				anima.SetTrigger("MidOn");
			}
			else
			{
				anima.SetTrigger("On");
			}
		}
		else if (cla == 0)
		{
			anima.SetTrigger("MidOff");
		}
		else
		{
			anima.SetTrigger("Off");
		}
	}

	public void Correct()
	{
		isCorrect = true;
		if (cla == 0)
		{
			anima.SetTrigger("MidOn");
		}
		else
		{
			anima.SetTrigger("On");
		}
	}

	public void Wrong()
	{
		anima.SetTrigger("Wrong");
		starttime = 0f;
		canblink = true;
	}

	public int Getcla()
	{
		return cla;
	}

	public void Blink()
	{
		SEMgr.Inst.so210Blink.PlaySE();
		starttime = 0f;
		if (cla == 0)
		{
			anima.SetTrigger("MidOn");
		}
		else
		{
			anima.SetTrigger("On");
		}
		canblink = true;
	}
}
