using UnityEngine;

public interface IKnockbackResistance
{
    void ApplyKnockback(Vector2 forceDirection, float strengthResist,float knockbackTimer);
}
