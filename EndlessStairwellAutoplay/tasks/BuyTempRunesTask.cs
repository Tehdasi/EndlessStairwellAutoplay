using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndlessStairwellAutoplay
{
	internal class BuyTempRunesTask : Task
	{
//		int originalFloor;

		public BuyTempRunesTask( int originalFloor )
		{
//			this.originalFloor= originalFloor;
			Add(
				new GotoFloorTask(GotoFloorTask.FloorType.tempRuneStore),
				new Task((m) =>
				{
					if (m.honey < 1 ) 
						return null;

					if (m.redTime < 170 && m.redRuneFragments >= 3 && m.greenRuneFragments >= 1 )
						return m.BuyRedTempRune();
					if (m.greenTime < 170 && m.greenRuneFragments >= 3 && m.blueRuneFragments >= 1)
						return m.BuyGreenTempRune();
					if (m.blueTime < 170 && m.blueRuneFragments >= 3 && m.redRuneFragments >= 1)
						return m.BuyBlueTempRune();

					return null;
				}),
				new GotoFloorTask(GotoFloorTask.FloorType.normal, originalFloor));

		}
	}
}
