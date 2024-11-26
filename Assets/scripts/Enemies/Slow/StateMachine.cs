
using System.Collections.Generic;

public class StateMachine
{
    public Dictionary<string, StateBase> states = new Dictionary<string, StateBase>();
    public StateBase currentState;

    public void AddState(string stateName, StateBase state) {
        states.Add(stateName, state);
    }

    public void ChangeState(string stateName) {
        if (currentState != null) {
            currentState.OnExit();
            currentState = null;
        }

        if (states.ContainsKey(stateName)) {
            currentState = states[stateName];
            currentState.OnEnter();
        }
    }

    public void OnUpdate() {
        currentState?.OnUpdate();
    }
}