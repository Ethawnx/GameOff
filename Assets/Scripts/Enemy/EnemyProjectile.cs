using UnityEngine;
public class EnemyProjectile : MonoBehaviour
{
    Rigidbody2D rb;
    Transform target;
    Transform gunTip;
    public float ProjectileSpeed;
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void FixedUpdate()
    {
        Vector2 dir = target.position - gunTip.position;
        dir.Normalize();
        rb.AddForce(dir * ProjectileSpeed);
        Destroy(this.gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")) 
        {
            Destroy(target.gameObject);
            Destroy(this);
        }
        else if (col.CompareTag("Ground")) 
        {
            Destroy(this);
        }
    }
    public void SetGunTransform(Transform gun) 
    {
        if (gun != null) 
        {
            gunTip = gun;
        }
    }
    public void SetTargetTransfrom(Transform target) 
    {
        if (target != null) 
        {
            this.target = target;
        }
    }
}
