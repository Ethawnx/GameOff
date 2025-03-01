using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    public float rotationSpeed = 30f; // Speed of rotation in degrees per second
    public float waitTime = 2f;
    public GameObject BulletPrefab;
    public Transform LaserGun;
    public float FireRate = 1f;
    public float BulletSpeed = 5f;

    private float nextFireRate;

    Player Target;

    private void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    public void Shoot()
    {
        Vector2 targetDir = Target.transform.position - transform.position;
        targetDir.Normalize();

        Vector2 gunTip = (Vector2)transform.position - new Vector2(0f, 0.4f);
        if (Time.time >= nextFireRate)
        {
            nextFireRate = Time.time + 1f / FireRate;
            GameObject bullet = Instantiate(BulletPrefab, gunTip, Quaternion.identity);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bulletRB.linearVelocity = BulletSpeed * Time.fixedDeltaTime * targetDir;
        }
    }
}
