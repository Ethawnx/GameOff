using UnityEngine;
public class EnemyProjectile : MonoBehaviour
{
    Rigidbody2D rb;
    Transform target;
    Transform gunTip;
    Player player;

    public float ProjectileSpeed;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    public void FixedUpdate()
    {
        if (gunTip != null)
        {
            Vector2 dir = target.position - gunTip.position;
            dir.Normalize();
            rb.AddForce(dir * ProjectileSpeed, ForceMode2D.Impulse);
        }
        Destroy(this.gameObject, 3f);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !player.IsInvulnerable) 
        {
            //Destroy(target.gameObject);
            player.Die();
            Destroy(this.gameObject);
        }
    }
    public void SetGunTransform(Transform gun) 
    {
        if (gun != null) 
        {
            gunTip = gun;
        }
    }
}
