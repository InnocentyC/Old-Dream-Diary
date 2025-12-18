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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>(); 

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
   
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = 0;

        // 2. 处理人物翻转 
        if (movement.x != 0)
        {
           
            mySpriteRenderer.flipX = movement.x < 0;
        }
        
    }
    void FixedUpdate()
    {
        // 3. 计算原本应该移动到的位置
        Vector2 targetPos = rb.position + movement * speed * Time.fixedDeltaTime;

        // 4. 限制范围 (Clamp)
        
        targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);

        // 5. 最终应用移动
        // 还要锁定 Y 轴位置，防止物理碰撞导致被挤上去
        // 如果你的场景是纯平地，可以保持当前的 Y 轴位置
        targetPos.y = rb.position.y;

        rb.MovePosition(targetPos);
    }


}
