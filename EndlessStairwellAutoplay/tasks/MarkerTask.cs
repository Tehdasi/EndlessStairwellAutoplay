using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndlessStairwellAutoplay
{
	internal class MarkerTask : Task
	{
		bool first;
		public string name;

		public MarkerTask( string name ) 
		{
			this.name = name;
			first = true;
			Add((m) => { if (first) { first = false; return new ExportAct(name); } else return null; });
		}
	}
}
