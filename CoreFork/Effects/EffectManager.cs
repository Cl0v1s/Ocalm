using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CoreFork
{
	public class EffectManager
	{

		public static List<Light> lights;
		public static List<Particle> particles;
		public static Texture2D cleaner;

		public EffectManager ()
		{
		}

		public static void initialize()
		{
			lights=new List<Light>();
			particles = new List<Particle> ();
			cleaner=new Texture2D(Core.graphicsD,1,1);
			cleaner.SetData (new Color[]{ Color.Black });
		}

		public static void update()
		{
			for(int i = 0; i < particles.Count; i++) 
			{
				Particle e = particles [i];
				e.update();
			}
			for(int i = 0; i < lights.Count; i++) 
			{
				Light e = lights [i];
				e.update();
			}
		}

		public static Particle addParticle(Particle  par1)
		{
			particles.Add (par1);
			return par1;
		}

		public static void removeParticle(Particle par1)
		{
			removeLight (par1.light);
			particles.Remove (par1);
		}

		public static Light addLight(Light par1)
		{
			lights.Add (par1);
			return par1;
		}

		public static void removeLight(Light par1)
		{
			lights.Remove (par1);
		}


		public static void draw(SpriteBatch par1)
		{
			par1.Draw (cleaner, new Rectangle (0, 0, Core.graphics.PreferredBackBufferWidth, Core.graphics.PreferredBackBufferHeight), Color.White);
			for(int i = 0; i < particles.Count; i++) 
			{
				Particle e = particles [i];
				e.draw (par1);
			}
			for(int i = 0; i < lights.Count; i++) 
			{
				Light e = lights [i];
				e.draw (par1);
			}
		}
	}
}

