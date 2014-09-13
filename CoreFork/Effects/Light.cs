using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace CoreFork
{
	public class Light : Effect
	{

		public Light (int par1x, int par2y, float scale, Color par4color)
		{
			life = 0.4F;
			shape = new Rectangle (par1x-(int)(100*scale)/2, par2y-(int)(100*scale)/2, (int)(100 * scale), (int)(100 * scale));
			color = par4color;
			size = scale;
			sprite = Core.Png [6];
		}

		public void changePosition(int par1x, int par2y)
		{
			shape.X = par1x - (int)(100 * size) / 2;
			shape.Y = par2y - (int)(100 * size) / 2;
		}

		public override void update()
		{
			life -= 0.01F;
			if (life <= 0)
				EffectManager.removeLight (this);
		}

		public override void draw(SpriteBatch par1)
		{
			par1.Draw (sprite, shape, color*life);
		}
	}
}

