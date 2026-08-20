using UnityEngine;
using UnityEngine.VFX;

public class Monster30Chain : MonoBehaviour
{
	public LayerMask attackLayer;

	public LineRenderer lr_Laser;

	public LineRenderer lr_Shadow;

	public VisualEffect ve_Trail;

	public int damage;

	public float chainHeight;

	public float trailRatePerMeter;

	public float changeRateInterval;

	private UnitProperty monster30Ppt;

	private Transform tsf1;

	private Transform tsf2;

	private RaycastHit hit;

	private float changeRateIntervalTimer;

	private void Update()
	{
		if (tsf1 == null || tsf2 == null || !tsf1.gameObject.activeSelf || !tsf2.gameObject.activeSelf)
		{
			RecycleSelf();
			return;
		}
		Vector3 vector = tsf2.position - tsf1.position;
		float magnitude = vector.magnitude;
		if (UnitDotsSyncSystem.Raycast(tsf1.position, vector.normalized, magnitude, GameConst.Filter_MonsterAoeNoSpell, out var result))
		{
			TakeDamageInfo_Dots info = TakeDamageInfo_Dots.NewInfo(monster30Ppt.myEntity);
			info.damage = damage;
			info.teammateTakeDamageRatio = 2f;
			UnitDotsSyncSystem.AddTakeDamageRequest(result.entity, info);
			if (GameMgr.IsHarmony_Static)
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster30_ChainHit_H", result.point, 1f);
			}
			else
			{
				ObjPoolMgr.Inst.GetGO("Prefabs/EF/EF_Monster30_ChainHit", result.point, 1f);
			}
			SEMgr.Inst.spell3007Hit.PlaySE();
			RecycleSelf();
			return;
		}
		lr_Laser.SetPosition(0, Tool2D.GetLayerPoint(tsf1.position + new Vector3(0f, 0f, 0f - chainHeight)));
		lr_Laser.SetPosition(1, Tool2D.GetLayerPoint(tsf2.position + new Vector3(0f, 0f, 0f - chainHeight)));
		lr_Shadow.SetPosition(0, Tool2D.IgnoreZPoint(tsf1, 1.05f));
		lr_Shadow.SetPosition(1, Tool2D.IgnoreZPoint(tsf2, 1.05f));
		ve_Trail.SetVector3("Point1", Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(tsf1.position + new Vector3(0f, 0f, 0f - chainHeight)), 1.12f));
		ve_Trail.SetVector3("Point2", Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(tsf2.position + new Vector3(0f, 0f, 0f - chainHeight)), 1.12f));
		changeRateIntervalTimer += Time.deltaTime;
		if (changeRateIntervalTimer >= changeRateInterval)
		{
			changeRateIntervalTimer = 0f;
			ve_Trail.SetFloat("Rate", Vector3.Distance(tsf1.position, tsf2.position) * trailRatePerMeter);
		}
	}

	private void RecycleSelf()
	{
		ve_Trail.Stop();
		lr_Laser.SetPosition(0, Vector3.zero);
		lr_Laser.SetPosition(1, Vector3.zero);
		lr_Shadow.SetPosition(0, Vector3.zero);
		lr_Shadow.SetPosition(1, Vector3.zero);
		ve_Trail.SetVector3("Point1", Vector3.zero);
		ve_Trail.SetVector3("Point2", Vector3.zero);
		base.gameObject.SetActive(value: false);
	}

	public void Iniatialize(UnitProperty monster30Ppt, Transform tsf1, Transform tsf2)
	{
		if (tsf1 == null || tsf2 == null || !tsf1.gameObject.activeSelf || !tsf2.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		this.monster30Ppt = monster30Ppt;
		this.tsf1 = tsf1;
		this.tsf2 = tsf2;
		ve_Trail.SetVector3("Point1", Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(tsf1.position + new Vector3(0f, 0f, 0f - chainHeight)), 1.12f));
		ve_Trail.SetVector3("Point2", Tool2D.IgnoreZPoint(Tool2D.GetLayerPoint(tsf2.position + new Vector3(0f, 0f, 0f - chainHeight)), 1.12f));
		ve_Trail.transform.SetParent(base.transform.parent);
		ve_Trail.transform.position = Vector3.zero;
		if (GameMgr.IsMobile_Static)
		{
			ve_Trail.enabled = false;
		}
		else
		{
			ve_Trail.Play();
		}
		ve_Trail.SetFloat("Rate", Vector3.Distance(tsf1.position, tsf2.position) * trailRatePerMeter);
	}
}
