using OpenQA.Selenium.DevTools.V112.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace EndlessStairwellAutoplay.tasks
{
	internal class WinGameTask : Task
	{
		public WinGameTask() {
			subTasks.Add(new FarmPermRunesTask());
			AddMarker("1st Prestige");
			subTasks.Add(new FarmCocoaUpgradesTask());
			AddMarker("All Cocoa Upgrades");
			subTasks.Add(new FarmPlasmUpgradesTask());
			AddMarker("All Plasm Upgrades");
			subTasks.Add(new GotoFloorTask(GotoFloorTask.FloorType.normal, 99));
			subTasks.Add(new Task((m) =>
			{
				if (m.floor == 0)
					return null;
				else
					return m.AlterPrestige();
			}));

			Add( new FarmCocoaBarsTask() );
		}
	}
}
