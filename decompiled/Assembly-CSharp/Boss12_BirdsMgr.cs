using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss12_BirdsMgr : MonoBehaviour
{
	[Header("死亡")]
	public List<Boss12_Bird> birds = new List<Boss12_Bird>();

	public float deathAnimTime;

	public int birdAmounts;

	public int birdCounter;

	public float birdCircleRadius;

	public Boss12 boss12;

	public Vector3 corpseCenter;

	public void StartDeadAnimation()
	{
		StartCoroutine(DeadAnimation());
	}

	private IEnumerator DeadAnimation()
	{
		birdCounter = 0;
		Boss12_Bird component = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss12_Bird", base.transform.position + Tool2D.GetDir(Vector3.up, Random.Range(0, 360)) * 30f).GetComponent<Boss12_Bird>();
		birds.Add(component);
		if (component.transform.position.x > LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x)
		{
			component.targetPos = corpseCenter + Tool2D.GetDir(Vector3.up, Random.Range(0, -180)) * Random.Range(0f, birdCircleRadius);
			component.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		else
		{
			component.targetPos = corpseCenter + Tool2D.GetDir(Vector3.up, Random.Range(0, 180)) * Random.Range(0f, birdCircleRadius);
			component.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		yield return new WaitForSeconds(2f);
		while (birdCounter < birdAmounts)
		{
			for (int i = 0; i < 10; i++)
			{
				Boss12_Bird component2 = ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss12_Bird", base.transform.position + Tool2D.GetDir(Vector3.up, Random.Range(0, 360)) * 30f).GetComponent<Boss12_Bird>();
				birds.Add(component2);
				component2.targetPos = base.transform.position + (Vector3)(Random.insideUnitCircle * birdCircleRadius);
				if (component2.transform.position.x > LevelMgr.Inst.CurrentRoomCtrller.CenterPoint.x)
				{
					component2.targetPos = corpseCenter + Tool2D.GetDir(Vector3.up, Random.Range(0, -180)) * Random.Range(0f, birdCircleRadius);
					component2.transform.localScale = new Vector3(-1f, 1f, 1f);
				}
				else
				{
					component2.targetPos = corpseCenter + Tool2D.GetDir(Vector3.up, Random.Range(0, 180)) * Random.Range(0f, birdCircleRadius);
					component2.transform.localScale = new Vector3(1f, 1f, 1f);
				}
				birdCounter++;
			}
			yield return new WaitForSeconds(0.1f);
		}
		yield return new WaitForSeconds(deathAnimTime);
		boss12.myPpt.AnnouncedDeath();
		yield return new WaitForSeconds(1f);
		foreach (Boss12_Bird bird in birds)
		{
			if (bird.transform.position.x > corpseCenter.x)
			{
				bird.targetPos = corpseCenter + Tool2D.GetDir(Vector3.up, Random.Range(0, -180)) * 100f;
				bird.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			else
			{
				bird.targetPos = corpseCenter + Tool2D.GetDir(Vector3.up, Random.Range(0, 180)) * 100f;
				bird.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			bird.countDown = true;
		}
	}
}
