using UnityEngine;

[ExecuteAlways]
public class UIColorCopy : MonoBehaviour
{
	public CanvasRenderer thisImage;

	public CanvasRenderer CopyFrom;

	public void Update()
	{
		thisImage.SetColor(CopyFrom.GetColor());
	}
}
