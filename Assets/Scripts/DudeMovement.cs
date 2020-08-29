// Copyright (c) Breach AS. All rights reserved.
using UnityEngine;
using UnityEngine.AI;

namespace Breach
{
    [RequireComponent(typeof(Dude))]
    public class DudeMovement : MonoBehaviour
    {
        private const float _rotationSpeed = 3f;

        private float _speed;
        private DudeData _data;
        private Transform _transform;
        private Transform playerTransform;
        private NavMeshPath path;

        //private Rigidbody _rigidBody;

        private void OnEnable() => GetComponent<Health>().OnDie += () => enabled = false;

        private void Start()
        {
            _transform = transform;
            _data = GetComponent<Dude>().DudeData;
            _speed = _data.MoveSpeed;
            playerTransform = FindObjectOfType<PlayerMovement>().transform;
            path = new NavMeshPath();

            //_rigidBody = GetComponent<Rigidbody>();
            //_rigidBody.velocity = _data.Velocity;
        }

        private void FixedUpdate()
        {
            var targetPosition = playerTransform.position;

            bool foundPath = NavMesh.CalculatePath(_transform.position, targetPosition, NavMesh.AllAreas, path);

            if (foundPath)
            {
                Vector3 nextDestination = path.corners[1];
                Vector3 directionToTarget = nextDestination - _transform.position;
                Vector3 flatDirection = new Vector3(directionToTarget.x, 0, directionToTarget.z);

                directionToTarget = Vector3.Normalize(flatDirection);

                var desiredRotation = Quaternion.LookRotation(directionToTarget);
                var finalRotation = Quaternion.Slerp(_transform.rotation, desiredRotation, Time.deltaTime * _rotationSpeed);
                _transform.rotation = finalRotation;

                if (Vector3.Distance(_transform.position, directionToTarget) > 1f)
                    _transform.position += _speed * Time.deltaTime * directionToTarget;
            }

            UpdateDudeData();

            // move forwards
            //_rigidBody.AddForce(_transform.forward * _speed * Time.deltaTime);
            // suicide if we're way out of bounds
            //if (_transform.position.magnitude > 100f)
            //    Destroy(gameObject);
        }

        private void UpdateDudeData()
        {
            //_data.Velocity = _rigidBody.GetPointVelocity(_transform.position);
            _data.Position = _transform.position;
            _data.Rotation = _transform.rotation;
        }
    }
}