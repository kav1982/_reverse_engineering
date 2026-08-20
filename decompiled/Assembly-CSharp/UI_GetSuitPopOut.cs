using UnityEngine;
using UnityEngine.UI;

public class UI_GetSuitPopOut : MonoBehaviour
{
	public int time = 3;

	private float timecount;

	private bool popoutplay;

	public Text text;

	private void Start()
	{
		GetComponent<Animation>().Play("popin");
	}

	private void Update()
	{
		timecount += Time.deltaTime;
		if (timecount >= (float)time)
		{
			if (!popoutplay)
			{
				popoutplay = true;
				GetComponent<Animation>().Play("popout");
			}
			if (timecount >= (float)(time + 1))
			{
				Object.Destroy(base.gameObject);
			}
		}
	}
}
