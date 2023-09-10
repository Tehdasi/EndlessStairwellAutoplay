using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndlessStairwellAutoplay
{
	internal class FightTask : Task
	{
		public FightTask( bool useVanilla ) 
		{
			Add((m) => {
				if (m.isDead)
					return new ClickAct("#deathClose");

				if (m.inFight)
				{
					if ( useVanilla && m.energy < 20 && m.vanillaHoney > 0)
						return m.UseVanillaHoney();
					else if ( m.curHealth < 50 && m.honey > 20 )
						return m.UseHoney();
					else
					{
						if (m.energy > 20)
							return m.Attack();
						else
							return new WaitAct();
					}
				}
				else
					return null;
			} );
		}
	}
}
