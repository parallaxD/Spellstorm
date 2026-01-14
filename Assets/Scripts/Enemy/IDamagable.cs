using UnityEngine;

public interface IDamagable
{
    void TakeDamage(int damage);
    void TakeDamage(int damage, Vector2 damageDirection);
    bool IsAlive { get; }

    GameObject gameObject { get => gameObject; }
}
