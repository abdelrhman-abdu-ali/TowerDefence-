using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float speed = 20f;
    [SerializeField] protected float lifetime = 5f;

    protected Rigidbody rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void Initialize(Vector3 direction)
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero; // Reset velocity
            rb.AddForce(direction.normalized * speed, ForceMode.Impulse);
        }

        Invoke("ReturnToPool", lifetime);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }

        ReturnToPool();
    }

    protected virtual void ReturnToPool()
    {
        PooledObject pooledObject = GetComponent<PooledObject>();
        if (pooledObject != null)
        {
            pooledObject.ReturnToPool();
        }
    }
}