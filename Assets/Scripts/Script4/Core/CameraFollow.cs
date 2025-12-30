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

        // 1. 计算玩家移动量
        Vector3 playerMovement = target.position - lastPlayerPos;
        lastPlayerPos = target.position;

        // 2. 【核心逻辑】判断相机是否在边界
        bool isAtLeftEdge = transform.position.x <= minX;  // 相机在左边界
        bool isAtRightEdge = transform.position.x >= maxX;  // 相机在右边界

        // 3. 【核心逻辑】在边界时的特殊处理
        if (isAtLeftEdge || isAtRightEdge)
        {
            // 计算人物相对于相机中心的位置
            float playerRelativeToCam = target.position.x - transform.position.x;

            // 在左边界：人物往左走，相机不动；人物往右走，只有超过中心才动相机
            if (isAtLeftEdge)
            {
                if (playerMovement.x < 0)
                {
                    // 人物往左走（继续往外），相机不动
                    return;
                }
                else if (playerRelativeToCam < 0)
                {
                    // 人物往右走但还没到中心（中心位置是0），相机不动
                    return;
                }
            }

            // 在右边界：人物往右走，相机不动；人物往左走，只有超过中心才动相机
            if (isAtRightEdge)
            {
                if (playerMovement.x > 0)
                {
                    // 人物往右走（继续往外），相机不动
                    return;
                }
                else if (playerRelativeToCam > 0)
                {
                    // 人物往左走但还没到中心（中心位置是0），相机不动
                    return;
                }
            }
        }

        // 4. 正常跟随逻辑（人物动多少，相机动多少）
        Vector3 newCameraPos = transform.position + playerMovement;
        newCameraPos.y = fixedY;

        // 5. 限制在边界内
        if (maxX >= minX)
        {
            newCameraPos.x = Mathf.Clamp(newCameraPos.x, minX, maxX);
        }
        else
        {
            newCameraPos.x = background.bounds.center.x;
        }

        transform.position = newCameraPos;
    }
}
