using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Treasure_Hunter.Enumerations;

namespace Treasure_Hunter.Mazes
{
    
	public class CellularAutomata : MonoBehaviour, IMaze
	{
		private bool[][] _map;
		private readonly System.Random _rng;
		private int _height;
		private int _width;

		public CellularAutomata ()
		{
			_rng = new System.Random((int)DateTime.Now.Ticks);

			this.RandomFillPercentage = 40;
			this.Turns = 4;
		}

		public int RandomFillPercentage { get; set; }

		public int Turns { get; set; }

	    public int Length { get { return _map.Length; } }

	    public int Width { get { return _map[0].Length; } }

        public Dictionary<Vector3, MazeComponentType> MazeComponents { get; private set; }

	    public void GenerateMaze(int heightTmp = 50, int widthTmp = 50)
		{
			this._height = heightTmp;
			this._width = widthTmp;
			
			_map = new bool[this._height][];
			for (var i = 0; i < this._height; i++)
			{
				_map[i] = new bool[this._width];
			}

			this.RandomMapFill();
			for (var i = 0; i < this.Turns; i++)
			{
				this.SmoothMap();
			}
		}

	    public Point GetExitCoords()
	    {
	        var foundEmptyRow = false;
	        var firstEmptyRow = this.Length - 1;
	        for (; firstEmptyRow > 0; firstEmptyRow--)
	        {
	            if (foundEmptyRow)
	            {
	                break;
	            }

                for (int i = 0; i < this.Width; i++)
	            {
	                if (!IsPointAWall(firstEmptyRow, i))
	                {
	                    foundEmptyRow = true;
	                    break;
	                }
	            }
	        }

            while (true)
            {
                var randomPostionX = _rng.Next(this.Width - 2) + 1;
                if (!this.IsPointAWall(randomPostionX, firstEmptyRow - 1))
                {
                    Debug.Log(string.Format("EXIT = x = {0}, y ={1}", randomPostionX, firstEmptyRow));
                    return new Point(randomPostionX, firstEmptyRow);
                }
            }
        }

	    public Point GetPlayerCoords()
	    {
            var foundEmptyRow = false;
            var firstEmptyRow = 0;
            for (; firstEmptyRow < this.Length; firstEmptyRow++)
            {
                if (foundEmptyRow)
                {
                    break;
                }

                for (int i = 0; i < this.Width; i++)
                {
                    if (!IsPointAWall(firstEmptyRow, i))
                    {
                        foundEmptyRow = true;
                        break;
                    }
                }
            }

            while (true)
            {
                var randomPostionX = _rng.Next(this.Width);
                if (!this.IsPointAWall(firstEmptyRow, randomPostionX))
                {
                    Debug.Log(string.Format("PLAYER = x = {0}, y ={1}", randomPostionX, firstEmptyRow));
                    return new Point(randomPostionX, firstEmptyRow);
                }
            }
        }

	    public bool IsPointAWall(int x, int y)
		{
            //Debug.Log(string.Format("x = {0}, y ={1}", x, y));
			return this._map [x] [y];
		}

        #region Private methods

        private void RandomMapFill()
		{
			for (var i = 0; i < this._height; i++)
			{
				for (var j = 0; j < this._width; j++)
				{
					if (i == 0 || j == 0 || i == _height - 1 || j == _width - 1)
						_map[i][j] = true;
					else
						_map[i][j] = (_rng.Next(0, 100) < this.RandomFillPercentage);
				}
			}
		}
		
		private int GetNeighboursCount(int cellX, int cellY)
		{
			var count = 0;
			for (var i = cellY - 1; i <= cellY + 1; i++)
			{
				for (var j = cellX - 1; j <= cellX + 1; j++)
				{
					if (i >= 0 && j >= 0 && i < this._height && j < this._width)
					{
						if ((i != cellX || j != cellY) && _map[i][j]) count++;
					}
					else
					{
						count++;
					}
				}
			}
			return count;
		}
		
		private void SmoothMap()
		{
			for (var i = 0; i < this._height; i++)
			{
				for (var j = 0; j < this._width; j++)
				{
					var neighbourCount = this.GetNeighboursCount(i, j);
					if (neighbourCount > 4)
					{
						_map[i][j] = true;
					}
					else if (neighbourCount < 4)
					{
						_map[i][j] = false;
					}
				}
			}
		}

        #endregion
    }
}
