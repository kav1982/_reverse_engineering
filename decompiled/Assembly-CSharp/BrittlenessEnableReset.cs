using UnityEngine;

public class BrittlenessEnableReset : MonoBehaviour
{
	public UnitBase unitBase;

	public UnitProperty unitProperty;

	private void OnDisable()
	{
		unitBase.enabled = true;
		unitProperty.enabled = true;
	}
}
