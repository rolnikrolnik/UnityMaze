using UnityEngine;
using System;
using System.Collections.Generic;
using Treasure_Hunter.Enumerations;

namespace Treasure_Hunter.Mazes
{
	public class Maze : IMaze
	{
		
		private List<List<Cell>> cells;
		private List<Point> activeCells;
		private readonly System.Random random;
		private int height;
		private int width;
		private bool[][] mazeArray;

		public bool IsPrim { get; set; }
        public int Length { get { return mazeArray.Length;  } }
        public int Width { get { return mazeArray[0].Length; }}
        public Dictionary<Vector3, MazeComponentType> MazeComponents { get; private set; }  
		
		public Maze()
		{
			random = new System.Random();
		    this.IsPrim = false;
		}
		
		public void GenerateMaze(int heightTmp = 20, int widthTmp = 20)
		{
			height = heightTmp/2;
			width = widthTmp/2;
			
			activeCells = new List<Point>();
			cells = new List<List<Cell>>();
			for (var i = 0; i < height; i++)
			{
				var cellRow = new List<Cell>();
				for (var j = 0; j < width; j++)
				{
					cellRow.Add(new Cell());
				}
				cells.Add(cellRow);
			}
			
			// random cell to start with
			var randomCell = GetRandomCell();
			cells[randomCell.Y][randomCell.X].IsVisited = true;
			activeCells.Add(randomCell);
			
			while (activeCells.Count > 0)
			{
				// select cell from list, mark as visited
				var selectedCell = SelectCellFromList(this.IsPrim);
				cells[selectedCell.Y][selectedCell.X].IsVisited = true;
				
				// select neighbor to carve into
				MazeDirections selectedDirection;
				var neighborCell = SelectNeighbour(selectedCell, out selectedDirection);
				if (neighborCell != null) //carve to the selected neighbor
				{
					// mark as visited
					cells[neighborCell.Y][neighborCell.X].IsVisited = true;
					//carving in cells
					cells[selectedCell.Y][selectedCell.X].DestroyWall((int) selectedDirection);
					cells[neighborCell.Y][neighborCell.X].DestroyWall((int) GetOppositeDirection(selectedDirection));
					//adding to list of active cells
					activeCells.Add(neighborCell);
				}
				else  //if neighborCell is null - all cells around starting cell were visited
				{
					activeCells.Remove(selectedCell);
				}
			}

			this.CreateMazeArray();
		}

	    public Point GetExitCoords()
	    {
            while (true)
            {
                var lastRowOfMaze = this.Length - 1;
                var randomPostionX = random.Next(this.Width - 2) + 1;
                if (!this.IsPointAWall(randomPostionX, lastRowOfMaze - 1))
                {
                    return new Point(randomPostionX, lastRowOfMaze);
                }
            }
        }

	    public Point GetPlayerCoords()
	    {
            while (true)
            {
                var randomPostionX = random.Next(this.Width);
                if (!this.IsPointAWall(1, randomPostionX))
                {
                    return new Point(randomPostionX, 1);
                }
            }
        }

	    public bool IsPointAWall(int x, int y)
		{
		    if (x < this.Length
		        && x >= 0
		        && y < this.Width
		        && y >= 0)
		    {
		        return this.mazeArray[x][y];
		    }
		    else
		    {
                throw new Exception("Out of bounds exception!");
		    }
		}

        #region Private methods

        private Point SelectNeighbour(Point startingCell, out MazeDirections selectedDirection)
		{
			Point neighborCell = null;
			var startingX = startingCell.X;
			var startingY = startingCell.Y;
			var directionsCounter = 0;
			selectedDirection = MazeDirections.None;
			
			while (true)
			{
				var randomDirection = GetRandomDirection();
				switch (randomDirection)
				{
				case MazeDirections.W:
					if (startingX > 0)
					{
						if(!cells[startingY][startingX - 1].IsVisited)
							neighborCell = new Point(startingX - 1, startingY);
					}
					break;
				case MazeDirections.S:
					if (startingY < height - 1)
					{
						if (!cells[startingY + 1][startingX].IsVisited)
							neighborCell = new Point(startingX, startingY + 1);
					}
					break;
				case MazeDirections.E:
					if (startingX < width - 1)
					{
						if (!cells[startingY][startingX + 1].IsVisited)
							neighborCell = new Point(startingX + 1, startingY);
					}
					break;
				case MazeDirections.N:
					if (startingY > 0)
					{
						if (!cells[startingY - 1][startingX].IsVisited)
							neighborCell = new Point(startingX, startingY - 1);
					}
					break;
				}
				
				if (neighborCell == null)
				{
					directionsCounter |= (int)randomDirection;
					if (directionsCounter ==
					    (int)MazeDirections.W +
					    (int)MazeDirections.E +
					    (int)MazeDirections.S +
					    (int)MazeDirections.N)
						break;
				}
				else
				{
					selectedDirection = randomDirection;
					break;
				}
			}
			return neighborCell;
		}
		
		private void CreateMazeArray()
		{
			var mazePrintHeight = 2*height + 1;
			var mazePrintWidth = 2*width + 1;
			this.mazeArray = new bool[mazePrintHeight][];
			for (var i = 0; i < mazePrintHeight; i++)
			{
				this.mazeArray[i] = new bool[mazePrintWidth];
			}
			
			for (var i = 0; i < mazePrintHeight; i++)
			{
				for (var j = 0; j < mazePrintWidth; j++)
				{
					if (j%2 == 0 && i%2 == 0)
						this.mazeArray[i][j] = true;
					else
						this.mazeArray[i][j] = false;
				}
			}
			
			// fill out maze with walls
			//# N #
			//W   E 
			//# S #
			
			for (var i = 0; i < cells.Count; i++)
			{
				var mazePrintRow = 2*i;
				for (var j = 0; j < cells[i].Count; j++)
				{
					var mazePrintColumn = 2*j;
					if ((cells[i][j].Walls & (int) MazeDirections.W) > 0)
					{
						this.mazeArray[mazePrintRow + 1][mazePrintColumn] = true;
					}
					if ((cells[i][j].Walls & (int) MazeDirections.S) > 0)
					{
						this.mazeArray[mazePrintRow + 2][mazePrintColumn + 1] = true;
					}
					if ((cells[i][j].Walls & (int)MazeDirections.E) > 0)
					{
						this.mazeArray[mazePrintRow + 1][mazePrintColumn + 2] = true;
					}
					if ((cells[i][j].Walls & (int)MazeDirections.N) > 0)
					{
						this.mazeArray[mazePrintRow][mazePrintColumn + 1] = true;
					}
				}
			}
		}

		private Point SelectCellFromList(bool isPrim)
		{
			// random selection of cell - almost like Prim's algorithm  ||  latest cell from list - recursive backtracker
			return isPrim ? activeCells[random.Next(activeCells.Count)] : activeCells[activeCells.Count - 1];
		}
		
		private Point GetRandomCell()
		{
			return new Point(random.Next(width), random.Next(height));
		}
		
		private MazeDirections GetRandomDirection()
		{
			var values = Enum.GetValues(typeof(MazeDirections));
			return (MazeDirections)values.GetValue(random.Next(values.Length));
		}
		
		private static MazeDirections GetOppositeDirection(MazeDirections mazeDirection)
		{
			switch (mazeDirection)
			{
			case MazeDirections.E:
				return MazeDirections.W;
			case MazeDirections.N:
				return MazeDirections.S;
			case MazeDirections.S:
				return MazeDirections.N;
			case MazeDirections.W:
				return MazeDirections.E;
			default:
				return MazeDirections.None;
			}
		}

	#endregion		
	}
}
