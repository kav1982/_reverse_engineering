using UnityEngine;

public class ScriptableObjMgr : MonoBehaviour
{
	public IntArraySO activateGirlLayerNeed;

	public TalentUpgrade2 talentUpgrade2;

	public EndlessTalentUpgrade EndlessTalentUpgrade;

	private static TestController _testCtrller;

	public StringArraySO themeAmbient;

	public StringArraySO themeMusic;

	public bool AutoInit;

	public TestController testCtrller => staticTestCtrller;

	public static TestController staticTestCtrller
	{
		get
		{
			if (_testCtrller == null)
			{
				_testCtrller = Resources.Load<TestController>("GameInitData/SO_TestCtrller");
				if (_testCtrller == null)
				{
					_testCtrller = ScriptableObject.CreateInstance<TestController>();
					_testCtrller.name = "SO_TestCtrller";
				}
			}
			return _testCtrller;
		}
	}

	public static ScriptableObjMgr Inst { get; private set; }

	private void Awake()
	{
		if (AutoInit)
		{
			Initialize();
		}
	}

	public void Initialize()
	{
		Inst = this;
	}
}
