using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Treasure_Hunter.Enumerations;
using UnityEngine;
using Random = System.Random;

namespace Treasure_Hunter.Mazes
{
    public class MazeConverter
    {
        private IMaze maze;
        private Random random;

        private int mazeLength;
        private int mazeWidth;
        private Vector3 mazeWallScale;

        public MazeConverter(IMaze maze, Vector3 mazeWallScale)
        {
            this.maze = maze;
            this.random = new Random();
            this.mazeLength = this.maze.Length;
            this.mazeWidth = this.maze.Width;
            this.mazeWallScale = mazeWallScale;
            this.MazeComponents = new Dictionary<Vector3, MazeComponentType>();
            this.PlayerCoords = new Vector3();

            this.ConvertMaze();
        }

        public Dictionary<Vector3, MazeComponentType> MazeComponents { get; private set; }
        public Vector3 PlayerCoords { get; private set; }

        private void ConvertMaze()
        {
            this.SetPlayerPosition();
            this.AddExitComponent();

            for (var x = 0; x < this.mazeLength; x++)
            {
                for (var y = 0; y < this.mazeWidth; y++)
                {
                    var componentVector = new Vector3(x, 1, y) + mazeWallScale / 2;

                    if (this.maze.IsPointAWall(x, y))
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

        private void SetPlayerPosition()
        {
            while (true)
            {
                var randomPostionX = random.Next(this.maze.Width);
                var playerVector = new Vector3(randomPostionX, 1, 1);
                if (!this.maze.IsPointAWall(1, randomPostionX))
                {
                    this.PlayerCoords = playerVector + mazeWallScale * 0.5f;
                    return;
                }
            }
        }

        private void AddExitComponent()
        {
            while (true)
            {
                var lastRowOfMaze = this.mazeLength - 1;
                var randomPostionX = random.Next(this.mazeWidth - 2) + 1;
                if (!this.maze.IsPointAWall(randomPostionX, lastRowOfMaze - 1))
                {
                    this.MazeComponents.Add(
                        new Vector3(randomPostionX, 1, lastRowOfMaze) + mazeWallScale / 2,
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

    }
}
