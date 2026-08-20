using UnityEngine;
using UnityEngine.UI;

public class UIReverseMask : Image
{
	public override Material materialForRendering
	{
		get
		{
			Material obj = new Material(base.materialForRendering);
			obj.SetInt("_StencilComp", 6);
			return obj;
		}
	}
}
