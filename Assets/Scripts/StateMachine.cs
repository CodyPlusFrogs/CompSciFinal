using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//use this for state machines
public class StateMachine : MonoBehaviour
{
    //if we are started yet
    bool isStarted;
    //all of the states
    List<State> states = new List<State>();
    int currStateIndex;



    class state1 : State
    {
        public override void doEveryLoop()
        {
            throw new NotImplementedException();
        }

        public override bool shouldExit()
        {
            throw new NotImplementedException();
        }
    }
    //Creates a new state machine
    public StateMachine()
    {
        //Start at the first state
        currStateIndex = 0;
        //initialize the first state
        states[currStateIndex].init();




        //don't start yet
        isStarted = false;
    }


    /// <summary>
    /// Adds a state to the list
    /// </summary>
    /// <param name="state">The state object (must be initialized)</param>
    public void addState(State state)
    {
        states.Add(state);
    }

    //called every update
    public void FixedUpdate()
    {
        
        //only do stuff if we are started
        if (isStarted)
        {
            //get the current state
            State currentState = states[currStateIndex];

            //update the current state
            if (currentState.update())
            {
                //returns true if it is done
                nextState();
            }
        }
        
        
    }

    /// <summary>
    /// Starts the state machine
    /// </summary>
    public void startStateMachine()
    {
        isStarted = true;
    }


    /// <summary>
    /// Stops the state machine
    /// </summary>
    public void stopStateMachine()
    {
        isStarted = false;
    }

    /// <summary>
    /// goes to the next state in the list
    /// </summary>
    private void nextState()
    {
        currStateIndex++;
        //initialize the next state
        states[currStateIndex].init();
    }
}   


/// <summary>
/// This is one state in the state machine
/// </summary>
public abstract class State {

    //called on fixed update of the unity engine
    public bool update()
    {
        doEveryLoop();
        //see if we need to exit the state
        return shouldExit();
    }
    //called every update
    public abstract void doEveryLoop();


    //called when we first switch to this state
    public void init()
    {
        //call this in case there's anything special to do
        specialInitialize();
    }

    //Override this to do anything special in initialization
    public void specialInitialize()
    {

    }

    //if the state is done
    public abstract bool shouldExit();    
}

