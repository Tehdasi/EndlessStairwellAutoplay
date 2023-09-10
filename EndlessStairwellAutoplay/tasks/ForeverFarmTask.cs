using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace EndlessStairwellAutoplay
{
	internal class ForeverFarmTask : Task
	{
		Dictionary<int, float> floorAverageHp;

		int FloorForFarming( Model m )
		{
			return 1;
		}

		public ForeverFarmTask()
		{
			floorAverageHp = new Dictionary<int, float>();

			Add(new GotoFloorTask(GotoFloorTask.FloorType.rooms, 0));
			Add((m) =>
			{
				if (m.isDead)
					return m.CloseDeathScreen();

				if (m.inFight)
				{
					return InsertTask(m, new FightTask(true));
				}

				if (m.redTime < 5 && m.greenTime < 5 && m.blueTime < 5 &&
					m.redRuneFragments > 30 && m.greenRuneFragments > 30 && m.blueRuneFragments > 30 &&
					m.honey > 9)
					return InsertTask(m, new BuyTempRunesTask(m.floor));

				if (m.retreatStarted || m.curHealth < 100 || m.roomsAway == 4)
					return m.ToStairs();
				else if (m.atStairs)
					return m.EnterFloor();
				else
					return m.NextRoom();
			});
		}
	}
}
