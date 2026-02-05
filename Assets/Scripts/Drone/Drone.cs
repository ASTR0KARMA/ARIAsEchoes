using UnityEngine;

public enum DroneLocomotionMode
{
    Follow,
    Target,
    None
}

public class Drone : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;

    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Tilt")]
    [SerializeField] private float maxTiltAngle = 30f;
    [SerializeField] private float tiltSpeed = 5f;

    [Header("Follow")]
    [SerializeField] private float followDistance = 3f;
    [SerializeField] private float followHeightOffset = 2f;
    [SerializeField] private float followStopDistance = 0.5f;

    [Header("Target")]
    [SerializeField] private float targetHeightOffset = 1.5f;
    [SerializeField] private float targetStopDistance = 0.2f;

    private DroneLocomotionMode currentMode = DroneLocomotionMode.Follow;
    private static Drone INSTANCE;

    private float currentTiltX;
    private float currentTiltZ;
    private float currentYaw;

    private void Awake()
    {
        INSTANCE = this;
    }

    private void Update()
    {
        if (target == null || currentMode == DroneLocomotionMode.None)
            return;

        Vector3 destination = GetDestination();
        Vector3 toDestination = destination - transform.position;
        float distance = toDestination.magnitude;
        float stopDistance = currentMode == DroneLocomotionMode.Follow ? followStopDistance : targetStopDistance;

        bool isMoving = distance > stopDistance;

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        }

        UpdateRotation(toDestination, isMoving);
    }

    private void UpdateRotation(Vector3 toDestination, bool isMoving)
    {
        Vector3 lookDirection = (target.position - transform.position).normalized;

        if (lookDirection != Vector3.zero)
        {
            float targetYaw = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;
            currentYaw = Mathf.LerpAngle(currentYaw, targetYaw, rotationSpeed * Time.deltaTime);
        }

        float targetTiltX = 0f;
        float targetTiltZ = 0f;

        if (isMoving)
        {
            Vector3 localDirection = Quaternion.Euler(0f, -currentYaw, 0f) * toDestination.normalized;
            targetTiltX = Mathf.Clamp(localDirection.z * maxTiltAngle, -maxTiltAngle, maxTiltAngle);
            targetTiltZ = Mathf.Clamp(-localDirection.x * maxTiltAngle, -maxTiltAngle, maxTiltAngle);
        }

        currentTiltX = Mathf.Lerp(currentTiltX, targetTiltX, tiltSpeed * Time.deltaTime);
        currentTiltZ = Mathf.Lerp(currentTiltZ, targetTiltZ, tiltSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(currentTiltX, currentYaw, currentTiltZ);
    }

    private Vector3 GetDestination()
    {
        return currentMode switch
        {
            DroneLocomotionMode.Follow => GetFollowDestination(),
            DroneLocomotionMode.Target => target.position + Vector3.up * targetHeightOffset,
            _ => transform.position
        };
    }

    private Vector3 GetFollowDestination()
    {
        Vector3 directionFromTarget = transform.position - target.position;
        directionFromTarget.y = 0f;

        if (directionFromTarget.sqrMagnitude < 0.001f)
        {
            directionFromTarget = -target.forward;
        }

        directionFromTarget.Normalize();

        Vector3 destination = target.position + directionFromTarget * followDistance;
        destination.y = target.position.y + followHeightOffset;

        return destination;
    }

    public static void SwitchMode(DroneLocomotionMode newMode, Transform newTarget)
    {
        if (INSTANCE == null) return;

        INSTANCE.currentMode = newMode;
        INSTANCE.target = newTarget;
    }
}