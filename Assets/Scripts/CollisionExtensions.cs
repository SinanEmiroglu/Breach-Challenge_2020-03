using UnityEngine;

public static class CollisionExtensions
{
    public static bool WasTop(this Collision collision)
    {
        return collision.contacts[0].normal.y < -0.5f;
    }

    public static bool WasSide(this Collision collision)
    {
        return collision.contacts[0].normal.x < -0.5f ||
               collision.contacts[0].normal.x > 0.5f;
    }
}