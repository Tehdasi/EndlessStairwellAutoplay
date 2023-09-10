using OpenQA.Selenium.DevTools.V112.HeapProfiler;

namespace EndlessStairwellAutoplay
{
	internal static class Program
	{ 

		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Num.DoTests();

			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			ApplicationConfiguration.Initialize();
			var f = new Form1();
			f.StartPosition = FormStartPosition.Manual;
			f.Location = new Point(3000, 200);
			Application.Run(f);

		}
	}
}