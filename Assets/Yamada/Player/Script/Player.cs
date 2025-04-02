using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   [Header("プレイヤーオブジェクト"), SerializeField]
   private GameObject m_playerObject;
   [Header("移動速度"), SerializeField]
   private float m_speed;
   public float maxSpeed = 2f; 
   [Header("ジャンプ力"), SerializeField]
   private float m_jumpPower;

   Rigidbody m_rigidbody;

   private int m_jumpNum = 2;

   public bool m_isGameOver { get; private set; }

   // Start is called before the first frame update
   void Start()
   {
      m_rigidbody = GetComponent<Rigidbody>();
   }

    // Update is called once per frame
   void Update()
   {
      KeyPush();

      if(m_playerObject.transform.position.y <= -1.5f)
      {
         Debug.Log("場外に落ちた");
         m_isGameOver = true;
         // 一定時間後に削除
         Destroy(m_playerObject, 2.0f);
      }
   }


   //�L�[�{�[�h����
   private void KeyPush()
   {
      Vector3 moveDirection = Vector3.zero;
      // カメラのトランスフォームを取得
      Transform cameraTransform = Camera.main.transform;
      // �J�����̑O�����ƉE�������擾�iY�������͖������Đ����ړ��̂ݍl���j
      Vector3 forward = cameraTransform.forward;
      Vector3 right = cameraTransform.right;
      forward.y = 0; // �㉺�̌X���𖳎����Đ��������̂ݗ��p
      right.y = 0;

      forward.Normalize();
      right.Normalize();

      // キーボード処理
      if (Input.GetKey(KeyCode.W)) moveDirection += forward;
      if (Input.GetKey(KeyCode.S)) moveDirection -= forward;
      if (Input.GetKey(KeyCode.A)) moveDirection -= right;
      if (Input.GetKey(KeyCode.D)) moveDirection += right;
      if (m_jumpNum > 0 && Input.GetKeyDown(KeyCode.Space))
      {
         m_jumpNum -= 1;
         m_rigidbody.AddForce(Vector3.up * m_jumpPower);
      }

      if (moveDirection != Vector3.zero)
      {
         moveDirection.Normalize();
         ////モデルに回転を与える(補間して)
         Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
         m_playerObject.transform.rotation = Quaternion.Slerp(m_playerObject.transform.rotation, targetRotation, Time.deltaTime * 10f);

         m_rigidbody.AddForce(moveDirection * m_speed, ForceMode.Acceleration);
      }

      // 最大速度を超えないように制御
      if (m_rigidbody.linearVelocity.magnitude > maxSpeed)
      {
         m_rigidbody.linearVelocity = m_rigidbody.linearVelocity.normalized * maxSpeed;
      }
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag("Laser"))
      {
         Debug.Log("レーザーに当たった");
         m_isGameOver = true;
      }
   }

   private void OnCollisionEnter(Collision collision)
   {
      if(collision.gameObject.CompareTag("Ground"))
      {
         m_jumpNum = 2;
      }
   }
}
