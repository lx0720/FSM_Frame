using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class StateMachine : State
    {
        public StateMachine(StateName stateName) : base(stateName)
        {

            manageredStates = new Dictionary<StateName, State>();

            
        }

        /// <summary>
        /// Ĭ��״̬
        /// </summary>
        private State defaultState;

        private State currentState;

        private bool machineRuning = false;

        /// <summary>
        /// ��ǰ״̬�������State
        /// </summary>
        private Dictionary<StateName,State> manageredStates;

        /// <summary>
        /// ����״̬���״̬
        /// </summary>
        /// <param name="state"></param>
        public State AddState(State state)
        {
            if(manageredStates.ContainsKey(state.GetStateName()))
                return manageredStates[state.GetStateName()];

            manageredStates.Add(state.GetStateName(),state);

             if(manageredStates.Count == 1)
            {
                defaultState = state;
                currentState = state;
            }

            return state;
        }
        /// <summary>
        /// ����״̬�������״̬
        /// </summary>
        /// <param name="state"></param>
        public State AddState(StateName stateName)
        {
            if(manageredStates.ContainsKey(stateName))
                return manageredStates[stateName];

            State newState = new State(stateName);
            manageredStates.Add(stateName,newState);

            if(manageredStates.Count == 1)
            {
                defaultState = newState;
                currentState = newState;
            }

            return newState;
        }
        /// <summary>
        /// �Ƴ�״̬
        /// </summary>
        /// <param name="state"></param>
        public void RemoveState(StateName stateName)
        {
            if(manageredStates.ContainsKey(stateName) && manageredStates[stateName]!=currentState)
            {
                State removeState = manageredStates[stateName];

                if(removeState == defaultState)
                {
                    defaultState = null;

                    ChooseNewDefaultState();
                }
                manageredStates.Remove(stateName);
            }
        }

        private void ChooseNewDefaultState()
        {
            foreach(var state in manageredStates)
            {
                defaultState = state.Value;
                return;
            }
        }
        /// <summary>
        /// ״̬���Ľ���
        /// </summary>
        /// <param name="enterParameters"></param>
        /// <param name="updateParameters"></param>
        public override void EnterState(object[] enterParameters, object[] updateParameters)
        {
            base.EnterState(enterParameters, updateParameters);
            if(defaultState == null)
                return;
            currentState = defaultState;
            ///����Ĭ����״̬
            currentState.EnterState(enterParameters,updateParameters);

        }
        /// <summary>
        /// ״̬�����˳�
        /// </summary>
        /// <param name="exitParameters"></param>
        public override void ExitState(object[] exitParameters)
        {
            base.ExitState(exitParameters);
        }
    }
}