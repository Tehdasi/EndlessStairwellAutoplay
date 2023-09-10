using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndlessStairwellAutoplay
{
	internal class HuntTask : Task
	{
		public HuntTask( int maxRoomsAway, int roomsFloor, 
			Func<Model,bool> goalFunc, bool useHoney= true, bool useVanillaHoney= true )
		{
			Add(new GotoFloorTask(GotoFloorTask.FloorType.rooms, roomsFloor));
			Add( (m) =>
			{
				if ( useVanillaHoney && m.energy < 20 && m.vanillaHoney > 0)
					return m.UseVanillaHoney();

				if ( useHoney && m.curHealth < 50 && m.honey > 20)
					return m.UseHoney();

				if (m.isDead)
					return m.CloseDeathScreen();

				if (m.inFight)
					return InsertTask(m, new FightTask( useVanillaHoney ));

				if (m.redTime < 5 && m.greenTime < 5 && m.blueTime < 5 &&
					m.redRuneFragments > 30 && m.greenRuneFragments > 30 && m.blueRuneFragments > 30 &&
					m.honey > 9)
					return InsertTask(m, new BuyTempRunesTask(m.floor));

				if (goalFunc(m))
					return null;

				{
					if (m.retreatStarted || m.curHealth < 100 || m.roomsAway == maxRoomsAway)
						return m.ToStairs();
					else if (m.atStairs)
						return m.EnterFloor();
					else
						return m.NextRoom();
				}

				return new WaitAct();
			});

		}
	}
}
