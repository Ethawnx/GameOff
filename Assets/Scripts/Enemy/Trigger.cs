using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class Trigger : MonoBehaviour
{

    public float radius = 1f;
    [Range(0,1)]
    public float angThreshold = 0.5f;

    [SerializeField]
    private PlayerData playerData;

    private Transform target;
    private Enemy enemy;
    [SerializeField]
    private Light2D flashlight;
    private float detectedTime;
    private int facingDirection;
    private float _timer;
    private Transform enemyGun;
    void Start() 
    {
        _timer = 5f;
        target = GameObject.FindWithTag("Player").transform;
        enemy = GetComponentInParent<Enemy>();
        enemyGun = GetComponentInParent<Light2D>().transform;
        //flashlight = GetComponent<Light2D>();
    }
    private void Update()
    {
        //Debug.Log(IsPlayerInLineOfSight());
        
        facingDirection = enemy.GetFacingDirection();
        //Debug.Log(_timer);
        flashlight.color = enemy.IsAggressive? Color.red: Color.white;
        if (IsPlayerInLineOfSight() && enemy.IsAggressive)
        {
            _timer = 5f;
            AimAtTarget();
        }
        else if (!IsPlayerInLineOfSight() && enemy.IsAggressive) 
        {
            _timer -= Time.deltaTime;
        }
        if (!IsPlayerInLineOfSight() && _timer < 0) 
        {
            ResetAim();
            enemy.SetAggresive(false);
        }
        //Debug.Log(enemy.IsAggressive);
        //Debug.Log("IsPlayerDetected " + IsPlayerDetected());
        //Debug.Log("Contains " + Contains(target.position));
        //Debug.Log("LOS " + IsPlayerInLineOfSight());
        //Debug.Log(target.name);
    }
    private void OnDrawGizmos()
    {
        target = GameObject.FindWithTag("Player").transform;
        //Gizmos.color = IsPlayerDetected() ? Color.white : Color.red;

        Vector2 origin = transform.position;

        Gizmos.DrawWireSphere(origin, radius);

        float p = angThreshold;
        float x = Mathf.Sqrt(1 - p * p );

        Vector2 vTop = transform.right * p + transform.up * x;
        Vector2 vBot = transform.right * p + transform.up * (-x);
        Gizmos.DrawRay(origin, vTop * radius);
        Gizmos.DrawRay(origin, vBot * radius);
    }

    public bool Contains(Vector2 position) 
    {
        Vector2 vecToTargetWorld = (target.transform.position - transform.position).normalized;
        
        if (Vector2.Distance(position, transform.position) > radius)
            return false;
        if ((facingDirection * vecToTargetWorld.x) < angThreshold)
            return false;

        else return true;
    }
    public bool IsPlayerInLineOfSight()
    {
        //_timer = 5f;
        Vector2 vecToTargetWorld = (target.transform.position - transform.position).normalized;
        Vector2 offset = new Vector2(0.3f * facingDirection, 0f);
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + offset, vecToTargetWorld);
        Debug.DrawRay((Vector2)transform.position + offset * facingDirection, vecToTargetWorld * radius, Color.blue);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            //Debug.Log(hit.collider.tag);
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsPlayerDetected() 
    {
        detectedTime += Time.deltaTime;
        if (Contains(target.transform.position) && IsPlayerInLineOfSight()) 
        {
            if (detectedTime > 0.3f) 
            {
                flashlight.color = Color.red;
                enemy.SetAggresive(true);
                return true;
            }
            else 
            {
                return false;
            }
        }
        else 
        {
            detectedTime = 0f;
            flashlight.color = Color.white;
            return false;
        }
    }
    private void AimAtTarget()
    {
        Vector2 direction = target.position - enemy.transform.position;
        direction.Normalize(); 
        // Rotate the gun to point at the player
        float angle = Mathf.Atan2(direction.y, direction.x * facingDirection) * Mathf.Rad2Deg;
        enemyGun.localRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
    }
    public void ResetAim() 
    {
        _timer = 5f;
        if (facingDirection == 1) 
        {
            enemyGun.localRotation = Quaternion.Euler(new Vector3(0, 0, -90f));
        }
        else 
        {
            enemyGun.localRotation = Quaternion.Euler(new Vector3(0, 0, 90f * facingDirection));
        }
    }
}
