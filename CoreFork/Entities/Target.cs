using System;
using Microsoft.Xna.Framework;

namespace CoreFork
{
	public class Target : Entity
	{


		public Magnet.Gender type;
		Light light;
		Particle[] particles;
		bool isHeart=false;


		public Target (Party par1scene, int par2x,int pary)
			:base(par1scene,par2x,pary,20,20)
		{
			if (rand.Next (1, 3) == 1) {
				sprite = Core.Png [2];
				type = Magnet.Gender.Minus;
			} else {
				sprite = Core.Png [5];
				type = Magnet.Gender.Plus;
			}
			if (rand.Next (1, 20) == 1)
				isHeart = true;
			boundingboxes = Utils.getBoundingBoxFrom (sprite, shape);
			light = EffectManager.addLight (new Light (shape.X + shape.Width / 2, shape.Y + shape.Height / 2,0.5F,Color.White));
			particles=new Particle[rand.Next(3,6)];
			for (int i = 0; i < particles.Length; i++) {
				particles [i] = EffectManager.addParticle (new Particle (shape.X + shape.Width / 2 + (shape.Width / 2), shape.Y + shape.Height / 2 + (shape.Height / 2), Color.DarkOrange, Particle.Sort.Fixed));
			}
		}

		protected override bool checkCollision (Microsoft.Xna.Framework.Vector2 par1)
		{
			Rectangle rec = new Rectangle ((int)par1.X+boundingboxes[dir].X, (int)par1.Y+boundingboxes[dir].Y, boundingboxes [dir].Width, boundingboxes [dir].Height);
			for(int i=0;i<world.entities.Count;i++)
			{
				Entity e = world.entities [i];
				if (e != this && e is Player && type==((Player)e).type)
				{
					Rectangle other=new Rectangle ((int)e.shape.X+e.boundingboxes[dir].X, (int)e.shape.Y+e.boundingboxes[dir].Y, e.boundingboxes [dir].Width, e.boundingboxes [dir].Height);
					if (other.Intersects (rec)) {
						((Player)e).goal (1);
						Core.playSound(Core.Sounds [2], 0.6F);
						world.killEntity (this);
						EffectManager.removeLight (light);
						for (int t = 0; t < particles.Length; t++) {
							EffectManager.removeParticle (particles [t]);
						}
						if (isHeart)
							((Player)e).heal (0.5F);
						Random rand = new Random ();
						world.spawnEntity (new Target (world, rand.Next (10, (int)world.canvas.X - 10), rand.Next (10, (int)world.canvas.Y - 10))); 
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

		public override void draw (Microsoft.Xna.Framework.Graphics.SpriteBatch par1)
		{
			par1.Draw (sprite, shape, new Rectangle (shape.Width * ((int)frame), 0, shape.Width, shape.Height), Color.White);
			if (isHeart) 
				par1.Draw (Core.Png [7], new Vector2 (shape.X, shape.Y), Color.White);

		}


	}
}

