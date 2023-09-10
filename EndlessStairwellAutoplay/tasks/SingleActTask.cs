using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EndlessStairwellAutoplay
{

	internal class SingleActTask : Task
	{
		bool done;

		public SingleActTask( Act act )
		{
			done = false;

			Add( (m)=> {
				if (!done)
				{
					done = true;
					return act;
				}
				else
					return null;
			} );
		}
	}
}
