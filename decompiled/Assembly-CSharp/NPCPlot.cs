public class NPCPlot
{
	public int hdID;

	public bool isInteract;

	public NPCPlot(int hdID)
	{
		this.hdID = hdID;
		isInteract = false;
	}

	public void SetNewState(int hdID)
	{
		this.hdID = hdID;
		isInteract = false;
	}
}
