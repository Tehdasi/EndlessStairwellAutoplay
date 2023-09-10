using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndlessStairwellAutoplay
{
	internal class CocaPrestigeTask : Task
	{
		public CocaPrestigeTask(bool getItems, int targetLevel, int targetVanilla = 0, int[]? buy = null, bool prestigeStop= false ) 
		{
			parms = $" target lvl:{targetLevel} buy:{buy}";
			Add(new HuntTask(3, 1, (m) => { return m.level >= 20; }));
			if (getItems)
				Add(
					new HuntTask(99, 3, (m) => { return m.haveKey; }),
					new HuntTask(99, 4, (m) => { return m.haveRing && m.level >= 30; }));

			Add(new HuntTask(3, 5, (m) => { return m.level >= 55; }),
				new HuntTask(3, 6, (m) => { return m.level >= 100; }),
				new HuntTask(3, 7, (m) => { return m.level >= 250; }),
				new HuntTask(3, 8, 
					(m) => { return m.level >= targetLevel && m.vanillaHoney > targetVanilla;  }, useVanillaHoney: false),
				new GotoFloorTask(GotoFloorTask.FloorType.normal, 99));

			if (!prestigeStop)
			{
				if (buy!= null)
				{
					foreach( int i in buy )
						Add(new SingleActTask(Model.BuyCocoaUpgrade(i)));
				}

				Add(new Task((m) =>
				{
					if (m.floor == 0)
						return null;
					else
						return m.AlterPrestige();
				}));
			}
		}
	}
}
