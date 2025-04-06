using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;  // 每秒钟移动多少距离
    private Vector3 xRotation = Vector3.zero; // 角色
    private Vector3 yRotation = Vector3.zero; // 视角
    private Vector3 thrusterForce = Vector3.zero;

    private float camRotationTotal = 0f;
    [SerializeField]
    private float camRotationLimit = 85f;

    // 使用栈记录最近的移动操作
    private Stack<Vector3> movementHistory = new Stack<Vector3>();

    public void Move(Vector3 _velocity)
    {
        // 将当前的移动操作入栈
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
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime); // 相邻两次 FixedUpdate 的时间间隔
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
            camRotationTotal = Mathf.Clamp(camRotationTotal, -camRotationLimit, camRotationLimit); // 限制视角
            cam.transform.localEulerAngles = new Vector3(camRotationTotal, 0f, 0f);
        }
    }

    private void UndoLastMove()
    {
        // 如果有历史记录，则撤销上一次移动
        if (movementHistory.Count > 0)
        {
            velocity = movementHistory.Pop(); // 撤销上一次的移动
            velocity = -velocity; // 反向移动
        }
    }

    private void FixedUpdate() // 每秒钟执行50次
    {
        PerformMovement();
        PerformRotation();
    }

    private void Update()
    {
        // 检测撤销操作（按下 Z 键）
        if (Input.GetKeyDown(KeyCode.Z))
        {
            UndoLastMove();
        }
    }

}
