using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class Monster43_Drain : MonoBehaviour
{
	public LineRenderer thisLineRenderer;

	public LineRenderer thisShadowLineRenderer;

	public Monster43 master;

	public float particleZOffest;

	public float lineHeight;

	public Entity targetEntity;

	private Entity tempEtt;

	private Entity lastFrameEtt;

	public ParticleSystem drainParticles;

	public float alphaChangeSpeed;

	private float nowAlphaFixer;

	private void Start()
	{
	}

	private void OnEnable()
	{
		nowAlphaFixer = 0f;
		drainParticles.Stop();
		drainParticles.Clear();
		thisLineRenderer.SetPosition(0, master.drainParticles.transform.position + new Vector3(0f, lineHeight, 0f));
		thisLineRenderer.SetPosition(1, master.drainParticles.transform.position + new Vector3(0f, lineHeight, 0f));
		thisShadowLineRenderer.SetPosition(0, Tool2D.GetLayerPoint(master.transform.position, LayerCorrectType.Shadow));
		thisShadowLineRenderer.SetPosition(1, Tool2D.GetLayerPoint(master.transform.position, LayerCorrectType.Shadow));
	}

	private void Update()
	{
		bool num = UnitDotsSyncSystem.EntityIsValid(targetEntity);
		if (!num)
		{
			targetEntity = Entity.Null;
		}
		if (num)
		{
			nowAlphaFixer += Time.deltaTime * alphaChangeSpeed;
			if (!drainParticles.isPlaying)
			{
				drainParticles.Play();
			}
		}
		else
		{
			nowAlphaFixer -= Time.deltaTime * alphaChangeSpeed;
			if (drainParticles.isPlaying)
			{
				drainParticles.Stop();
			}
		}
		nowAlphaFixer = Mathf.Clamp(nowAlphaFixer, 0f, 1f);
		thisLineRenderer.material.SetFloat("_Fade", nowAlphaFixer);
		thisShadowLineRenderer.material.SetFloat("_Fade", nowAlphaFixer);
		thisLineRenderer.SetPosition(0, master.drainParticles.transform.position);
		thisShadowLineRenderer.SetPosition(0, Tool2D.GetLayerPoint(master.transform.position, LayerCorrectType.Shadow));
		if (num)
		{
			Vector3 vector = UnitDotsSyncSystem.GetComponentData<LocalTransform>(targetEntity).Position;
			thisLineRenderer.SetPosition(1, Tool2D.GetLayerPoint(vector + new Vector3(0f, lineHeight, 0f)));
			thisShadowLineRenderer.SetPosition(1, Tool2D.GetLayerPoint(vector + new Vector3(0f, lineHeight, 0f), LayerCorrectType.Shadow));
			drainParticles.transform.position = thisLineRenderer.GetPosition(1) + new Vector3(0f, 0f, 0f - particleZOffest);
			_ = Tool2D.IgnoreZPoint(master.drainParticles.transform.position - vector).normalized;
			thisLineRenderer.material.SetFloat("_Length", Tool2D.IgnoreZPoint(master.drainParticles.transform.position - vector).magnitude);
			thisShadowLineRenderer.material.SetFloat("_Length", Tool2D.IgnoreZPoint(master.transform.position - vector).magnitude);
		}
	}
}
