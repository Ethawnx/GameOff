using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TriggerHasTarget", story: "[Agent] Detects [Player]", category: "Action", id: "1e84a4268159e172bdf46923aa5fa4b4")]
public partial class TriggerHasTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    Trigger trigger;
    protected override Status OnStart()
    {
        trigger = Agent.Value.GetComponentInChildren<Trigger>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (trigger.IsPlayerDetected()) 
        {
            return Status.Success;
        }
        else
        {
            return Status.Running;
        }
    }

    protected override void OnEnd()
    {
    }
}

