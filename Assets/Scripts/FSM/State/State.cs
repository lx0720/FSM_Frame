using System;
using System.Collections;
using System.Collections.Generic;

namespace FSM
{
    /// <summary>
    /// 状态的基类
    /// </summary>
    public class State
    {
        #region  Fields

        /// <summary>
        /// 状态的名字
        /// </summary>
        private StateName stateName;

        /// <summary>
        /// 状态进入
        /// </summary>
        public event Action<object[]> OnStateEnter;
        /// <summary>
        /// 状态持续更新
        /// </summary>
        public event Action<object[]> OnStateUpdate;
        /// <summary>
        /// 状态退出
        /// </summary>
        public event Action<object[]> OnStateExit;
        /// <summary>
        /// 可以转换的状态和转换条件
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
        /// 注册可以转换的状态
        /// </summary>
        /// <param name="stateName">状态名字</param>
        /// <param name="condition">状态转换条件</param>
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
        /// 移除转换状态
        /// </summary>
        /// <param name="stateName">状态名字</param>
        /// <param name="condition">状态转换条件</param>
        public void UnRegisterTransition(StateName stateName,Func<bool> condition)
        {
            if(TransitionStateDict.ContainsKey(stateName))
            {
                TransitionStateDict.Remove(stateName);
            }
        }
        /// <summary>
        /// 状态进入(存在状态的转换),进入执行之后需要执行更新事件
        /// </summary>
        /// <param name="parameters">参数</param>
        public virtual void EnterState(object[] enterParameters,object[] updateParameters)
        {
            OnStateEnter?.Invoke(enterParameters);
            FSMHelper.Instance.AddStateUpdateEvent(stateName,updateParameters,OnStateUpdate);
        }
        /// <summary>
        /// 状态退出(存在状态的转换),退出之后需要退出更新事件
        /// </summary>
        /// <param name="parameters">参数</param>
        public virtual void ExitState(object[] exitParameters)
        {
            FSMHelper.Instance.RemoveStateUpdateEvent(stateName);
            OnStateExit?.Invoke(exitParameters);
        }

        #endregion

    }
}
