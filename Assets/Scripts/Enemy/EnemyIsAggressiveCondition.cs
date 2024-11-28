using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "EnemyIsAggressive", story: "Is [Agent] Aggresive", category: "Conditions", id: "b03211e968166e636624bc520ef4c6fc")]
public partial class EnemyIsAggressiveCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    Enemy enemy;
    public override bool IsTrue()
    {
        if (enemy.IsAggressive) 
        {
            return true;
        }
        else 
            return false;
    }

    public override void OnStart()
    {
        enemy = Agent.Value.GetComponentInChildren<Enemy>();
    }

    public override void OnEnd()
    {
    }
}
