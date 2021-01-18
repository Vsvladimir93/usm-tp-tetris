using System;

namespace TetrisApp
{
	abstract class Figure : Matrix, IRotational
	{
		public Figure(int width, int height, int x = 0, int y = 0) : base(width, height)
		{
			Position = new Position(x, y);
			InitMatrix();
		}

		public Position Position { get; set; }

		public new Figure Copy()
		{
			Figure copy = (Figure)this.MemberwiseClone();

			copy.Data = new bool[this.Data.Length][];
			for (int x = 0; x < Width; x++)
			{
				copy.Data[x] = new bool[Height];
				for (int y = 0; y < Height; y++)
				{
					copy.Data[x][y] = this.Data[x][y];
				}
			}

			return copy;
		}

		private void InitMatrix()
		{
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					Data[x][y] = true;
				}
			}
		}

		public abstract void Rotate();

	}

}
