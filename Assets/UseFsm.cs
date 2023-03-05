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
        /// 主要状态机
        /// </summary>
        /// <returns></returns>
        StateMachine mainStateMachine = new StateMachine(StateName.None);
        /// <summary>
        /// 子状态
        /// </summary>
        /// <returns></returns>
        State idleState = new State(StateName.Idling);
        State walkState = new State(StateName.Walking);
        State runState = new State(StateName.Runing);
        ///运动状态机
        StateMachine moveStateMachine = new StateMachine(StateName.Movement);

        ///建立联系
        mainStateMachine.AddState(idleState);
        mainStateMachine.AddState(moveStateMachine);
        moveStateMachine.AddState(walkState);
        moveStateMachine.AddState(runState);

        mainStateMachine.OnStateEnter += objects => {Debug.Log("Mian状态机进入!");};
        mainStateMachine.OnStateUpdate += objects => {Debug.Log("Mian状态机更新!");};
        mainStateMachine.OnStateExit += objects => {Debug.Log("Mian状态机退出!");};

        moveStateMachine.OnStateEnter += objects => {Debug.Log("move状态机进入!");};
        moveStateMachine.OnStateUpdate += objects => {Debug.Log("move状态机更新!");};
        moveStateMachine.OnStateExit += objects => {Debug.Log("move状态机退出!");};

        idleState.OnStateEnter += objects => {Debug.Log("idle状态进入!");};
        idleState.OnStateUpdate += objects => {Debug.Log("idle状态更新!");};
        idleState.OnStateExit += objects => {Debug.Log("idle状态退出!");};

        walkState.OnStateEnter += objects => {Debug.Log("walk状态进入!");};
        walkState.OnStateUpdate += objects => {Debug.Log("walk状态更新!");};
        walkState.OnStateExit += objects => {Debug.Log("walk状态退出!");};

        runState.OnStateEnter += objects => {Debug.Log("run状态进入!");};
        runState.OnStateUpdate += objects => {Debug.Log("run状态更新!");};
        runState.OnStateExit += objects => {Debug.Log("run状态退出!");};
    
        idleState.RegisterTransitionState(StateName.Movement,()=>{return speed>0;});
        moveStateMachine.RegisterTransitionState(StateName.Idling,()=>{return speed<=0;});

        walkState.RegisterTransitionState(StateName.Runing,()=>{return speed >=3;});
        runState.RegisterTransitionState(StateName.Walking,()=>{return speed <3 && speed>=1;});

        mainStateMachine.EnterState(null,null);
    }
}
