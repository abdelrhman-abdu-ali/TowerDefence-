using UnityEngine;

public class BasicTower : BaseTower
{

    protected override void FindAndShootEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        float closestDistance = float.MaxValue;
        BaseEnemy closestEnemy = null;

        foreach (var hitCollider in hitColliders)
        {
            BaseEnemy enemy = hitCollider.GetComponent<BaseEnemy>();
            if (enemy != null && enemy.IsAlive)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }

        if (closestEnemy != null)
        {
            Shoot(closestEnemy.transform.position);
        }
    }

    public override void Shoot(Vector3 targetPosition)
    {
        if (Time.time - lastFireTime >= 1f / fireRate)
        {
            GameObject projectileobj = Instantiate(towerProjectilePrefab, firePoint.position, Quaternion.identity);
            BaseProjectile projectile = projectileobj.GetComponent<BaseProjectile>();
            Vector3 direction = (targetPosition - firePoint.position).normalized;

            projectile.Initialize(direction);

            lastFireTime = Time.time;
        }
    }
}
