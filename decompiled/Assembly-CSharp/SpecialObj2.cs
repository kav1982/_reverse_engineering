using UnityEngine;

public class SpecialObj2 : LayerCorrect
{
	[Space(50f)]
	public LayerMask checkLayer;

	public float checkInterval;

	public float checkRadius;

	private float checkIntervalTimer;

	private void Update()
	{
		checkIntervalTimer += Time.deltaTime;
		if (!(checkIntervalTimer >= checkInterval))
		{
			return;
		}
		checkIntervalTimer = 0f;
		Collider[] array = Physics.OverlapSphere(base.transform.position, checkRadius, checkLayer);
		for (int i = 0; i < array.Length; i++)
		{
			UnitProperty component = array[i].GetComponent<UnitProperty>();
			if (component.unitCfg.id / 100 != 1001 && component.unitCfg.id / 100 != 1002 && component.unitCfg.id != 300101 && component.unitCfg.id / 100 != 1055 && !component.unitCfg.isFly)
			{
				component.SetMucus(checkInterval, 0.6f, 1f, changeColor: false);
			}
		}
	}
}
