using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CoreFork
{
	public class Utils
	{

		public static Color[,] getColourMap(Texture2D par1)
		{
			Color[] unsort = new Color[par1.Width * par1.Height];
			par1.GetData (unsort);
			Color[,] sort = new Color[par1.Width, par1.Height];
			for (int x = 0; x < par1.Width; x++) {
				for (int y = 0; y < par1.Height; y++) {
					sort [x, y] = unsort [x + y * par1.Width];
				}
			}
			return sort;
		}

		public static Color[] unsortColourMap(Texture2D par1,Color[,] par2)
		{
			Color[] unsort = new Color[par1.Width * par1.Height];
			for (int x = 0; x < par1.Width; x++) {
				for (int y = 0; y < par1.Height; y++) {
					unsort [x + y * par1.Width] = par2 [x, y];
				}
			}
			return unsort;
		}

		public static Texture2D alterTexture(Texture2D par1, Vector2 par2, Color par3)
		{
			Color[,] sort = getColourMap (par1);
			sort [(int)par2.X, (int)par2.Y] = par3;
			Color[] unsort = new Color[par1.Width * par1.Height];
			for (int x = 0; x < par1.Width; x++) {
				for (int y = 0; y < par1.Height; y++) {
					unsort [x + y * par1.Width] = sort [x, y];
				}
			}
			par1.SetData (unsort);
			return par1;
		}

		public static Texture2D alterTexture(Texture2D par1, Rectangle par2, Color par3)
		{
			Color[,] sort = getColourMap (par1);
			for (int x = par2.X; x < par2.Width; x++) {
				for (int y = par2.Y; y < par2.Height; y++) {
					sort [x, y] = par3;
				}
			}
			par1.SetData (unsortColourMap(par1,sort));;
			return par1;
		}

		public static Rectangle[] getBoundingBoxFrom(Texture2D par1,Rectangle par2shape)
		{
			Color[,] sort = getColourMap (par1);
			Rectangle[] result = new Rectangle[4];
			//Searching left bounding
			for(int i=0;i<par1.Width/par2shape.Width;i++)
			{
				for(int x=i*par2shape.Width;x<(i+1)*par2shape.Width;x++)
				{
					bool f = false;
					for(int u=0;u<par1.Height;u++)
					{

						if (sort [x, u].PackedValue != 0) 
						{
							result [i].X = x-i*par2shape.Width;
							result [i].Y = u;
							f = true;
							break;
						}
					}
					if (f)
						break;
				}
			}
			//searching right bounding
			for(int i=0;i<par1.Width/par2shape.Width;i++)
			{
				for(int x=(i+1)*par2shape.Width-1;x>(i)*par2shape.Width+1;x--)
				{
					bool f = false;
					for(int u=par1.Height-1;u>0;u--)
					{

						if (sort [x, u].PackedValue != 0) 
						{
							result [i].Width = x-i*par2shape.Width-result [i].X;
							result [i].Height = u-result [i].Y;
							f = true;
							break;
						}
					}
					if (f)
						break;
				}
			}
			return result;
		}
	}
}

