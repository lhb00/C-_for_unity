using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    Vector3 target = new Vector3(8, 1.5f, 0);

    void Update() // �� ������ �Լ��� ��� vector3 Ŭ�������� �⺻ �����Ǵ� �Լ���.
    {   //1.MoveTowards
        /*transform.position = Vector3.MoveTowards(transform.position
            , target, 2f); // ����̵�, �Ű������� (������ġ, ��ǥ��ġ, �ӵ�)��.*/

        //2.SmoothDamp
        /*Vector3 velo = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position
            , target, ref velo, 1f); // �ε巯�� ���� �̵�, �Ű������� (������ġ, ��ǥ��ġ, �����ӵ�, �ӵ�)��.
        // ref : ���� ����, �ǽð����� �ٲ�� �� ���� O */

        //3.Lerp
        /*transform.position = Vector3.Lerp(transform.position
            , target, 0.05f); // ���� ����, SmoothDamp ���� �� ���ӽð�. ������ �Ű������� ����Ͽ� �ӵ� ����. �ִ밪 1.*/

        //4.SLerp
        transform.position = Vector3.Slerp(transform.position
            , target, 0.1f); //���� ���� ����, ȣ�� �׸��� �̵�
    }
}