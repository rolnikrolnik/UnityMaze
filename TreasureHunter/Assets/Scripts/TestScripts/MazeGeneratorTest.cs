using UnityEngine;
using System.Collections;
using Treasure_Hunter.Managers;
using Treasure_Hunter.Controllers;

namespace Treasure_Hunter.TestScripts
{
    public class MazeGeneratorTest : MonoBehaviour
    {

        public MazeManager MazeManager;
        public PlayerController Player;
        void Start()
        {
            StartCoroutine(MazeManager.Activate());
            StartCoroutine(MazeManager.Init());
            Player.ChController.enabled = true;
            Player.Animator.enabled = true;
            Player.IsEnabled = true;
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                foreach (var wall in GameObject.FindGameObjectsWithTag("MazeComponent"))
                {
                    Destroy(wall);
                }
                StartCoroutine(MazeManager.GenerateMaze(MazeManager.MazeType));
            }
        }
    }
}