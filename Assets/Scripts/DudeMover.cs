// Copyright (c) Breach AS. All rights reserved.
using UnityEngine;

namespace Breach
{
    [RequireComponent(typeof(Dude))]
    public class DudeMover : MonoBehaviour
    {
        private float _speed;
        private DudeData _data;
        private Transform _transform;
        private Rigidbody _rigidBody;

        void Start()
        {
            _transform = transform;
            _rigidBody = GetComponent<Rigidbody>();
            _data = GetComponent<Dude>().DudeData;
            _speed = _data.MoveSpeed;
            _rigidBody.velocity = _data.Velocity;
            Debug.Log("Speed - " + _speed);
        }

        void FixedUpdate()
        {
            // move forwards
            _rigidBody.AddForce(_transform.forward * _speed * Time.deltaTime);
            _data.Velocity = _rigidBody.GetPointVelocity(_transform.position);
            _data.Position = _transform.position;
            _data.Rotation = _transform.rotation;

            // suicide if we're way out of bounds
            if (_transform.position.magnitude > 100f)
                Destroy(gameObject);
        }
    }
}