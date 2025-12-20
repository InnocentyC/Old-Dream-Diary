
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [Header("����")]
    public Transform target;          // ����
    public SpriteRenderer background; // ����


    [Header("ƽ���� (ԽСԽ�죬0.1-0.3 ���)")]
    public float smoothTime = 0.2f;   // �滻�� smoothSpeed

    [Header("������������")]
    // ���ͼƬ�زĵ� PPU 
    public float PPU = 100f;

    private float camWidth;
    private float minX, maxX;
    private float fixedY; // �̶��� Y ��

    // SmoothDamp ��Ҫ���м������������¼��ǰ�ٶ�
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        float camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        if (background != null)
        {
            Bounds bgBounds = background.bounds;

            // 1. ���������ƶ��ļ���
            minX = bgBounds.min.x + camWidth;
            maxX = bgBounds.max.x - camWidth;

            // 2. ���ؼ��������߶�Ϊ���������ĸ߶�
            // ���������Զ�����ű���ͼ���м�
            fixedY = bgBounds.center.y;
        }
        else
        {
            Debug.LogError("��ѱ���ͼƬ�ϸ������ CameraFollow �ű���");
        }
    }

    void LateUpdate()
    {
        if (target == null || background == null) return;

        // 1. ȷ��Ŀ��λ��
        // ����ϣ�����ȥ���ȥ���ǵ�X���̶���Y��ԭ����Z
        Vector3 targetPos = new Vector3(target.position.x, fixedY, transform.position.z);



        // 2. ����Ŀ��λ�� (Clamp)
        // ���Ż����������ƶ�ǰ������Ŀ��㣬����������߽�ʱ�����ͣ�£�������ײǽ
        if (maxX >= minX)
        {
            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
        }
        else
        {
            targetPos.x = background.bounds.center.x;
        }


        Vector3 finalPos = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        // 3. ʹ�� SmoothDamp ����˿���ƶ�
        // �������Զ�������ǰλ�õ�Ŀ��λ�õ�ƽ�����ɣ�������������
        float pixelX = Mathf.Round(finalPos.x * PPU) / PPU;
        float pixelY = Mathf.Round(finalPos.y * PPU) / PPU;

        transform.position = new Vector3(pixelX, pixelY, -10f);
    }
}
