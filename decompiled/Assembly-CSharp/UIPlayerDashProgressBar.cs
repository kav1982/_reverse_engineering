using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerDashProgressBar : MonoBehaviour
{
	public CanvasGroup canvas;

	public Image image;

	private static readonly int Process = Shader.PropertyToID("_Progress");

	private void Update()
	{
		World defaultGameObjectInjectionWorld = World.DefaultGameObjectInjectionWorld;
		if (defaultGameObjectInjectionWorld == null || !defaultGameObjectInjectionWorld.IsCreated)
		{
			return;
		}
		using EntityQuery entityQuery = defaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<SpellDashDriverSingleton>());
		if (!entityQuery.IsEmpty)
		{
			SpellDashDriverSingleton singleton = entityQuery.GetSingleton<SpellDashDriverSingleton>();
			canvas.alpha = ((IsHeating() || singleton.IsDashing) ? 1 : 0);
			if (!(canvas.alpha <= 0f))
			{
				float progress = GetProgress(singleton);
				image.material.SetFloat(Process, IsHeating() ? 1f : (1f - progress));
				image.fillAmount = progress;
			}
		}
	}

	private bool IsHeating()
	{
		return PlayerMgr.Inst.PlayerCtrller.dashOverHeatProgressRatio > 0f;
	}

	private float GetProgress(SpellDashDriverSingleton data)
	{
		if (IsHeating())
		{
			return 1f - PlayerMgr.Inst.PlayerCtrller.dashOverHeatProgressRatio;
		}
		if (data.IsDashing)
		{
			return data.DashRemainingTime / data.TotalDashTime;
		}
		return 0f;
	}
}
