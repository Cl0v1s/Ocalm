using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CoreFork
{
	public class Title : Scene
	{
		int lastGameScore=-1;
		public Rectangle screen;
		float startOpacity=1F;
		Party next;
		Texture2D background;
		Texture2D scoreBackground;
		int timeIn=0;

		public Title ()
		{
			background = new Texture2D (Core.graphicsD, 1, 1);
			background.SetData(new Color[]{new Color(30,30,30)});
			screen = new Rectangle (0, 0, Core.graphics.PreferredBackBufferWidth, Core.graphics.PreferredBackBufferHeight+30);
			scoreBackground = new Texture2D (Core.graphicsD, 1, 1);
			scoreBackground.SetData (new Color[]{ new Color(60,60,60) });
		}

		public override void initialize ()
		{
			Core.PlaySong (Core.Sounds [0], 0.5F);
			base.initialize ();
		}

		public void initialize(Party par1,int score)
		{
			lastGameScore = score;
			EffectManager.particles = new System.Collections.Generic.List<Particle> ();
			EffectManager.lights = new System.Collections.Generic.List<Light> ();
			screen = new Rectangle (-1, -(int)canvas.Y, (int)canvas.X + 1, (int)canvas.Y+30);
			if (score > Core.bestScore) {
				Saver meta = new Saver ("Meta");
				meta.addData ("BestScore", score);
				SaveManager.replaceSaver("Meta",meta);
				SaveManager.save ();
			}
			base.initialize ();
		}

		public override void update ()
		{
			timeIn += 1;
			EffectManager.update ();
			if (screen.Y < 0 && screen.X == 0) {
				screen.Y -= 15;
			} else if (screen.Y+screen.Height < 400 && screen.X < 0) {
				screen.Y += 15;
			}
			if (screen.Y <= -canvas.Y)
				next.initialize ();
			startOpacity -= 0.025F;
			if (startOpacity < -1F)
				startOpacity = 1F;
			base.update ();
		}

		public override void handleEvent (Events par1, object par2)
		{
			//TODO: implement touch screen
			if (timeIn < 35)
				return;
			if (par1 == Scene.Events.Input) {
				switch ((Keys)par2) {
				case Keys.Space:
					start ();
					break;
				case Keys.Right:
					if(lastGameScore==-1)
						new Credit ().initialize ();
					break;
				case Keys.Left:
					if(lastGameScore==-1)
						new Credit ().initialize ();
					break;
				}
			}
		}

		void start()
		{
            if (next == null)
            {
                Core.playSound(Core.Sounds[4], 0.6F);
                screen = new Rectangle(0, -1, (int)canvas.X, (int)canvas.Y);
                next = new Party();
                EffectManager.particles = new System.Collections.Generic.List<Particle>();
                EffectManager.lights = new System.Collections.Generic.List<Light>();
                if (lastGameScore > Core.bestScore)
                {
                    Core.bestScore = lastGameScore;
                }
            }
		}

		public override void draw (Microsoft.Xna.Framework.Graphics.SpriteBatch par1)
		{

			if (next != null)
				next.draw (par1);
			//background
			par1.Draw (background, new Rectangle (screen.X, screen.Y, (int)canvas.X, (int)screen.Height), Color.White);
			//Version and author
			Core.drawText (par1, "V." + Core.GetVersion (), 2, 0, 0.5F, Color.DimGray);


			//render title
			Vector2 t;
			if (lastGameScore <= Core.bestScore) {
				t = Core.fontRenderer.MeasureString ("Ocalm") * 4F;
				Core.drawText (par1, "Ocalm", (int)(screen.X + canvas.X / 2 - t.X / 2), (int)(screen.Y + 100 + 50), 4F, Color.LightGray);
				if (lastGameScore == -1) {
					float sf = 1F;
					t = Core.fontRenderer.MeasureString ("Best Score: " + Core.bestScore + " point(s).")*sf;
					while (t.X > Core.graphics.PreferredBackBufferWidth - 20) {
						sf -= 0.1F;
						t = Core.fontRenderer.MeasureString ("Best Score: " + Core.bestScore + " point(s).")*sf;
					}
					Core.drawText (par1, "Best Score: " + Core.bestScore + " point(s).", (int)(screen.X + canvas.X / 2 - t.X / 2), (int)(screen.Y + 220 + 150), sf, Color.LightGray);
				}


			} else {
				float sc = 4F;
				t = Core.fontRenderer.MeasureString ("New Best Score !") * sc;
				Vector2 to = Core.fontRenderer.MeasureString ("Ocalm") * 4F;
				while (t.X > Core.graphics.PreferredBackBufferWidth - 50 || t.Y > to.Y) {
					sc -= 0.1F;
					t = Core.fontRenderer.MeasureString ("New Best Score !") * sc;
				}
				Core.drawText (par1, "New Best Score !", (int)(screen.X + canvas.X / 2 - t.X / 2), (int)(screen.Y + 100 + 90), sc, Color.LightGray);
			}
			par1.Draw (Core.Png [1], new Vector2 (screen.X + canvas.X / 2 - 32, screen.Y + 30+50), new Rectangle (3*64, 0, 64, 64), Color.White);
			par1.Draw (Core.Png [0], new Rectangle ((int)(screen.X + canvas.X / 2 - 32 - 44), (int)(screen.Y + 40+32+50),64,64), new Rectangle (3 * 64, 0, 64, 64), Color.White, 6F, new Vector2 (32, 32), SpriteEffects.None, 0F);
			par1.Draw (Core.Png [0], new Rectangle ((int)(screen.X + canvas.X / 2 - 32 + 109), (int)(screen.Y + 40+32+50),64,64), new Rectangle (3 * 64, 0, 64, 64), Color.White, 0.26F, new Vector2 (32, 32), SpriteEffects.None, 0F);
			//affichage du score précédent
			int y = 0;
			if (lastGameScore != -1) {
				par1.Draw (scoreBackground, new Rectangle ((int)canvas.X / 2 - 250 / 2, screen.Y+270, 250, 90), Color.White * 0.8F);
				t = Core.fontRenderer.MeasureString ("Your score:");
				Core.drawText (par1, "Your score:", (int)(screen.X+canvas.X / 2 - t.X / 2), (int)(screen.Y+280), 1F, Color.DimGray);
				t = Core.fontRenderer.MeasureString (lastGameScore+" vs "+Core.bestScore +"points");
				float s = 1F;
				while (t.X > 230 || t.Y > 90) {
					s -= 0.1F;
					t = Core.fontRenderer.MeasureString (lastGameScore+" vs "+Core.bestScore +"points")*s;
				}
				Core.drawText (par1, lastGameScore+" vs "+Core.bestScore +" points", (int)(screen.X+canvas.X / 2 - t.X / 2), (int)(screen.Y+320), s, Color.DimGray);
				y = 95;

			}
			//press start
			t = Core.fontRenderer.MeasureString ("- Press start -");
			Core.drawText (par1, "- Press start -", (int)(screen.X+canvas.X / 2 - t.X / 2), (int)(screen.Y+220+50+y), 1F, Color.SlateGray*Math.Abs(startOpacity));
			if (lastGameScore == -1) {
				t = Core.fontRenderer.MeasureString (">");
				Core.drawText (par1, ">", (int)(screen.X + canvas.X / 2 + 170), (int)(screen.Y + 220), 1F, Color.SlateGray);
				t = Core.fontRenderer.MeasureString ("<");
				Core.drawText (par1, "<", (int)(screen.X + 30), (int)(screen.Y + 220), 1F, Color.SlateGray);
			}

			base.draw (par1);
		}



	}
}

