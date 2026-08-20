namespace GameServer;

public class Response<T_Data> where T_Data : class
{
	public StatusCode code;

	public string message;

	public T_Data data;
}
public class Response : Response<object>
{
}
