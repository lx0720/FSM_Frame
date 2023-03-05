using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class UseFsm : MonoBehaviour
{
    public float speed;
    private void Start()
    {
        /// <summary>
        /// ��Ҫ״̬��
        /// </summary>
        /// <returns></returns>
        StateMachine mainStateMachine = new StateMachine(StateName.None);
        /// <summary>
        /// ��״̬
        /// </summary>
        /// <returns></returns>
        State idleState = new State(StateName.Idling);
        State walkState = new State(StateName.Walking);
        State runState = new State(StateName.Runing);
        ///�˶�״̬��
        StateMachine moveStateMachine = new StateMachine(StateName.Movement);

        ///������ϵ
        mainStateMachine.AddState(idleState);
        mainStateMachine.AddState(moveStateMachine);
        moveStateMachine.AddState(walkState);
        moveStateMachine.AddState(runState);

        mainStateMachine.OnStateEnter += objects => {Debug.Log("Mian״̬������!");};
        mainStateMachine.OnStateUpdate += objects => {Debug.Log("Mian״̬������!");};
        mainStateMachine.OnStateExit += objects => {Debug.Log("Mian״̬���˳�!");};

        moveStateMachine.OnStateEnter += objects => {Debug.Log("move״̬������!");};
        moveStateMachine.OnStateUpdate += objects => {Debug.Log("move״̬������!");};
        moveStateMachine.OnStateExit += objects => {Debug.Log("move״̬���˳�!");};

        idleState.OnStateEnter += objects => {Debug.Log("idle״̬����!");};
        idleState.OnStateUpdate += objects => {Debug.Log("idle״̬����!");};
        idleState.OnStateExit += objects => {Debug.Log("idle״̬�˳�!");};

        walkState.OnStateEnter += objects => {Debug.Log("walk״̬����!");};
        walkState.OnStateUpdate += objects => {Debug.Log("walk״̬����!");};
        walkState.OnStateExit += objects => {Debug.Log("walk״̬�˳�!");};

        runState.OnStateEnter += objects => {Debug.Log("run״̬����!");};
        runState.OnStateUpdate += objects => {Debug.Log("run״̬����!");};
        runState.OnStateExit += objects => {Debug.Log("run״̬�˳�!");};
    
        idleState.RegisterTransitionState(StateName.Movement,()=>{return speed>0;});
        moveStateMachine.RegisterTransitionState(StateName.Idling,()=>{return speed<=0;});

        walkState.RegisterTransitionState(StateName.Runing,()=>{return speed >=3;});
        runState.RegisterTransitionState(StateName.Walking,()=>{return speed <3 && speed>=1;});

        mainStateMachine.EnterState(null,null);
    }
}
