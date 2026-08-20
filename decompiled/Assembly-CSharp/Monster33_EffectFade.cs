using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Monster33_EffectFade : LayerCorrect
{
	[Space(50f)]
	public Animator effectAnimator;

	public List<ParticleSystem> allParticles = new List<ParticleSystem>();

	public List<GameObject> Veins = new List<GameObject>();

	public SpriteRenderer VeinSurfaceRenderer;

	public List<Transform> explodePoints = new List<Transform>();

	public GameObject deadVein;

	public ParticleSystem bumpParticle;

	public SpriteRenderer spriteRenderer;

	public static string[] corpseMeat = new string[4] { "EF_Corpse_Meat1", "EF_Corpse_Meat2", "EF_Corpse_Meat3", "EF_Corpse_Bone1" };

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void StartFade(UnitProperty thisPpt)
	{
		effectAnimator.Play("fade");
		foreach (ParticleSystem allParticle in allParticles)
		{
			allParticle.Stop();
		}
		foreach (GameObject vein in Veins)
		{
			vein.SetActive(value: false);
		}
		foreach (Transform explodePoint in explodePoints)
		{
			if (!GameMgr.IsHarmony_Static)
			{
				VeinExplode(explodePoint, thisPpt);
			}
		}
		deadVein.SetActive(value: true);
	}

	public void VeinExplode(Transform explodePoint, UnitProperty thisPpt)
	{
		Vector3 vector = new Vector3(explodePoint.position.x, explodePoint.position.y, thisPpt.transform.position.z);
		BloodSplatCreateSystem.Inst.CreateBloodSplat(new CreateBloodSplatRequest
		{
			directional = false,
			point = vector,
			size = thisPpt.unitCfg.bloodSplatSize
		});
		ObjPoolMgr inst = ObjPoolMgr.Inst;
		FixedString128Bytes deadEF = thisPpt.unitCfg.deadEF;
		inst.GetGO("Prefabs/EF/" + deadEF.ToString(), vector);
		for (int i = 0; i < thisPpt.unitCfg.corpseCount; i++)
		{
			CorpseSystem.Inst.CreateCorpse(CorpseType.Flesh, vector, Vector3.zero);
		}
	}
}
