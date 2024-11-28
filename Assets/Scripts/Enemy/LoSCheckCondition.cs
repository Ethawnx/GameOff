using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "LoSCheck", story: "[Agent] Has Line of Sight", category: "Conditions", id: "e528b0d2c065bb7aa0e3d7508ec1840f")]
public partial class LoSCheckCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    Trigger trigger;
    public override bool IsTrue()
    {
        if (Agent == null) return false;

        if (trigger.IsPlayerInLineOfSight()) return true;
        else
            return false;
    }

    public override void OnStart()
    {
        trigger = Agent.Value.GetComponentInChildren<Trigger>();
    }

    public override void OnEnd()
    {
    }
}
