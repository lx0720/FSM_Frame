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
        #region  Fields

        /// <summary>
        /// ״̬������
        /// </summary>
        private StateName stateName;

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
        /// <summary>
        /// ����ת����״̬��ת������
        /// </summary>
        private Dictionary<StateName,Func<bool>> TransitionStateDict;

        #endregion

        #region  Methods

        public State(StateName stateName)
        {
            this.stateName = stateName;
            TransitionStateDict = new Dictionary<StateName, Func<bool>>();
        }


        public StateName GetStateName()=>stateName;
        /// <summary>
        /// ע�����ת����״̬
        /// </summary>
        /// <param name="stateName">״̬����</param>
        /// <param name="condition">״̬ת������</param>
        public void RegisterTransitionState(StateName stateName,Func<bool> condition)
        {
            if(!TransitionStateDict.ContainsKey(stateName))
            {
                TransitionStateDict.Add(stateName,condition);
            }
            else
            {
                TransitionStateDict[stateName] = condition;
            }
        }
        /// <summary>
        /// �Ƴ�ת��״̬
        /// </summary>
        /// <param name="stateName">״̬����</param>
        /// <param name="condition">״̬ת������</param>
        public void UnRegisterTransition(StateName stateName,Func<bool> condition)
        {
            if(TransitionStateDict.ContainsKey(stateName))
            {
                TransitionStateDict.Remove(stateName);
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

        #endregion

    }
}
