using System.Collections;
using System.Collections.Generic;

public class QuestData
{
    public string questName;
    public int[] npcId;

    public QuestData(string name, int[] npc) // 구조체 생성을 위해 매개변수 생성자를 작성 
    {
        questName = name;
        npcId = npc;
    }
}
