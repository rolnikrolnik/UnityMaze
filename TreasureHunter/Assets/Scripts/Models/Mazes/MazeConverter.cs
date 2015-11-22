using System;
using System.Collections.Generic;
using System.Linq;
using Treasure_Hunter.Enumerations;
using UnityEngine;
using Random = System.Random;

namespace Treasure_Hunter.Mazes
{
    public class MazeConverter
    {
        #region Fields

        private IMaze maze;
        private Random random;
        private readonly Vector3 mazeWallScale;
        private Vector3 playersVector;

        #endregion

        public MazeConverter(IMaze maze, Vector3 mazeWallScale, int trapProbability, int monsterProbability)
        {
            this.maze = maze;
            this.random = new Random();
            this.mazeWallScale = mazeWallScale;
            this.PlayerCoords = new Vector3();
            this.TrapProbability = trapProbability;
            this.MonsterProbability = monsterProbability;

            this.ConvertMaze();
        }


        #region Properties

        public Dictionary<Vector3, MazeComponentType> MazeComponents { get; private set; }
        public Vector3 PlayerCoords { get; private set; }
        public int TrapProbability { get; set; }
        public int MonsterProbability { get; set; }

        #endregion

        #region Private methods

        private void ConvertMaze()
        {
            this.MazeComponents = new Dictionary<Vector3, MazeComponentType>();
            this.SetPlayerPosition();
            this.AddExitComponent();
            this.AddWalls();
            this.AddTraps();
            this.RemoveTrapsAroundPlayer();
            this.MakeSpaceForTrophyRoom();
        }

        private void MakeSpaceForTrophyRoom()
        {
            var exitPosition = this.MazeComponents.FirstOrDefault(pair => pair.Value == MazeComponentType.EXIT).Key;
            var exitX = exitPosition.x;
            var exitZ = exitPosition.z;
            var wallX = mazeWallScale.x/2;
            var wallZ = mazeWallScale.z/2;
            var startingX = exitX - 4 * wallX;
            var startingY = exitZ + wallZ;
            for (float i = startingX; i <= startingX + 9 * wallX; i += wallX)
            {
                for (float j = startingY ; j <= startingY + 9*wallZ; j += wallZ)
                {
                    var neighbourVector = new Vector3(i, 1 + mazeWallScale.y / 2, j);
                    if (this.MazeComponents.ContainsKey(neighbourVector))
                    {
                        if (this.MazeComponents[neighbourVector] == MazeComponentType.WALL)
                        {
                            this.MazeComponents.Remove(neighbourVector);
                            this.MazeComponents.Add(neighbourVector, MazeComponentType.FLOOR);
                        }
                    }
                }
            }
        }

        private void RemoveTrapsAroundPlayer()
        {
            var playerX = this.PlayerCoords.x;
            var playerZ = this.PlayerCoords.z;
            for (float i = playerX - 1f; i <= playerX + 1; i += mazeWallScale.x / 2)
            {
                for (float j = playerZ - 1f; j <= playerZ + 1; j += mazeWallScale.z / 2)
                {
                    var neighbourVector = new Vector3(i, 1 + mazeWallScale.y / 2, j);
                    if (this.MazeComponents.ContainsKey(neighbourVector))
                    {
                        if (this.MazeComponents[neighbourVector] == MazeComponentType.STATIONARY_TRAP
                            || this.MazeComponents[neighbourVector] == MazeComponentType.ACROSS_TRAP
                            || this.MazeComponents[neighbourVector] == MazeComponentType.DOWN_TRAP
                            || this.MazeComponents[neighbourVector] == MazeComponentType.MONSTER)
                        {
                            this.MazeComponents.Remove(neighbourVector);
                            this.MazeComponents.Add(neighbourVector, MazeComponentType.FLOOR);
                        }
                    }
                }
            }
        }

        private void AddTraps()
        {
            for (var x = 0; x < this.maze.Length; x++)
            {
                for (var y = 0; y < this.maze.Width; y++)
                {
                    var componentVector = new Vector3(x, 1, y) + mazeWallScale/2;

                    if (!this.maze.IsPointAWall(x, y)
                        && this.IsTrapPossible(componentVector))
                    {
                            this.AddTrapComponent(componentVector);
                    }
                }
            }
        }

