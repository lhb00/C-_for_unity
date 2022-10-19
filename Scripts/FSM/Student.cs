using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StudentStates { RestAndSleep = 0, StudyHard, TakeAExam, PlayAGame, HitTheBottle, GMessageReceive }
public class Student : BaseGameEntity
{
    private int knowledge;
    private int stress;
    private int fatigue;
    private int totalScore;
    private Locations currentLocation;
    private StudentStates currentState;

    // student가 가지고 있는 모든 상태, 현재 상태
    private State<Student>[] states;
    // private State<Student> currentState;
    private StateMachine<Student> stateMachine; // 상태를 관리하는 것은 StateMachine에게 위임하기 때문에 Student 본인이 가질 수 있는 상태를 제외하고 상태와 관련된 변수, 메소드 전부 삭제 \

    public StudentStates CurrentState => currentState;

    public int Knowledge
    {
        set => knowledge = Mathf.Max(0, value); // 0과 value 중 더 큰 값을 Speed에 저장
        get => knowledge;
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

    public int TotalScore
    {
        set => totalScore = Mathf.Clamp(value, 0 ,100); // value 값의 범위가 0 < value < 100 이면 value 값을 반환, 0보다 작으면 0, 100보다 크면 100 반환
        get => totalScore;
    }

    public Locations CurrecntLocation
    {
        set => currentLocation = value;
        get => currentLocation;
    }

    public override void Setup(string name)
    {
        // 기반 클래스의 Setup 메소드 호출(ID, 이름, 색상 설정)
        base.Setup(name);

        // 생성되는 오브젝트 이름 설정
        gameObject.name = $"{ID:D2}_Student_{name}";

        // Student가 가질 수 있는 상태 개수만큼 메모리 할당, 각 상태에 클래스 메모리 할당
        states = new State<Student>[6];
        states[(int)StudentStates.RestAndSleep] = new StudentOwnedStates.RestAndSleep();
        states[(int)StudentStates.StudyHard] = new StudentOwnedStates.StudyHard();
        states[(int)StudentStates.TakeAExam] = new StudentOwnedStates.TakeAExam();
        states[(int)StudentStates.PlayAGame] = new StudentOwnedStates.PlayAGame();
        states[(int)StudentStates.HitTheBottle] = new StudentOwnedStates.HitTheBottle();
        states[(int)StudentStates.GMessageReceive] = new StudentOwnedStates.GlobalMessageReceive();

        // 현재 상태를 백병원에서 쉬는 "RestAndSleep" 상태로 설정
        // currentState = states[(int)StudentStates.RestAndSleep];

        // 상태를 관리하는 StateMachine에 메모리 할당, 첫 상태 결정
        stateMachine = new StateMachine<Student>();
        stateMachine.Setup(this, states[(int)StudentStates.RestAndSleep]);

        // 전역 상태 결정
        stateMachine.SetGlobalState(states[(int)StudentStates.GMessageReceive]);

        knowledge = 0;
        stress = 0;
        fatigue = 0;
        totalScore = 0;
        currentLocation = Locations.SweetHome;

        PrintText("나 김두한이다!");
    }

    public override void Updated()
    {

        /* if(currentState != null)
        {
            currentState.Execute(this);
        } */

        stateMachine.Execute();
    }

    public void ChangeState(StudentStates newState)
    {
        /* // 새로 바꾸려는 상태가 null이면 상태 변경 X
        if (states[(int)newState] == null) return;

        // 현재 재생중인 상태가 있으면 Exit() 실행
        if(currentState != null)
        {
            currentState.Exit(this);
        }

        // 새로운 상태로 변경, 바뀐 상태의 Enter() 실행
        currentState = states[(int)newState];
        currentState.Enter(this); */

        currentState = newState;

        stateMachine.ChangeState(states[(int)newState]);
    }

    public override bool HandleMessage(Telegram telegram)
    {
        // return false;
        return stateMachine.HandleMessage(telegram);
    }
}
