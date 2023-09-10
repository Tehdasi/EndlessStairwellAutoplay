//using OpenQA.Selenium.DevTools.V111.Debugger;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EndlessStairwellAutoplay
{
	internal class GotoFloorTask : Task
	{
		public enum FloorType
		{
			normal,
			tempRuneStore,
			permRuneStore,
			rooms,
			firstRooms
		}

		int targetFloor;
		FloorType floorType;
		int cycle;

		public GotoFloorTask( FloorType ft,  int targetFloor= -1)
		{
			cycle = 1;
			this.targetFloor = targetFloor;
			this.floorType= ft;

			parms = $"{ft} {targetFloor}";

			Add(new GotoStairsTask());
			Add( (m)=> {
				long rf = targetFloor;

				switch (floorType)
				{
					case FloorType.rooms:
						if (m.roomFloors[targetFloor] == -1)
						{
							int targetSection = targetFloor / 5;
							return InsertTask(m, new ExploreFloorsTask(targetSection * 50, (targetSection + 1) * 50));
						}
						else
							rf = m.roomFloors[targetFloor];
						break;
					case FloorType.permRuneStore:
						if (m.permRuneShopFloor == -1)
							return InsertTask(m, new ExploreFloorsTask(1, 10));
						else
							rf = m.permRuneShopFloor;
						break;
					case FloorType.tempRuneStore:
						if (m.tempRuneShopFloor == -1)
							return InsertTask(m, new ExploreFloorsTask(1, 10));
						else
							rf = m.tempRuneShopFloor;
						break;
					case FloorType.firstRooms:
						{
							// goto ground floor, walk upwards until a room is hit
							if (m.roomFloors[0] == m.floor)
								return null;
							else
							{
								if (cycle == 0 )
								{
									cycle++;

									if (m.hasGroundFloorJump)
										return Model.FloorGroundJump();
									else
										return InsertTask( m, new GotoFloorTask(FloorType.normal, 1 ) );
								}
								else
									return Model.FloorUp();
							}
						}

				}


				if (rf == m.floor)
					return null;

				if (rf == 99 && m.hasFloor99Jump)
					return Model.Floor99Jump();

				if (rf > m.floor)
					return new ClickAct("#floorUpButton");
				else
					return new ClickAct("#floorDownButton");
			} );
		}

	}
}
