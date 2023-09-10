 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndlessStairwellAutoplay
{
	internal class FarmCocoaBarsTask : Task
	{
		public FarmCocoaBarsTask()
		{
			Num[] barTargets = { 
				Num.From( 1e10 ),
				Num.Parse("1e1000000")
			}; 

			Add(m=> {
				// got enough to buy a bar
				if (m.cocoaHoney > barTargets[m.cocoaBars])
					return InsertTask(m, new BuyCocoaBarTask());

				// assumption being, if we are over 1000 levels, we have bazillions of levels
				if (m.level > Num.From(1000))
				{
					Task t = new Task(
							new GotoFloorTask(GotoFloorTask.FloorType.normal, 99),
							new Task((m) =>
							{
								if (m.floor == 0)
									return null;
								else
									return m.AlterPrestige();
							}));
					return InsertTask(m, t);
				}

				return InsertTask(m, new Task(m =>
				{
					// assuming this is directly after a prestige
					if (m.floor != m.roomFloors[0])
						return Model.FloorUp();

					if (m.level < Num.From(1000))
					{
						if (m.cocoaHoney == Num.From(0))
							// if we don't have any cocoa honey, we need to go for higher floors to get
							// the levels required to prestige
							return InsertTask(m, new MultifloorHuntTask(1000, (m) => { return m.level >= Num.From(1000); }));
						else
							// if we have cocoa honey, it'll be lots, so we just need to go to the first set of rooms,
							// fight one fight, then prestige
							return InsertTask(m, new HuntTask(1000, 0, (m) => { return m.level >= Num.From(1000); }));
					}
					return null;
				}));
			});
		}
	}
}
