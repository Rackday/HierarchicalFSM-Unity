using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    public State initialState;
    public State currentState;
    public NPCController controller;

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
        if (currentState != null)
        {
            //Executes the actions of the current state
            foreach (Action action in currentState.GetActions())
            {
                action.Execute(this);
            }

            //Foreach through all the current state transitions
            foreach (StateTransition transition in currentState.Transitions())
            {
                //Checks if the transition is triggered (true)
                if (transition.IsTriggered(this))
                {
                    currentState.ExitAction().Execute(this); //Exececutes the exit action of the current state
                    currentState = transition.TargetState(); //Changes the state
                    currentState.GetEntryAction().Execute(this); //Executes the entry action
                    break;
                }
            }
        }
    }
}
