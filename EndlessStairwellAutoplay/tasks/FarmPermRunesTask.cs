using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndlessStairwellAutoplay
{
	internal class FarmPermRunesTask : Task
	{

		public FarmPermRunesTask()
		{
			Func<Model,bool> huntGoal = (m) => {
				return m.honey > 0 &&
					((m.redRunes < 10 && m.redRuneFragments >= 4) ||
					(m.greenRunes < 5 && m.greenRuneFragments >= 4) ||
					(m.blueRunes < 5 && m.blueRuneFragments >= 4)); };

			// assumes that we start at ground floor
			Add((m) => {
				if (!huntGoal(m) && !m.floorHasRooms)
					return Model.FloorUp();

				if (m.redRunes == 10 && m.greenRunes == 5 && m.blueRunes == 5)
					return null;

				if ( !huntGoal(m) )
					return InsertTask( m, new HuntTask( 3, 0, huntGoal) );
				
				if( !m.atStairs )
					return InsertTask( m, new GotoStairsTask());

				if (!m.atPermRuneShop)
					return InsertTask(m, new GotoFloorTask(GotoFloorTask.FloorType.permRuneStore));

				if (m.honey > 0 && m.redRunes < 10 && m.redRuneFragments >= 4)
					return m.BuyRedPermRune();

				if (m.honey > 0 && m.greenRunes < 5 && m.greenRuneFragments >= 4)
					return m.BuyGreenPermRune();

				if (m.honey > 0 && m.blueRunes < 5 && m.blueRuneFragments >= 4)
					return m.BuyBluePermRune();


				return InsertTask(m, new HuntTask(3, 0, huntGoal));
			} );
		}
	}
}
