using UnityEngine;

public class MappingCam : MonoBehaviour
{
	public static float MappingCamHeightOrder = 100f;

	public MeshRenderer mr_Output;

	public Camera cam;

	public int pixelPerMeter;

	public void Initialize(Vector3 texturePoint, float textureWidth, float textureHeight, MappingCamType type)
	{
		base.transform.position = Tool2D.IgnoreZPoint(texturePoint, MappingCamHeightOrder);
		MappingCamHeightOrder += 1f;
		mr_Output.transform.localScale = new Vector3(textureWidth, textureHeight, 1f);
		mr_Output.transform.position = texturePoint;
		cam.clearFlags = CameraClearFlags.Color;
		RenderTexture renderTexture = new RenderTexture((int)(textureWidth * (float)pixelPerMeter), (int)(textureHeight * (float)pixelPerMeter), 0);
		cam.targetTexture = renderTexture;
		cam.orthographicSize = textureHeight / 2f;
		mr_Output.material.SetTexture("_MainTex", renderTexture);
	}

	public void MappingObj(GameObject go)
	{
		go.transform.position = Tool2D.IgnoreZPoint(go.transform.position, base.transform.position.y);
	}
}
