using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    public int EnemyFacingDirection {  get; private set; }
    public bool IsAggressive { get; private set; }
    public float EnemyHealth = 100f;

    private Rigidbody2D rb;
    private float currentHealth;
    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        
    }
    void Start()
    {
        IsAggressive = false;
        EnemyFacingDirection = 1;
        currentHealth = EnemyHealth;
    }
    void Update() 
    {
        if (currentHealth <= 0f) 
        {
            Die();
        }
    }
    void FixedUpdate() 
    {
        rb.AddForce(Vector2.up * -9.8f, ForceMode2D.Force);
    }
    public void TakeDamage(float damage, bool IsDaggerCharged) 
    {
        if (IsDaggerCharged)
        {
            currentHealth -= damage * 4;
        } 
        else 
        {
            currentHealth -= damage;
        }
    }
    private void Die() 
    {
        GameObject.Destroy(this.gameObject);
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
    public void SetAggresive(bool isAgressive) 
    {
        if (isAgressive) 
        {
            IsAggressive = true;
        }
        else 
        {
            IsAggressive = false;
        }
    }
}
