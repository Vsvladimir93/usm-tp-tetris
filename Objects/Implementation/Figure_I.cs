namespace TetrisApp
{
	sealed class Figure_I : Figure
	{
		public Figure_I(int x = 0, int y = 0) : base(1, 4)
		{
			Position = new Position(x, y);
		}		

		public override void Rotate()
		{
			Replace(MatrixCalculation.Rotate(this));
		}

	}
}
