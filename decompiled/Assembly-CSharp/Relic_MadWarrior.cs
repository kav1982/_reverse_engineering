using UnityEngine;

public class Relic_MadWarrior : MonoBehaviour
{
	public GameObject go_PS;

	public Material mat;

	public GameObject go_PS_H;

	public Material mat_H;

	public float checkInterval;

	private MeshRenderer mr;

	private RelicConfig relicCfg;

	private float checkIntervalTimer;

	public float ExtraDamageRatio { get; private set; }

	public void Intialize(RelicConfig relicCfg)
	{
		this.relicCfg = relicCfg;
		if (GameMgr.IsHarmony_Static)
		{
			Object.Destroy(go_PS);
			go_PS = go_PS_H;
			mat = mat_H;
		}
		else
		{
			Object.Destroy(go_PS_H);
		}
		if (mr == null)
		{
			mr = Object.Instantiate(PlayerMgr.Inst.PlayerCtrller.mr, PlayerMgr.Inst.PlayerCtrller.mr.transform.parent);
			mr.material = mat;
			mr.transform.position += new Vector3(0f, 0f, 0.001f);
			mr.material.SetTexture("_MainTex", PlayerMgr.Inst.PlayerCtrller.mr.material.mainTexture);
			go_PS.transform.SetParent(mr.transform.parent);
			go_PS.transform.localPosition = Vector3.zero + new Vector3(0f, 0f, 0.001f);
			go_PS.transform.localScale = Vector3.one;
			mr.gameObject.SetActive(value: false);
			go_PS.SetActive(value: false);
		}
	}

	private void Update()
	{
		checkIntervalTimer += Time.deltaTime;
		if (checkIntervalTimer >= checkInterval)
		{
			checkIntervalTimer = 0f;
			if (PlayerMgr.Inst.PlayerHPRatio <= (float)relicCfg.int1.result / 100f)
			{
				ExtraDamageRatio = (float)relicCfg.int2.result / 100f;
				mr.gameObject.SetActive(value: true);
				go_PS.SetActive(value: true);
			}
			else
			{
				ExtraDamageRatio = 0f;
				mr.gameObject.SetActive(value: false);
				go_PS.SetActive(value: false);
			}
		}
	}

	public void DestroySelf()
	{
		Object.Destroy(base.gameObject);
	}

	private void OnDestroy()
	{
		if ((bool)mr)
		{
			Object.Destroy(mr.gameObject);
		}
		if ((bool)go_PS)
		{
			Object.Destroy(go_PS);
		}
	}
}
