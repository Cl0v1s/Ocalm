using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CoreFork
{
	public class Background
	{

		public static List<Effect> effects;
		static Random rand = new Random ();

		public Background ()
		{
		}

		public static void initialize()
		{
			effects = new List<Effect> ();
		}

		public static void update()
		{
			for (int i = 0; i < effects.Count; i++) {
				effects [i].update ();
			}
		
			if (rand.Next (1, 20) == 1) {
				Party world = (Party)Core.scene;
				Vector2 pos = new Vector2 (rand.Next (0, (int)world.canvas.X), rand.Next (0, (int)world.canvas.Y));
				Color col = Color.GreenYellow;
				float dur = (float)rand.Next (1, 5);
				switch (rand.Next (1, 5)) {
				case 1:
					col = Color.BlueViolet;
					break;
				case 2:
					col = Color.Gold;
					break;
				case 3:
					col = Color.Orchid;
					break;
				case 4:
					col = Color.SkyBlue;
					break;
				case 5:
					col = Color.Olive;
					break;
				}
				for (int i = 0; i < rand.Next (3, 5); i++) {
				addEffect (new Wave ((int)pos.X, (int)pos.Y, 1F * (0.8F * i), dur, col));
				}
			}

		}

		public static Effect addEffect(Effect  par1)
		{
			effects.Add (par1);
			return par1;
		}

		public static void removeParticle(Particle par1)
		{
			removeEffect (par1.light);
			removeEffect (par1);
		}

		public static void removeEffect(Effect par1)
		{
			effects.Remove (par1);
		}

		public static void draw(SpriteBatch par1)
		{
			for (int i = 0; i < effects.Count; i++) {
				effects [i].draw (par1);
			}
		}
	}
}

