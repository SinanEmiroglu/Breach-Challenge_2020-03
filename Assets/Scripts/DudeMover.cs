// Copyright (c) Breach AS. All rights reserved.
using UnityEngine;

namespace Breach
{
    public class DudeMover : MonoBehaviour
    {
        private float speed;
        private Rigidbody rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            speed = Random.Range(29, 31);
        }

        void FixedUpdate()
        {
            // move forwards
            rb.AddForce(transform.forward * speed * Time.deltaTime);

            // suicide if we're way out of bounds
            if (transform.position.magnitude > 100f)
                Destroy(gameObject);
        }
    }
}