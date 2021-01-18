namespace TetrisApp
{
	sealed class Figure_J : Figure
	{
		public Figure_J(int x = 0, int y = 0) : base(2, 3)
		{
			Position = new Position(x, y);
			InitFigureShape();
		}

		public override void Rotate()
		{
			Replace(MatrixCalculation.Rotate(this));
		}

		private void InitFigureShape()
		{
			Data[0][0] = false;
			Data[0][1] = false;
		}

	}
}
