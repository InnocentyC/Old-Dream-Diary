using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    private SpriteRenderer spriteRenderer;
    public float moveSpeed = 5f;

    // 鼠标点击交互的回调
    private void OnMouseDown()
    {
        // 当鼠标点击角色时触发
        Debug.Log("角色被点击了！");
        // 可以在这里添加其他交互逻辑，比如改变颜色、播放动画等
    }

    void Start()
    {
        // 获取 SpriteRenderer 组件
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 更新方法，用于检测输入和移动角色
    void Update()
    {
        // 获取水平输入（-1 到 1，A/D 或 ←/→ 控制）
        float horizontalInput = Input.GetAxis("Horizontal");

        // 计算移动向量
        Vector3 moveDirection = new Vector3(horizontalInput, 0, 0);

        // 移动角色
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // 如果需要角色朝向移动方向，可以添加以下代码
        if (horizontalInput != 0)
        {
            spriteRenderer.flipX = horizontalInput < 0; // 根据输入翻转角色
        }
    }
}
