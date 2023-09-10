using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndlessStairwellAutoplay
{
	internal class FarmCocoaUpgradesTask : Task
	{
		public FarmCocoaUpgradesTask()
		{
			Add( 
				new CocaPrestigeTask(true, 500),
				new CocaPrestigeTask(true, 500),
				new CocaPrestigeTask(true, 1000, buy: new int[] { 2 }),
				new CocaPrestigeTask(true, 500),
				new CocaPrestigeTask(true, 500),
				new CocaPrestigeTask(true, 1000),
				new CocaPrestigeTask(true, 2000, buy: new int[] { 4 }),
				new CocaPrestigeTask(true, 2000, targetVanilla: 20),
				new CocaPrestigeTask(true, 2000, targetVanilla: 20),
				new CocaPrestigeTask(true, 2000, targetVanilla: 20, buy: new int[] { 3, 5, 6, 7 }),
				new CocaPrestigeTask(true, 2000, targetVanilla: 20, buy: new int[] { 8 }));
		}
	}
}
