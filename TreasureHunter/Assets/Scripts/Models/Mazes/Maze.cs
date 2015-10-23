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
		private readonly System.Random random;
		private int _height;
		private int _width;
		private bool[][] mazeArray;

		public bool IsPrim { get; set; }
        public int Length { get { return mazeArray.Length;  } }
        public int Width { get { return mazeArray[0].Length; }}
        public Dictionary<Vector3, MazeComponentType> MazeComponents { get; private set; }  
        public Vector3 MazeWallScale { get; set; }
		
		public Maze(Vector3 mazeWallScale)
		{
			random = new System.Random();
            this.MazeComponents = new Dictionary<Vector3, MazeComponentType>();
		    this.MazeWallScale = mazeWallScale;
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

		    this.FillVectorCollections();
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

	    public Vector3 GetPlayerCoords()
	    {
	        while (true)
	        {
                var randomPostionX = random.Next(this.Width);
                var playerVector = new Vector3(randomPostionX, 1, 1);
	            if (!this.IsPointAWall(1, randomPostionX))
	            {
	                return playerVector + this.MazeWallScale*0.5f;
	            }
            }
        }

        #region Private methods

        #region Vector3 prepare methods

        // TODO : refactor and put in another class

        private void FillVectorCollections()
        {
            this.AddExitComponent();

            for (var x = 0; x < this.Length; x++)
            {
                for (var y = 0; y < this.Width; y++)
                {
                    var componentVector = new Vector3(x, 1, y) + MazeWallScale / 2;

                    if (this.IsPointAWall(x, y))
                    {
                        this.AddWallComponents(componentVector);
                    }
                    else
                    {
                        this.AddOtherComponents(componentVector);
                    }
                }
            }
        }

	    private void AddExitComponent()
        {
            while (true)
            {
                var lastRowOfMaze = this.Length - 1;
                var randomPostionX = random.Next(this.Width - 2) + 1;
                if (!IsPointAWall(randomPostionX, lastRowOfMaze - 1))
                {
                    this.MazeComponents.Add(
                        new Vector3(randomPostionX, 1, lastRowOfMaze) + MazeWallScale / 2, 
                        MazeComponentType.EXIT);
                    return;
                }
            }
        }

	    private void AddWallComponents(Vector3 componentVector)
	    {
            try
            {
                this.MazeComponents.Add(componentVector, MazeComponentType.WALL);
            }
            catch (Exception ex)
            {
                Debug.Log("Tried to add wall which is exit");
                Debug.Log(ex);
            }
        }

	    private void AddOtherComponents(Vector3 componentVector)
	    {
	        if (this.IsTrapPossible(componentVector))
	        {
                var obstacleProbability = 2; // TODO : pass obstacle probability to Maze class
                var isTrap = random.Next(10) < obstacleProbability;

                if (isTrap)
                {
                    this.AddObstaclesComponents(componentVector);
                }
                else
                {
                    this.MazeComponents.Add(componentVector, MazeComponentType.FLOOR);
                }
            }
	        else
	        {
                this.MazeComponents.Add(componentVector, MazeComponentType.FLOOR);
            }

        }

        private bool IsTrapPossible(Vector3 componentVector)
        {
            var leftCell = new Vector3(componentVector.x - 1, componentVector.y, componentVector.z);
            var rightCell = new Vector3(componentVector.x + 1, componentVector.y, componentVector.z);
            var upperCell = new Vector3(componentVector.x, componentVector.y, componentVector.z + 1);
            var lowerCell = new Vector3(componentVector.x, componentVector.y, componentVector.z - 1);

            var neighboursList = new List<Vector3> { leftCell, rightCell, upperCell, lowerCell };

            foreach (var neighbour in neighboursList)
            {
                if (!this.MazeComponents.ContainsKey(neighbour))
                {
                    continue;
                }

                if (this.MazeComponents[neighbour] == MazeComponentType.STATIONARY_TRAP
                    || this.MazeComponents[neighbour] == MazeComponentType.ACROSS_TRAP
                    || this.MazeComponents[neighbour] == MazeComponentType.DOWN_TRAP
                    || this.MazeComponents[neighbour] == MazeComponentType.MONSTER)
                {
                    return false;
                }
            }
            return true;
        }

        private void AddObstaclesComponents(Vector3 componentVector)
	    {
            // TODO : write check for across/down traps
	        var trapProbability = random.Next(10);
            if (trapProbability < 5)
            {
                var trapTypeProbability = random.Next(10);
                if (trapTypeProbability < 3)
                {
                    this.MazeComponents.Add(componentVector, MazeComponentType.STATIONARY_TRAP);
                }
                else if (trapTypeProbability < 7)
                {
                    this.MazeComponents.Add(componentVector, MazeComponentType.ACROSS_TRAP);
                }
                else
                {
                    this.MazeComponents.Add(componentVector, MazeComponentType.DOWN_TRAP);
                }
            }
	        else
	        {
                this.MazeComponents.Add(componentVector, MazeComponentType.MONSTER);
            }
	    }

        #endregion

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
			return isPrim ? _activeCells[random.Next(_activeCells.Count)] : _activeCells[_activeCells.Count - 1];
		}
		
		private Point GetRandomCell()
		{
			return new Point(random.Next(_width), random.Next(_height));
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
