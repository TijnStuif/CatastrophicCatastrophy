﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blok3Game.Engine.GameObjects
{
	public class SpriteSheet
	{
		protected Texture2D sprite;
		Rectangle spriteRectangle;
		protected bool[] collisionMask;
		protected int sheetIndex;
		protected int sheetColumns;
		protected int sheetRows;
		protected bool mirror;
		protected float radians;


		public SpriteSheet(string assetname, int sheetIndex = 0)
		{
			// retrieve the sprite
			sprite = GameEnvironment.AssetManager.GetSprite(assetname);

			radians = 0.0f;

			// construct the collision mask
			Color[] colorData = new Color[sprite.Width * sprite.Height];
			collisionMask = new bool[sprite.Width * sprite.Height];
			sprite.GetData(colorData);
			for (int i = 0; i < colorData.Length; ++i)
			{
				collisionMask[i] = colorData[i].A != 0;
			}

			sheetColumns = 1;
			sheetRows = 1;

			// see if we can extract the number of sheet elements from the assetname
			string[] assetSplit = assetname.Split('@');
			if (assetSplit.Length <= 1)
			{
				// Set SheetIndex for Sprites not being a spritesheet
				// so the whole sprite rectangle is used for drawing.
				SheetIndex = sheetIndex;
				return;
			}

			string sheetNrData = assetSplit[assetSplit.Length - 1];
			string[] colRow = sheetNrData.Split('x');
			sheetColumns = int.Parse(colRow[0]);
			if (colRow.Length == 2)
			{
				sheetRows = int.Parse(colRow[1]);
			}

			// Set SheetIndex for Spritesheet for multiple sprites inside
			// so a partial rectangle part is used for drawing.
			SheetIndex = sheetIndex;
		}

		public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 origin, float scale, Color color)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (mirror)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			spriteBatch.Draw(sprite, position, spriteRectangle, color,
				radians, origin, scale, spriteEffects, 0.0f);
		}

		public bool IsTranslucent(int x, int y)
		{
			int column_index = sheetIndex % sheetColumns;
			int row_index = sheetIndex / sheetColumns % sheetRows;

			int index = column_index * Width + x + (row_index * Height + y) * sprite.Width;

			if (index < 0 || index >= collisionMask.Length)
			{
				// Handle out-of-bounds index
				return false; // or throw an exception, or handle it in some other way
			}

			return collisionMask[column_index * Width + x + (row_index * Height + y) * sprite.Width];
		}

		public Texture2D Sprite
		{
			get { return sprite; }
		}

		public Vector2 Center
		{
			get { return new Vector2(Width, Height) / 2; }
		}

		public int Width
		{
			get
			{ return sprite.Width / sheetColumns; }
		}

		public int Height
		{
			get
			{ return sprite.Height / sheetRows; }
		}

		public bool Mirror
		{
			get { return mirror; }
			set { mirror = value; }
		}

		public float Radians
		{
			get { return radians; }
			set { radians = value; }
		}

		public int NumberOfSheetElements
		{
			get { return sheetColumns * sheetRows; }
		}

		public int SheetIndex
		{
			get { return sheetIndex; }
			set
			{
				if (value < NumberOfSheetElements && value >= 0)
				{
					sheetIndex = value;

					// recalculate the part of the sprite to draw
					int columnIndex = sheetIndex % sheetColumns;
					int rowIndex = sheetIndex / sheetColumns;
					spriteRectangle = new Rectangle(columnIndex * Width, rowIndex * Height, Width, Height);
				}
			}
		}

		public Rectangle Bounds
		{
			get
			{
				return new Rectangle(0, 0, Width, Height);
			}
		}
	}
}