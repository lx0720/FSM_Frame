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
            ///状态机更新绑定
            StateMachineEventBind();
        }

        /// <summary>
        /// 默认状态
        /// </summary>
        private State defaultState;

        private State currentState;

        /// <summary>
        /// 当前状态机管理的State
        /// </summary>
        private Dictionary<StateName,State> manageredStates;

        private void StateMachineEventBind()
        {
            OnStateUpdate +=  GetCurrentStateTransition;
        }
        /// <summary>
        /// 根据状态添加状态
        /// </summary>
        /// <param name="state"></param>
        public State AddState(State state)
        {
            //if(stateRunning) return null;
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
        /// 根据状态名称添加状态
        /// </summary>
        /// <param name="state"></param>
        public State AddState(StateName stateName)
        {
            //if(stateRunning) return null;
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
        /// 移除状态
        /// </summary>
        /// <param name="state"></param>
        public void RemoveState(StateName stateName)
        {
            //if(stateRunning) return;
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
        /// 状态机的进入
        /// </summary>
        /// <param name="enterParameters"></param>
        /// <param name="updateParameters"></param>
        public override void EnterState(object[] enterParameters, object[] updateParameters)
        {
            base.EnterState(enterParameters, updateParameters);
            if(defaultState == null)
                return;
            currentState = defaultState;
            ///进入默认子状态
            currentState.EnterState(enterParameters,updateParameters);

        }
        /// <summary>
        /// 状态机的退出
        /// </summary>
        /// <param name="exitParameters"></param>
        public override void ExitState(object[] exitParameters)
        {
            if(currentState!=null)
            {
                currentState.ExitState(exitParameters);
            }
            base.ExitState(exitParameters);
        }

        /// <summary>
        /// 得到当前可以转换的状态
        /// </summary>
        /// <param name="param"></param>
        private void GetCurrentStateTransition(object[] param)
        {
            StateName stateName = currentState.CheckCurrentStateTransition();
            if(stateName != StateName.None)
            {
                TransitionToState(stateName);
            }
        }
        /// <summary>
        /// 状态转换
        /// </summary>
        /// <param name="stateName"></param>
        private void TransitionToState(StateName stateName)
        {
                if(manageredStates.ContainsKey(stateName))
                {
                    currentState.ExitState(null);
                    currentState = manageredStates[stateName];
                    currentState.EnterState(null,null);
                }
        }
    }
}