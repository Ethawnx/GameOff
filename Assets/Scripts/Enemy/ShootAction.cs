using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections;
using Unity.VisualScripting;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Shoot", story: "[Agent] Shoots [Projectile] at [Target]", category: "Action", id: "3b83d3c56419ae1ee7bdf07b890fb6df")]
public partial class ShootAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Projectile;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<Transform> EnemyGun;
    [SerializeReference] public BlackboardVariable<float> FireRate;
    float _nextFireTime;
    Trigger trigger;
    protected override Status OnStart()
    {
        trigger = Agent.Value.GetComponentInChildren<Trigger>();
        return Status.Running;

    }

    protected override Status OnUpdate()
    {
        Shoot(FireRate);
        if (!trigger.IsPlayerInLineOfSight()) 
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
    private void Shoot(float fireRate) 
    {
        if (Time.time >= _nextFireTime)
        {
            _nextFireTime = Time.time + 1f/fireRate;
            GameObject Bullet = (GameObject)GameObject.Instantiate(Projectile, Agent.Value.transform.position, Quaternion.identity);
            EnemyProjectile bulletProjectile = Bullet.GetComponent<EnemyProjectile>();
            bulletProjectile.SetGunTransform(EnemyGun.Value);
            //bulletProjectile.SetTargetTransfrom(Target.Value);
        }
    }

}

