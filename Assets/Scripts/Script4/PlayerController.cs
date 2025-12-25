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
        bool isWalking = Mathf.Abs(horizontalInput) > 0.1f;
        animator.SetBool("IsWalking", isWalking);
        // 3. �������﷭ת 
        if (horizontalInput != 0)
        {
            mySpriteRenderer.flipX = horizontalInput > 0;
        }

    }
    void FixedUpdate()
    {
        Vector2 targetPos = rb.position;
        
        // 计算目标位置
        if (movement.x != 0)
        {
            targetPos += Vector2.right * movement.x * speed * Time.fixedDeltaTime;
        }
        
        // 边界限制
        targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
        
        // 使用MovePosition完全控制移动
        rb.MovePosition(targetPos);
    }


}
