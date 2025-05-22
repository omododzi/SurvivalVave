using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;

[RequireComponent(typeof(CharacterController))]
public class MovePlayer : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float rotateSpeed = 10f;
    
    [Header("Gravity Settings")]
    public float gravity = -9.81f;
    public float groundedGravity = -0.5f;
    
    private Vector3 velocity;
    private Vector2 inputDirection;
    private CharacterController controller;
    
    // Новые поля для Input System
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction runAction;
    
    public GameObject camera;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        // Инициализация Input System
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        runAction = playerInput.actions["Run"];
    }

    void Update()
    {
        Move();
        ApplyGravity();
        controller.Move(velocity * Time.deltaTime);
    }

    void Move()
    {
        // Получаем направление камеры
        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();
        
        // Получаем ввод из новой системы ввода
        inputDirection = moveAction.ReadValue<Vector2>();
        
        if (inputDirection.magnitude > 1f)
        {
            inputDirection.Normalize();
        }
        
        // Рассчитываем направление движения
        Vector3 moveDirection = cameraForward * inputDirection.y + cameraRight * inputDirection.x;
        
        // Проверяем бег
        float currentSpeed = runAction.IsPressed() ? runSpeed : moveSpeed;
        Vector3 movement = moveDirection * currentSpeed;
        
        velocity.x = movement.x;
        velocity.z = movement.z;

        // Поворот персонажа
        if (inputDirection.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    void ApplyGravity()
    {
        if (!controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = groundedGravity;
        }
    }
}