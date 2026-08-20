using System;
using System.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class QualityPreset : MonoBehaviour
{
	[BurstCompile]
	public struct CpuBenchmarkJob : IJobParallelFor
	{
		public NativeArray<float> result;

		public void Execute(int index)
		{
			float num = (float)index * 0.1f;
			for (int i = 0; i < 500000; i++)
			{
				num = math.sin(num) * math.cos(num + 1.2345f) + math.sqrt(num + 0.123f);
			}
			result[index] = num;
		}
	}

	public static bool cpuTesting = false;

	public static float LastTestScore = 0f;

	public static int QualitySet = 1;

	public static QualityPreset Inst;

	public static Action<float> OnFinish;

	public static void SetQuality()
	{
		RuntimePlatform platform = Application.platform;
		int systemMemorySize = SystemInfo.systemMemorySize;
		switch (platform)
		{
		case RuntimePlatform.IPhonePlayer:
			SetIOSQuality(systemMemorySize);
			break;
		case RuntimePlatform.Android:
			SetAndroidQuality(systemMemorySize);
			break;
		}
	}

	private static void SetIOSQuality(int memoryMB)
	{
		int num = memoryMB / 1024;
		if (num <= 3)
		{
			SetLow();
		}
		else if (num == 4)
		{
			SetMedium();
		}
		else if (num >= 6)
		{
			SetHigh();
		}
		else
		{
			SetMedium();
		}
	}

	private static void SetAndroidQuality(int memoryMB)
	{
		int num = memoryMB / 1024;
		Debug.Log($"Android 内存: {memoryMB}MB / {num}GB");
		if (num <= 6)
		{
			SetLow();
		}
		else if (num >= 12)
		{
			SetHigh();
		}
		else
		{
			CpuTest();
		}
	}

	public static void CpuTest(Action<float> callback = null)
	{
		if (cpuTesting)
		{
			Debug.Log("CPU 测试已在进行中");
			return;
		}
		cpuTesting = true;
		CreateTesterAndRun(delegate(float cost)
		{
			cpuTesting = false;
			LastTestScore = cost;
			Debug.Log($"CPU 测试纯耗时: {cost} 秒");
			if (cost < 0.8f)
			{
				SetHigh();
			}
			else if (cost < 1.2f)
			{
				SetMedium();
			}
			else
			{
				SetLow();
			}
			callback?.Invoke(cost);
			OnFinish?.Invoke(cost);
		});
	}

	private static void CreateTesterAndRun(Action<float> callback)
	{
		if (Inst != null)
		{
			Debug.Log("已有 CPU 测试实例");
			return;
		}
		Inst = new GameObject("CPUPerformanceTester").AddComponent<QualityPreset>();
		Inst.StartCoroutine(Inst.CPUScoreTestCoroutine(callback));
	}

	private IEnumerator CPUScoreTestCoroutine(Action<float> callback)
	{
		int num = Mathf.Max(1, SystemInfo.processorCount);
		NativeArray<float> result = new NativeArray<float>(num, Allocator.TempJob);
		CpuBenchmarkJob cpuBenchmarkJob = default(CpuBenchmarkJob);
		cpuBenchmarkJob.result = result;
		CpuBenchmarkJob jobData = cpuBenchmarkJob;
		float start = Time.realtimeSinceStartup;
		JobHandle handle = IJobParallelForExtensions.Schedule(jobData, num, 1);
		while (!handle.IsCompleted)
		{
			yield return null;
		}
		handle.Complete();
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		result.Dispose();
		float obj = realtimeSinceStartup - start;
		callback?.Invoke(obj);
		UnityEngine.Object.Destroy(base.gameObject);
		Inst = null;
	}

	private static void SetLow()
	{
		QualitySet = 0;
		Debug.Log("[Quality] Low");
	}

	private static void SetMedium()
	{
		QualitySet = 1;
		Debug.Log("[Quality] Medium");
	}

	private static void SetHigh()
	{
		QualitySet = 2;
		Debug.Log("[Quality] High");
	}
}
