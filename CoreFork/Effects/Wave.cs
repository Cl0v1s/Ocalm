using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoreFork
{
	public class Wave : Effect
	{

		float speed;

		public Wave (int par1x, int par2y, float par3size, float par4duration, Color par5color)
		{
			sprite = Core.Png [8];
			position = new Vector2 (par1x, par2y);
			size = par3size;
			life = par4duration;
			color = par5color;
			speed = new Random ().Next (1, 10) / 100F;
		}

		public override void update()
		{
			life -= 0.1F;
			size += speed;
			if (life <= 0)
				Background.removeEffect (this);
		}

		public override void draw(SpriteBatch par1)
		{
			par1.Draw (sprite, position, new Rectangle (0, 0, sprite.Width, sprite.Height), color*life*0.8F, 0F, new Vector2 (sprite.Width / 2, sprite.Height / 2), size,SpriteEffects.None,0F);
		}

	}
}
