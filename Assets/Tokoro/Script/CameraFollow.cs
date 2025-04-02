using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // プレイヤーのTransform
    public Vector3 offset = new Vector3(0, 5, -7); // カメラのオフセット
    public float smoothSpeed = 5.0f; // カメラの追従スピード

    void LateUpdate()
    {
        if (target == null) return;

        // 目的地を計算
        Vector3 desiredPosition = target.position + offset;

        // 現在位置からスムーズに移動
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // 常にプレイヤーの方向を見る
        transform.LookAt(target);
    }
}
