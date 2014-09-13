using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CoreFork
{
	public class Effect
	{


		public Rectangle shape;
		public Vector2 position;
		protected Texture2D sprite;
		protected Color color;
		protected float size;
		public float life;


		public Effect ()
		{
		}

		public virtual void update()
		{

		}

		public virtual void draw(SpriteBatch par1)
		{

		}
	}
}

