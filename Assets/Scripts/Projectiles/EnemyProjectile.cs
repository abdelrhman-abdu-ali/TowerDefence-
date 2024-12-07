
using TMPro;
using UnityEngine;

public class EnemyProjectile : BaseProjectile
{
    [SerializeField] private float slowEffect = 0.5f;
   
    protected override void OnCollisionEnter(Collision collision)
    {
        BaseTower tower = collision.gameObject.GetComponent<BaseTower>();
        if (tower != null)
        {
            tower.TakeDamage(damage);

        }

        Destroy(gameObject);
    }
}