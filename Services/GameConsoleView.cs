using System;
using System.Runtime.CompilerServices;
using System.Text;
using TetrisApp.Objects;

namespace TetrisApp
{
	sealed class GameConsoleView
	{
		private GameConsoleView()
		{ }

		[MethodImpl(MethodImplOptions.Synchronized)]
		public static void PrintMatrixAt(Matrix matrix, int left, int top, bool background = true)
		{
			StringBuilder sb = new StringBuilder();
			Console.ForegroundColor = ConsoleColor.Red;
			if (background) 
				Console.BackgroundColor = ConsoleColor.DarkBlue;
			for (int y = 0; y < matrix.Height; y++)
			{
				for (int x = 0; x < matrix.Width; x++)
				{
					sb.Append(matrix.Data[x][y] ? '■' : ' ');
				}
				Console.SetCursorPosition(left, top + y);
				Console.WriteLine(sb.ToString());
				sb.Clear();
			}
			Console.ForegroundColor = ConsoleColor.White;
			Console.BackgroundColor = ConsoleColor.Black;
		}

		public static void PrintRules(int left, int top)
		{
			string[] rules = Rules.GetRuleset();

			int width = 0;

			for (int i = 0; i < rules.Length; i++)
			{
				width = width > rules[i].Length ? width : rules[i].Length;
			}

			PrintBorder(width, rules.Length, left, top);

			Console.ForegroundColor = ConsoleColor.Cyan;

			for (int i = 0; i < rules.Length; i++)
			{
				Console.SetCursorPosition(left + 1, top + 1 + i);
				Console.WriteLine(rules[i]);
			}
			
			Console.ForegroundColor = ConsoleColor.White;
		}

		public static void PrintScore(int score, int left, int top)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			
			Console.SetCursorPosition(left, top);
			Console.WriteLine("\tScore : \t {0}", score);

			Console.ForegroundColor = ConsoleColor.White;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public static void PrintNextFigure(Figure f, int left, int top)
		{
			ClearNextFigurePlace(left, top);
			Console.ForegroundColor = ConsoleColor.DarkYellow;

			Console.SetCursorPosition(left, top);
			Console.WriteLine("\tNext figure");
			PrintMatrixAt(f, left + 3, top + 2, false);
			Console.ForegroundColor = ConsoleColor.White;
			
		}

		private static void ClearNextFigurePlace(int left, int top)
		{
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
			for (int x = 0; x < 22; x++)
			{
				for (int y = 0; y < 7; y++)
				{
					Console.SetCursorPosition(left + x, top + y);
					Console.Write(' ');
				}
			}
		}

		public static void PrintGameOver(int left, int top)
		{
			string gameOver = "Game Over";
			PrintBorder(gameOver.Length, 1, left - (gameOver.Length / 2), top);
			Console.BackgroundColor = ConsoleColor.DarkBlue;
			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Console.SetCursorPosition(left + 1 - (gameOver.Length / 2), top + 1);
			Console.WriteLine(gameOver);
			Console.ResetColor();
		}

		public static void PrintBorder(int w, int h, int left, int top)
		{
			// Print Top and Bottom borders
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			for (int x = 1; x <= w; x++)
			{
				Console.SetCursorPosition(left + x, top);
				Console.Write("═");
				Console.SetCursorPosition(left + x, top + h + 1);
				Console.Write("═");
			}
			// Print Left and Right borders
			for (int y = 1; y <= h; y++)
			{
				Console.SetCursorPosition(left, top + y);
				Console.Write("║");
				Console.SetCursorPosition(left + w + 1, top + y);
				Console.Write("║");
			}
			// Print corners
			Console.SetCursorPosition(left, top);
			Console.Write("╔");
			Console.SetCursorPosition(left + w + 1, top);
			Console.Write("╗");
			Console.SetCursorPosition(left, top + h + 1);
			Console.Write("╚");
			Console.SetCursorPosition(left + w + 1, top + h + 1);
			Console.Write("╝");
			Console.ForegroundColor = ConsoleColor.White;
		}
	}
}
