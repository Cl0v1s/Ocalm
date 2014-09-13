#region Using Statements
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

#endregion
namespace CoreFork
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Core : Game
	{
		public static GraphicsDeviceManager graphics;
		public static SpriteBatch spriteBatch;
		public static GraphicsDevice graphicsD;
		public static ContentManager content;

		public static Scene scene;
		public static Texture2D[] Png;
		public static SoundEffect[] Sounds;
		public static string currentMusic;
		public static FontRenderer fontRenderer;

		public static int bestScore=0;

		public Core ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";	  
			content = Content;
			graphics.PreferredBackBufferWidth = 400;
			graphics.PreferredBackBufferHeight = 400;  
			graphics.IsFullScreen = false;

		}

		public static string GetVersion()
		{
			return "1.0";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			base.Initialize ();
			graphicsD = GraphicsDevice;
			loadMemory ();
			EffectManager.initialize ();
			Background.initialize ();
			new Title ().initialize ();
		}

		void loadMemory()
		{
			if (SaveManager.load ()) {

				Saver meta = SaveManager.getSaver ("Meta");
				bestScore = meta.getDataAsInt ("BestScore");
			}
		}

		protected override void UnloadContent ()
		{
			Content.Dispose ();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);

			Png = new Texture2D[9];
			Png [0] = Content.Load<Texture2D> ("Hero_B");
			Png [1] = Content.Load<Texture2D> ("Hero_R");
			Png [2] = Content.Load<Texture2D> ("Core_B");
			Png [3] = Content.Load<Texture2D> ("-");
			Png [4] = Content.Load<Texture2D> ("+");
			Png [5] = Content.Load<Texture2D> ("Core_R");
			Png [6] = Content.Load<Texture2D> ("Light");
			Png [7] = Content.Load<Texture2D> ("Heart");
			Png [8] = Content.Load<Texture2D> ("Wave");

			Sounds = new SoundEffect[5];
			Sounds [0] = Content.Load<SoundEffect> ("Musics/theme");
			Sounds [0].Name = "Theme";
			Sounds [1] = Content.Load<SoundEffect> ("Sounds/Explode");
			Sounds [2] = Content.Load<SoundEffect> ("Sounds/Target");
			Sounds [3] = Content.Load<SoundEffect> ("Sounds/Magnet");
			Sounds [4] = Content.Load<SoundEffect> ("Sounds/Yes");


			fontRenderer = new FontRenderer (FontLoader.Load ("Font/font.fnt"), Content.Load<Texture2D> ("Font/font_0"));


		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
				Exit ();
			}
			Input.update ();
			scene.update ();
			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (new Color(40,40,40));
			spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
			Background.draw (spriteBatch);
			spriteBatch.End ();
			spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
			scene.draw (spriteBatch);
			spriteBatch.End ();
			spriteBatch.Begin (SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
			EffectManager.draw (spriteBatch);
			spriteBatch.End ();

			base.Draw (gameTime);
		}

		public static void PlaySong(SoundEffect par1title, float par2volume)
		{
			if (currentMusic != par1title.Name) {
				Core.write ("Playing " + par1title.Name);
				par1title.Play (par2volume,0F,1F);
				currentMusic = par1title.Name;
			} else
				Core.write (par1title.Name + " is already running.");
		}

		public static void playSound(SoundEffect par1sound, float par2volume)
		{
            try
            {
                par1sound.Play(par2volume, 0F, 1F);
            }
            catch (InstancePlayLimitException)
            {

            }
		}

		public static void drawText(SpriteBatch par6canvas, string par1, int par2x, int par3y, float par4size, Color par5color)
		{
			fontRenderer.DrawText (par6canvas, par2x, par3y, par1, par5color, par4size);
		}

		public static void write(object par1)
		{
			#if DEBUG
			Debug.WriteLine ("Core ["+DateTime.Now+"]: " + par1.ToString ());
			#endif
		}

	}
}

