namespace TetrisApp
{
	sealed class Figure_O : Figure
	{
		public Figure_O(int x = 0, int y = 0) : base(2, 2)
		{
			Position = new Position(x, y);
		}

		public override void Rotate()
		{
			return;
		}
	}
}
