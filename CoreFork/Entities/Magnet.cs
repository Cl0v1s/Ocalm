using System;
using Microsoft.Xna.Framework;

namespace CoreFork
{
	public class Magnet : Entity
	{

		public enum Gender
		{
			Plus,
			Minus,
		}

		public Gender type; 
		public float life = 1F;

		public Magnet (Gender par6type,Party par1scene, int par2x, int par3y)
			:base(par1scene,par2x,par3y,20,20)
		{
			type = par6type;
			switch (type) {
			case Gender.Plus:
				sprite = Core.Png [4];
				break;
			case Gender.Minus:
				sprite = Core.Png [3];
				break;
			}
			boundingboxes = Utils.getBoundingBoxFrom (sprite, shape);
		}

		public override void update ()
		{
			life -= 0.005F;
			if (life <= 0)
				world.killEntity (this);
			position += 0.03F * velocity;
			if (!checkCollision (position))
				shape = new Rectangle ((int)position.X, (int)position.Y, shape.Width, shape.Height);
			else {
				world.killEntity (this);
			}
		}

		protected override bool checkCollision (Vector2 par1)
		{
			Rectangle rec = new Rectangle ((int)par1.X+boundingboxes[dir].X, (int)par1.Y+boundingboxes[dir].Y, boundingboxes [dir].Width, boundingboxes [dir].Height);
			foreach (Entity e in world.entities) {
				if (e != this && (e is Player) && ((Player)e).type==type && life>0.3F) {
					Rectangle other=new Rectangle ((int)e.shape.X+e.boundingboxes[dir].X, (int)e.shape.Y+e.boundingboxes[dir].Y, e.boundingboxes [dir].Width, e.boundingboxes [dir].Height);
					if (other.Intersects (rec)) {
						if (life > 0.3F) {
							((Player)e).damage (0.15F);
							Core.playSound (Core.Sounds [3], 0.6F);
							if (velocity.X > 0) {
								new Explosion (new Vector2 (rec.X + rec.Width / 2, rec.Y + rec.Height / 2), directions.Left, 2F, 0.1F, Color.DarkOrange);
							} else if (velocity.X < 0) {
								new Explosion (new Vector2 (rec.X + rec.Width / 2, rec.Y + rec.Height / 2), directions.Right, 2F, 0.1F, Color.DarkOrange);
							}

							if (velocity.Y > 0) {
								new Explosion (new Vector2 (rec.X + rec.Width / 2, rec.Y + rec.Height / 2), directions.Up, 2F, 0.1F, Color.DarkOrange);
							} else if (velocity.Y < 0) {
								new Explosion (new Vector2 (rec.X + rec.Width / 2, rec.Y + rec.Height / 2), directions.Down, 2F, 0.1F, Color.DarkOrange);
							}


						}
						return true;
					}
				}
			}
			if (rec.X < 0 && velocity.X<0)
				position.X = world.canvas.X;
			if (rec.X + rec.Width > world.canvas.X && velocity.X>0) {
				position.X = -shape.Width;
			}
			if (rec.Y < 0 && velocity.Y<0)
				position.Y = world.canvas.Y;
			if (rec.Y + rec.Height > world.canvas.Y && velocity.Y>0)
				position.Y = -shape.Height;

			return false;
		}

		public override void draw (Microsoft.Xna.Framework.Graphics.SpriteBatch par1)
		{
			float opacity = life;
			if (world.player.type != type)
				opacity = opacity / 2;
			else
				opacity += 0.3F;
			par1.Draw (sprite, shape, new Rectangle (shape.Width * ((int)frame), 0, shape.Width, shape.Height), Color.White*opacity);
		}
	}
}

