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

        public CellularAutomata()
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
                this.SmoothMap(isFirstSmoothing: true);
            }

            for (var i = 0; i < this.Turns - 1; i++)
            {
                this.SmoothMap(isFirstSmoothing: false);
            }

            if (!this.IsMapGeneratedCorrectly())
            {
                this.GenerateMaze(heightTmp, widthTmp);
            }
        }



        public Point GetExitCoords()
        {
            var foundEmptyRow = false;
            var firstEmptyRow = this.Length - 1;
            for (; firstEmptyRow > 0; firstEmptyRow--)
            {
                for (int i = 0; i < this.Width; i++)
                {
                    if (!IsPointAWall(firstEmptyRow, i))
                    {
                        return new Point(i, firstEmptyRow);
                    }
                }
            }

            Debug.LogError("EXIT NOT FOUND");
            return new Point(0, 0);
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
                    return new Point(randomPostionX, firstEmptyRow);
                }
            }
        }

        public bool IsPointAWall(int x, int y)
        {
            return this._map[x][y];
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

        private int GetNeighboursCount(int cellX, int cellY, int steps)
        {
            var count = 0;
            for (var i = cellY - steps; i <= cellY + steps; i++)
            {
                for (var j = cellX - steps; j <= cellX + steps; j++)
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

        private void SmoothMap(bool isFirstSmoothing)
        {
            for (var i = 0; i < this._height; i++)
            {
                for (var j = 0; j < this._width; j++)
                {
                    if (isFirstSmoothing)
                    {
                        var oneStepNeighbour = this.GetNeighboursCount(i, j, 1);
                        var twoStepsNieghbours = this.GetNeighboursCount(i, j, 2);
                        if (oneStepNeighbour >= 5 || twoStepsNieghbours <= 2)
                        {
                            _map[i][j] = true;
                        }
                        else
                        {
                            _map[i][j] = false;
                        }
                    }
                    else
                    {
                        var oneStepNeighbour = this.GetNeighboursCount(i, j, 1);
                        if (oneStepNeighbour >= 5)
                        {
                            _map[i][j] = true;
                        }
                        else
                        {
                            _map[i][j] = false;
                        }
                    }
                }
            }
        }

        private bool IsMapGeneratedCorrectly()
        {
            return IsFillPercentageHigherThan(0.4f);
        }

        private bool IsFillPercentageHigherThan(float fillPercentageLevel)
        {
            var allCells = 0;
            var emptyCells = 0;
            for (int i = 0; i < _map.Length; i++)
            {
                for (int j = 0; j < _map[0].Length; j++)
                {
                    if (!_map[i][j])
                    {
                        emptyCells++;
                    }

                    allCells++;
                }
            }

            var score = (float)emptyCells / (float)allCells;
            return score > fillPercentageLevel;
        }

        #endregion
    }
}
