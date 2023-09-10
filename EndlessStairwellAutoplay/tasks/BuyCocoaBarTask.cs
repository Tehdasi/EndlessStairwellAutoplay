using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndlessStairwellAutoplay
{
	internal class BuyCocoaBarTask : Task
	{
		public BuyCocoaBarTask() 
		{
			Add(
				new GotoFloorTask(GotoFloorTask.FloorType.normal, 151),
				new Task((m) =>
				{
					if (m.floor == 0)
						return null;
					else
						return m.CocoaBarPrestige();
				})
				); 
		}
	}
}
