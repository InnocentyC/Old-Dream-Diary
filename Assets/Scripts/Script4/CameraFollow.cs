using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [Header("设置")]
    public Transform target;          // 主角
    public SpriteRenderer background; // 背景


    [Header("平滑度 (越小越快，0.1-0.3 最佳)")]
    public float smoothTime = 0.2f;   // 替换了 smoothSpeed

    [Header("像素完美设置")]
    // 你的图片素材的 PPU 
    public float PPU = 100f;

    private float camWidth;
    private float minX, maxX;
    private float fixedY; // 固定的 Y 轴

    // SmoothDamp 需要的中间变量，用来记录当前速度
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        float camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        if (background != null)
        {
            Bounds bgBounds = background.bounds;

            // 1. 计算左右移动的极限
            minX = bgBounds.min.x + camWidth;
            maxX = bgBounds.max.x - camWidth;

            // 2. 【关键】锁定高度为背景的中心高度
            // 这样相机永远正对着背景图的中间
            fixedY = bgBounds.center.y;
        }
        else
        {
            Debug.LogError("请把背景图片拖给相机的 CameraFollow 脚本！");
        }
    }

    void LateUpdate()
    {
        if (target == null || background == null) return;

        // 1. 确定目标位置
        // 我们希望相机去哪里？去主角的X，固定的Y，原本的Z
        Vector3 targetPos = new Vector3(target.position.x, fixedY, transform.position.z);



        // 2. 限制目标位置 (Clamp)
        // 【优化】我们在移动前就限制目标点，这样相机到边界时会减速停下，而不是撞墙
        if (maxX >= minX)
        {
            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
        }
        else
        {
            targetPos.x = background.bounds.center.x;
        }


        Vector3 finalPos = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        // 3. 使用 SmoothDamp 进行丝滑移动
        // 它可以自动处理当前位置到目标位置的平滑过渡，彻底消除抖动
        float pixelX = Mathf.Round(finalPos.x * PPU) / PPU;
        float pixelY = Mathf.Round(finalPos.y * PPU) / PPU;

        transform.position = new Vector3(pixelX, pixelY, -10f);
    }
}
