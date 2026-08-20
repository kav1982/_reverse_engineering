using UnityEngine;

public class Curse_RandomBombMono : LayerCorrect
{
	[Space(50f)]
	public GameObject go_Model;

	public GameObject go_ExplosionEF;

	public float recycleDelay;

	public AudioSource as_;

	private CurseConfig curseCfg;

	private bool isExplosioned;

	private float timer;

	public override void OnEnable()
	{
		base.OnEnable();
		go_Model.SetActive(value: true);
		go_ExplosionEF.SetActive(value: false);
		as_.Play();
		if (as_.volume != DataMgr.settingData.GetFinalSound())
		{
			as_.volume = DataMgr.settingData.GetFinalSound();
		}
		isExplosioned = false;
		timer = 0f;
	}

	public void Explode()
	{
		isExplosioned = true;
		go_Model.SetActive(value: false);
		go_ExplosionEF.SetActive(value: true);
		as_.Stop();
		SEMgr.Inst.spell1012Explosion.PlaySE();
	}

	private void Update()
	{
		if (isExplosioned)
		{
			timer += Time.deltaTime;
			if (timer >= recycleDelay)
			{
				timer = 0f;
				ObjPoolMgr.Inst.RecycleGO(base.gameObject);
			}
		}
	}
}
