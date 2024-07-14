using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/State")]
public class State : ScriptableObject
{
    //Variables
    [SerializeField] private Action entryAction;
    [SerializeField] private Action[] actions;
    [SerializeField] private Action exitAction;
    [SerializeField] private Transition[] transitions;
    [SerializeField] private State initialSubState;
    [SerializeField] private List<State> subStates;

    //Methods
    public Action GetEntryAction() => entryAction;
    public Action[] GetActions() => actions;
    public Action ExitAction() => exitAction;
    public Transition[] Transitions() => transitions;
    public List<State> SubStates() => subStates;

    //Runs the entry action of the currentState
    public void Enter(FSM fsm)
    {
        entryAction?.Execute(fsm);

        if (initialSubState != null)
        {
            fsm.ChangeState(initialSubState);
        }
    }

    //Executes all the actions of the state
    public void ExecuteActions(FSM fsm)
    {
        foreach (Action action in actions)
        {
            action?.Execute(fsm);
        }
    }

    //Gets a triggered transition
    public Transition GetTriggeredTransition(FSM fsm)
    {
        foreach (Transition transition in transitions)
        {
            if (transition.IsTriggered(fsm))
            {
                return transition;
            }
        }

        return null;
    }
}
