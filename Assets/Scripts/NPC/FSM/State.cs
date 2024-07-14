using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject
{
    private Action entryAction;
    private Action[] actions;
    private Action exitAction;
    private StateTransition[] transitions;

    public Action GetEntryAction() => entryAction;

    public Action[] GetActions() => actions;

    public Action ExitAction() => exitAction;

    public StateTransition[] Transitions() => transitions;
}
