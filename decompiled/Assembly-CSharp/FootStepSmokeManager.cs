using UnityEngine;

public class FootStepSmokeManager : MonoBehaviour
{
	public ParticleSystem smoke;

	private void Update()
	{
		if (PlayerMgr.Inst.TryGetPlayerPpt(out var playerPpt))
		{
			bool flag = PlayerController.NeedShowFlyAnima(playerPpt);
			if (!flag && smoke.isStopped)
			{
				smoke.Play();
			}
			else if (flag && !smoke.isStopped)
			{
				smoke.Stop();
			}
		}
	}
}
