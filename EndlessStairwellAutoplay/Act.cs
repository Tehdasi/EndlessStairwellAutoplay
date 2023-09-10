using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndlessStairwellAutoplay
{
	internal class ClickAct : Act
	{
		public string selector;

		public ClickAct(string selector  )
		{
			this.selector = selector;
		}
	}
	internal class ClickDismissAct : Act
	{
		public string selector;

		public ClickDismissAct(string selector)
		{
			this.selector = selector;
		}
	}

	internal class ExportAct : Act
	{
		public string name;

		public ExportAct(string name)
		{
			this.name = name;
		}
	}

	internal class WaitAct : Act
	{
	}

	internal class BadAct : Act
	{
	}



	internal class Act
	{
	}
}
