using System;
using Microsoft.Xna.Framework.Input;


namespace CoreFork
{
	public class Input
	{
		public Input ()
		{
		}

		public static void update()
		{
			if (Keyboard.GetState ().GetPressedKeys ().Length > 0) {
				Keys k = Keyboard.GetState ().GetPressedKeys () [0];
				Core.scene.handleEvent (Scene.Events.Input, k);
			}
		}
	}
}

