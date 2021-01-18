using System;
using System.Text;
using System.Collections.Generic;

namespace TetrisApp
{
	sealed class MatrixCalculation
	{
		private MatrixCalculation()
		{ }

		public static void AddOnMatrix(Matrix root, Matrix child, Position pos)
		{
			CheckForSize(root, child, pos);
			for (int x = 0; x < child.Width; x++)
			{
				for (int y = 0; y < child.Height; y++)
				{
					if (root.Data[x + pos.X][y + pos.Y])
						continue;

					root.Data[x + pos.X][y + pos.Y] = child.Data[x][y];
				}
			}
		}

		public static Matrix AddToMatrix(Matrix mTop, Position mTopPos, Matrix mBot, Position mBotPos)
		{
			int w = GetRelativeWidth(mTop, mTopPos, mBot, mBotPos);
			int h = GetRelativeHeight(mTopPos, mBot, mBotPos);

			Matrix tmp = new Matrix(w, h);

			KeyValuePair<Position, Position> relativePos = GetRelativePositions(mTopPos, mBotPos);

			AddOnMatrix(tmp, mTop, relativePos.Key);
			AddOnMatrix(tmp, mBot, relativePos.Value);

			return tmp;
		}


		public static bool HasSpaceUnder(Matrix field, Matrix figure, Position fPos, Matrix heap, Position hPos)
		{
			Matrix fieldCopy = field.Copy();

			AddOnMatrix(fieldCopy, heap, hPos);

			int fieldY = 0;
			int fieldX = 0;
			int[] fBotBorder = figure.GetBottomBorder();
			bool[] nextXBlocks = new bool[figure.Width];

			for (int x = 0; x < figure.Width; x++)
			{
				fieldY = fPos.Y + figure.Height - fBotBorder[x];
				fieldX = x + fPos.X;
				nextXBlocks[x] = !fieldCopy.Data[fieldX][fieldY];
			}

			bool hasSpaceUnder = true;

			for (int x = 0; x < nextXBlocks.Length; x++)
			{
				if (!nextXBlocks[x])
				{
					hasSpaceUnder = false;
				}
			}

			return hasSpaceUnder;
		}

		public static bool HasSpaceLeft(Matrix field, Matrix figure, Position fPos, Matrix heap, Position hPos)
		{
			if (fPos.X == 0)
				return false;

			Matrix fieldCopy = field.Copy();

			AddOnMatrix(fieldCopy, heap, hPos);

			int fieldY = 0;
			int fieldX = 0;
			int[] fLeftBorder = figure.GetLeftBorder();
			bool[] nextYBlocks = new bool[figure.Height];

			for (int y = 0; y < figure.Height; y++)
			{
				fieldY = y + fPos.Y;
				fieldX = fPos.X - 1 + fLeftBorder[y];
				nextYBlocks[y] = !fieldCopy.Data[fieldX][fieldY];
			}

			bool hasSpaceUnder = true;

			for (int y = 0; y < nextYBlocks.Length; y++)
			{
				if (!nextYBlocks[y])
				{
					hasSpaceUnder = false;
				}
			}

			return hasSpaceUnder;
		}

		public static bool HasSpaceRight(Matrix field, Matrix figure, Position fPos, Matrix heap, Position hPos)
		{
			if (fPos.X == field.Width - 1)
				return false;

			Matrix fieldCopy = field.Copy();

			AddOnMatrix(fieldCopy, heap, hPos);

			int fieldY = 0;
			int fieldX = 0;
			int[] fRightBorder = figure.GetRightBorder();
			bool[] nextYBlocks = new bool[figure.Height];

			for (int y = 0; y < figure.Height; y++)
			{
				fieldY = y + fPos.Y;
				fieldX = fPos.X + figure.Width - fRightBorder[y];
				nextYBlocks[y] = fieldX >= field.Width ? false : !fieldCopy.Data[fieldX][fieldY];
			}

			bool hasSpaceUnder = true;

			for (int y = 0; y < nextYBlocks.Length; y++)
			{
				if (!nextYBlocks[y])
				{
					hasSpaceUnder = false;
				}
			}

			return hasSpaceUnder;
		}

