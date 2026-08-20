using System;
using System.Collections.Generic;

public class MobileData
{
	public enum AimType
	{
		StrongAutoAim,
		WeakAutoAim
	}

	public bool mobileStickMoveLerp = true;

	public bool indieInteractButton;

	public float virtualStickScale = 1f;

	public float virtualStickPosition;

	public bool virtualStickRecover = true;

	public float rightStickSensitiive = 0.5f;

	public AimType aimType;

	public bool halfAutoAimRange;

	public bool canFoldWand = true;

	public bool wandFolded;

	public List<MobileVirtualButtonData> virtualStickData2 = new List<MobileVirtualButtonData>(Enum.GetNames(typeof(VirtualStickSizeAdjust.AdjustType)).Length);

	public MobileData()
	{
		for (int i = 0; i < virtualStickData2.Count; i++)
		{
			virtualStickData2[i] = new MobileVirtualButtonData();
		}
	}
}
