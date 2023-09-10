using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndlessStairwellAutoplay
{
	internal class TestFarm : Task
	{
		public TestFarm() 
		{
			subTasks.Add(new FarmPermRunesTask());
			AddMarker("1st Prestige");
			subTasks.Add(new MultifloorHuntTask( 3, (m) => false, true, true  ));

		}

	}
}
