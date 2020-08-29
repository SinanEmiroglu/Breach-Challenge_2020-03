using UnityEngine;

public class Spawnable : PooledMonoBehaviour
{
    [SerializeField]
    private float returnToPoolDelay = 3f;

    private void Start()
    {
        GetComponent<Health>().OnDie += () => ReturnToPool(returnToPoolDelay);
    }
}