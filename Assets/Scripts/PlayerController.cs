
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("�ƶ�����")]
    public float speed = 5f;

    [Header("��ͼ�߽�����")]
    public GameObject background; 
    private float minX; // ���
    private float maxX;  // �ұ�

    private Rigidbody2D rb;
    private SpriteRenderer mySpriteRenderer; 
    private Vector2 movement;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // --- �Զ�����߽�ĺ����߼� ---
        if (background != null)
        {
            // 1. ��ȡ���������ǵ���Ⱦ�� (Renderer)
          
            Renderer bgRenderer = background.GetComponent<Renderer>();
            Renderer playerRenderer = GetComponent<Renderer>();

            if (bgRenderer != null && playerRenderer != null)
            {
                // 2. ��ȡ���ǵ�һ����� (extents.x ����������ȵ�һ��)
                float playerHalfWidth = playerRenderer.bounds.extents.x;

                // 3. ������߽磺����������� + ���ǰ��
                // bounds.min.x ����������������������ߵĵ�
                minX = bgRenderer.bounds.min.x + playerHalfWidth;

                // 4. �����ұ߽磺���������ұ� - ���ǰ��
                // bounds.max.x ���������������������ұߵĵ�
                maxX = bgRenderer.bounds.max.x - playerHalfWidth;

                Debug.Log($"�߽����Զ�����: �� {minX} / �� {maxX}");
            }
            else
            {
                Debug.LogError("���󣺱���������ȱ�� Renderer �����");
            }
        }
        else
        {
            Debug.LogError("���� Inspector ����аѡ��������塿�Ͻ�ȥ��");
        }
    }

    void Update()
    {
        // 1. �������룬�����������ƶ�

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        movement.x = horizontalInput;
        movement.y = 0;

        // 2. �������ƣ�������Ӧ�����ӳ�
        bool isWalking = horizontalInput != 0;
        animator.SetBool("IsWalking", isWalking);
        // 3. �������﷭ת 
        if (horizontalInput != 0)
        {
            mySpriteRenderer.flipX = horizontalInput > 0;
        }

    }
    void FixedUpdate()
    {
        // ֱ�������ٶȣ����⻬��Ч��
        Vector2 velocity = new Vector2(movement.x * speed, rb.velocity.y);

        // Ӧ���ٶ�
        rb.velocity = velocity;

        // ����λ�÷�Χ
        Vector2 clampedPos = rb.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, minX, maxX);

        // ��������߽磬ǿ������λ��
        if (Mathf.Abs(clampedPos.x - rb.position.x) > 0.01f)
        {
            rb.position = clampedPos;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }


}
