using System;
using System.Collections;
using System.Collections.Generic;

namespace FSM
{
    /// <summary>
    /// ״̬�Ļ���
    /// </summary>
    public class State
    {
        /// <summary>
        /// ״̬������
        /// </summary>
        private StateName stateName;
        /// <summary>
        /// ״̬�Ƿ�����
        /// </summary>
        protected bool stateRunning = false;
        /// <summary>
        /// ����ת����״̬��ת������
        /// </summary>
        private Dictionary<StateName,Func<bool>> transitionStateDict;

         #region State Events
        /// <summary>
        /// ״̬����
        /// </summary>
        public event Action<object[]> OnStateEnter;
        /// <summary>
        /// ״̬��������
        /// </summary>
        public event Action<object[]> OnStateUpdate;
        /// <summary>
        /// ״̬�˳�
        /// </summary>
        public event Action<object[]> OnStateExit;

        #endregion

        #region State Methods

        private void StateBaseEventBind()
        {
            OnStateEnter += objects => {stateRunning = true;};
            OnStateExit += objects =>{stateRunning = false;};
        }

        public State(StateName stateName)
        {
            this.stateName = stateName;
            transitionStateDict = new Dictionary<StateName, Func<bool>>();
            StateBaseEventBind();
        }

        public StateName GetStateName()=>stateName;
        /// <summary>
        /// ע�����ת����״̬
        /// </summary>
        /// <param name="stateName">״̬����</param>
        /// <param name="condition">״̬ת������</param>
        public void RegisterTransitionState(StateName stateName,Func<bool> condition)
        {
            if(!transitionStateDict.ContainsKey(stateName))
            {
                transitionStateDict.Add(stateName,condition);
            }
            else
            {
                transitionStateDict[stateName] = condition;
            }
        }
        /// <summary>
        /// �Ƴ�ת��״̬
        /// </summary>
        /// <param name="stateName">״̬����</param>
        /// <param name="condition">״̬ת������</param>
        public void UnRegisterTransition(StateName stateName)
        {
            if(transitionStateDict.ContainsKey(stateName))
            {
                transitionStateDict.Remove(stateName);
            }
        }
        /// <summary>
        /// ״̬����(����״̬��ת��),����ִ��֮����Ҫִ�и����¼�
        /// </summary>
        /// <param name="parameters">����</param>
        public virtual void EnterState(object[] enterParameters,object[] updateParameters)
        {
            OnStateEnter?.Invoke(enterParameters);
            FSMHelper.Instance.AddStateUpdateEvent(stateName,updateParameters,OnStateUpdate);
        }
        /// <summary>
        /// ״̬�˳�(����״̬��ת��),�˳�֮����Ҫ�˳������¼�
        /// </summary>
        /// <param name="parameters">����</param>
        public virtual void ExitState(object[] exitParameters)
        {
            FSMHelper.Instance.RemoveStateUpdateEvent(stateName);
            OnStateExit?.Invoke(exitParameters);
        }
        /// <summary>
        /// ��鵱ǰ����ת����״̬
        /// </summary>
        /// <returns></returns>
        public StateName CheckCurrentStateTransition()
        {
            foreach(var state in transitionStateDict)
            {
                if(state.Value())
                {
                    return state.Key;
                }
            }
            return StateName.None;
        }
        #endregion

    }
}
