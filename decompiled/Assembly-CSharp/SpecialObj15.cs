using Unity.Mathematics;
using UnityEngine;

public class SpecialObj15 : MonoBehaviour
{
	public Transform tsf_Tsf;

	public LayerMask checkLayer;

	public Vector3 halfSize;

	public float checkInterval;

	private float checkIntervalTimer;

	private void Start()
	{
		tsf_Tsf.position = Tool2D.GetLayerPoint(base.transform, LayerCorrectType.SO15);
	}

	private void Update()
	{
		checkIntervalTimer += Time.deltaTime;
		if (!(checkIntervalTimer >= checkInterval))
		{
			return;
		}
		checkIntervalTimer = 0f;
		Collider[] array = Physics.OverlapBox(base.transform.position, halfSize, quaternion.identity, checkLayer);
		for (int i = 0; i < array.Length; i++)
		{
			UnitProperty component = array[i].GetComponent<UnitProperty>();
			if (component != null)
			{
				component.SetReverseMove(0.5f);
			}
		}
	}
}
