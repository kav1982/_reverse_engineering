using Unity.Entities;

public interface IDotsTriggerReceiver : IDotsPhysicsReciever
{
	void OnTriggerEnter_Dots(Entity other);

	void OnTriggerStay_Dots(Entity other);

	void OnTriggerExit_Dots(Entity other);
}
