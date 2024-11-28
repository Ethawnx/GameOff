using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.Splines;
using Unity.Mathematics;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Chase", story: "Navigate [Agent] to the nearest [WayPoint]", category: "Action", id: "90010e6876f7ddb45659497c349731b1")]
public partial class ChaseAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<List<GameObject>> WayPoint;
    GameObject nearestWayPoint;
    //Rigidbody2D rb;
    Transform target;
    Animator anim;
    float Speed = 4f;
    float distance;
    Vector2 agentPosition;

    protected override Status OnStart()
    {
        agentPosition = Agent.Value.transform.position;

        target = GameObject.FindGameObjectWithTag("Player").transform;
        anim = Agent.Value.GetComponent<Animator>();
        nearestWayPoint = GetClosestWaypoints(target.position, WayPoint.Value);
        distance = MathF.Abs(agentPosition.x - nearestWayPoint.transform.position.x);
        anim.SetFloat("Velocity", Speed);
        if (distance <= 0.1f)
        {
            agentPosition.x = nearestWayPoint.transform.position.x;
            Agent.Value.transform.right = Vector3.right * Agent.Value.GetComponent<Enemy>().GetFacingDirection();
            anim.SetFloat("Velocity", 0);
            return Status.Success;
        }
        else
            return Status.Running;

    }

    protected override Status OnUpdate()
    {
        //Debug.Log(rb.gameObject.name);
        agentPosition = Agent.Value.transform.position;
        distance = MathF.Abs(agentPosition.x - nearestWayPoint.transform.position.x);
        //Debug.Log(distance);
        if (distance <= 0.1f)
        {
            agentPosition.x = nearestWayPoint.transform.position.x;
            Agent.Value.transform.right = Vector3.right * Agent.Value.GetComponent<Enemy>().GetFacingDirection();
            anim.SetFloat("Velocity", 0);
            return Status.Success;
        }

        float speed = Speed;


        Vector2 toDestination = (Vector2)nearestWayPoint.transform.position - agentPosition;
        toDestination.y = 0.0f;
        toDestination.Normalize();
        agentPosition += toDestination * (speed * Time.deltaTime);
        Agent.Value.transform.position = agentPosition;

        // Look at the target.
        Vector2 toPlayer = (Vector2)target.position - agentPosition;
        toPlayer.y = 0.0f;
        toPlayer.Normalize();
        Agent.Value.transform.right = toPlayer;
        return Status.Running;
    }

    protected override void OnEnd()
    {
        anim.SetFloat("Velocity", 0);
    }
    private GameObject GetClosestWaypoints(Vector2 targetPos, List<GameObject> waypoints) 
    {
        GameObject closest = null;
        float minDistance = float.MaxValue;

        foreach (GameObject waypoint in waypoints) 
        {
            float distance = Vector2.Distance (waypoint.transform.position, targetPos);
            if (distance < minDistance) 
            {
                minDistance = distance;
                closest = waypoint;
            }
        }
        return closest;
    }
}

