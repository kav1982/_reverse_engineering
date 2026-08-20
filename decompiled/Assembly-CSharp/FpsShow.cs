using Unity.Entities;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class FpsShow : MonoBehaviour
{
	public Text fpsText;

	public Text bigFpsText;

	public GameObject obj;

	private string memery;

	private float interval = 0.1f;

	private float intervalTimer;

	private EntityQuery spellQuery;

	private bool bigMode;

	private bool showFpsLastFrame;

	private bool showFps;

	private ProfilerRecorder triangleCountRecorder;

	private ProfilerRecorder cpuUsageRecorder;

	private ProfilerRecorder batchesRecorder;

	private ProfilerRecorder drawCallRecorder;

	public float batchCount;

	public float drallCalls;

	private void Start()
	{
		spellQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(typeof(SpellConfigComponentData));
	}

	private void StartFpsShow()
	{
		batchesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Batches Count");
		drawCallRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
	}

	private void StopFpsShow()
	{
		batchesRecorder.Dispose();
		drawCallRecorder.Dispose();
	}

	private void UpdateFpsShow()
	{
		if (batchesRecorder.Valid && batchesRecorder.LastValue != 3 && batchesRecorder.LastValue != 0L)
		{
			batchCount = batchesRecorder.LastValue;
		}
		if (drawCallRecorder.Valid && drawCallRecorder.LastValue != 3 && batchesRecorder.LastValue != 0L)
		{
			drallCalls = drawCallRecorder.LastValue;
		}
		if (Input.GetKeyDown(KeyCode.F))
		{
			fpsText.gameObject.SetActive(!fpsText.gameObject.activeSelf);
			bigFpsText.gameObject.SetActive(!bigFpsText.gameObject.activeSelf);
		}
		intervalTimer += Time.deltaTime;
		if (intervalTimer > interval)
		{
			intervalTimer = 0f;
			fpsText.text = GameMgr.Inst.GetFps().ToString("F1") + " | " + GameMgr.Inst.GetFps5().ToString("F1");
			Text text = fpsText;
			text.text = text.text + "\nB: " + batchCount + " | D: " + drallCalls;
			Text text2 = fpsText;
			text2.text = text2.text + "\nSC: " + spellQuery.CalculateEntityCount();
			if ((bool)ObjPoolMgr.Inst)
			{
				fpsText.text += $" | POC:{ObjPoolMgr.Inst.GetPoolObjectCount()} | ENT: {World.DefaultGameObjectInjectionWorld.EntityManager.UniversalQuery.CalculateEntityCount()}";
			}
			long num = Profiler.GetTotalReservedMemoryLong() / 1048576;
			long num2 = Profiler.GetTotalAllocatedMemoryLong() / 1048576;
			long num3 = Profiler.GetTotalUnusedReservedMemoryLong() / 1048576;
			memery = "\n" + $"{num3} | {num2} | {num}";
			fpsText.text += memery;
			Text text3 = fpsText;
			text3.text = text3.text + "\n<size=25>" + SystemInfo.graphicsDeviceName + "</size>";
			Text text4 = fpsText;
			text4.text = text4.text + "\n<size=25>" + SystemInfo.graphicsDeviceVersion + "</size>";
			fpsText.text += $"\n<size=25>{SystemInfo.maxComputeBufferInputsVertex}</size>";
			bigFpsText.text = "FPS：" + GameMgr.Inst.GetFps().ToString("F0").PadRight(4);
			Text text5 = bigFpsText;
			text5.text = text5.text + "\n5秒平均FPS：" + GameMgr.Inst.GetFps5().ToString("F0").PadRight(4);
		}
	}

	private void Update()
	{
		showFpsLastFrame = showFps;
		showFps = ScriptableObjMgr.Inst.testCtrller.ShowFps;
		obj.SetActive(showFps);
		if (showFps && !showFpsLastFrame)
		{
			StartFpsShow();
		}
		else if (!showFps && showFpsLastFrame)
		{
			StopFpsShow();
		}
		if (showFps)
		{
			UpdateFpsShow();
		}
	}
}