		public static bool CheckPlaceAfterRotate(Matrix field, Matrix figure, Position fPos, Matrix heap, Position hPos)
		{

			Matrix fieldCopy = field.Copy();

			AddOnMatrix(fieldCopy, heap, hPos);

			for (int x = fPos.X; x < (fPos.X + figure.Width); x++)
			{
				for (int y = fPos.Y; y < (fPos.Y + figure.Height); y++)
				{
					if (y > fieldCopy.Height - 1)
						return false;
					if (!figure.Data[x - fPos.X][y - fPos.Y])
					{
						continue;
					}
					else
					{
						if (x >= field.Width)
							return false;

						if (fieldCopy.Data[x][y])
						{
							return false;
						}
					}
				}
			}

			return true;
		}

		private static int GetRelativeWidth(Matrix m1, Position m1pos, Matrix m2, Position m2pos)
		{
			return Math.Max(m1pos.X + m1.Width, m2pos.X + m2.Width) - Math.Min(m1pos.X, m2pos.X);
		}

		private static int GetRelativeHeight(Position mTopPos, Matrix mBot, Position mBotPos)
		{
			if (mTopPos.Y >= mBotPos.Y)
			{
				return mBot.Height;
			}
			else
			{
				return (mBotPos.Y - mTopPos.Y) + mBot.Height;
			}
		}

		private static KeyValuePair<Position, Position> GetRelativePositions(Position p1, Position p2)
		{
			int x1, x2;
			int y1, y2;

			if (p1.X <= p2.X)
			{
				x1 = 0;
				x2 = p2.X - p1.X;
			}
			else
			{
				x2 = 0;
				x1 = p1.X - p2.X;
			}

			if (p1.Y <= p2.Y)
			{
				y1 = 0;
				y2 = p2.Y - p1.Y;
			}
			else
			{
				y2 = 0;
				y1 = p1.Y - p2.Y;
			}

			return KeyValuePair.Create(new Position(x1, y1), new Position(x2, y2));
		}

		public static void CheckForSize(Matrix mBase, Matrix mChild, Position pos = new Position())
		{
			if (
					mBase.Width < (mChild.Width + pos.X)
					|| mBase.Height < (mChild.Height + pos.Y)
				)
			{
				Console.WriteLine("BaseWidth {0} BaseHeight {1} ChildWidth {2} ChildHeight {3} CPosX {4} CPosY {5}               ",
					mBase.Width, mBase.Height, mChild.Width, mChild.Height, pos.X, pos.Y);
				throw new Exception("Base matrix is less than child");
			}
		}

		public static Matrix Rotate(Matrix matrix)
		{
			int m = matrix.Height;
			int n = matrix.Width;
			bool[][] rotated = new bool[matrix.Height][];

			for (int x = 0; x < matrix.Height; x++)
			{
				rotated[x] = new bool[matrix.Width];
			}

			for (int i = 0; i < n; i++)
				for (int j = 0; j < m; j++)
					rotated[j][n - 1 - i] = matrix.Data[i][j];

			return new Matrix(rotated);
		}

		public static int GetFirstFullRowIndex(Matrix matrix)
		{
			for (int y = 0; y < matrix.Height; y++)
			{
				bool isFull = true;
				for (int x = 0; x < matrix.Width; x++)
				{
					if (!matrix.Data[x][y])
						isFull = false;
				}
				if (isFull)
					return y;
			}

			return -1;
		}

		public static Matrix RemoveRowFromMatrix(Matrix matrix, int row)
		{
			Matrix reduced = new Matrix(matrix.Width, matrix.Height - 1);
			int offsetY = 0;

			for (int y = 0; y < matrix.Height; y++)
			{
				if (y == row)
				{
					offsetY++;
					continue;
				}

				for (int x = 0; x < matrix.Width; x++)
				{
					reduced.Data[x][y - offsetY] = matrix.Data[x][y];
				}
			}

			
			return reduced;
		}



	}
}
