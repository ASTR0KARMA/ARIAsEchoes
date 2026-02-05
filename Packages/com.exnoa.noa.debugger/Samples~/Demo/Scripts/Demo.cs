#if NOA_DEBUGGER
using NoaDebugger;
using UnityEngine.Rendering;
#endif
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NoaDebuggerDemo
{
    public class Demo : MonoBehaviour
    {
        [Serializable]
        struct LayoutSettings
        {
            public Vector3 cameraPosition;
            public Vector3 cameraRotation;
            public float cameraFieldOfView;
            public float lightIntensity;

            public void Apply(Camera camera, Light light)
            {
                camera.transform.localPosition = cameraPosition;

                camera.transform.localRotation = Quaternion.Euler(
                    cameraRotation.x,
                    cameraRotation.y,
                    cameraRotation.z);

                camera.fieldOfView = cameraFieldOfView;
                light.intensity = lightIntensity;
            }
        }

        bool isInitialized = false;

        [SerializeField]
        Button initializeButton;
        [SerializeField]
        Button showButton;
        [SerializeField]
        Button destroyButton;
        [SerializeField]
        Transform spawnPosition;
        [SerializeField]
        GameObject plane;
        [SerializeField]
        GameObject planeURP;
        [SerializeField]
        GameObject[] spawnObjects;
        [SerializeField]
        GameObject[] spawnURPObjects;
        [SerializeField]
        Vector3 spawnRangeMin;
        [SerializeField]
        Vector3 spawnRangeMax;
        [SerializeField]
        Vector3 spawnObjectVelocity;

        [SerializeField]
        Camera sceneCamera;
        [SerializeField]
        LayoutSettings portraitLayoutSettings;
        [SerializeField]
        LayoutSettings landscapeLayoutSettings;
        [SerializeField]
        Light scenetLight;

        bool isPortrait = false;
#if NOA_DEBUGGER
        DebugCommandRegistrationSample debugCommandRegistrationSample;
        UIElementRegistrationSample uiElementRegistrationSample;
        static readonly System.Random RandomGenerator = new(Environment.TickCount);
#endif

        void Awake()
        {
            initializeButton.onClick.AddListener(OnInitialize);
            showButton.onClick.AddListener(OnShowNoaDebugger);
            destroyButton.onClick.AddListener(OnDestroyNoaDebugger);
        }

        IEnumerator Start()
        {
            yield return Initialize();
        }

        void Update()
        {
            if (!isInitialized)
            {
                return;
            }
#if NOA_DEBUGGER

            if (IsOrientationChanged())
            {
#if NOA_DEBUGGER
                uiElementRegistrationSample.ReregistrationForOrientation(IsPortrait());
#endif
                SetLayoutSettings();
            }

            CheckDestroyAll();
#endif

        }

        void OnDestroy()
        {
#if NOA_DEBUGGER
            debugCommandRegistrationSample.Destroy();
#endif
        }

        IEnumerator Initialize()
        {
#if NOA_DEBUGGER
            if (!NoaDebug.IsInitialized)
            {
                NoaDebug.Initialize();
                isInitialized = false;
            }

            while (!NoaDebug.IsInitialized)
            {
                yield return null;
            }

            if (!isInitialized)
            {
                debugCommandRegistrationSample = new DebugCommandRegistrationSample(SpawnObject, DestroyAll, this);
                uiElementRegistrationSample = new UIElementRegistrationSample(IsPortrait());
                ControllerSetCustomActionSample.SetCustomAction();
            }

            if (Demo.IsURP())
            {
                planeURP.gameObject.SetActive(true);
            }
            else
            {
                plane.gameObject.SetActive(true);
            }
#endif
            isInitialized = true;
            Debug.Log("NoaDebugger Initialized");

            yield return null;
        }

        void OnInitialize()
        {
            StartCoroutine(Initialize());
        }

        void OnShowNoaDebugger()
        {
#if NOA_DEBUGGER
            NoaDebug.Show();
#endif
        }

        void OnDestroyNoaDebugger()
        {
#if NOA_DEBUGGER
            debugCommandRegistrationSample.Destroy();
            NoaDebug.Destroy();
#endif
            isInitialized = false;
            Debug.Log("NoaDebugger Destroyed");
        }

        void SetLayoutSettings()
        {
            if (IsPortrait())
            {
                portraitLayoutSettings.Apply(sceneCamera, scenetLight);
            }
            else
            {
                landscapeLayoutSettings.Apply(sceneCamera, scenetLight);
            }
        }

        bool IsPortrait()
        {
            return Screen.width < Screen.height;
        }

        bool IsOrientationChanged()
        {
            bool previousIsPortraitVariable = isPortrait;
            isPortrait = IsPortrait();

            return previousIsPortraitVariable != isPortrait;
        }

#if NOA_DEBUGGER
        void SpawnObject(DebugCommandRegistrationSample.ObjectType type, int count)
        {
            for (int i = 0; i < count; i++)
            {
                SpawnObject(type);
            }
        }

        void CheckDestroyAll()
        {
            var fps = NoaProfiler.LatestFpsInfo;
            if (fps == null)
            {
                return;
            }

            if (fps.IsProfiling && fps.CurrentFps <= 5)
            {
                DestroyAll();
            }
        }
#endif

        void DestroyAll()
        {
            if (spawnPosition == null)
            {
                return;
            }

            foreach (Transform objectInstance in spawnPosition)
            {
                Destroy(objectInstance.gameObject);
            }
        }

#if NOA_DEBUGGER
        void SpawnObject(DebugCommandRegistrationSample.ObjectType type)
        {
            GameObject[] selectGameObjects()
            {
                if (Demo.IsURP())
                {
                    return spawnURPObjects;
                }
                else
                {
                    return spawnObjects;
                }
            }

            if (spawnPosition == null)
            {
                return;
            }

            var instanceObject = Instantiate(selectGameObjects()[(int)type], spawnPosition.transform);

            var addPositionX = RandomGenerator.Next(
                Mathf.FloorToInt(spawnRangeMin.x),
                Mathf.FloorToInt(spawnRangeMax.x));

            var addPositionY = RandomGenerator.Next(
                Mathf.FloorToInt(spawnRangeMin.y),
                Mathf.FloorToInt(spawnRangeMax.y));

            var addPositionZ = RandomGenerator.Next(
                Mathf.FloorToInt(spawnRangeMin.z),
                Mathf.FloorToInt(spawnRangeMax.z));

            instanceObject.transform.localPosition += new Vector3(addPositionX, addPositionY, addPositionZ);
            var rigidBody = instanceObject.GetComponent<Rigidbody>();
            rigidBody.useGravity = true;
#if UNITY_6000_0_OR_NEWER
            rigidBody.linearVelocity = spawnObjectVelocity;
#else
            rigidBody.velocity = spawnObjectVelocity;
#endif
            instanceObject.SetActive(true);
        }

        static bool IsURP()
        {
            return GraphicsSettings.defaultRenderPipeline != null &&
                   GraphicsSettings.defaultRenderPipeline.GetType().Name.Contains("Universal");
        }
#endif
    }
}
