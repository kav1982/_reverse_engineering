using UnityEngine;

public class SpecialObj3 : MonoBehaviour, IRoomObjExtraData, ITrap
{
	public Transform tsf_Layer;

	public BoxCollider bc;

	public AIPattern pattern;

	public Animator animaStyle1;

	public Animator animaStyle2;

	public AnimaEvent aeStyle1;

	public AnimaEvent aeStyle2;

	[Header("Pattern3")]
	public TriggerIn triggerEnter;

	private Animator applyedAnima;

	private float animaDelayTime;

	private float animaDelayTimer;

	private bool isDelayFinish;

	private bool isPattern3Attating;

	private bool isInvalid;

	private void Start()
	{
		tsf_Layer.position = Tool2D.GetLayerPoint(base.transform) + new Vector3(0f, 0f, 0.009f);
		if (Random.Range(0, 2) == 0)
		{
			applyedAnima = animaStyle1;
			animaStyle1.gameObject.SetActive(value: true);
			aeStyle1.DoAction = DoAnima;
			Object.Destroy(animaStyle2.gameObject);
		}
		else
		{
			applyedAnima = animaStyle2;
			animaStyle2.gameObject.SetActive(value: true);
			aeStyle2.DoAction = DoAnima;
			Object.Destroy(animaStyle1.gameObject);
		}
		switch (pattern)
		{
		case AIPattern.Pattern1:
			bc.enabled = true;
			applyedAnima.Play("ShowDirect");
			break;
		case AIPattern.Pattern2:
			bc.enabled = false;
			applyedAnima.Play("HideDirect");
			break;
		case AIPattern.Pattern3:
			bc.enabled = false;
			triggerEnter.Initialize(TriggerEnter);
			applyedAnima.Play("HideDirect");
			break;
		default:
			Debug.LogError(pattern);
			break;
		}
	}

	private void Update()
	{
		if (!isInvalid && !isDelayFinish && pattern == AIPattern.Pattern2)
		{
			animaDelayTimer += Time.deltaTime;
			if (animaDelayTimer >= animaDelayTime)
			{
				isDelayFinish = true;
				applyedAnima.Play("Loop");
			}
		}
	}

	private void TriggerEnter(Collider other)
	{
		if (!isPattern3Attating)
		{
			if (other.tag == "Monster" || other.tag == "Teammate")
			{
				TriggerOnce();
			}
			else if (other.IsPlayerTrigger())
			{
				TriggerOnce();
			}
		}
		void TriggerOnce()
		{
			UnitProperty component = other.GetComponent<UnitProperty>();
			if (component != null && !component.IsFly)
			{
				isPattern3Attating = true;
				applyedAnima.Play("Once", 0, 0f);
			}
		}
	}

	private void DoAnima(string animaName)
	{
		switch (animaName)
		{
		case "PlaySE":
			SEMgr.Inst.so3.PlaySE(base.transform.position);
			break;
		case "Out":
			if (bc != null)
			{
				bc.enabled = true;
			}
			break;
		case "In":
			if (bc != null)
			{
				bc.enabled = false;
			}
			isPattern3Attating = false;
			break;
		default:
			Debug.LogError(animaName);
			break;
		}
	}

	public void SetExtraData(float data1, float data2, float data3)
	{
		animaDelayTime = data1;
	}

	public void SetTrapInvalid()
	{
		if (!isInvalid)
		{
			isInvalid = true;
			applyedAnima.Play("HideDirect");
			Object.Destroy(bc);
			if (triggerEnter != null)
			{
				triggerEnter.gameObject.SetActive(value: false);
			}
		}
	}
}
