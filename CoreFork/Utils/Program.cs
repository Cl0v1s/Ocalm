#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;

#endregion
namespace CoreFork
{
	static class Program
	{
		private static Core game;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main ()
		{
			game = new Core ();
			game.Run ();
		}
	}
}
