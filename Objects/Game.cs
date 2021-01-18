using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace TetrisApp
{
	class Game
	{
		private int delayTime;
		private const int MIN_DELAY_TIME = 100;
		private const int REDUCE_DELAY_STEP = 20;
		public bool isPaused = true;
		public bool isFinished = false;

		public const int FIELD_X = 20;
		public const int FIELD_Y = 5;
		public const int FIELD_WIDTH = 20;
		public const int FIELD_HEIGHT = 20;
		public const int RULES_X = Game.FIELD_X + Game.FIELD_WIDTH + 5;
		public const int RULES_Y = Game.FIELD_Y;
		public const int SCORE_X = Game.FIELD_X + Game.FIELD_WIDTH + 5;
		public const int SCORE_Y = Game.FIELD_Y + 20;
		public const int NEXT_FIGURE_X = Game.FIELD_X + Game.FIELD_WIDTH + 5;
		public const int NEXT_FIGURE_Y = Game.FIELD_Y + 12;
		public const int GAME_OVER_X = Game.FIELD_WIDTH / 2 + Game.FIELD_X;
		public const int GAME_OVER_Y = Game.FIELD_HEIGHT / 2 + Game.FIELD_Y;

		public Game(int startDelayTime = 200)
		{
			Console.SetWindowSize(Console.WindowWidth, 35);
			delayTime = startDelayTime;

			KeyInputListener listener = new KeyInputListener();

			KeyMap km = new KeyMap();

			listener.OnKeyPressed += (k) => { 
				GameService.React(k, this);
			};
			listener.Listen();
			New();
			Render();
		}

		public Matrix Field { get; private set; }
		public Heap Heap { get; set; }
		public Figure Figure { get; set; }
		public Figure NextFigure { get; set; }
		public int Score { get; set; }

		public void Start()
		{
			isPaused = false;
		}

		public void Pause()
		{
			isPaused = true;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void New()
		{
			isPaused = true;
			isFinished = false;
			Field = new Matrix(FIELD_WIDTH, FIELD_HEIGHT);
			Heap = new Heap(0, FIELD_HEIGHT);
			Figure = GameService.GetNextRandomFigure(this);
			NextFigure = GameService.GetNextRandomFigure(this);
			GameConsoleView.PrintBorder(FIELD_WIDTH, FIELD_HEIGHT, Game.FIELD_X, Game.FIELD_Y);
			GameConsoleView.PrintRules(RULES_X, RULES_Y);
			GameConsoleView.PrintScore(Score, SCORE_X, SCORE_Y);
			GameConsoleView.PrintNextFigure(NextFigure, NEXT_FIGURE_X, NEXT_FIGURE_Y);
			GameService.PrintField(this);			
		}

		private void Render()
		{
			while (true)
			{

				IfPausedWait();

				NextState();

				GameService.PrintField(this);

				if (isFinished)
					GameConsoleView.PrintGameOver(GAME_OVER_X, GAME_OVER_Y);

				DelayTime();

			}
		}

		private void NextState()
		{
			GameService.MoveFigureDown(this);
		}

		private void IfPausedWait()
		{
			while (isPaused)
			{
				Thread.Sleep(100);
			}
		}

		private void DelayTime()
		{
			Thread.Sleep(delayTime);
		}

		private void ReduceDelayTime()
		{
			delayTime -= REDUCE_DELAY_STEP;
		}

		public void ScoreUp(int amount = 100)
		{
			Score = Score + amount;
			GameConsoleView.PrintScore(Score, SCORE_X, SCORE_Y);
			ReduceDelayTime();
		}
	}
}
