using System;
using Unity.Behavior;
using Unity.VisualScripting;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "TriggerHasTarget", story: "[Trigger] Has Target", category: "Conditions", id: "986088578d3aca62d39fae464248a57d")]
public partial class TriggerHasTargetCondition : Condition
{
    [SerializeReference] public BlackboardVariable<Trigger> Trigger;

    public override bool IsTrue()
    {
        if (Trigger.Value.IsPlayerDetected() == true) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
