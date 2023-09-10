using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndlessStairwellAutoplay
{
	internal class FarmPlasmUpgradesTask : Task
	{
		public FarmPlasmUpgradesTask()
		{
			Add(new HuntTask(3, 1, (m) => { return m.level >= 20; }),
				new HuntTask(99, 10, (m) => { return m.honeyPlasm > 5; }, useVanillaHoney: false),

				new GotoFloorTask(GotoFloorTask.FloorType.normal, 149),
				new SingleActTask(Model.BuyPlasmUpgrade(9)),

				new HuntTask(99, 10, (m) => { return m.honeyPlasm > 34; }, useVanillaHoney: false),

				new GotoFloorTask(GotoFloorTask.FloorType.normal, 149),
				new SingleActTask(Model.BuyPlasmUpgrade(6)),
				new SingleActTask(Model.BuyPlasmUpgrade(7)),
				new SingleActTask(Model.BuyPlasmUpgrade(8)),
				new SingleActTask(Model.BuyPlasmUpgrade(10)),

				new HuntTask(99, 10, (m) => { return m.honeyPlasm > (30 + 35 + 60 + 75 + 150); }, useVanillaHoney: false),
				new GotoFloorTask(GotoFloorTask.FloorType.normal, 149),
				new SingleActTask(Model.BuyPlasmUpgrade(11)),
				new SingleActTask(Model.BuyPlasmUpgrade(12)),
				new SingleActTask(Model.BuyPlasmUpgrade(13)),
				new SingleActTask(Model.BuyPlasmUpgrade(14)),
				new SingleActTask(Model.BuyPlasmUpgrade(15)));
		}
	}
}