        private void AddWalls()
        {
            for (var x = 0; x < this.maze.Length; x++)
            {
                for (var y = 0; y < this.maze.Width; y++)
                {
                    var componentVector = new Vector3(x, 1, y) + mazeWallScale/2;

                    if (this.maze.IsPointAWall(x, y))
                    {
                        this.AddWallComponent(componentVector);
                    }
                }
            }
        }

        private void SetPlayerPosition()
        {
            var playerCoords = this.maze.GetPlayerCoords();
            this.playersVector = new Vector3(playerCoords.X, 1, playerCoords.Y);
            this.PlayerCoords = this.playersVector + mazeWallScale / 2;
        }

        private void AddExitComponent()
        {
            var exitCoords = this.maze.GetExitCoords();
            var exitVector = new Vector3(exitCoords.X, 1, exitCoords.Y);
            this.MazeComponents.Add(
                exitVector + mazeWallScale / 2, 
                MazeComponentType.EXIT);
        }

        private void AddWallComponent(Vector3 componentVector)
        {
            try
            {
                this.MazeComponents.Add(componentVector, MazeComponentType.WALL);
            }
            catch (Exception ex)
            {
                Debug.Log("Tried to add wall which is exit");
            }
        }

        private bool IsTrapPossible(Vector3 componentVector)
        {
            var trapProbability = this.TrapProbability; // TODO : pass obstacle probability to Maze class
            var isTrap = random.Next(100) < trapProbability;
            if (!isTrap)
            {
                return false;
            }

            var leftCell = new Vector3(componentVector.x - 1, componentVector.y, componentVector.z);
            var rightCell = new Vector3(componentVector.x + 1, componentVector.y, componentVector.z);
            var upperCell = new Vector3(componentVector.x, componentVector.y, componentVector.z + 1);
            var lowerCell = new Vector3(componentVector.x, componentVector.y, componentVector.z - 1);

            var neighboursList = new List<Vector3> {leftCell, rightCell, upperCell, lowerCell};

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


        private void AddTrapComponent(Vector3 componentVector)
        {
            try
            {
                var trapProbability = random.Next(100);
                if (trapProbability > this.MonsterProbability)
                {
                    var trapTypeProbability = random.Next(10);
                    if (trapTypeProbability < 3
                        && this.IsDownTrapPossible(componentVector))
                    {
                        this.MazeComponents.Add(componentVector, MazeComponentType.DOWN_TRAP);
                    }
                    else if (trapTypeProbability < 7
                             && this.IsAcrossTrapPossible(componentVector))
                    {
                        this.MazeComponents.Add(componentVector, MazeComponentType.ACROSS_TRAP);
                    }
                    else
                    {
                        this.MazeComponents.Add(componentVector, MazeComponentType.STATIONARY_TRAP);
                    }
                }
                else
                {
                    this.MazeComponents.Add(componentVector, MazeComponentType.MONSTER);
                }
            }
            catch (Exception e)
            {
                Debug.Log("Dictionary error during adding trap");
            }
        }

        private bool IsDownTrapPossible(Vector3 componentVector)
        {
            var upperCell = new Vector3(componentVector.x, componentVector.y, componentVector.z + 1);
            var lowerCell = new Vector3(componentVector.x, componentVector.y, componentVector.z - 1);

            if (this.MazeComponents.ContainsKey(upperCell)
                && this.MazeComponents.ContainsKey(lowerCell))
            {
                if (this.MazeComponents[upperCell] == MazeComponentType.WALL
                    && this.MazeComponents[lowerCell] == MazeComponentType.WALL)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsAcrossTrapPossible(Vector3 componentVector)
        {
            var leftCell = new Vector3(componentVector.x - 1, componentVector.y, componentVector.z);
            var rightCell = new Vector3(componentVector.x + 1, componentVector.y, componentVector.z);

            if (this.MazeComponents.ContainsKey(leftCell)
                && this.MazeComponents.ContainsKey(rightCell))
            {
                if (this.MazeComponents[leftCell] == MazeComponentType.WALL
                    && this.MazeComponents[rightCell] == MazeComponentType.WALL)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

    }
}
