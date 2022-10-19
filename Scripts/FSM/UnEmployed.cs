using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnEmployedStates { RestAndSleep = 0, PlayAGame, HitTheBottle, VisitBathroom, Global }

public class UnEmployed : BaseGameEntity
{
    private int bored;
    private int stress;
    private int fatigue;
    private Locations currentLocation;

    // UnEmployed가 가지고 있는 모든 상태, 상태 관리자
    private State<UnEmployed>[] states;
    private StateMachine<UnEmployed> stateMachine;

    public int Bored
    {
        set => bored = Mathf.Max(0, value);
        get => bored;
    }

    public int Stress
    {
        set => stress = Mathf.Max(0, value);
        get => stress;
    }

    public int Fatigue
    {
        set => fatigue = Mathf.Max(0, value);
        get => fatigue;
    }

    public Locations CurrentLocation
    {
        set => currentLocation = value;
        get => currentLocation;
    }

    public UnEmployedStates CurrentState { private set; get; } // 현재 상태

    public override void Setup(string name)
    {
        // 기반 클래스의 Setup 메소드의 호출(ID, 이름, 색상 설정)
        base.Setup(name);

        gameObject.name = $"{ID:D2}_UnEmployed_{name}";

        // UnEmployed가 가질 수 있는 상태 개수만큼 메모리 할당, 각 상태에 클래스 메모리 할당
        states = new State<UnEmployed>[5];
        states[(int)UnEmployedStates.RestAndSleep] = new UnEmployedOwnedStates.RestAndSleep();
        states[(int)UnEmployedStates.PlayAGame] = new UnEmployedOwnedStates.PlayAGame();
        states[(int)UnEmployedStates.HitTheBottle] = new UnEmployedOwnedStates.HitTheBottle();
        states[(int)UnEmployedStates.VisitBathroom] = new UnEmployedOwnedStates.VisitBathroom();
        states[(int)UnEmployedStates.Global] = new UnEmployedOwnedStates.StateGlobal();

        // 상태를 관리하는 StateMachine에 메모리 할당, 첫 상태를 설정
        stateMachine = new StateMachine<UnEmployed>();
        stateMachine.Setup(this, states[(int)UnEmployedStates.RestAndSleep]);
        stateMachine.SetGlobalState(states[(int)UnEmployedStates.Global]);

        bored = 0;
        stress = 0;
        fatigue = 0;
        currentLocation = Locations.SweetHome;
    }

    public override void Updated()
    {
        stateMachine.Execute();
    }

    public void ChangeState(UnEmployedStates newState)
    {
        CurrentState = newState;

        stateMachine.ChangeState(states[(int)newState]);
    }

    public void RevertToPreviousState()
    {
        stateMachine.RevertToPreviousState();
    }

    public override bool HandleMessage(Telegram telegram)
    {
        // return false;
        return stateMachine.HandleMessage(telegram);
    }
}
