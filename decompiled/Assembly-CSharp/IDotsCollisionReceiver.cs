using Unity.Physics.Stateful;

public interface IDotsCollisionReceiver : IDotsPhysicsReciever
{
	void OnCollisionEnter_Dots(StatefulCollisionEvent collision);

	void OnCollisionStay_Dots(StatefulCollisionEvent collision);

	void OnCollisionExit_Dots(StatefulCollisionEvent collision);
}
