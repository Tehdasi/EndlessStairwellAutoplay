using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndlessStairwellAutoplay
{
	internal class ExploreFloorsTask : Task
	{
		List<int>? targetFloors;

		public ExploreFloorsTask( int lowest, int highest) {
			Debug.Assert(highest >= lowest);
			parms = $"lowest:{lowest} highest:{highest}";
			Add(new GotoStairsTask());
			Add((m) =>
			{
				if (targetFloors == null)
				{
					targetFloors = new List<int>();
					for( int i = lowest; i <= highest; i++ )
						targetFloors.Add( i );
				}

				if ( targetFloors.Contains( m.floor) )
					targetFloors.Remove(m.floor );

				if (targetFloors.Count == 0)
					return null;

				int closestFloor = -1;
				int closestFloorDist = 999;

				foreach( int i in targetFloors )
				{
					int fd = Math.Abs(i - m.floor);

					if (fd < closestFloorDist)
					{
						closestFloor = i;
						closestFloorDist = fd;
					}
				}
				
				if( closestFloor < m.floor ) 
					return Model.FloorDown();
				else
					return Model.FloorUp();
			});
		}
	}
}
