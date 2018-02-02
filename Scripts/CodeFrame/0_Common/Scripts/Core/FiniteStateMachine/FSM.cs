using System;
using System.Collections.Generic;

public delegate void ActionHandler();

public delegate bool ConditionHandler();

public class Transition<S> where S : struct, IConvertible
{
    public ConditionHandler[] conditions;
    public S nextState;

    public bool AreAllConditionSatisfied()
    {
        foreach (ConditionHandler c in conditions)
        {
            if (!c()) return false;
        }
        return true;
    }
}

public class FSM<T> where T : struct, IConvertible
{
    public Dictionary<T, FSMState<T>> stateDict;
    public T currentStateID { get; private set; }
    public T lastStateID { get; private set; }
    private bool started;
    public FSMState<T> currentState { get { return stateDict[currentStateID]; } }

    public FSM()
    {
        stateDict = new Dictionary<T, FSMState<T>>();
        started = false;
    }

    public FSMState<T> CreateState(T stateID, ActionHandler updateActionHandler = null, ActionHandler enterActionHandler = null, ActionHandler exitActionHandler = null)
    {
        //Debuger.Log ("FSM.CreateState: stateID = "+ stateID);
        FSMState<T> newState = new FSMState<T>(this, updateActionHandler, enterActionHandler, exitActionHandler);
        stateDict.Add(stateID, newState);
        return newState;
    }

    //	public void AddState (T stateID, FSMState state) {
    //		stateDict [stateID] = state;
    //		//		state.SetOwner (this);
    //	}

    private void ChangeState(T newStateID)
    {
        //Debuger.Log ("ChangeState () from " + currentStateID + " to " + newStateID);
        if (!(newStateID.Equals(currentStateID)))
        {
            //Debuger.Log ("call " + currentStateID + "'s exitAction:" + stateDict[currentStateID].onExitAction);
            stateDict[currentStateID].onExitAction();
            lastStateID = currentStateID;
            currentStateID = newStateID;

            if (!stateDict.ContainsKey(currentStateID))
            {
                //Debuger.LogWarning ("Missing state:" + currentStateID);
            }
            //Debuger.Log ("call " + currentStateID + "'s enterAction:" + stateDict[currentStateID].onEnterAction);
            stateDict[currentStateID].onEnterAction();
        }
    }

    public void Start(T initialStateID)
    {
        //Debuger.Log ("Start FSM () from state:" + currentStateID);
        started = true;
        currentStateID = initialStateID;
        lastStateID = currentStateID;
        stateDict[currentStateID].onEnterAction();
    }

    public void Stop()
    {
        //Debuger.Log ("Stop FSM () from state:" + currentStateID);
        started = false;
        stateDict[currentStateID].onExitAction();
    }

    public void Update()
    {
        //Debuger.Log ("currentState = " + currentState + ", frame " + Time.frameCount);
        if (!started) return;
        if (currentState == null) return;

        foreach (Transition<T> trans in currentState.TransitionList)
        {
            if (trans.AreAllConditionSatisfied())
            {
                ChangeState(trans.nextState);
                return;
            }
        }

        //Debuger.Log ("currentState = " + currentState + ", frame " + Time.frameCount);
        //Debuger.Log ("currentState.updateAction = " + currentState.updateAction + ", frame " + Time.frameCount);
        currentState.onUpdateAction();
    }
}

public class FSMState<U> where U : struct, IConvertible
{
    public ActionHandler EmptyAction = delegate { };

    public FSM<U> owner;
    public ActionHandler onEnterAction;
    public ActionHandler onExitAction;
    public ActionHandler onUpdateAction;

    public List<Transition<U>> TransitionList;

    internal FSMState(FSM<U> owner, ActionHandler updateActionHandler = null, ActionHandler enterActionHandler = null, ActionHandler exitActionHandler = null)
    {
        this.owner = owner;

        this.onEnterAction = enterActionHandler == null ? EmptyAction : enterActionHandler;
        this.onExitAction = exitActionHandler == null ? EmptyAction : exitActionHandler;
        this.onUpdateAction = updateActionHandler == null ? EmptyAction : updateActionHandler;

        TransitionList = new List<Transition<U>>();
    }

    public void AddTransition(ConditionHandler cond, U transitToState)
    {
        TransitionList.Add(new Transition<U> { conditions = new ConditionHandler[] { cond }, nextState = transitToState });
    }

    public void AddTransition(ConditionHandler[] conds, U transitToState)
    {
        TransitionList.Add(new Transition<U> { conditions = conds, nextState = transitToState });
    }
}