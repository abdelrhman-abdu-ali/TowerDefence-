using UnityEngine;

public class RangedEnemy : BaseEnemy
{
    [SerializeField] private GameObject enemyProjectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootRange = 5f;
    private float lastShootTime;
   
    protected override void Update()
    {
        base.Update();

        if (targetTower != null)
        {
            float distanceToTower = Vector3.Distance(transform.position, targetTower.position);

            if (distanceToTower <= shootRange)
            {
                ShootAtTower();
            }
        }
    }

    private void ShootAtTower()
    {
        if (Time.time - lastShootTime >= 1f)
        {
            GameObject projectileObj = ObjectPoolManager.Instance.GetPooledObject(enemyProjectilePrefab);

            projectileObj.transform.position = shootPoint.position;
            projectileObj.transform.rotation = Quaternion.identity;

            BaseProjectile projectile = projectileObj.GetComponent<BaseProjectile>();

            Vector3 direction = (targetTower.position - shootPoint.position).normalized;

            projectile.Initialize(direction);

            lastShootTime = Time.time;
        }
    }
}