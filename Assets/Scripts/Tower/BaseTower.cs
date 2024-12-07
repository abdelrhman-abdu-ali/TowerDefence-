using UnityEngine;

public abstract class BaseTower : BaseEntity, IShootable
{
    [SerializeField] protected float fireRate = 1f;
    [SerializeField] protected float range = 10f;
    [SerializeField] protected Transform firePoint;

    public float FireRate => fireRate;
    public float Range => range;

    protected float lastFireTime;
    [SerializeField] protected GameObject towerProjectilePrefab;

    public virtual void Shoot(Vector3 targetPosition)
    {
        if (Time.time - lastFireTime >= 1f / fireRate)
        {
            GameObject projectileObj = ObjectPoolManager.Instance.GetPooledObject(towerProjectilePrefab);

            projectileObj.transform.position = firePoint.position;
            projectileObj.transform.rotation = Quaternion.identity;

            BaseProjectile projectile = projectileObj.GetComponent<BaseProjectile>();

            Vector3 direction = (targetPosition - firePoint.position).normalized;

            projectile.Initialize(direction);

            lastFireTime = Time.time;
        }
    }

    protected virtual void Update()
    {
        FindAndShootEnemy();
    }

    protected abstract void FindAndShootEnemy();
}

