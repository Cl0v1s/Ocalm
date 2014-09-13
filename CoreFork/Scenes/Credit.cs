using System;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CoreFork
{
	public class Credit : Scene
	{
		int timeIn=0;

		public Credit ()
		{
		}

		public override void update ()
		{
			timeIn += 1;
		}

		public override void handleEvent (Events par1, object par2)
		{
			//TODO: implement touch screen
			if (timeIn < 35)
				return;
			if (par1 == Scene.Events.Input) {
				switch ((Keys)par2) {
				case Keys.Left:
					new Title ().initialize ();
					break;
				case Keys.Right:
					new Title ().initialize ();
					break;
				}
			}
		}

		public override void draw (SpriteBatch par1)
		{
			Stream file = TitleContainer.OpenStream ("Credits.xml");
			XmlTextReader xml = new XmlTextReader (file);
			int y = 130;
			while (xml.Read ()) {
				bool writable = false;
				float size = 1F;
				int x = 50;
				int _y = 0;
				Color color = Color.Blue;
				if (xml.Name == "title") {
					writable = true;
					size = 1F;
					x = 60;
					color = Color.White;
					_y = 30;
				}
				else if(xml.Name=="people")
				{
					writable = true;
					size = 0.6F;
					x = 90;
					color = Color.LightGray;
					_y = 25;
				}
				if (writable) {
					Core.drawText (par1, xml.ReadString(), x, y, size, color);
					y += _y;
				}

			}
			Core.drawText (par1, ">", (int)(canvas.X / 2 + 170), (int)( + 220), 1F, Color.SlateGray);
			Core.drawText (par1, "<", (int)(+ 30), (int)( + 220), 1F, Color.SlateGray);
		}
	}
}

