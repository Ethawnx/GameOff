using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Trigger : MonoBehaviour
{
    public float radius = 1f;
    [Range(0,1)]
    public float angThreshold = 0.5f;

    [SerializeField]
    private PlayerData playerData;

    private Transform target;
    private int facingDirection;
    void Start() 
    {
        target = GameObject.FindWithTag("Player").transform;
    }
    private void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = IsPlayerDetected() ? Color.white : Color.red;

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
        facingDirection = GetComponentInParent<Enemy>().GetFacingDirection();
        Vector2 vecToTargetWorld = (target.transform.position - transform.position).normalized;
        
        if (Vector2.Distance(position, transform.position) > radius)
            return false;
        if ((facingDirection * vecToTargetWorld.x) < angThreshold)
            return false;

        else return true;
    }

    public bool IsPlayerInLineOfSight()
    {
        facingDirection = GetComponentInParent<Enemy>().GetFacingDirection();
        Vector2 vecToTargetWorld = (target.transform.position - transform.position).normalized;
        Vector2 offset = new Vector2(0.3f * facingDirection, 0f);
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + offset, vecToTargetWorld, radius - (offset.x * facingDirection));
        Debug.DrawRay((Vector2)transform.position + offset * facingDirection, vecToTargetWorld * radius, Color.blue);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Debug.Log(hit.collider.tag);
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsPlayerDetected() 
    {
        if (Contains(target.transform.position) && IsPlayerInLineOfSight()) 
        {
            return true;
        }
        else 
        {  
            return false;
        }
    }
}
