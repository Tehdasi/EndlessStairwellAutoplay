using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndlessStairwellAutoplay
{
	internal class GotoStairsTask : Task
	{
		public GotoStairsTask( bool useVanilla= true )
		{
			Add((m) =>
			{
				if (m.atStairs)
					return null;

				if (m.inFight)
					return InsertTask(m, new FightTask( useVanilla ));

				return m.ToStairs();
			});
		}
	}
}
