using Unity.Collections;
using UnityEngine;

public class BossDeadCreateEffect : MonoBehaviour
{
	public float offsetZ;

	public VariableFloat offsetX;

	public VariableFloat offsetHeight;

	public float deadEFScale = 1f;

	public bool useFinalExplode;

	public Transform tsf_specifiedBloodRoot;

	private UnitProperty myProperty;

	private bool createEffect;

	private float createTimer;

	private float createIntervalTimer;

	private bool exploded;

	public void OnEnable()
	{
		if (myProperty != null && !myProperty.enabled)
		{
			myProperty.enabled = true;
		}
		createEffect = false;
		createTimer = 0f;
		exploded = false;
	}

	private void Start()
	{
		myProperty = GetComponent<UnitProperty>();
	}

	private void Update()
	{
		if (!createEffect)
		{
			return;
		}
		Vector3 vector = ((tsf_specifiedBloodRoot != null) ? Tool2D.IgnoreZPoint(tsf_specifiedBloodRoot.transform.position) : base.transform.position);
		createTimer += Time.deltaTime;
		if (createTimer < 2f)
		{
			createIntervalTimer += Time.deltaTime;
			if (createIntervalTimer >= 0.2f)
			{
				createIntervalTimer = 0f;
				ObjPoolMgr inst = ObjPoolMgr.Inst;
				FixedString128Bytes deadEF = myProperty.unitCfg.deadEF;
				inst.GetGO("Prefabs/EF/" + deadEF.ToString(), vector + new Vector3(offsetX.RandomResult(), 0f - offsetZ, 0f - offsetHeight.RandomResult() - offsetZ), Vector3.one * deadEFScale, 2f);
				myProperty.unitCfg.deadSEs.Value.PlaySE();
			}
			return;
		}
		if (useFinalExplode && !exploded)
		{
			if (myProperty.unitCfg.deadEF == "EF_Dead_Blood")
			{
				SEMgr.Inst.eliteDieFinal.PlaySE();
			}
			else
			{
				myProperty.unitCfg.deadSEs.Value.PlaySE();
			}
			ObjPoolMgr inst2 = ObjPoolMgr.Inst;
			FixedString128Bytes deadEF = myProperty.unitCfg.deadEF;
			inst2.GetGO("Prefabs/EF/" + deadEF.ToString(), vector + new Vector3(0f, 0f - offsetZ, (0f - (offsetHeight.value1 + offsetHeight.value2)) / 2f - offsetZ), Vector3.one * deadEFScale * 2f, 2f);
			exploded = true;
		}
		createEffect = false;
		myProperty.UnitBas.DotsAnnouncedDeath();
	}

	public void CreateEffect()
	{
		createEffect = true;
	}
}
