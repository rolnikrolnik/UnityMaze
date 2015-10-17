using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using Treasure_Hunter.Enumerations;

namespace Treasure_Hunter.Mazes
{
	public class Maze : IMaze
	{
		
		private List<List<Cell>> _cells;
		private List<Point> _activeCells;
		private readonly System.Random _rng;
		private int _height;
		private int _width;
		private bool[][] mazeArray;

		public bool IsPrim { get; set; }
		
		public Maze()
		{
			_rng = new System.Random();
		}
		
		public void GenerateMaze(int heightTmp = 20, int widthTmp = 20)
		{
			_height = heightTmp/2;
			_width = widthTmp/2;
			
			_activeCells = new List<Point>();
			_cells = new List<List<Cell>>();
			for (var i = 0; i < _height; i++)
			{
				var cellRow = new List<Cell>();
				for (var j = 0; j < _width; j++)
				{
					cellRow.Add(new Cell());
				}
				_cells.Add(cellRow);
			}
			
			// random cell to start with
			var randomCell = GetRandomCell();
			_cells[randomCell.Y][randomCell.X].IsVisited = true;
			_activeCells.Add(randomCell);
			
			while (_activeCells.Count > 0)
			{
				// select cell from list, mark as visited
				var selectedCell = SelectCellFromList(this.IsPrim);
				_cells[selectedCell.Y][selectedCell.X].IsVisited = true;
				
				// select neighbor to carve into
				MazeDirections selectedDirection;
				var neighborCell = SelectNeighbour(selectedCell, out selectedDirection);
				if (neighborCell != null) //carve to the selected neighbor
				{
					// mark as visited
					_cells[neighborCell.Y][neighborCell.X].IsVisited = true;
					//carving in cells
					_cells[selectedCell.Y][selectedCell.X].DestroyWall((int) selectedDirection);
					_cells[neighborCell.Y][neighborCell.X].DestroyWall((int) GetOppositeDirection(selectedDirection));
					//adding to list of active cells
					_activeCells.Add(neighborCell);
				}
				else  //if neighborCell is null - all cells around starting cell were visited
				{
					_activeCells.Remove(selectedCell);
				}
			}

			this.CreateMazeArray();
		}

		public bool IsPointAWall(int x, int y)
		{
		    if (x < this.GetLength()
		        && x >= 0
		        && y < this.GetWidth()
		        && y >= 0)
		    {
		        return this.mazeArray[x][y];
		    }
		    else
		    {
                throw new Exception("Out of bounds exception!");
		    }
		}

	    public Vector3 GetPlayerCoords(Vector3 mazeWallScale, Vector3 mazeScale)
	    {
	        while (true)
	        {
                var randomPostion = _rng.Next(this.GetWidth());
                var playerVector = new Vector3(_rng.Next(this.GetWidth()), 1, 1);
	            if (this.IsPointAWall(0, randomPostion))
	            {
	                return Vector3.Scale(playerVector + mazeWallScale * 0.5f, mazeScale);
	            }
            }

        }

	    public int GetLength()
        {
            return mazeArray.Length;
        }

        public int GetWidth()
        {
            return mazeArray != null ? mazeArray[0].Length : 0;
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
						if(!_cells[startingY][startingX - 1].IsVisited)
							neighborCell = new Point(startingX - 1, startingY);
					}
					break;
				case MazeDirections.S:
					if (startingY < _height - 1)
					{
						if (!_cells[startingY + 1][startingX].IsVisited)
							neighborCell = new Point(startingX, startingY + 1);
					}
					break;
				case MazeDirections.E:
					if (startingX < _width - 1)
					{
						if (!_cells[startingY][startingX + 1].IsVisited)
							neighborCell = new Point(startingX + 1, startingY);
					}
					break;
				case MazeDirections.N:
					if (startingY > 0)
					{
						if (!_cells[startingY - 1][startingX].IsVisited)
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
			var mazePrintHeight = 2*_height + 1;
			var mazePrintWidth = 2*_width + 1;
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
			
			for (var i = 0; i < _cells.Count; i++)
			{
				var mazePrintRow = 2*i;
				for (var j = 0; j < _cells[i].Count; j++)
				{
					var mazePrintColumn = 2*j;
					if ((_cells[i][j].Walls & (int) MazeDirections.W) > 0)
					{
						this.mazeArray[mazePrintRow + 1][mazePrintColumn] = true;
					}
					if ((_cells[i][j].Walls & (int) MazeDirections.S) > 0)
					{
						this.mazeArray[mazePrintRow + 2][mazePrintColumn + 1] = true;
					}
					if ((_cells[i][j].Walls & (int)MazeDirections.E) > 0)
					{
						this.mazeArray[mazePrintRow + 1][mazePrintColumn + 2] = true;
					}
					if ((_cells[i][j].Walls & (int)MazeDirections.N) > 0)
					{
						this.mazeArray[mazePrintRow][mazePrintColumn + 1] = true;
					}
				}
			}
		}


		
		private Point SelectCellFromList(bool isPrim)
		{
			// random selection of cell - almost like Prim's algorithm  ||  latest cell from list - recursive backtracker
			return isPrim ? _activeCells[_rng.Next(_activeCells.Count)] : _activeCells[_activeCells.Count - 1];
		}
		
		private Point GetRandomCell()
		{
			return new Point(_rng.Next(_width), _rng.Next(_height));
		}
		
		private MazeDirections GetRandomDirection()
		{
			var values = Enum.GetValues(typeof(MazeDirections));
			return (MazeDirections)values.GetValue(_rng.Next(values.Length));
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
