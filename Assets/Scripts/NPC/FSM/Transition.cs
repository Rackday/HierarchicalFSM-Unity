using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Transition")] 
public class Transition : ScriptableObject
{
    //Variables
    [SerializeField] private State targetState;
    [SerializeField] private Condition condition;
    [SerializeField] private Action action;

    public bool IsTriggered(FSM fsm) => condition.CheckCondition(fsm);

    public State TargetState() => targetState;

    public Action GetAction() => action;
}
