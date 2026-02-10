using UnityEngine;

public class SinusAnimator : MonoBehaviour
{
    public enum Mode
    {
        SinusLight,
        SinusObject
    }

    [SerializeField] private Mode mode;
    [SerializeField] private float speed = 1f;

    [Header("Sinus Light")]
    [SerializeField] private Light targetLight;
    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 2f;

    [Header("Sinus Object")]
    [SerializeField] private Transform targetObject;
    [SerializeField] private float amplitude = 1f;

    private float originalY;

    private void Start()
    {
        if (mode == Mode.SinusObject && targetObject != null)
        {
            originalY = targetObject.position.y;
        }
    }

    private void Update()
    {
        float sin = Mathf.Sin(Time.time * speed);

        if (mode == Mode.SinusLight && targetLight != null)
        {
            float t = (sin + 1f) / 2f;
            targetLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
        }
        else if (mode == Mode.SinusObject && targetObject != null)
        {
            Vector3 pos = targetObject.position;
            pos.y = originalY + sin * amplitude;
            targetObject.position = pos;
        }
    }
}