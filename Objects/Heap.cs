namespace TetrisApp
{
	sealed class Heap : Matrix
	{
		public Heap(int x, int y, int width = 0, int height = 0) : base(width, height) 
		{
			Position = new Position(x, y);
		}

		public Position Position { get; set; }
	}
}
