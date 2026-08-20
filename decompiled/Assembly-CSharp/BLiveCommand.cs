using System.Collections.Generic;
using JetBrains.Annotations;

public abstract class BLiveCommand
{
	private static readonly Dictionary<BLiveCommandCacheType, int> _cacheCounts = new Dictionary<BLiveCommandCacheType, int>
	{
		{
			BLiveCommandCacheType.RelicAndCurse,
			200
		},
		{
			BLiveCommandCacheType.SummonEnemy,
			50
		},
		{
			BLiveCommandCacheType.NoCache,
			0
		}
	};

	public bool paid { get; protected set; }

	[CanBeNull]
	public string user { get; protected set; }

	public abstract BLiveCommandCacheType CacheType { get; }

	public int CacheCount => _cacheCounts[CacheType];

	protected BLiveCommand(bool paid, [CanBeNull] string user)
	{
		this.paid = paid;
		this.user = user;
	}

	public abstract void Execute();

	public abstract bool CanExecute();
}
