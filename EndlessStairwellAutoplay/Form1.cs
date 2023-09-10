using EndlessStairwellAutoplay.tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Internal;
//using OpenQA.Selenium.DevTools.V111.ServiceWorker;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace EndlessStairwellAutoplay
{
	public partial class Form1 : Form
	{
		private System.Windows.Forms.Timer timer;

		class Save
		{
			public string name = "";
			public string data = "";
			public TimeSpan length;

			public override string ToString()
			{
				return $"{name} - {length}";
			}
		}

		List<Save> saves;

		class Message
		{
			public enum Type
			{
				run, stop,
				step,
				export,
				exportDone,
				quit,
				import,
				model,
				reset,
				finished,
				setTask
			}


			public Type type;
			public Model? model;

			public string? exportData;
			public string? exportName;

			public string? taskStack;

			public Task? task;

			public Message(Type type)
			{
				this.type = type;
			}
		}


		ConcurrentQueue<Message>
			toForm, toThread;

		Thread workThread;

		DateTime start, finish;


		public Form1()
		{
			saves = new List<Save>();
			start = new DateTime(1, 1, 1);
			finish = new DateTime(1, 1, 1);

			timer = new System.Windows.Forms.Timer();
			timer.Interval = 50;
			timer.Tick += OnTimer;



			toForm = new ConcurrentQueue<Message>();
			toThread = new ConcurrentQueue<Message>();

			workThread = new Thread(new ThreadStart(WorkerThreadProc));
			workThread.Name = "Browser Worker";
			workThread.IsBackground = false;
			workThread.Start();

			LoadSaves();

			InitializeComponent();
			timer.Enabled = true;
			RefreshSaves();

			RefreshManualTasks();
		}

		void RefreshSaves()
		{
			listBox1.Items.Clear();
			foreach (var sv in saves)
				listBox1.Items.Add(sv);
		}

		class ManualTaskProxy
		{
			public Task task;
			public string name;

			public ManualTaskProxy(Task task, string name)
			{
				this.task = task;
				this.name = name;
			}

			public override string ToString()
			{
				return name;
			}
		}


		void RefreshManualTasks()
		{
			manualTaskslistBox.Items.Clear();
			manualTaskslistBox.Items.Add(
				new ManualTaskProxy(new ExploreFloorsTask(1, 50), "Explore floors 1 to 50"));
		}

		void LoadSaves()
		{
			if (File.Exists("saves.txt"))
			{
				var lns = File.ReadAllLines("saves.txt");

				foreach (var ln in lns)
				{
					var b = ln.Split(",");
					if (b[0] == "save")
					{
						Save s = new Save();
						s.name = b[1];
						s.length = TimeSpan.FromSeconds(float.Parse(b[2]));
						s.data = b[3];
						saves.Add(s);
					}
				}
			}
		}

		void WorkerThreadProc()
		{
			Task? task = new WinGameTask();
			bool running = false;
			bool step = false;
			Model model = new Model();
			DateTime lastModelUpdate = DateTime.Now;

			var sendModelToForm = () =>
			{
				var msg = new Message(Message.Type.model);
				msg.model = model.CloneForUi();
				if (task == null)
					msg.taskStack = "No task";
				else
					msg.taskStack = task.TaskStack();
				toForm.Enqueue(msg);
				lastModelUpdate = DateTime.Now;
			};


			List<string> clickHistory = new List<string>();

			var ds = ChromeDriverService.CreateDefaultService();
			ds.HideCommandPromptWindow = true;
			ChromeDriver driver = new ChromeDriver(ds, new ChromeOptions());
			model.driver = driver;
			driver.Navigate().GoToUrl("https://demonin.com/games/endlessStairwell/");
			Thread.Sleep(1 * 1000);




			bool quit = false;

			while (!quit)
			{
				Message? m;
				if (toThread.TryDequeue(out m))
				{
					switch (m.type)
					{
						case Message.Type.run:
							running = true;
							break;
						case Message.Type.stop:
							running = false;
							break;
						case Message.Type.step:
							step = true;
							break;
						case Message.Type.exportDone:
							running = true;
							break;
						case Message.Type.quit:
							running = false;
							quit = true;
							break;

						case Message.Type.setTask:
							task = m.task;
							sendModelToForm();
							break;

						case Message.Type.import:
							driver.ExecuteScript(@$"
    var lg= JSON.parse(atob('{m.exportData}'));
    reset();
    loadGame(lg);
    save();
");
							sendModelToForm();
							break;
					}
				}

				if (running || step)
				{
					step = false;
					model.Refresh();


					var a = task.GetAct(model);

					if ((DateTime.Now - lastModelUpdate).TotalMilliseconds > 100)
						sendModelToForm();


					if (a == null)
					{
						toForm.Enqueue(new Message(Message.Type.finished));
						task = null;
						sendModelToForm();
						running = false;
					}

					{
						ClickAct? ca = a as ClickAct;

						if (ca != null)
						{
							clickHistory.Add($"{task.DeepestTask()} {ca.selector}");
							try
							{
								driver.FindElement(OpenQA.Selenium.By.CssSelector(ca.selector)).Click();
							}
							catch (Exception ex)
							{
								Debug.WriteLine($"Failed to click: {ca.selector} msg: {ex.Message}");
							}

						}
					}
					{
						ClickDismissAct? cda = a as ClickDismissAct;
						 
						if (cda != null)
						{
							clickHistory.Add($"{task.DeepestTask()} {cda.selector}");
							driver.FindElement(OpenQA.Selenium.By.CssSelector(cda.selector)).Click();
							try
							{
								driver.SwitchTo().Alert().Accept();
							}
							// sometimes, there is no dialog
							catch (Exception)
							{
								//
							}
						}
					}



					{
						ExportAct? ea = a as ExportAct;
						if (ea != null)
						{
							driver.FindElement(OpenQA.Selenium.By.CssSelector("#exportButton")).Click();
							driver.SwitchTo().Alert().Accept();
							var msg = new Message(Message.Type.export);
							msg.exportName = ea.name;
							running = false;
							toForm.Enqueue(msg);
						}
					}
				}

				Thread.Sleep(10);
			}

			driver.Quit();
		}

		void SendModelToForm()
		{

		}



		private void OnTimer(object? sender, EventArgs e)
		{
			Message? m;
			while (toForm.TryDequeue(out m))
			{
				if (m.type == Message.Type.model)
				{
					infoTextBox.Text =
						$"Runes: r:{m.model.redRunes},g:{m.model.greenRunes},b:{m.model.blueRunes} \r\n" +
						$"Task Stack:\r\n" +
						$"{m.taskStack}";
				}

				if (m.type == Message.Type.finished)
				{
					finish = DateTime.Now;
				}

				if (m.type == Message.Type.export)
				{
					string ct = Clipboard.GetText();

					Save? s = saves.Find(s => { return s.name == m.exportName; });
					var tt = DateTime.Now - start;

					if (s == null)
					{
						s = new Save();
						s.name = m.exportName;
						s.length = new TimeSpan(1000, 0, 0, 0);
					}

					if (s.length > tt)
					{
						s.data = ct;
						s.length = tt;
						saves.Add(s);
					}

					RefreshSaves();
					toThread.Enqueue(new Message(Message.Type.exportDone));
				}
			}

			if (start.Year > 1)
			{
				if (finish.Year > 1)
					timeLabel.Text = $"time taken: {(finish - start).ToString()}";
				else
					timeLabel.Text = $"time taken: {(DateTime.Now - start).ToString()}";
			}
		}

		private void OnGo(object sender, EventArgs e)
		{
			var m = new Message(Message.Type.run);
			toThread.Enqueue(m);
			start = DateTime.Now;
		}

		private void OnFormClosed(object sender, FormClosedEventArgs e)
		{
			toThread.Enqueue(new Message(Message.Type.quit));
			List<string> ss = new List<string>();
			foreach (var s in saves)
				ss.Add($"save,{s.name},{s.length.TotalSeconds},{s.data}");

			File.WriteAllLines("saves.txt", ss);
		}

		void ShowTasks()
		{
			//string s = task.TaskStack();


			//tasksTextBox.Text = s;
		}

		private void OnStep(object sender, EventArgs e)
		{
			toThread.Enqueue(new Message(Message.Type.step));
		}

		private void OnReset(object sender, EventArgs e)
		{
			toThread.Enqueue(new Message(Message.Type.reset));
		}

		private void OnStop(object sender, EventArgs e)
		{
			toThread.Enqueue(new Message(Message.Type.stop));
		}

		private void OnMilestoneDoubleClick(object sender, EventArgs e)
		{
			var s = saves[listBox1.SelectedIndex];

			Message m = new Message(Message.Type.import);
			m.exportName = s.name;
			m.exportData = s.data;
			toThread.Enqueue(m);


			// reset the task
			m = new Message(Message.Type.setTask);
			m.task = new WinGameTask();

			// remove all the subtasks before the milestone
			bool done = false;
			while (!done)
			{
				MarkerTask? mt = m.task.subTasks[0] as MarkerTask;

				if (mt != null && mt.name == s.name)
					done = true;

				m.task.subTasks.RemoveAt(0);
			}

			toThread.Enqueue(m);
		}

		private void OnManualTaskRun(object sender, EventArgs e)
		{
			ManualTaskProxy? p = manualTaskslistBox.SelectedItem as ManualTaskProxy;

			if (p != null)
			{
				var m = new Message(Message.Type.setTask);
				m.task = p.task;
				toThread.Enqueue(m);
				toThread.Enqueue(new Message(Message.Type.run) );
			}
		}
	}
}