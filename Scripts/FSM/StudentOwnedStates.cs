using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StudentOwnedStates
{
    public class RestAndSleep : State<Student>
    {
        public override void Enter(Student entity)
        {
            entity.CurrecntLocation = Locations.SweetHome;
            entity.Stress = 0;

            entity.PrintText("이봐 심영이, 침대는 이제 내꺼다.");
            entity.PrintText("의사양반! 이 침대를 좀 지켜주시오!");
        }

        public override void Execute(Student entity)
        {
            entity.PrintText("Ah, 선생은 침대 없어요!");

            if(entity.Fatigue > 0)
            {
                entity.Fatigue -= 10;
            }

            else
            {
                entity.ChangeState(StudentStates.StudyHard);
            }    
        }

        public override void Exit(Student entity)
        {
            entity.PrintText("아으 시발 폭8이다!");
        }

        public override bool OnMessage(Student entity, Telegram telegram)
        {
            return false;
        }
    }

    public class StudyHard : State<Student>
    {
        public override void Enter(Student entity)
        {
            entity.CurrecntLocation = Locations.SweetHome;
            entity.Stress = 0;
            entity.PrintText("이 김두한이! 공부도 못했고, 머리도 나쁘고!");
        }

        public override void Execute(Student entity)
        {
            entity.PrintText("김두한, 이정재, 빡대가리에 해당");

            entity.Knowledge++;
            entity.Stress++;
            entity.Fatigue++;

            if(entity.Knowledge >=3 && entity.Knowledge <= 10)
            {
                int isExit = Random.Range(0, 2);
                if(isExit == 1 || entity.Knowledge == 10)
                {
                    entity.ChangeState(StudentStates.StudyHard);
                }
            }

            if(entity.Stress >= 20)
            {
                entity.ChangeState(StudentStates.PlayAGame);
            }

            if(entity.Fatigue >= 50)
            {
                entity.ChangeState(StudentStates.RestAndSleep);
            }
        }

        public override void Exit(Student entity)
        {
            entity.PrintText("Ah, 선생들 시험 보러 나오세요");
        }

        public override bool OnMessage(Student entity, Telegram telegram)
        {
            return false;
        }
    }

    public class TakeAExam : State<Student>
    {
        public override void Enter(Student entity)
        {
            entity.CurrecntLocation = Locations.LectureRoom;

            entity.PrintText("이런 건 아니야!!!!!!");
        }

        public override void Execute(Student entity)
        {
            int examScore = 0;

            if(entity.Knowledge == 10)
            {
                examScore = 10;
            }

            else
            {
                // randIndex가 지식 수치보다 낮으면 6 ~ 10점, 지식 수치보다 높으면 1 ~ 5점
                // 지식이 높을수록 높은 점수 받을 확률 높음
                int ranIndex = Random.Range(0, 10);
                examScore = ranIndex < entity.Knowledge ? Random.Range(6, 11) : Random.Range(1, 6);
            }

            // 시험 직후 지식은 0, 피로는 5 ~ 10 증가
            entity.Knowledge = 0;
            entity.Fatigue += Random.Range(5, 11);

            // 시험에서 획득한 점수를 TotalScore에 추가, 결과 출력
            entity.TotalScore += examScore;
            entity.PrintText($"Ah, 김두한, 이정재, ${examScore}점에 총점{entity.TotalScore}라면 당신들 병신입니까?");

            if(entity.TotalScore >= 100)
            {
                GameController.Stop(entity);
                return;
            }

            // 시험 점수에 따라 다음 행동 설정
            if(examScore <= 3)
            {
                // HitTheBottle 상태
                entity.ChangeState(StudentStates.HitTheBottle);
            }

            else if(examScore <= 7)
            {
                // StudyHard 상태
                entity.ChangeState(StudentStates.StudyHard);
            }

            else
            {
                // PlayAGame 상태
                entity.ChangeState(StudentStates.PlayAGame);
            }
        }

        public override void Exit(Student entity)
        {
            entity.PrintText("이렇게 죽어선 안돼 임마!");
        }

        public override bool OnMessage(Student entity, Telegram telegram)
        {
            return false;
        }

    }

    public class PlayAGame : State<Student>
    {
        public override void Enter(Student entity)
        {
            entity.CurrecntLocation = Locations.PCRoom;

            entity.PrintText("We are all crazy boy~");
        }

        public override void Execute(Student entity)
        {
            entity.PrintText("ㅎㅎㅎ 기분이 썩 괜찮아");

            int ranState = Random.Range(0, 10);

            if(ranState == 0 || ranState == 9)
            {
                entity.Stress += 20;

                // HitTheBottle 상태
                entity.ChangeState(StudentStates.HitTheBottle);
            }
            else
            {
                entity.Stress--;
                entity.Fatigue += 2;

                if(entity.Stress <= 0)
                {
                    // StudyHard 상태
                    entity.ChangeState(StudentStates.StudyHard);
                }
            }
        }

        public override void Exit(Student entity)
        {
            entity.PrintText("시공의 폭풍은 정말 최고야!");
        }

        public override bool OnMessage(Student entity, Telegram telegram)
        {
            return false;
        }
    }

    public class HitTheBottle : State<Student>
    {
        public override void Enter(Student entity)
        {
            entity.CurrecntLocation = Locations.Pub;

            entity.PrintText("이런 날에 술보다 좋은 친구가 어디있냐?");
        }

        public override void Execute(Student entity)
        {
            entity.PrintText("거 밴드 반주하라~");

            entity.Stress -= 5;
            entity.Fatigue += 5;

            if(entity.Stress <= 0 || entity.Fatigue >= 50)
            {
                // RestAndSleep 상태
                entity.ChangeState(StudentStates.RestAndSleep);
            }
        }

        public override void Exit(Student entity)
        {
            entity.PrintText("이 풍진 세상 좆까~");
        }

        public override bool OnMessage(Student entity, Telegram telegram)
        {
            return false;
        }
    }

    public class GlobalMessageReceive : State<Student>
    {
        public override void Enter(Student entity)
        {
            
        }

        public override void Execute(Student entity)
        {
            
        }

        public override void Exit(Student entity)
        {
            
        }

        public override bool OnMessage(Student entity, Telegram telegram)
        {
            entity.PrintText($"Receive Message : sender({telegram.sender}, receiver({telegram.receiver})");

            if(telegram.message.Equals("GO_PCROOM"))
            {
                if (entity.CurrentState == StudentStates.PlayAGame)
                {
                    entity.PrintText($"Send Message {telegram.receiver}님이 {telegram.sender}님에게 ALREADY_PCROOM 전송");
                    MessageDispatcher.Instance.DispatchMessage(0, telegram.receiver, telegram.sender, "ALREADY_PCROOM");
                }

                else
                {
                    int stateIndex = Random.Range(0, 2);

                    if(stateIndex == 0)
                    {
                        entity.PrintText($"Send Message {telegram.receiver}님이 {telegram.sender}님에게 GO_PCROOM 전송");
                        MessageDispatcher.Instance.DispatchMessage(0, telegram.receiver, telegram.sender, "GO_PCROOM");

                        // PlayAGame 상태
                        entity.ChangeState(StudentStates.PlayAGame);
                    }

                    else
                    {
                        entity.PrintText($"Send Message {telegram.receiver}님이 {telegram.sender} SORRY 전송");
                        MessageDispatcher.Instance.DispatchMessage(0, telegram.receiver, telegram.sender, "SORRY");
                    }
                }

                return true;
            }

            return false;
        }
    }
}
