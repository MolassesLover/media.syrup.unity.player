// Copyright © 2022, Maeve "Molasses" Garside
// Licensed under the MIT license, making this script copyleft.
// Check the LICENSE.md file for further information.

using UnityEngine;

namespace SyrupPlayer
{
    public class WallRunning : MonoBehaviour
    {
        public LayerMask wallRunLayer;
        public float wallCheckDistance = 0.95f;
    
        [HideInInspector]
        public RaycastHit leftWallHit;
        [HideInInspector]
        public RaycastHit rightWallHit;
        [HideInInspector]
        public bool wallRight;
        [HideInInspector]
        public bool wallLeft;
        public Transform playerOrientation;
        [HideInInspector]
        public Vector3 wallNormal;
        public bool gluedToWall = false;


        // Update() is called once per frame.
        public void Update() {
            wallRight = Physics.Raycast(transform.position, playerOrientation.right, out rightWallHit, wallCheckDistance, wallRunLayer);
            wallLeft = Physics.Raycast(transform.position, -playerOrientation.right, out leftWallHit, wallCheckDistance, wallRunLayer);
        
            Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
            // ↑ This is the same as:
            /*
            if(wallRight) { 
                wallNormal = rightWallHit.normal 
            } else {
                wallNormal = leftWallHit.normal 
            }
            */
        }
    }
}