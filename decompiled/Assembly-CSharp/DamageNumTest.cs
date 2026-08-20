using UnityEngine;
using UnityEngine.VFX;

public class DamageNumTest : MonoBehaviour
{
	public VisualEffect vfx;

	public int spawnNumCountPerFrame;

	public Vector2 damageNumRange;

	public float spawnCoordRange;

	public Color[] colors;

	private GraphicsBuffer damageNumBuffer;

	private GraphicsBuffer colorBuffer;

	private Vector4[] damageNums;

	private int countID = Shader.PropertyToID("Count");

	private void Start()
	{
		damageNumBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, spawnNumCountPerFrame, 16);
		damageNums = new Vector4[spawnNumCountPerFrame];
		colorBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, colors.Length, 16);
		colorBuffer.SetData(colors);
		vfx.SetGraphicsBuffer("DamageData", damageNumBuffer);
		vfx.SetGraphicsBuffer("ColorData", colorBuffer);
	}

	private void OnDestroy()
	{
		damageNumBuffer?.Release();
		colorBuffer?.Release();
	}

	private void Update()
	{
		Vector2 zero = Vector2.zero;
		Vector2 vector = new Vector2(spawnCoordRange, spawnCoordRange);
		for (int i = 0; i < spawnNumCountPerFrame; i++)
		{
			float x = Random.Range(zero.x, vector.x);
			float y = Random.Range(vector.y, zero.y);
			float z = Random.Range(damageNumRange.x, damageNumRange.y);
			float w = Random.Range(0, colors.Length);
			damageNums[i] = new Vector4(x, y, z, w);
		}
		damageNumBuffer.SetData(damageNums);
		vfx.SetInt(countID, damageNums.Length);
		vfx.Play();
	}
}
