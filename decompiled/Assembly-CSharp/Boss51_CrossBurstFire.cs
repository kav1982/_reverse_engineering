using UnityEngine;

public class Boss51_CrossBurstFire : MonoBehaviour
{
	public float delayTime;

	private float delayTimer;

	private bool created;

	private bool isDiagonal;

	public SpriteRenderer SR_Cross;

	public void Initialize(bool isDiagonal)
	{
		this.isDiagonal = isDiagonal;
		delayTimer = 0f;
		created = false;
		SR_Cross.transform.localEulerAngles = new Vector3(0f, 0f, isDiagonal ? 45 : 0);
		SR_Cross.color = new Color(1f, 1f, 1f, 0f);
	}

	private void Update()
	{
		delayTimer += Time.deltaTime;
		if (delayTimer < delayTime)
		{
			SR_Cross.color = new Color(1f, 1f, 1f, delayTimer / delayTime);
		}
		else
		{
			SR_Cross.color = new Color(1f, 1f, 1f, 0f);
		}
		if (delayTimer > delayTime && !created)
		{
			created = true;
			float num = (isDiagonal ? 45 : 0);
			for (int i = 0; i < 4; i++)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Boss51_BurstFire", base.transform.position).GetComponent<Boss51_BurstFire>().Initialize(base.transform.position, Tool2D.GetDir(num + (float)(i * 90)));
			}
		}
		if (delayTimer > 4f)
		{
			ObjPoolMgr.Inst.RecycleGO(base.gameObject);
		}
	}
}
