using Unity.Collections;

namespace Unity.Physics.Stateful;

public struct StatefulSimulationEventBuffers<T> where T : unmanaged, IStatefulSimulationEvent<T>
{
	public NativeList<T> Previous;

	public NativeList<T> Current;

	public void AllocateBuffers()
	{
		Previous = new NativeList<T>(Allocator.Persistent);
		Current = new NativeList<T>(Allocator.Persistent);
	}

	public void Dispose()
	{
		if (Previous.IsCreated)
		{
			Previous.Dispose();
		}
		if (Current.IsCreated)
		{
			Current.Dispose();
		}
	}

	public void SwapBuffers()
	{
		NativeList<T> previous = Previous;
		Previous = Current;
		Current = previous;
		Current.Clear();
	}

	public void GetStatefulEvents(NativeList<T> statefulEvents, bool sortCurrent = true)
	{
		GetStatefulEvents(Previous, Current, statefulEvents, sortCurrent);
	}

	public static void GetStatefulEvents(NativeList<T> previousEvents, NativeList<T> currentEvents, NativeList<T> statefulEvents, bool sortCurrent = true)
	{
		if (sortCurrent)
		{
			currentEvents.Sort();
		}
		statefulEvents.Clear();
		int i = 0;
		int j = 0;
		while (i < currentEvents.Length && j < previousEvents.Length)
		{
			int num = previousEvents[j].CompareTo(currentEvents[i]);
			if (num == 0)
			{
				T value = currentEvents[i];
				value.State = StatefulEventState.Stay;
				statefulEvents.Add(in value);
				i++;
				j++;
			}
			else if (num < 0)
			{
				T value2 = previousEvents[j];
				value2.State = StatefulEventState.Exit;
				statefulEvents.Add(in value2);
				j++;
			}
			else
			{
				T value3 = currentEvents[i];
				value3.State = StatefulEventState.Enter;
				statefulEvents.Add(in value3);
				i++;
			}
		}
		if (i == currentEvents.Length)
		{
			for (; j < previousEvents.Length; j++)
			{
				T value4 = previousEvents[j];
				value4.State = StatefulEventState.Exit;
				statefulEvents.Add(in value4);
			}
		}
		else if (j == previousEvents.Length)
		{
			for (; i < currentEvents.Length; i++)
			{
				T value5 = currentEvents[i];
				value5.State = StatefulEventState.Enter;
				statefulEvents.Add(in value5);
			}
		}
	}
}
