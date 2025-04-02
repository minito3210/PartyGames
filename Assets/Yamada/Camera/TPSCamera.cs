using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TPSCamera : MonoBehaviour
{

   [SerializeField] GameObject player;
   public float distance; // カメラとプレイヤー間の距離
   public float height; // カメラの高さ
   public float smoothSpeed; // カメラの回転速度

                             // Start is called before the first frame update
   void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

      // マウスの移動量を取得
      float my = Input.GetAxis("Mouse Y");
      float mx = Input.GetAxis("Mouse X");

      // X方向に一定量移動していれば横回転
      //0.0000001fは滑らかさ
      if (Mathf.Abs(mx) > 0.0000001f)
      {
         mx = mx * 5;

         // 回転軸はワールド座標のY軸
         transform.RotateAround(player.transform.position, Vector3.up, mx);

      }
   }

   //void LateUpdate()
   //{
   //   // プレイヤーの中心位置を計算
   //   Vector3 playerCenter = player.transform.position + Vector3.up * height;

   //   // プレイヤーの後ろに位置するターゲット位置を計算
   //   Vector3 targetPosition = playerCenter - player.transform.forward * distance;


   //   // カメラの位置を滑らかに更新
   //   Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
   //   transform.position = smoothedPosition;

   //   // カメラは常にプレイヤーを向く
   //   transform.LookAt(player.transform);
   //}

}
