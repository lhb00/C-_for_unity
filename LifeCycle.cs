using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCycle : MonoBehaviour
{
    void Awake() // ���� ������Ʈ ���� �� ��, �ѹ��� ����
    {
        Debug.Log("�÷��̾� �����Ͱ� �غ�Ǿ����ϴ�.");
    }

    void OnEnable() //������Ʈ Ȱ��ȭ
    {
        Debug.Log("�÷��̾ �α����߽��ϴ�.");
    }

    void Start() // ������Ʈ ���� ����, ���� ����
    {
        Debug.Log("��� ��� ì����ϴ�.");
    }

    void FixedUpdate() //���� ���� ������Ʈ, ������ �ֱ�� CPU�� ���
    {
        Debug.Log("�̵�~");
    }
    
    void Update() //���� ���� ������Ʈ, ���� FixedUpdate()�� 60������ �ֱ��̱� ������ �� Update()�� �� ���� �����.
    {
        Debug.Log("���� ���");
    }

    void LateUpdate() // ��� ������Ʈ�� ���� ����
    {
        Debug.Log("����ġ ȹ��.");
    }

    void OnDisable() //������Ʈ ��Ȱ��ȭ
    {
        Debug.Log("�÷��̾ �α׾ƿ��߽��ϴ�.");
    }

    void OnDestroy() // ���� ��ü
    {
        Debug.Log("�÷��̾� �����͸� �����Ͽ����ϴ�.");
    }
}
