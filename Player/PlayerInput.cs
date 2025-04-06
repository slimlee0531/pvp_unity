using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 8f;
    [SerializeField]
    private float thrusterForce = 20f;  // 推力
    [SerializeField]
    private PlayerControler controler;
    [SerializeField]
    private ConfigurableJoint joint;

    // 使用队列存储按键操作
    private Queue<Vector3> movementQueue = new Queue<Vector3>();
    private Queue<Vector3> rotationQueue = new Queue<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // 隐藏鼠标
    }

    // Update is called once per frame
    void Update()
    {
        // 缓存移动输入
        float xMov = Input.GetAxisRaw("Horizontal");
        float yMov = Input.GetAxisRaw("Vertical");
        Vector3 velocity = (transform.right * xMov + transform.forward * yMov).normalized * speed;
        movementQueue.Enqueue(velocity);

        // 缓存旋转输入
        float xMouse = Input.GetAxisRaw("Mouse X");
        float yMouse = Input.GetAxisRaw("Mouse Y");
        Vector3 yRotation = new Vector3(0f, xMouse, 0f) * lookSensitivity;
        Vector3 xRotation = new Vector3(-yMouse, 0f, 0f) * lookSensitivity;
        rotationQueue.Enqueue(new Vector3(yRotation.y, xRotation.x, 0));

        // 处理 Thruster 力
        Vector3 force = Vector3.up * thrusterForce;
        if (Input.GetButton("Jump"))
        {
            force = Vector3.up * thrusterForce;
            joint.yDrive = new JointDrive
            {
                positionSpring = 0f,
                positionDamper = 0f,
                maximumForce = 0f,
            };
        }
        else
        {
            joint.yDrive = new JointDrive
            {
                positionSpring = 20f,
                positionDamper = 0f,
                maximumForce = 40f,
            };
        }
        controler.Thruster(force);

        // 执行队列中的操作
        ProcessInputs();
    }

    // 按队列顺序处理输入
    private void ProcessInputs()
    {
        // 处理移动队列
        while (movementQueue.Count > 0)
        {
            Vector3 velocity = movementQueue.Dequeue();
            controler.Move(velocity);
        }

        // 处理旋转队列
        while (rotationQueue.Count > 0)
        {
            Vector3 rotation = rotationQueue.Dequeue();
            Vector3 yRotation = new Vector3(0f, rotation.x, 0f);
            Vector3 xRotation = new Vector3(rotation.y, 0f, 0f);
            controler.Rotate(yRotation, xRotation);
        }
    }
}
