using Treasure_Hunter.Controllers;
using Treasure_Hunter.Managers;
using UnityEngine;

namespace Treasure_Hunter.TestScripts
{
    public class MazeTestHelper : MonoBehaviour
    {
        #region SCENE REFERENCES

        public LineRenderer Line;
        public PlayerController Player;
        public MazeManager MazeManager;

        #endregion

        #region MONO BEHAVIOUR

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Keypad5))
            {
                ShowLine();
            }
            else if(Input.GetKeyDown(KeyCode.Keypad6))
            {
                Line.enabled = false;
            }
        }

        #endregion

        private void ShowLine()
        {
            Line.enabled = true;
            Vector3 terrainPosition = GameObject.Find(MazeManager.TerrainName).transform.position;
            Vector3 startPosition = new Vector3(Player.transform.position.x, 5, Player.transform.position.z);
            Vector3 exitPosition = MazeManager.exitComponent.transform.position;
            startPosition.y = terrainPosition.y;
            exitPosition.y = terrainPosition.y;
            NavMeshPath navMeshPath = new NavMeshPath();
            NavMesh.CalculatePath(startPosition, exitPosition, NavMesh.AllAreas, navMeshPath);
            if (navMeshPath.status == NavMeshPathStatus.PathComplete)
            {
                Line.SetVertexCount(navMeshPath.corners.Length);
                for (int i = 0; i < navMeshPath.corners.Length; i++)
                {
                    Line.SetPosition(i, navMeshPath.corners[i]);
                }
             }
        }
    }
}
