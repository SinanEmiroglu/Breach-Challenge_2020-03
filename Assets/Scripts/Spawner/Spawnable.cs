using UnityEngine;

namespace Breach
{
    public class Spawnable : PooledMonoBehaviour
    {
        [SerializeField] private float returnToPoolDelay = 3f; // Delay allow us to play a death animation if it's necessary

        private void Start()
        {
            GetComponent<Health>().OnDie += () => ReturnToPool(returnToPoolDelay);
        }
    }
}