using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDataBase
{
    static readonly EntityDataBase instance = new EntityDataBase();
    public static EntityDataBase Instance => instance;

    // 모든 에이전트의 정보가 저장되는 자료구조
    // <에이전트 이름, BaseGameEntity 타입의 에이전트 정보>
    private Dictionary<string, BaseGameEntity> entityDictionary;

    public void Setup()
    {
        entityDictionary = new Dictionary<string, BaseGameEntity>();
    }

    // 에이전트 등록
    public void RegisterEntity(BaseGameEntity newEntity)
    {
        entityDictionary.Add(newEntity.EntityName, newEntity);
    }

    // 에이전트 이름을 기준으로 에이전트 정보 검색(BaseGameEntity)
    public BaseGameEntity GetEntityFromID(string entityName)
    {
        foreach (KeyValuePair<string, BaseGameEntity> entity in entityDictionary)
        {
            if(entity.Key == entityName)
            {
                return entity.Value;
            }

        }

        return null;
    }

    // 에이전트 삭제 \
    public void RemoveEntity(BaseGameEntity removeEntity)
    {
        entityDictionary.Remove(removeEntity.EntityName);
    }
}
