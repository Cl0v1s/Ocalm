using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace CoreFork
{
	public class Entity
	{

		public enum directions
		{
			Up,
			Down, 
			Left, 
			Right,
		}

		public Vector2 position;
		public Rectangle shape;
		public Rectangle[] boundingboxes;
		public Vector2 velocity;

		protected Random rand = new Random ();

		public Party world;
		public Texture2D sprite;
		protected float frame;
		public int dir = 0;

		public Entity (Party par5scene, int par1x, int par2y, int par3width, int par4height)
		{
			shape = new Rectangle (par1x, par2y, par3width, par4height);
			position = new Vector2 (par1x, par2y);
			world = par5scene;
			boundingboxes = new Rectangle[1];
			boundingboxes [0] = new Rectangle(0,0,shape.Width,shape.Height);
		}

		public virtual void update()
		{
			position += 0.03F * velocity;
			velocity -= 0.03F * velocity;
			if (!checkCollision (position))
				shape = new Rectangle ((int)position.X, (int)position.Y, shape.Width, shape.Height);
			else {
				position = new Vector2 (shape.X, shape.Y);
				velocity = Vector2.Zero;
			}
		}

		protected virtual bool checkCollision(Vector2 par1)
		{
			Rectangle rec = new Rectangle ((int)par1.X+boundingboxes[dir].X, (int)par1.Y+boundingboxes[dir].Y, boundingboxes [dir].Width, boundingboxes [dir].Height);
			foreach (Entity e in world.entities) {
				if (e != this && !(e is Target)) {
					Rectangle other=new Rectangle ((int)e.shape.X+e.boundingboxes[dir].X, (int)e.shape.Y+e.boundingboxes[dir].Y, e.boundingboxes [dir].Width, e.boundingboxes [dir].Height);
					if (other.Intersects (rec)) {
						return true;
					}
				}
			}
			if (rec.X < 0)
				return true;
			if (rec.X + rec.Width > world.canvas.X)
				return true;
			if (rec.Y < 0)
				return true;
			if (rec.Y + rec.Height > world.canvas.Y)
				return true;

			return false;
		}

		public virtual void draw(SpriteBatch par1)
		{
			par1.Draw (sprite, shape, new Rectangle (shape.Width * ((int)frame), 0, shape.Width, shape.Height), Color.White);
		}
	}
}

