using UnityEngine;
using UnityEngine.SceneManagement;

public class BackCampPortalMono : MonoBehaviour
{
	public float hoverSpeed;

	public float hoverAmplitude;

	public Transform tsf_Hover;

	public GameObject go_Outline;

	private float hoverTimer;

	private void Update()
	{
		hoverTimer += hoverSpeed * Time.deltaTime;
		tsf_Hover.localPosition = new Vector3(0f, Mathf.Sin(hoverTimer) * hoverAmplitude, 0f);
	}

	public void Select()
	{
		go_Outline.SetActive(value: true);
	}

	public void Unselect()
	{
		go_Outline.SetActive(value: false);
	}

	public void Interact()
	{
		PlayerMgr.Inst.PlayerCtrller.StopMotion();
		UIMgr.Inst.uiFade.Show(delegate
		{
			GameMgr.Inst.DestroyAllTeammate();
			GameMgr.Inst.ClearAllPool();
			GameMgr.Inst.AllFunctionReset();
			DataMgr.selectedWorldData.inBattle9 = false;
			if (DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Nightmare1)
			{
				DataMgr.selectedWorldData.storyFinishNightmare1 = true;
			}
			else if (DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Nightmare2)
			{
				DataMgr.selectedWorldData.storyFinishNightmare2 = true;
			}
			else if (DataMgr.selectedWorldData.selectedDifficulty == DifficultyType.Nightmare3)
			{
				DataMgr.selectedWorldData.storyFinishNightmare3 = true;
			}
			if (GameMgr.IsMobile_Static)
			{
				MobileMgr.inst.PluginActivity.UploadItemSnapshot(2);
			}
			DataMgr.selectedWorldData.BackCampCheckPlot();
			DataMgr.SaveSelectedWorldData();
			TimeScaleMgr.Inst.ClearAllTimeScaleModifyRequest();
			SceneManager.LoadScene("Camp");
		});
	}
}
