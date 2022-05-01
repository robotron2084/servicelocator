using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.enemyhideout.servicelocator.samples
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]private float _speed = 1.0f;
        [SerializeField]private float _boost = 10.0f;
        /// <summary>
        /// Speed of rotation in degrees.
        /// </summary>
        [SerializeField] private float _rotSpeed = 10.0f;

        private Rigidbody2D _rBod;
        
        void Awake()
        {
            _rBod = GetComponent<Rigidbody2D>();
            ServiceLocator.Register<PlayerMovement>(this);
        }

        void FixedUpdate()
        {
            _rBod.AddForce(transform.up * _speed);
            ApplyForce(KeyCode.W, _rBod, transform.up * _boost);
            ApplyRotationalForce(KeyCode.A, _rBod, _rotSpeed);
            ApplyRotationalForce(KeyCode.D, _rBod, -_rotSpeed);
        }

        private static void ApplyForce(KeyCode keyCode, Rigidbody2D rBod, Vector2 force)
        {
            if (Input.GetKey(keyCode))
            {
                rBod.AddForce(force);
            }
        }

        private static void ApplyRotationalForce(KeyCode keyCode, Rigidbody2D rBod, float forceInDegrees)
        {
            if (Input.GetKey(keyCode))
            {
                float impulse = forceInDegrees;
                rBod.AddTorque(impulse);
            }
            
        }
    }
}