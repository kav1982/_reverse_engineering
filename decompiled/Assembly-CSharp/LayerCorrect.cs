using UnityEngine;

public class LayerCorrect : MonoBehaviour
{
	public bool everyFrame;

	public bool afterEnableDestroy;

	public UpdateTiming updateTiming;

	public LayerCorrectType correctType = LayerCorrectType.Coordinate;

	public Transform tsf_Layer;

	public virtual void OnEnable()
	{
		if (tsf_Layer != null)
		{
			CorrectLayerOnce();
		}
		if (afterEnableDestroy)
		{
			Object.Destroy(this);
		}
	}

	public virtual void FixedUpdate()
	{
		if (everyFrame)
		{
			UpdateTiming updateTiming = this.updateTiming;
			if ((updateTiming == UpdateTiming.OnFixedUpdate || updateTiming == UpdateTiming.Both) && tsf_Layer != null)
			{
				CorrectLayerOnce();
			}
		}
	}

	public virtual void LateUpdate()
	{
		if (everyFrame)
		{
			UpdateTiming updateTiming = this.updateTiming;
			if ((updateTiming == UpdateTiming.OnLateUpdate || updateTiming == UpdateTiming.Both) && tsf_Layer != null)
			{
				CorrectLayerOnce();
			}
		}
	}

	public void CorrectLayerOnce()
	{
		tsf_Layer.position = Tool2D.GetLayerPoint(base.transform, correctType);
	}
}
