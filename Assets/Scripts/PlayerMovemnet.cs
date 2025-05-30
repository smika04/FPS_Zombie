using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    bool isMoving;

    private Vector3 lastPosition = Vector3.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // ��������, �� �������� �� ����
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // ���� �� ���� � ���� ���� � ��������
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // ���������� ��������� �� ����
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // ������ ��������� ����� ��� �� ������
        velocity.y += gravity * Time.deltaTime;

        // ��'������ �������������� ��� � ���������
        Vector3 finalMove = move * speed;
        finalMove.y = velocity.y;

        controller.Move(finalMove * Time.deltaTime);

        // �������
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // ����������, �� �������� ��������
        isMoving = (lastPosition != transform.position && isGrounded);
        lastPosition = transform.position;
    }

}
