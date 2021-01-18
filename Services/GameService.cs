using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TetrisApp
{
	sealed class GameService
	{
		private GameService()
		{ }

		[MethodImpl(MethodImplOptions.Synchronized)]
		public static void PrintField(Game game)
		{
			Matrix composedField = ComposeObjects(game);
			GameConsoleView.PrintMatrixAt(composedField, Game.FIELD_X + 1, Game.FIELD_Y + 1);
		}

		private static Matrix ComposeObjects(Game game)
		{
			Matrix tmp = game.Field.Copy();

			MatrixCalculation.AddOnMatrix(tmp, game.Figure, game.Figure.Position);

			MatrixCalculation.AddOnMatrix(tmp, game.Heap, game.Heap.Position);

			return tmp;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public static void React(ConsoleKeyInfo keyInfo, Game game)
		{
			switch (keyInfo.Key)
			{
				case ConsoleKey.UpArrow:
					if (!game.isPaused && !game.isFinished)
						OnKeyUpPressed(game);
					break;
				case ConsoleKey.DownArrow:
					if (!game.isPaused && !game.isFinished)
						OnKeyDownPressed(game);
					break;
				case ConsoleKey.LeftArrow:
					if (!game.isPaused && !game.isFinished)
						OnKeyLeftPressed(game);
					break;
				case ConsoleKey.RightArrow:
					if (!game.isPaused && !game.isFinished)
						OnKeyRightPressed(game);
					break;
				case ConsoleKey.S:
					if (!game.isFinished)
						OnKeyStartPressed(game);
					break;
				case ConsoleKey.P:
					if (!game.isPaused && !game.isFinished)
						OnKeyPausePressed(game);
					break;
				case ConsoleKey.N:
					OnKeyNewPressed(game);
					break;

			}

		}
		private static void OnKeyUpPressed(Game game)
		{
			KeyValuePair<bool, int> kv = CanRotate(game);

			if (kv.Key)
			{
				game.Figure.Rotate();
				game.Figure.Position = new Position(game.Figure.Position.X + kv.Value, game.Figure.Position.Y);
				PrintField(game);
			}
			else
			{
				return;
			}
		}

		private static void OnKeyDownPressed(Game game)
		{
			MoveFigureDown(game);
			PrintField(game);
			if (game.isFinished)
				GameConsoleView.PrintGameOver(Game.GAME_OVER_X, Game.GAME_OVER_Y);
		}

		private static void OnKeyLeftPressed(Game game)
		{
			if (CanMoveFigureLeft(game))
			{
				game.Figure.Position = new Position(game.Figure.Position.X - 1, game.Figure.Position.Y);
				PrintField(game);
			}
		}

		private static void OnKeyRightPressed(Game game)
		{
			if (CanMoveFigureRight(game))
			{
				game.Figure.Position = new Position(game.Figure.Position.X + 1, game.Figure.Position.Y);
				PrintField(game);
			}
		}

		private static void OnKeyStartPressed(Game game)
		{
			game.Start();
		}

		private static void OnKeyPausePressed(Game game)
		{
			game.Pause();
		}


		private static void OnKeyNewPressed(Game game)
		{
			game.New();
		}

		public static void MoveFigureDown(Game game)
		{
			if (CanMoveFigureDown(game))
			{
				game.Figure.Position = new Position(game.Figure.Position.X, game.Figure.Position.Y + 1);
			}
			else
			{
				ConnectToHeap(game);
			}
		}

		public static Figure GetNextRandomFigure(Game game)
		{
			Random rand = new Random();
			int randomX = rand.Next(0, (Game.FIELD_WIDTH - 4));
			switch (rand.Next(0, 4))
			{

				case 1:
					return new Figure_J(randomX);
				case 2:
					return new Figure_O(randomX);
				case 3:
					return new Figure_T(randomX);
				case 0:
				default:
					return new Figure_I(randomX);
			}
		}

		public static bool CanMoveFigureDown(Game game)
		{
			return game.Figure.Position.Y + game.Figure.Height < Game.FIELD_HEIGHT
				&& MatrixCalculation.HasSpaceUnder(game.Field, game.Figure, game.Figure.Position, game.Heap, game.Heap.Position);
		}

		public static bool CanMoveFigureLeft(Game game)
		{
			return game.Figure.Position.X > 0
				&& MatrixCalculation.HasSpaceLeft(game.Field, game.Figure, game.Figure.Position, game.Heap, game.Heap.Position);
		}

		public static bool CanMoveFigureRight(Game game)
		{
			return game.Figure.Position.X < (game.Field.Width - game.Figure.Width)
				&& MatrixCalculation.HasSpaceRight(game.Field, game.Figure, game.Figure.Position, game.Heap, game.Heap.Position);
		}

		public static KeyValuePair<bool, int> CanRotate(Game game)
		{
			Figure tmp = game.Figure.Copy();
			tmp.Rotate();


			int offsetX = 0;
			if (tmp.Position.X + tmp.Width >= game.Field.Width)
			{
				offsetX = game.Field.Width - (tmp.Position.X + tmp.Width);
			}

			if (!MatrixCalculation.CheckPlaceAfterRotate(game.Field, tmp, tmp.Position, game.Heap, game.Heap.Position))
			{
				return KeyValuePair.Create(false, 0);
			}

			return KeyValuePair.Create(true, offsetX);
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private static void ConnectToHeap(Game game)
		{
			game.Heap.Replace(MatrixCalculation.AddToMatrix(game.Figure, game.Figure.Position, game.Heap, game.Heap.Position));
			game.Heap.Position = new Position(0, (Game.FIELD_HEIGHT - game.Heap.Height));
			if (IsGameOver(game))
			{
				FinishGame(game);
				return;
			}
			game.Figure = game.NextFigure;
			game.NextFigure = GetNextRandomFigure(game);
			GameConsoleView.PrintNextFigure(game.NextFigure, Game.NEXT_FIGURE_X, Game.NEXT_FIGURE_Y);
			if (game.Heap.Width == game.Field.Width)
			{
				ReduceHeapIfNeeded(game);
			}
		}

		private static void ReduceHeapIfNeeded(Game game)
		{
			int fullRowIndex = MatrixCalculation.GetFirstFullRowIndex(game.Heap);
			while (fullRowIndex != -1)
			{
				game.Heap.Replace(MatrixCalculation.RemoveRowFromMatrix(game.Heap, fullRowIndex));
				game.Heap.Position = new Position(game.Heap.Position.X, game.Heap.Position.Y + 1);
				game.ScoreUp();
				fullRowIndex = MatrixCalculation.GetFirstFullRowIndex(game.Heap);
			}
		}

		private static bool IsGameOver(Game game)
		{
			return game.Heap.Height == game.Field.Height;
		}

		private static void FinishGame(Game game)
		{
			game.isPaused = true;
			game.isFinished = true;
		}

	}
}
