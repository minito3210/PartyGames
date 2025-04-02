using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMove : MonoBehaviour
{

   [Header("寿命（秒）"), SerializeField]
   private float lifetime = 3f; // レーザーの消滅までの時間

   private Vector3 moveDirection = Vector3.forward; // デフォルトは前方向
   private float m_speed;

   // 初期化メソッド（外部から方向を設定する）
   public void SetDirection(Vector3 direction)
   {
      moveDirection = direction.normalized;
   }
   public void SetSpeed(float speed)
   {
      m_speed = speed;
   }

   // Start is called before the first frame update
   void Start()
   {
      // 一定時間後に削除
      Destroy(gameObject, lifetime);
   }

   // Update is called once per frame
   void Update()
    {
      Move();
    }

   private void Move()
   {
      moveDirection.Normalize();
      transform.position += moveDirection * m_speed * Time.deltaTime;
   }
}
