using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int EnemyFacingDirection {  get; private set; }

    private Rigidbody2D rb;

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        EnemyFacingDirection = 1;
    }
    void FixedUpdate() 
    {
        rb.AddForce(Vector2.up * -9.8f, ForceMode2D.Force);
    }
    public int GetFacingDirection() 
    {
        if (transform.rotation.y == 0f) 
        {
            return 1;
        }
        else 
        {
            return -1;
        }
    }
}
