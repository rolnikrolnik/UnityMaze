using System;

namespace MapGenerator
{
    class CellularAutomata
    {
        private readonly bool[][] _map;
        private readonly Random _rng;
        private readonly int _height;
        private readonly int _width;
        private readonly int _randomFillPercentage;

        public CellularAutomata(int heightTmp = 50, int widthTmp = 50, int randomFillPercentage = 40)
        {
            this._height = heightTmp;
            this._width = widthTmp;
            this._randomFillPercentage = randomFillPercentage;
            this._rng = new Random();

            _map = new bool[this._height][];
            for (var i = 0; i < this._height; i++)
            {
                _map[i] = new bool[this._width];
            }
        }

        public void GenerateCave(int turns = 4)
        {
            this.RandomMapFill();
            for (var i = 0; i < turns; i++)
            {
                this.SmoothMap();
            }
        }

        public void PrintCave()
        {
            for (var i = 0; i < this._height; i++)
            {
                for (var j = 0; j < this._width; j++)
                {
                    Console.Write(_map[i][j] ? '#' : ' ');
                }
                Console.WriteLine();
            }
        }

        private void RandomMapFill()
        {
            for (var i = 0; i < this._height; i++)
            {
                for (var j = 0; j < this._width; j++)
                {
                    if (i == 0 || j == 0 || i == _height - 1 || j == _width - 1)
                        _map[i][j] = true;
                    else
                        _map[i][j] = (_rng.Next(0, 100) < _randomFillPercentage);
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

    }
}
