namespace TetrisApp
{
	class Matrix
	{
		public Matrix(int width, int height)
		{
			Width = width;
			Height = height;
			InitMatrix();
		}

		public Matrix(bool[][] matrix)
		{
			Width = matrix.Length;
			Height = matrix[0].Length;
			Data = matrix;
		}
		public bool[][] Data { get; protected set; }
		public int Width { get; protected set; }
		public int Height { get; protected set; }

		private void InitMatrix()
		{
			Data = new bool[Width][];
			for (int x = 0; x < Width; x++)
				Data[x] = new bool[Height];
		}

		public Matrix Copy()
		{
			Matrix m = new Matrix(Width, Height);
			m.Data = new bool[m.Width][];
			for (int x = 0; x < Width; x++)
			{
				m.Data[x] = new bool[m.Height];
				for (int y = 0; y < Height; y++)
				{
					m.Data[x][y] = this.Data[x][y];
				}
			}
			return m;
		}

		public void Replace(Matrix m)
		{
			Width = m.Width;
			Height = m.Height;
			Data = m.Data;
		}

		public int[] GetLeftBorder()
		{
			int[] border = new int[Height];

			for (int y = 0; y < Height; y++)
			{
				int counter = 0;
				for (int x = 0; x < Width; x++)
				{
					if (Data[x][y])
					{
						break;
					}
					else
					{
						counter++;
					}
				}
				border[y] = counter;
			}

			return border;
		}

		public int[] GetRightBorder()
		{
			int[] border = new int[Height];

			for (int y = 0; y < Height; y++)
			{
				int counter = 0;
				for (int x = Width - 1; x >= 0; x--)
				{
					if (Data[x][y])
					{
						break;
					}
					else
					{
						counter++;
					}
				}
				border[y] = counter;
			}

			return border;
		}

		public int[] GetBottomBorder()
		{
			int[] border = new int[Width];

			for (int x = 0; x < Width; x++)
			{
				int counter = 0;
				for (int y = Height - 1; y >= 0; y--)
				{
					if (Data[x][y])
					{
						break;
					}
					else
					{
						counter++;
					}
				}
				border[x] = counter;
			}

			return border;
		}
	}
}
