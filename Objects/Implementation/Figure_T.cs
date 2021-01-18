namespace TetrisApp
{
	sealed class Figure_T : Figure
	{
		public Figure_T(int x = 0, int y = 0) : base(3, 2)
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
			Data[2][0] = false;
			//Data[1][0] = false;
			//Data[1][1] = false;
			//Data[4][0] = false;
			//Data[4][1] = false;
			//Data[5][0] = false;
			//Data[5][1] = false;
		}

	}
}
