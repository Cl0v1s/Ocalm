using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace CoreFork
{
	public class Scene
	{

		public enum Events
		{
			Input,
		}

		public Vector2 canvas;


		public Scene ()
		{
			canvas = new Vector2 (Core.graphics.PreferredBackBufferWidth, Core.graphics.PreferredBackBufferHeight);
		}

		public virtual void initialize()
		{
			Core.scene = this;
		}


		public virtual void update()
		{

		}

		public virtual void handleEvent(Scene.Events par1, object par2)
		{

		}

		public virtual void draw(SpriteBatch par1)
		{

		}

		public virtual void dispose(Scene par1)
		{
			par1.initialize ();
		}


	}
}

