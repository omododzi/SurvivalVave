using System;
using UnityEngine;

public class Camera : MonoBehaviour
{
   private Transform target;
   public Vector3 offset =  new Vector3(5f, 5f, 5f);
   
   public float followspeed = 10f;
   public float rotatespeed = 10f;
   public float smoots = 1;
   
   private Vector3 velocity = Vector3.zero;
   private bool targetfound = false;

   void Start()
   {
      target = GameObject.FindGameObjectWithTag("Player").transform;
   }
   private void LateUpdate()
   {
      Debug.Log(target.name);
      if (target == null && !targetfound)
      {
         FindeTarget();
      }
      else
      {
         targetfound = true;
      }
      Follow();
   }

   void FindeTarget()
   {
      if (target == null)
      {
         target = GameObject.FindGameObjectWithTag("Player").transform;
      }
   }

   void Follow()
   {
      Vector3 desierposition =
         target.position + target.right * offset.x + target.up * offset.y + target.forward * offset.z;
      
      transform.position = Vector3.SmoothDamp(
         transform.position,
         desierposition,
         ref velocity,Mathf.Infinity,
         Time.deltaTime);
   }
   private void RotateTowardsTarget()
   {
      Vector3 lookAtPoint = target.position + Vector3.up;
      Vector3 direction = lookAtPoint - transform.position;
        
      // Используем SmoothDamp и для вращения (более плавно)
      Quaternion desiredRotation = Quaternion.LookRotation(direction);
      transform.rotation = Quaternion.Slerp(
         transform.rotation,
         desiredRotation,
         rotatespeed * Time.deltaTime
      );
   }
}
