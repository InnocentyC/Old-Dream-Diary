using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [Header("设置")]
    public Transform target;          // 主角
    public SpriteRenderer background; // 背景


    [Header("玩家引用")]
    public PlayerController playerController; // 用于获取玩家速度

    [Header("像素完美设置")]
    // 你的图片素材的 PPU 
    public float PPU = 100f;

    private float camWidth;
    private float minX, maxX;
    private float fixedY; // 固定的 Y 轴

    private Vector3 lastPlayerPos; // 记录上一帧玩家位置

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        float camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        if (background != null)
        {
            Bounds bgBounds = background.bounds;

            // 1. 计算相机可以移动的左右边界
            // 确保相机不会显示背景外的区域
            minX = bgBounds.min.x + camWidth;
            maxX = bgBounds.max.x - camWidth;
            
            Debug.Log($"相机边界计算: 背景范围({bgBounds.min.x:F2}, {bgBounds.max.x:F2}), 相机宽度({camWidth:F2}), 可移动范围({minX:F2}, {maxX:F2})");

            // 2. 【关键】锁定高度为背景的中心高度
            // 这样相机永远正对着背景图的中间
            fixedY = bgBounds.center.y;
        }
        else
        {
            Debug.LogError("请把背景图片拖给相机的 CameraFollow 脚本！");
        }
        
        // 初始化玩家位置记录并设置相机初始位置
        if (target != null)
        {
            lastPlayerPos = target.position;
            
            // 设置相机初始位置在人物正中
            Vector3 initialPos = new Vector3(target.position.x, fixedY, transform.position.z);
            initialPos.x = Mathf.Clamp(initialPos.x, minX, maxX);
            transform.position = initialPos;
            
            Debug.Log($"相机初始位置设置: 人物位置{target.position.x:F2} → 相机位置{initialPos.x:F2}");
        }
    }

    void LateUpdate()
    {
        if (target == null || background == null) return;

        // 1. 确定目标位置
        // 我们希望相机去哪里？去主角的X，固定的Y，原本的Z
        Vector3 targetPos = new Vector3(target.position.x, fixedY, transform.position.z);



        // 2. 限制目标位置 (Clamp)
        // 【关键优化】在SmoothDamp之前就限制目标位置，确保不会超出边界
        if (maxX >= minX)
        {
            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
            
            // 调试输出
            if (targetPos.x == minX || targetPos.x == maxX)
            {
                Debug.Log($"相机到达边界，目标位置: {targetPos.x:F2}");
            }
        }
        else
        {
            // 背景太小，相机固定在中心
            targetPos.x = background.bounds.center.x;
            Debug.LogWarning("背景比相机视口还小，相机固定在中心");
        }


        // 3. 【完全同步】直接跟随，人物动多少相机就动多少
        // 计算玩家移动了多少，相机就移动多少
        Vector3 playerMovement = target.position - lastPlayerPos;
        
        // 调试：输出相机跟随的移动量
        if (Mathf.Abs(playerMovement.x) > 0.001f)
        {
            Debug.Log($"相机跟随原始: 玩家移动量={playerMovement.x:F6}, 玩家位置={target.position.x:F6}, 上一帧={lastPlayerPos.x:F6}");
        }
        
        // 计算相机的新位置（直接应用相同的移动量）
        Vector3 newCameraPos = transform.position + playerMovement;
        newCameraPos.y = fixedY; // Y轴保持固定
        
        // 4. 【优先处理边界】如果超出边界，先限制移动
        Vector3 clampedPos = newCameraPos;
        if (maxX >= minX)
        {
            float originalX = clampedPos.x;
            clampedPos.x = Mathf.Clamp(clampedPos.x, minX, maxX);
            
            // 如果被边界限制了，记录实际移动量
            if (Mathf.Abs(clampedPos.x - originalX) > 0.001f)
            {
                Debug.Log($"相机被边界限制: 计算位置{originalX:F2} → 限制后{clampedPos.x:F2}");
            }
        }
        else
        {
            clampedPos.x = background.bounds.center.x;
        }
        
        // 5. 【测试】禁用像素对齐，直接使用限制后的位置
        // 6. 最终位置（人物停止，相机也立即停止）
        Vector3 finalCameraPos = clampedPos;

        transform.position = finalCameraPos;
        
        // 7. 更新玩家位置记录
        lastPlayerPos = target.position;
    }
}
