using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FSM
{
    /// <summary>
    /// FSM״̬������(�������Stateִ��Mono�Ĵ���)
    /// </summary>
    public class FSMHelper : MonoBehaviour
    {
        /// <summary>
        /// һ������Update�¼���Сģ����
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
        /// �����¼���ִ�м��
        /// </summary>
        [SerializeField]
        private float invokeInterval = -1;

        /// <summary>
        /// ���е�FSMHelperModule
        /// </summary>
        /// <typeparam name="StateName">��ǰ״̬����</typeparam>
        /// <typeparam name="FSMHelperModule">��ǰ״̬ӵ�е�״̬����ģ��</typeparam>
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
                /// ����״̬�¼�ִ�м��
                if(invokeInterval <= 0)
                {
                    yield return 0;
                }
                else
                {
                    yield return new WaitForSeconds(invokeInterval);
                }
                ///�������е��¼�
                for(int i=0;i<stateUpdateArray.Length;i++)
                {
                    stateUpdateArray[i].stateUpdateEvent?.Invoke(stateUpdateArray[i].stateParameters);
                }

            }
        }
        /// <summary>
        /// ���״̬�����¼�
        /// </summary>
        /// <param name="stateName">״̬����</param>
        /// <param name="stateUpdateParameters">״̬���²���</param>
        /// <param name="stateUpdateEvent">״̬�����¼�</param>
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
        /// �Ƴ�״̬�����¼�
        /// </summary>
        /// <param name="stateName">״̬����</param>
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
