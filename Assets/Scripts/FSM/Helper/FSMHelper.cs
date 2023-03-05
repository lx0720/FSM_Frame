using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FSM
{
    /// <summary>
    /// FSM状态帮助类(负责帮助State执行Mono的代码)
    /// </summary>
    public class FSMHelper : MonoBehaviour
    {
        /// <summary>
        /// 一个处理Update事件的小模块类
        /// </summary>
        public class StateUpdateModule
        {
            public object[] stateParameters;
            public Action<object[]> stateUpdateEvent;

            public StateUpdateModule(object[] stateParameters,Action<object[]> stateUpdateEvent)
            {
                this.stateParameters = stateParameters;
                this.stateUpdateEvent = stateUpdateEvent;
            }
        }
        /// <summary>
        /// 更新事件的执行间隔
        /// </summary>
        [SerializeField]
        private float invokeInterval = -1;

        /// <summary>
        /// 所有的FSMHelperModule
        /// </summary>
        /// <typeparam name="StateName">当前状态名字</typeparam>
        /// <typeparam name="FSMHelperModule">当前状态拥有的状态更新模块</typeparam>
        /// <returns></returns>
        private Dictionary<StateName,StateUpdateModule> StateUpdateModuleDict = new Dictionary<StateName,StateUpdateModule>();

        private StateUpdateModule[] stateUpdateArray;
        public static FSMHelper Instance{private set;get;}

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }
        private IEnumerator Start()
        {
            while(true)
            {
                /// 更新状态事件执行间隔
                if(invokeInterval <= 0)
                {
                    yield return 0;
                }
                else
                {
                    yield return new WaitForSeconds(invokeInterval);
                }
                ///更新其中的事件
                for(int i=0;i<stateUpdateArray.Length;i++)
                {
                    stateUpdateArray[i].stateUpdateEvent?.Invoke(stateUpdateArray[i].stateParameters);
                }

            }
        }
        /// <summary>
        /// 添加状态更新事件
        /// </summary>
        /// <param name="stateName">状态名字</param>
        /// <param name="stateUpdateParameters">状态更新参数</param>
        /// <param name="stateUpdateEvent">状态更新事件</param>
        public void AddStateUpdateEvent(StateName stateName,object[] stateUpdateParameters,Action<object[]> stateUpdateEvent)
        {
            if(!StateUpdateModuleDict.ContainsKey(stateName))
            {
                StateUpdateModuleDict.Add(stateName,new StateUpdateModule(stateUpdateParameters,stateUpdateEvent));
            }
            else
            {
                StateUpdateModuleDict[stateName] = new StateUpdateModule(stateUpdateParameters,stateUpdateEvent);
            }
            CopyStateInArray();
        }
        /// <summary>
        /// 移除状态更新事件
        /// </summary>
        /// <param name="stateName">状态名字</param>
        public void RemoveStateUpdateEvent(StateName stateName)
        {
            if(StateUpdateModuleDict.ContainsKey(stateName))
            {
                StateUpdateModuleDict.Remove(stateName);
                CopyStateInArray();
            }
        }


        public void CopyStateInArray()
        {
            stateUpdateArray = new StateUpdateModule[StateUpdateModuleDict.Count];
            int i = 0;
            foreach(var state in StateUpdateModuleDict)
            {
                stateUpdateArray[i] = state.Value;
                i++;
            }
        }

    }
}
