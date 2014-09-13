using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoreFork
{
	public class Particle : Effect
	{
		public enum Sort
		{
			Fixed,
			Explosion,
			Fog,
		}

		float duration=1F;
		public Vector2 link;
		public Vector2 velocity=Vector2.Zero;
		public Light light;
		Random rand=new Random();
		Sort type;

		public Particle (int par1x, int par2y, Color par4color, Sort par5type)
		{
			life = 1F;
			int side = rand.Next (3, 5);
			if (par5type == Sort.Fog)
				side = rand.Next (5, 15);
			link = new Vector2 (par1x, par2y);
			if (par5type == Sort.Fixed) {
				int g = rand.Next (1, 3);
				if (g != 1)
					g = -1;
				int h = rand.Next (1, 3);
				if (h != 1)
					h = -1;
				par1x += rand.Next (0, 15) * g;
				par2y += rand.Next (0, 15) * h;
			}
			shape = new Rectangle (par1x, par2y, side, side);
			position = new Vector2 (shape.X, shape.Y);
			color = par4color;
			sprite = new Texture2D (Core.graphicsD, 1, 1);
			sprite.SetData (new Color[]{ color });
			if(par5type != Sort.Fog)
				light = EffectManager.addLight (new Light (shape.X + shape.Width / 2, shape.Y + shape.Height / 2, side / 10F, color));
			else
				life = 0.3F;
			type = par5type;
		}

		public override void update()
		{
			if (velocity == Vector2.Zero) {
				if (type != Sort.Explosion) {
					int distance = (int)(Math.Abs (position.X - link.X) + Math.Abs (position.Y - link.Y));
					if (distance < 10) {
						int g = rand.Next (1, 3);
						if (g != 1)
							g = -1;
						int h = rand.Next (1, 3);
						if (h != 1)
							h = -1;
						velocity = new Vector2 (rand.Next (3, 6) * g, rand.Next (3, 6) * h);
					} else {
						int g = 1;
						if (position.X > link.X)
							g = -1;
						int h = 1;
						if (position.Y > link.Y)
							h = -1;
						velocity = new Vector2 (rand.Next (3, 6) * g, rand.Next (3, 6) * h);
					}
				}
			}
			position += 0.03F * velocity;
			if(type == Sort.Fixed)
				velocity -= 0.03F * velocity;
			if (velocity.X < 1 && velocity.Y < 1)
				velocity = Vector2.Zero;
			shape.X = (int)position.X;
			shape.Y = (int)position.Y;
			if(light != null)
				light.changePosition (shape.X + shape.Width / 2, shape.Y + shape.Height / 2);
			if (Sort.Fixed == type) {
				if (light != null)
					light.life = 0.4F;
			}
			else {
				life -= 0.01F;
				duration -= 0.01F;
				if(light != null)
					light.life = 0.4F*duration;
				if (life <= 0)
					EffectManager.removeParticle (this);
			}
		}

		public override void draw(SpriteBatch par1)
		{
			par1.Draw (sprite, shape, Color.White);
		}
	}
}

