using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class LoadUIs : IEnumerable<LoadUI>, IEnumerable
{
	public string desc;

	public List<LoadUI> ObjLoads;

	public IEnumerator<LoadUI> GetEnumerator()
	{
		return ObjLoads.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
