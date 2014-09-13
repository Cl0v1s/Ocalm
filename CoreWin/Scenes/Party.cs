using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace CoreFork
{
	public class Party : Scene
	{

		public List<Entity> entities = new List<Entity> ();
		public Player player;
		public float countDown;
		public float difficulty=0;
		Texture2D countDownBackground;
		public bool pause=false;
		int pauseTime=0;


		public Party ()
		{
			player = (Player)spawnEntity (new Player (this, (int)(canvas.X/2)-35, (int)(canvas.Y/2)-32, 64, 64));
			Random rand = new Random ();
			spawnEntity(new Target (this, rand.Next (10, (int)canvas.X - 10), rand.Next (10, (int)canvas.Y - 10))); 
			countDown = 5F;
			countDownBackground = new Texture2D (Core.graphicsD, 1, 1);
			countDownBackground.SetData (new Color[]{ new Color(60,60,60) });
		}

		public override void initialize ()
		{
			base.initialize ();
		}

		public override void update ()
		{
			pauseTime -= 1;
			if (!pause) {
				EffectManager.update ();
				Background.update ();	
				difficulty = (int)(player.score / 10);
				countDown -= 0.025F;
				if (countDown > 0)
					return;
				for (int i = 0; i < entities.Count; i++)
					entities [i].update ();

				if (player.dead) {
					new Title ().initialize (this, player.score);
				}


				base.update ();
			}
		}

		public override void handleEvent (Events par1, object par2)
		{
			//TODO: implements touch screen
			if (par1 == Scene.Events.Input) {
				switch ((Keys)par2) {
				case Keys.Up:
					player.move (Entity.directions.Up);
					break;
				case Keys.Down:
					player.move (Entity.directions.Down);
					break;
				case Keys.Left:
					player.move (Entity.directions.Left);
					break;
				case Keys.Right:
					player.move (Entity.directions.Right);
					break;
				case Keys.Space:
					player.change ();
					break;
				case Keys.Escape:
					if (pauseTime <= 0) {
						pause = !pause;
						pauseTime = 10;
					}
					break;
				}
			}
		}

		public override void draw (Microsoft.Xna.Framework.Graphics.SpriteBatch par1)
		{
			//Gestion du compte à rebours
			par1.Draw (countDownBackground, new Rectangle ((int)canvas.X / 2 - 250 / 2, 300, 250, 90), Color.White*0.8F);
			if (!pause) {
				if (countDown > 0) {
					if (countDown > 4) {
						Vector2 v = Core.fontRenderer.MeasureString ("Ready ?") * 2F;
						Core.drawText (par1, "Ready ?", (int)(canvas.X / 2 - v.X / 2), (int)(310), 2F, Color.DimGray * 0.9F);
					} else if (countDown < 1) {
						Vector2 v = Core.fontRenderer.MeasureString ("Go !") * 3F;
						Core.drawText (par1, "Go !", (int)(canvas.X / 2 - v.X / 2), (int)(290), 3F, Color.DimGray * 0.9F);
					} else {
						Vector2 v = Core.fontRenderer.MeasureString (((int)countDown).ToString ()) * 3F;
						Core.drawText (par1, ((int)countDown).ToString (), (int)(canvas.X / 2 - v.X / 2), (int)(290), 3F, Color.DimGray * 0.9F);
					}
				} else {
					Vector2 v = Core.fontRenderer.MeasureString (((int)player.score).ToString ()) * 3F;
					Core.drawText (par1, ((int)player.score).ToString (), (int)(canvas.X / 2 - v.X / 2), (int)(290), 3F, Color.DimGray * 0.9F);
				}
			} else {
				Vector2 v = Core.fontRenderer.MeasureString ("Pause") * 3F;
				Core.drawText (par1, "Pause",(int)(canvas.X / 2 - v.X / 2), (int)(290), 3F, Color.DimGray * 0.9F);
			}


			foreach (Entity e in entities) {
				if(!(e is Player))
					e.draw (par1);
			}
			player.draw (par1);
		}

		public Entity spawnEntity(Entity par1)
		{
			entities.Add (par1);
			return par1;
		}

		public void killEntity(Entity par1)
		{
			entities.Remove (par1);
		}

		public override void dispose (Scene par1)
		{
			base.dispose (par1);
		}
	}
}

