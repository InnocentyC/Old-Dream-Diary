using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移动设置")]
    public float speed = 5f;

    [Header("地图边界限制")]
    public GameObject background; 
    private float minX; // 左边
    private float maxX;  // 右边

    private Rigidbody2D rb;
    private SpriteRenderer mySpriteRenderer; 
    private Vector2 movement;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // --- 自动计算边界的核心逻辑 ---
        if (background != null)
        {
            // 1. 获取背景和主角的渲染器 (Renderer)
          
            Renderer bgRenderer = background.GetComponent<Renderer>();
            Renderer playerRenderer = GetComponent<Renderer>();

            if (bgRenderer != null && playerRenderer != null)
            {
                // 2. 获取主角的一半宽度 (extents.x 就是物体宽度的一半)
                float playerHalfWidth = playerRenderer.bounds.extents.x;

                // 3. 计算左边界：背景的最左边 + 主角半宽
                // bounds.min.x 是物体在世界坐标中最左边的点
                minX = bgRenderer.bounds.min.x + playerHalfWidth;

                // 4. 计算右边界：背景的最右边 - 主角半宽
                // bounds.max.x 是物体在世界坐标中最右边的点
                maxX = bgRenderer.bounds.max.x - playerHalfWidth;

                Debug.Log($"边界已自动计算: 左 {minX} / 右 {maxX}");
            }
            else
            {
                Debug.LogError("错误：背景或主角缺少 Renderer 组件！");
            }
        }
        else
        {
            Debug.LogError("请在 Inspector 面板中把【背景物体】拖进去！");
        }
    }

    void Update()
    {
        // 1. 接收输入，不处理物理移动

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        movement.x = horizontalInput;
        movement.y = 0;

        // 2. 动画控制：立即响应，无延迟
        bool isWalking = Mathf.Abs(horizontalInput) > 0.1f;
        animator.SetBool("IsWalking", isWalking);
        // 3. 处理人物翻转 
        if (horizontalInput != 0)
        {
            mySpriteRenderer.flipX = horizontalInput < 0;
        }

    }
    void FixedUpdate()
    {
        if (movement.x == 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);
        }

        // 限制位置范围
        Vector2 clampedPos = rb.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, minX, maxX);

        // 如果超出边界，强制修正位置
        if (Mathf.Abs(clampedPos.x - rb.position.x) > 0.01f)
        {
            rb.position = clampedPos;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }


}
