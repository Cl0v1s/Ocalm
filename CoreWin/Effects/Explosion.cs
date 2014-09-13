using System;
using Microsoft.Xna.Framework;

namespace CoreFork
{
	public class Explosion
	{

		Random rand=new Random();

		public Explosion (Vector2 origin,Entity.directions dir, float intensity,float duration, Color color)
		{
			for (int i = 0; i < rand.Next (1, (int)(5 + 2 * intensity)); i++) {
				Particle e = new Particle ((int)origin.X, (int)origin.Y, color, Particle.Sort.Explosion);
				int g = rand.Next (1, 3);
				if (g != 1)
					g = -1;
				switch (dir) {
				case Entity.directions.Down:
					e.velocity = new Vector2 (rand.Next (0, (int)(10 * intensity)) * g, (int)(50 * intensity));
					break;
				case Entity.directions.Up:
					e.velocity = new Vector2 (rand.Next (0, (int)(10 * intensity)) * g, (int)(50 * intensity)*-1);
					break;
				case Entity.directions.Left:
					e.velocity = new Vector2 ((int)(50*intensity)*-1, rand.Next (0, (int)(10 * intensity)) * g);
					break;
				case Entity.directions.Right:
					e.velocity = new Vector2 ((int)(50 * intensity), rand.Next (0, (int)(10 * intensity)) * g);
					break;

				}
				e.life = duration;
				EffectManager.addParticle (e);
			}
		}
	}
}

