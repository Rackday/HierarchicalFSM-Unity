using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    [SerializeField] private State initialState;
    [SerializeField] private State currentState;
    [SerializeField] private NPCController controller;

    public NPCController Controller => controller;

    void Awake()
    {
        controller = GetComponent<NPCController>(); 
    }

    void Start()
    {
        currentState = initialState; //Sets a state
    }

    void Update()
    {
        if (currentState == null) return;

        ExecuteState(currentState);
    }


    private void ExecuteState(State state)
    {
        state.ExecuteActions(this);

        Transition triggeredTransition = state.GetTriggeredTransition(this);

        if (triggeredTransition != null)
        {
            ChangeState(triggeredTransition.TargetState());
        }
        else if (state.SubStates() != null)
        {
            foreach (State substate in state.SubStates())
            {
                ExecuteState(substate);
            }
        }
    }

    //Changes the current state to a new state
    public void ChangeState(State newState)
    {
        currentState?.ExitAction().Execute(this); //Exececutes the exit action of the current state
        currentState = newState; //Changes the state
        currentState?.Enter(this); //Executes
    }
}
