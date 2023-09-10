using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndlessStairwellAutoplay
{
	internal class MultifloorHuntTask : Task
	{
		//class FloorLevel
		//{
		//	public Num minLevel;
		//	public int floorNum;

		//	public FloorLevel(Num  minLevel, int floorNum )
		//	{
		//		this.minLevel = minLevel;
		//		this.floorNum = floorNum;
		//	}
		//}

		Num[] fls;

		public MultifloorHuntTask(
			int maxRoomsAway, Func<Model, bool> goalFunc, bool useHoney = true, bool useVanillaHoney = true )
		{
			fls = new Num[] {
				Num.From(13),
				Num.From(14),
				Num.From(16),
				Num.From(17),
				Num.From(18)
			};

			Add( (m)=> {
				if (goalFunc(m))
					return null;


				int targetRooms= -1;

				for( int i= 0; i< fls.Length && targetRooms==-1; i++ )
				{
					if( m.level <= fls[i] )
						targetRooms = i;
				}

				int ttf = -1;

				if (targetRooms != -1)
				{

					ttf = m.roomFloors[targetRooms];

					if( ttf== -1 )
						return InsertTask(m, new GotoFloorTask(GotoFloorTask.FloorType.rooms, targetRooms));
				}


				Debug.Assert(ttf != -1);

				if( ttf != m.floor )
					return InsertTask( m, new GotoFloorTask( GotoFloorTask.FloorType.normal, ttf ) );

				if (useVanillaHoney && m.energy < 20 && m.vanillaHoney > 0)
					return m.UseVanillaHoney();

				if (useHoney && m.curHealth < 50 && m.honey > 20)
					return m.UseHoney();

				if (m.isDead)
					return m.CloseDeathScreen();

				if (m.inFight)
					return InsertTask(m, new FightTask(useVanillaHoney));


				if (m.redTime < 5 && m.greenTime < 5 && m.blueTime < 5 &&
					m.redRuneFragments > 30 && m.greenRuneFragments > 30 && m.blueRuneFragments > 30 &&
					m.honey > 9)
					return InsertTask(m, new BuyTempRunesTask(m.floor));


				{
					if (m.retreatStarted || m.curHealth < 100 || m.roomsAway == maxRoomsAway)
						return m.ToStairs();
					else if (m.atStairs)
						return m.EnterFloor();
					else
						return m.NextRoom();
				}

			});
		}
	}
}
