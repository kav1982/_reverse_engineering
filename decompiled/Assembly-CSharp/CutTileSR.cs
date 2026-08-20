using UnityEngine;

[ExecuteAlways]
public class CutTileSR : MonoBehaviour
{
	public Texture2D texture2D;

	public Material material;

	public int verticalCount;

	public int HorizontalCount;

	public float offsetVertical;

	public float offsetHorizontal;

	public Vector3 scaler = new Vector3(1f, 1f, 1f);

	public Vector3 childOffset;

	public void ReGenerateTile()
	{
	}
}
