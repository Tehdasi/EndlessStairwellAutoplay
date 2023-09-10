//using OpenQA.Selenium.DevTools.V111.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndlessStairwellAutoplay
{
	internal class Task
	{
		public Func<Model, Act?>? func;
		public List<Task> subTasks;
		public string parms;
		

		public Task()
		{
			parms = "";
			subTasks = new List<Task>();
		}

		public Task(Func<Model, Act?> func)
		{
			this.func = func;
			parms = "";
			subTasks = new List<Task>();
		}

		public Task(params Task[] subTasks)
		{
			this.subTasks = subTasks.ToList();
			parms = "";
		}




		public string TaskStack()
		{
			string s = "";
			Task? t = this;

			while( t != null) 
			{
				s += t.TaskDesc() + "\r\n";

				if (t.subTasks.Count > 0)
					t = t.subTasks[0];
				else
					t = null;
			}

			return s;
		}

		public string TaskDesc()
		{
			if (func != null)
				return "Custom code";
			else
				return $"{GetType().Name} {parms}";
		}


		public string DeepestTask()
		{
			Task? t = this;

			while (t.subTasks.Count > 0 )
				t= t.subTasks[0];

			return TaskDesc(); 
		}

		virtual public Act? InsertTask( Model m, Task t )
		{
			Act? act = t.GetAct(m);
			
			if( act!= null )
				subTasks.Insert(0, t);

			return act;
		}


		//virtual public void AddStep( Func<Model,Act> func )
		//{
		//	Step s = new Step();
		//	s.func = func;
		//	subTasks.Add(s);
		//}

		virtual public void AddMarker(string name)
		{
			subTasks.Add( new MarkerTask(name));
		}

		public void Add(params Task[] subTasks)
		{
			this.subTasks.AddRange( subTasks );
		}

		public void Add(Func<Model, Act?> func)
		{
			this.subTasks.Add(new Task( func ));
		}

		public virtual Act? GetAct(Model m)
		{
			if (func != null)
				return func(m);
			else
			{
				if (subTasks == null || subTasks.Count == 0)
					return null;
				else
				{
					Act? a;

					a = subTasks[0].GetAct(m);
					if (a == null)
						subTasks.RemoveAt(0);

					while ((a == null) && (subTasks.Count > 0))
					{
						a = subTasks[0].GetAct(m);
						if (a == null)
							subTasks.RemoveAt(0);
					}

					return a;
				}
			}
		}

		//virtual public Act GetAct( Model m )
		//{
		//	Act a= subTasks[0].GetAct(m);

		//	while( a.GetType() == typeof(DoneStepAct)  )
		//	{
		//		subTasks.RemoveAt(0);
		//		if (subTasks.Count == 0)
		//			return new DoneStepAct();
		//		else
		//			a= subTasks[0].GetAct(m);
		//	}

		//	return a;
		//}

	}
}
