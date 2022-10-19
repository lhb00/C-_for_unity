using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnEmployedOwnedStates
{
    public class RestAndSleep : State<UnEmployed>
    {
        public override void Enter(UnEmployed entity)
        {
            entity.CurrentLocation = Locations.SweetHome;
            entity.Stress = 0;
            entity.Fatigue = 0;

            entity.PrintText("이 집 침대는 정말 일품이란 말이야");
        }

        public override void Execute(UnEmployed entity)
        {
            string state = Random.Range(0, 2) == 0 ? "거 자는데 왜 이렇게 시끄러워~" : "썅 두한이~";
            entity.PrintText(state);

            // 지루함은 70%의 확률로 증가, 30% 확률로 감소
            entity.Bored += Random.Range(0, 100) < 70 ? 1 : -1;

            if(entity.Bored >= 4)
            {
                entity.ChangeState(UnEmployedStates.PlayAGame);

                string receiver = "의사양반";
                entity.PrintText($"Send Message : {entity.EntityName}님이 {receiver}에게 GO_PCROOM 전송");
                MessageDispatcher.Instance.DispatchMessage(0, entity.EntityName, receiver, "GO_PCROOM");

                // PlayAGame 상태
                entity.ChangeState(UnEmployedStates.PlayAGame);
            }
        }

        public override void Exit(UnEmployed entity)
        {
            entity.PrintText("두한이가 알아서 해라~");
        }

        public override bool OnMessage(UnEmployed entity, Telegram telegram)
        {
            return false;
        }
    }

    public class PlayAGame : State<UnEmployed>
    {
        public override void Enter(UnEmployed entity)
        {
            entity.CurrentLocation = Locations.PCRoom;
            entity.PrintText("지금부터 야인들의 마피아 게임을 시작한다.");
        }

        public override void Execute(UnEmployed entity)
        {
            entity.PrintText("마피아는 총 3명이다.");

            int randState = Random.Range(0, 10);
            if (randState == 0 || randState == 9)
            {
                entity.Stress += 20;

                // HitTheBottle 상태
                entity.ChangeState(UnEmployedStates.HitTheBottle);
            }

            else
            {
                entity.Bored--;
                entity.Fatigue += 2;

                if(entity.Bored <= 0 || entity.Fatigue >= 50)
                {
                    // RestAndSleep 상태
                    entity.ChangeState(UnEmployedStates.RestAndSleep);
                }
            }
        }

        public override void Exit(UnEmployed entity)
        {
            entity.PrintText("야피아 게임 4편은 없다");
        }

        public override bool OnMessage(UnEmployed entity, Telegram telegram)
        {
            return false;
        }
    }

    public class HitTheBottle : State<UnEmployed>
    {
        public override void Enter(UnEmployed entity)
        {
            entity.CurrentLocation = Locations.Pub;
            entity.PrintText("이 술집이지만은, 나는 오늘도 고기!만! 먹을거야!");
        }

        public override void Execute(UnEmployed entity)
        {
            entity.PrintText("술 한잔 했습니다... 심영물이 잘 안돼도 좋습니다.");

            entity.Bored += Random.Range(0, 2) == 0 ? 1 : -1;

            entity.Stress -= 4;
            entity.Fatigue += 4;

            if(entity.Stress <= 0 || entity.Fatigue >= 50)
            {
                // RestAndSleep 상태로 변경
                entity.ChangeState(UnEmployedStates.RestAndSleep);
            }
        }

        public override void Exit(UnEmployed entity)
        {
            entity.PrintText("두한아, 일어서라, 일어서!");
        }

        public override bool OnMessage(UnEmployed entity, Telegram telegram)
        {
            return false;
        }
    }

    public class VisitBathroom : State<UnEmployed>
    {
        public override void Enter(UnEmployed entity)
        {
            entity.PrintText("본인 심영 배탈 났다구요!");
        }

        public override void Execute(UnEmployed entity)
        {
            entity.PrintText("Ah, 시발 존나 더럽네;;");

            // 바로 직전 상태로 되돌아감
            entity.RevertToPreviousState();
        }

        public override void Exit(UnEmployed entity)
        {
            entity.PrintText("헣헣헣헣 다 나았다!");
        }

        public override bool OnMessage(UnEmployed entity, Telegram telegram)
        {
            return false;
        }
    }

    // 전역 상태 : StateGlobal
    // 현재 상태와 별개로 실행, 화장실을 갈지 말지 결정
    public class StateGlobal : State<UnEmployed>
    {
        public override void Enter(UnEmployed entity)
        {
            
        }

        public override void Execute(UnEmployed entity)
        {
            if(entity.CurrentState == UnEmployedStates.VisitBathroom)
            {
                return;
            }

            int bathroomState = Random.Range(0, 100);
            if(bathroomState < 10)
            {
                entity.ChangeState(UnEmployedStates.VisitBathroom);
            }
        }

        public override void Exit(UnEmployed entity)
        {
            
        }

        public override bool OnMessage(UnEmployed entity, Telegram telegram)
        {
            return false;
        }
    }
}
