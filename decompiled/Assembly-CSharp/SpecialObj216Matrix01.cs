using UnityEngine;

public class SpecialObj216Matrix01 : LayerCorrect
{
	[Space(50f)]
	public GameObject go_Wrong;

	public int damage;

	public ParticleSystem[] ps_ChangeColors;

	private bool isWrong;

	private Vector3 backPoint;

	private bool isEntered;

	private int theDamage;

	private void Update()
	{
		if (!isEntered)
		{
			for (int i = 0; i < ps_ChangeColors.Length; i++)
			{
				Color red = Color.red;
				ParticleSystem.MainModule main = ps_ChangeColors[i].main;
				red.a = main.startColor.color.a;
				main.startColor = red;
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.IsPlayerTrigger())
		{
			PlayerMgr.Inst.PlayerPpt.TakeDamage(theDamage, AttackerType.NothingSpecial);
			PlayerMgr.Inst.PlayerT.position = backPoint;
			go_Wrong.SetActive(value: false);
			go_Wrong.SetActive(value: true);
			if (isWrong && !isEntered)
			{
				isEntered = true;
			}
		}
	}

	public void SetWrong(bool isWrong, Vector3 backPoint)
	{
		theDamage = damage;
		this.isWrong = isWrong;
		this.backPoint = backPoint;
	}

	public void SetFinish(bool isEntered, Vector3 backPoint)
	{
		theDamage = 0;
		this.isEntered = isEntered;
		this.backPoint = backPoint;
	}
}
