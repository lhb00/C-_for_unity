using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> where T : BaseGameEntity // class
{
    private T ownerEntity; // StateMachine의 소유주
    private State<T> currentState; //현재 상태
    // 상태 블립 : 에이전트의 상태가 변경될 때, 바로 직전의 상태로 복귀한다는 조건하에 다른 상태로 변경하는 것
    private State<T> previousState; // 이전 상태
    private State<T> globalState; // 전역 상태
    // 에이전트가 어떤 상태를 수행할 때, 모든 상태에서 지속적으로 업데이트 되어야 하는 조건 논리가 있을 때 이 논리를 소유하고 있는 상태

    public void Setup(T owner, State<T> entryState)
    {
        ownerEntity = owner;
        currentState = null;
        previousState = null;
        globalState = null;

        // entryState 상태로 상태 변경
        ChangeState(entryState);
    }

    public void Execute()
    {
        if(currentState != null)
        {
            currentState.Execute(ownerEntity);
        }
    }

    public void ChangeState(State<T> newState)
    {
        // 새로 바꾸려는 상태가 null이면 상태 변경 X
        if (newState == null) return;

        // 현재 재생중인 상태가 있으면 Exit() 실행
        if(currentState != null)
        {
            // 상태가 변경되면 현재 상태는 이전 상태가 되므로 previousState에 저장
            previousState = currentState;

            currentState.Exit(ownerEntity);
        }

        // 새로운 상태로 변경, 바뀐 상태의 Enter() 실행
        currentState = newState;
        currentState.Enter(ownerEntity);
    }

    public void SetGlobalState(State<T> newState)
    {
        globalState = newState;
    }

    public void RevertToPreviousState()
    {
        ChangeState(previousState);
    }

    public bool HandleMessage(Telegram telegram)
    {
        if(globalState != null && globalState.OnMessage(ownerEntity, telegram))
        {
            return true;
        }

        if(currentState != null && currentState.OnMessage(ownerEntity, telegram))
        {
            return true;
        }

        return false;
    }
}
