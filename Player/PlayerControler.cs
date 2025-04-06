using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;  // ÿ�����ƶ����پ���
    private Vector3 xRotation = Vector3.zero; // ��ɫ
    private Vector3 yRotation = Vector3.zero; // �ӽ�
    private Vector3 thrusterForce = Vector3.zero;

    private float camRotationTotal = 0f;
    [SerializeField]
    private float camRotationLimit = 85f;

    // ʹ��ջ��¼������ƶ�����
    private Stack<Vector3> movementHistory = new Stack<Vector3>();

    public void Move(Vector3 _velocity)
    {
        // ����ǰ���ƶ�������ջ
        if (_velocity != Vector3.zero)
        {
            movementHistory.Push(velocity);
        }
        velocity = _velocity;
    }

    public void Rotate(Vector3 _yRotation, Vector3 _xRotation)
    {
        xRotation = _xRotation;
        yRotation = _yRotation;
    }

    public void Thruster(Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }

    private void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime); // �������� FixedUpdate ��ʱ����
        }

        if (thrusterForce != Vector3.zero)
        {
            rb.AddForce(thrusterForce);
        }
    }

    private void PerformRotation()
    {
        if (yRotation != Vector3.zero)
        {
            rb.transform.Rotate(yRotation);
        }

        if (xRotation != Vector3.zero)
        {
            camRotationTotal += xRotation.x;
            camRotationTotal = Mathf.Clamp(camRotationTotal, -camRotationLimit, camRotationLimit); // �����ӽ�
            cam.transform.localEulerAngles = new Vector3(camRotationTotal, 0f, 0f);
        }
    }

    private void UndoLastMove()
    {
        // �������ʷ��¼��������һ���ƶ�
        if (movementHistory.Count > 0)
        {
            velocity = movementHistory.Pop(); // ������һ�ε��ƶ�
            velocity = -velocity; // �����ƶ�
        }
    }

    private void FixedUpdate() // ÿ����ִ��50��
    {
        PerformMovement();
        PerformRotation();
    }

    private void Update()
    {
        // ��⳷������������ Z ����
        if (Input.GetKeyDown(KeyCode.Z))
        {
            UndoLastMove();
        }
    }

}
