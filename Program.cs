using System;

namespace TetrisApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Game game = new Game();
			game.New();
		}

		public static void Test()
		{
			Matrix top = GenMatrix();
			Position pTop = new Position(0, 4);

			Matrix bot = GenRevMatrix();
			Position pBot = new Position(0, 4);

			//Console.WriteLine(MatrixCalculation.HasSpaceUnder(top, pTop, bot, pBot));

		}

		public static Matrix GenMatrix()
		{
			Matrix m = new Matrix(5, 3);
			for (int x = 0; x < m.Width; x++)
			{
				for (int y = 0; y < m.Height; y++)
				{
					m.Data[x][y] = true;
				}
			}

			m.Data[0][1] = false;
			m.Data[0][2] = false;
			m.Data[1][2] = false;
			m.Data[3][2] = false;
			m.Data[4][2] = false;
			m.Data[4][1] = false;

			return m;
		}

		public static Matrix GenRevMatrix()
		{
			Matrix m = new Matrix(5, 3);
			for (int x = 0; x < m.Width; x++)
			{
				for (int y = 0; y < m.Height; y++)
				{
					m.Data[x][y] = true;
				}
			}

			m.Data[1][0] = false;
			m.Data[2][0] = false;
			m.Data[3][0] = false;
			m.Data[2][1] = false;

			return m;
		}

	}
}
