﻿using Patterns;
using System;
using System.Collections;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Bioweapon
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private GameManagerScriptableObject setUpData;

        public GameManagerScriptableObject SetUpData { get => setUpData;}
        public FSM gameStateMachine;

        //you can ignore this awake as because yan chun FSM dont allow empty events
        private void Awake()
        {
            //just ignore this
            EventManager.Instance.AddListener(EventName.TURN_START, (Action)MonkeyMove);
            EventManager.Instance.AddListener(EventName.TURN_COMPLETE, (Action)MonkeyMove);
            EventManager.Instance.AddListener(EventName.TURN_END, (Action)MonkeyMove);

            void MonkeyMove()
            {
            }
        }


        private void Start()
        {
            gameStateMachine = new FSM();
            gameStateMachine.Add((int)GameEvent.Start, new StartTurnState(gameStateMachine, (int)GameEvent.Start));
            gameStateMachine.Add((int)GameEvent.End, new EndTurnState(gameStateMachine, (int)GameEvent.Start));
            gameStateMachine.SetCurrentState((int)GameEvent.Start); 
        }

        private void Update()
        {
            gameStateMachine.Update();
        }

        private void OnGUI()
        {
            if(GUILayout.Button("End Event"))
            {
                EventManager.Instance.TriggerEvent(EventName.TURN_END);
            }
        }

    }

    public class TurnState : FSMState
    {
        //add any modification here :)
        public TurnState(FSM fsm, int id ) : base(fsm, id)
        {
            
        }
    }

    /// <summary>
    /// start turn state is where all the action from previous turns occur so that player can start doing their decision
    /// </summary>
    public class StartTurnState : TurnState
    {
        private float elapseTime;

        public StartTurnState(FSM fsm, int id) : base(fsm, id)
        {
        }

        public override void Enter()
        {
            Debug.Log("Start turn");

            //do have something here
            EventManager.Instance.TriggerEvent(EventName.TURN_START);
            elapseTime = 0;
        }


        public override void Update()
        {
            if(elapseTime < GameManager.Instance.SetUpData.TimePassPerTurn)
            {
                Debug.Log("running");
                elapseTime += Time.deltaTime;
            }
            else
            {
                EventManager.Instance.TriggerEvent(EventName.TURN_COMPLETE);
                mFsm.SetCurrentState((int)GameEvent.End);
            }
        }
    }

    /// <summary>
    /// End turn state is where player plans to end their turn 
    /// </summary>
    public class EndTurnState : TurnState
    {
        public EndTurnState(FSM fsm, int id) : base(fsm, id)
        {
        }

        public override void Enter()
        {
            Debug.Log("end turn phase");
            EventManager.Instance.AddListener(EventName.TURN_END, (Action)StartNewTurn);            
        }

        public override void Exit()
        {
            EventManager.Instance.RemoveListener(EventName.TURN_END, (Action)StartNewTurn);
        }

        private void StartNewTurn()
        { //no update but just check if player end their turn
            mFsm.SetCurrentState((int)GameEvent.Start);
        }
    }
}