
using UnityEngine;

public abstract class BaseEnemy : BaseEntity, IMoveable
{
    [SerializeField] protected float movementSpeed = 5f;
    [SerializeField] protected float attackRange = 2f;
    [SerializeField] protected float damage = 10f;

    protected Transform targetTower;

    public float MovementSpeed => movementSpeed;

    public virtual void Move(Vector3 destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, movementSpeed * Time.deltaTime);
    }

    protected virtual void Update()
    {
        if (targetTower != null)
        {
            Move(targetTower.position);

            if (Vector3.Distance(transform.position, targetTower.position) <= attackRange)
            {
                AttackTower();
            }
        }
    }

    protected virtual void AttackTower()
    {
        IDamageable towerHealth = targetTower.GetComponent<IDamageable>();
        towerHealth?.TakeDamage(damage);
    }

    public void SetTargetTower(Transform tower)
    {
        targetTower = tower;
    }
}