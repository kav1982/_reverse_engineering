using UnityEngine;

public class Monster50_CircleBullet : MonoBehaviour
{
	public enum EffectState
	{
		Shoot,
		Rotate,
		Die
	}

	public float maxRadius;

	public float nowRadius;

	public float rotateSpeed;

	public float moveSpeed;

	private Vector3 followPoint;

	public Vector3 moveDir;

	[Header("状态机")]
	private StateVariableMgr varMgr = new StateVariableMgr();

	private EffectState _state;

	private bool stateQuit;

	private float stateExistTime;

	public EffectState state
	{
		get
		{
			return _state;
		}
		set
		{
			stateExistTime = 0f;
			stateQuit = true;
			_state = value;
			varMgr.Clear();
		}
	}

	private void Initialize()
	{
		followPoint = Tool2D.IgnoreZPoint(base.transform.position);
	}

	private void Update()
	{
		stateExistTime += Time.deltaTime;
		if (stateQuit)
		{
			stateQuit = false;
		}
		switch (state)
		{
		}
	}
}
