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
            public StateName stateName;
            public object[] stateParameters;
            public Action<object[]> stateUpdateEvent;

            public StateUpdateModule(StateName stateName,object[] stateParameters,Action<object[]> stateUpdateEvent)
            {
                this.stateName = stateName;
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


        public static FSMHelper Instance{private set;get;}

        private void Awake()
        {
            GameObject fsmHelper = GameObject.FindGameObjectWithTag(StaticGameTag.FSMHelperTag);
            if(fsmHelper == null)
            {
                fsmHelper = new GameObject(StaticGameTag.FSMHelperTag);
                fsmHelper.tag = StaticGameTag.FSMHelperTag;
            }
            Instance = fsmHelper.AddComponent<FSMHelper>();
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
                foreach(StateUpdateModule state in StateUpdateModuleDict.Values)
                {
                    state.stateUpdateEvent?.Invoke(state.stateParameters);
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
                StateUpdateModuleDict.Add(stateName,new StateUpdateModule(stateName,stateUpdateParameters,stateUpdateEvent));
            }
            else
            {
                StateUpdateModuleDict[stateName] = new StateUpdateModule(stateName,stateUpdateParameters,stateUpdateEvent);
            }
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
            }
        }
    }
}
