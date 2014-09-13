using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CoreFork
{
	public class Player : Entity
	{

		public Magnet.Gender type;
		public float life = 1F;
		public int score;
		public bool dead=false;
		int deathTime;
		int magnetTime=70;
		int changeTime=0;
		int alertTime=30;
		int speed;


		public Player (Party par5scene,int par1x,int par2y,int par3w,int par4h)
			:base(par5scene,par1x,par2y,par3w,par4h)
		{
			sprite = Core.Png [0];
			boundingboxes=Utils.getBoundingBoxFrom (sprite,shape);
			type = Magnet.Gender.Minus;
		}

		public override void update ()
		{
			magnetTime -= 1;
			changeTime -= 1;
			if (magnetTime <= 0) 
			{
				if (world.difficulty<15)
					magnetTime = (int)Math.Abs (50 - world.difficulty);
				else
					magnetTime = 20;
				fire ();
			}
				position += 0.03F * velocity;
				velocity -= 0.03F * velocity;
				if (!checkCollision (position))
					shape = new Rectangle ((int)position.X, (int)position.Y, shape.Width, shape.Height);
				else {
					position = new Vector2 (shape.X, shape.Y);
				}
			if (life <= 0)
				life = 0;
			if (life <= 0.5F) {
				alertTime -= 1;
				if (alertTime <= 0) {
					alertTime = (int)(100 * (life+0.2F));
					EffectManager.addLight (new Light ((int)(position.X + boundingboxes [dir].X + boundingboxes [dir].Width / 2), (int)(position.Y + boundingboxes [dir].Y + boundingboxes [dir].Height / 2), 2F, Color.Red));
					EffectManager.addLight (new Light ((int)(position.X + boundingboxes [dir].X + boundingboxes [dir].Width / 2), (int)(position.Y + boundingboxes [dir].Y + boundingboxes [dir].Height / 2), 2F, Color.Red));
					EffectManager.addLight (new Light ((int)(position.X + boundingboxes [dir].X + boundingboxes [dir].Width / 2), (int)(position.Y + boundingboxes [dir].Y + boundingboxes [dir].Height / 2), 2F, Color.Red));
				}
			}

			if (life <= 0.3F) {
				for (int i = 0; i < rand.Next (0,(int)(2+(3*(1-life)))); i++) {
					Particle e = new Particle ((int)(position.X + boundingboxes [dir].X + boundingboxes [dir].Width / 2-2), (int)(position.Y + boundingboxes [dir].Y + boundingboxes [dir].Height / 2-2), Color.Gray, Particle.Sort.Fog);
					e.velocity = new Vector2 (rand.Next (-50, 50), rand.Next (-50, 50));
					EffectManager.addParticle (e);
				}
			}

			if (life <= 0) {
				velocity = Vector2.Zero;
					for (int i = 0; i < rand.Next (0,(int)(2+(3*(1-life)))); i++) {
						Particle e = new Particle ((int)(position.X + boundingboxes [dir].X + boundingboxes [dir].Width / 2-2), (int)(position.Y + boundingboxes [dir].Y + boundingboxes [dir].Height / 2-2), new Color(rand.Next(0,50),rand.Next(0,50),rand.Next(0,50)), Particle.Sort.Fog);
						e.velocity = new Vector2 (rand.Next (-50, 50), rand.Next (-50, 50));
						EffectManager.addParticle (e);
					}
					for (int i = 0; i < rand.Next (4, 10); i++) {
						directions dir = directions.Right;
					switch (rand.Next (1, 5)) {
						case 1:
							dir = directions.Left;
							break;
						case 2:
							dir = directions.Down;
							break;
						case 3:
							dir = directions.Up;
							break;
						}
					new Explosion (new Vector2 (shape.X + shape.Width / 2, shape.Y + shape.Height / 2), dir, 0.5F, 0.5F, Color.DarkOrange);
					}
				Core.playSound (Core.Sounds [1], rand.Next(1,6)/10F);
				deathTime += 1;
				if (deathTime > 50)
					dead = true;
			}
			//TODO: implements explosion on death
		}

		protected override bool checkCollision (Vector2 par1)
		{
			Rectangle rec = new Rectangle ((int)par1.X+boundingboxes[dir].X, (int)par1.Y+boundingboxes[dir].Y, boundingboxes [dir].Width, boundingboxes [dir].Height);
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

		public void change()
		{
			if (changeTime > 0)
				return;
			changeTime = 10;
			if (type == Magnet.Gender.Minus) {
				type = Magnet.Gender.Plus;
				sprite = Core.Png [1];
			} else {
				type = Magnet.Gender.Minus;
				sprite = Core.Png [0];
			}
		}

		void fire()
		{
			if (life<=0)
				return;
			Magnet m = new Magnet (type, world, (int)-30, (int)-30);
			Magnet m1 = new Magnet (type, world, (int)-30, (int)-30);
			int sped = speed + 20;
			if (world.difficulty > 30)
				m.life = 1F + 0.03F * score*0.5F;
			switch (dir) {
			case 0:
				m.velocity.Y = sped;
				m.position = new Microsoft.Xna.Framework.Vector2 (position.X + boundingboxes [dir].X, position.Y + boundingboxes [dir].Height + m.shape.Height);
				m1.position = new Microsoft.Xna.Framework.Vector2 (position.X + boundingboxes [dir].Width + boundingboxes [dir].X - m1.shape.Width, position.Y + boundingboxes [dir].Height + m.shape.Height);
				new Explosion (m.position+new Vector2(3,0), directions.Down, 2,0.2F, Color.GreenYellow);
				new Explosion (m.position+new Vector2(boundingboxes[dir].Width-boundingboxes[dir].X-3,0), directions.Down, 2,0.2F, Color.GreenYellow);
				break;
			case 1:
				m.velocity.X = sped;
				m.position = new Microsoft.Xna.Framework.Vector2 (position.X + boundingboxes [dir].Width+m.shape.Width, position.Y + boundingboxes[dir].Y);
				m1.position = new Microsoft.Xna.Framework.Vector2 (position.X + boundingboxes [dir].Width+m.shape.Width, position.Y + boundingboxes[dir].Y+boundingboxes[dir].Height-m1.shape.Height);
				new Explosion (m.position+new Vector2(0,3), directions.Right, 2,0.2F, Color.GreenYellow);
				new Explosion (m.position+new Vector2(0,boundingboxes[dir].Height-boundingboxes[dir].Y-3), directions.Right, 2,0.2F, Color.GreenYellow);
				break;
			case 2:
				m.velocity.X = -sped;
				m.position = new Microsoft.Xna.Framework.Vector2 (position.X -m.shape.Width, position.Y + boundingboxes[dir].Y);
				m1.position = new Microsoft.Xna.Framework.Vector2 (position.X -m.shape.Width-10, position.Y + boundingboxes[dir].Y+boundingboxes[dir].Height-m1.shape.Height);
				new Explosion (m.position+new Vector2(0,3), directions.Left, 2,0.2F, Color.GreenYellow);
				new Explosion (m.position+new Vector2(0,boundingboxes[dir].Height-boundingboxes[dir].Y-3), directions.Left, 2,0.2F, Color.GreenYellow);

				break;
			case 3:
				m.velocity.Y = -sped;
				m.position = new Microsoft.Xna.Framework.Vector2 (position.X + boundingboxes [dir].X, position.Y - m.shape.Height);
				m1.position = new Microsoft.Xna.Framework.Vector2 (position.X + boundingboxes [dir].Width+boundingboxes[dir].X-m1.shape.Width, position.Y - m.shape.Height);
				new Explosion (m.position+new Vector2(3,0), directions.Up, 2,0.2F, Color.GreenYellow);
				new Explosion (m.position+new Vector2(boundingboxes[dir].Width-boundingboxes[dir].X-3,0), directions.Up, 2,0.2F, Color.GreenYellow);
				break;
			}
			m1.velocity = m.velocity;
			world.spawnEntity (m1);
			world.spawnEntity (m);
		}

		public void damage(float par1amount)
		{
			life -= par1amount;
			EffectManager.addLight (new Light ((int)(world.canvas.X / 2), (int)(world.canvas.Y / 2), 10F, Color.DarkRed));
		}


		public void goal(int amount)
		{
			score += amount;
			EffectManager.addLight (new Light ((int)(world.canvas.X / 2), (int)(world.canvas.Y / 2), 10F, Color.GreenYellow));
		}

		public void heal(float par1)
		{
			if (par1 == 0)
				life = 1F;
			else
				life += par1;
			if (life >= 1F) {
				life = 1F;
			}
			EffectManager.addLight (new Light ((int)(world.canvas.X / 2), (int)(world.canvas.Y / 2), 10F, Color.LightPink));
		}

		public void move(Entity.directions par1)
		{
			if (life<=0)
				return;
			speed = (int)(100 + world.difficulty * 2);


			switch (par1) {
			case Entity.directions.Up:
				velocity.Y = -speed;
				velocity.X = 0;
				frame = 3;
				break;
			case directions.Down:
				velocity.Y = speed;
				velocity.X = 0;
				frame = 0;
				break;
			case directions.Right:
				velocity.X = speed;
				velocity.Y = 0;
				frame = 1;
				break;
			case directions.Left:
				velocity.X = -speed;
				velocity.Y = 0;
				frame = 2;
				break;
			}
			dir = (int)frame;
		}
	}
}

