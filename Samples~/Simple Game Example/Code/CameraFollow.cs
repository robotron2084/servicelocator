using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.enemyhideout.servicelocator.samples
{
    public class CameraFollow : MonoBehaviour
    {
        private Transform playerTransform;

        private float _zOffset = 0;
        // Start is called before the first frame update
        void Start()
        {
            var playerMovement = ServiceLocator.Get<PlayerMovement>();
            playerTransform = playerMovement.transform;
            _zOffset = transform.position.z;
        }

        // Update is called once per frame
        void Update()
        {
            var newPos = playerTransform.position;
            newPos.z = _zOffset;
            transform.position = newPos;
        }
    }
}