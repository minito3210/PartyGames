using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : NetworkBehaviour
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

   //サーバー側で使う移動・ジャンプ状態
   private Vector3 m_moveDirection = Vector3.zero;
   private bool m_jumpRequested = false;

   // Start is called before the first frame update
   void Start()
   {
      m_rigidbody = GetComponent<Rigidbody>();
   }

    // Update is called once per frame
   void Update()
   {
      if (IsOwner)
      {
         HandleInput();

         //KeyPush();

         if (m_playerObject.transform.position.y <= -3.5f)
         {
            Debug.Log("場外に落ちた");
            m_isGameOver = true;
            // 一定時間後に削除
            Destroy(m_playerObject, 2.0f);
         }
      }

      if (IsServer)
      {
         ServerUpdate();
      }
   }


   //入力処理(クライアントでのみ）
   private void HandleInput()
   {
      Vector3 moveDir = Vector3.zero;
      Transform cameraTransform = Camera.main.transform;
      Vector3 forward = cameraTransform.forward;
      Vector3 right = cameraTransform.right;
      forward.y = 0;
      right.y = 0;
      forward.Normalize();
      right.Normalize();

      if (Input.GetKey(KeyCode.W)) moveDir += forward;
      if (Input.GetKey(KeyCode.S)) moveDir -= forward;
      if (Input.GetKey(KeyCode.A)) moveDir -= right;
      if (Input.GetKey(KeyCode.D)) moveDir += right;

      bool jump = false;

      if (m_jumpNum > 0 && Input.GetKeyDown(KeyCode.Space))
      {
         jump = true;
         m_jumpNum -= 1;
         m_rigidbody.AddForce(Vector3.up * m_jumpPower);
      }

      //サーバーに入力情報を送信
      SendInputToServerRpc(moveDir, jump);
   }

   //クライアント→サーバー　入力送信
   [ServerRpc]
   private void SendInputToServerRpc(Vector3 moveDir, bool jump)
   {
      m_moveDirection = moveDir.normalized;
      if (jump && m_jumpNum > 0)
      {
         m_jumpRequested = true;
         m_jumpNum -= 1;
      }
   }

   //サーバー側の物理処理
   private void ServerUpdate()
   {
      if(m_moveDirection != Vector3.zero)
      {
         Quaternion targetRotation = Quaternion.LookRotation(m_moveDirection);
         m_playerObject.transform.rotation = Quaternion.Slerp(m_playerObject.transform.rotation, targetRotation, Time.deltaTime * 10.0f);

         m_rigidbody.AddForce(m_moveDirection * m_speed, ForceMode.Acceleration);
      }

      if(m_jumpRequested)
      {
         m_rigidbody.AddForce(Vector3.up * m_jumpPower);
         m_jumpRequested = false;
      }

      if(m_rigidbody.linearVelocity.magnitude > maxSpeed)
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
      if (collision.gameObject.CompareTag("Ground"))
      {
         m_jumpNum = 2;
      }
   }


   ////キーボード入力処理
   //private void KeyPush()
   //{
   //   Vector3 moveDirection = Vector3.zero;
   //   // カメラのトランスフォームを取得
   //   Transform cameraTransform = Camera.main.transform;
   //   Vector3 forward = cameraTransform.forward;
   //   Vector3 right = cameraTransform.right;
   //   forward.y = 0; 
   //   right.y = 0;

   //   forward.Normalize();
   //   right.Normalize();

   //   // キーボード処理
   //   if (Input.GetKey(KeyCode.W)) moveDirection += forward;
   //   if (Input.GetKey(KeyCode.S)) moveDirection -= forward;
   //   if (Input.GetKey(KeyCode.A)) moveDirection -= right;
   //   if (Input.GetKey(KeyCode.D)) moveDirection += right;
   //   if (m_jumpNum > 0 && Input.GetKeyDown(KeyCode.Space))
   //   {
   //      m_jumpNum -= 1;
   //      m_rigidbody.AddForce(Vector3.up * m_jumpPower);
   //   }

   //   if (moveDirection != Vector3.zero)
   //   {
   //      moveDirection.Normalize();
   //      //モデルに回転を与える(補間して)
   //      Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
   //      m_playerObject.transform.rotation = Quaternion.Slerp(m_playerObject.transform.rotation, targetRotation, Time.deltaTime * 10f);

   //      m_rigidbody.AddForce(moveDirection * m_speed, ForceMode.Acceleration);
   //   }

   //   // 最大速度を超えないように制御
   //   if (m_rigidbody.linearVelocity.magnitude > maxSpeed)
   //   {
   //      m_rigidbody.linearVelocity = m_rigidbody.linearVelocity.normalized * maxSpeed;
   //   }
   //}

   //サーバー側で行う処理

   //サーバーだけで呼び出すUpdate

   //衝突判定
}
