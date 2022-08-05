using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCycle : MonoBehaviour
{
    void Update() //���� ���� ������Ʈ, ���� FixedUpdate()�� 60������ �ֱ��̱� ������ �� Update()�� �� ���� �����.
    {
        if (Input.anyKeyDown)//���� �� �Է��� ����. anyKeyDown�� �ƹ� �Է��� ���ʷ� ���� �� true
            Debug.Log("�÷��̾ �ƹ� Ű�� �������ϴ�.");
        if (Input.GetKeyDown(KeyCode.Return))//���� �� �Է��� ����. GetKeyDown�� Ű���� �Է��� ������ true
            Debug.Log("�������� �����Ͽ����ϴ�.");

        if (Input.GetMouseButtonDown(0)) 
            Debug.Log("�̻��� �߻�!");

        if (Input.GetMouseButton(0)) //���콺 ��ư �Է� ������ true
            Debug.Log("�̻��� ������ ��...");

        if (Input.GetMouseButtonUp(0))
            Debug.Log("���� �̻��� �߻�!!");

        if (Input.GetKey(KeyCode.LeftArrow))
            Debug.Log("�������� �̵� ��");

        if (Input.GetKeyUp(KeyCode.RightArrow))
            Debug.Log("������ �̵��� ���߾����ϴ�.");

        if (Input.GetButtonDown("SuperFire"))
            Debug.Log("�ʻ��!");

        if (Input.GetButton("Fire1")) // Input ��ư �Է��� ������ true
            Debug.Log("���� ������ ��...");

        if (Input.GetButtonUp("Fire1"))
            Debug.Log("���� ����!!");
        
        if (Input.GetButton("Horizontal"))
            Debug.Log("Ⱦ �̵� ��..." + Input.GetAxisRaw("Horizontal")); //����, ���� ��ư �Է��� ������ float, raw�� �߰��ϸ� ������ 1 ������

        if (Input.GetButton("Vertical"))
        {
            Debug.Log("�� �̵� ��..." + Input.GetAxisRaw("Vertical"));
        }
    }
}
