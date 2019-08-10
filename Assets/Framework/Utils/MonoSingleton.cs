using System;
using System.Reflection;
using UnityEngine;
using Framework.Attributes;

namespace Framework.Utils
{
    /// <summary>
    /// Singleton implemetation of the <see cref="MonoBehaviour"/> class.
    /// </summary>
    /// <typeparam name="T">
    /// Any class that inherits from <see cref="MonoBehaviour"/>.
    /// </typeparam>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static bool applicationIsQuitting = false;

        private static T instance = null;

        private static bool verbose;

        /// <summary>
        /// Gets the shared instance.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    if (verbose)
                    {
                        Debug.LogWarning(string.Format("[{0}] Instance already destroyed on application quit. Won't create again.", typeof(T).FullName));
                    }

                    return null;
                }

                // Instance required for the first time, search if any has been already created.
                if (instance == null)
                {
                    verbose = Attribute.IsDefined(typeof(T), typeof(VerboseAttribute));

                    if (verbose)
                    {
                        Debug.Log(string.Format("[{0}] Checking for instances.", typeof(T).FullName));
                    }

                    instance = GameObject.FindObjectOfType(typeof(T)) as T;

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError(string.Format("[{0}] More than 1 instance found!", typeof(T).FullName));
                        return instance;
                    }

                    // Object not found, we create a temporary one
                    if (instance == null)
                    {
                        if (verbose)
                        {
                            Debug.Log(string.Format("[{0}] Creating a new instance.", typeof(T).FullName));
                        }

                        if (Attribute.IsDefined(typeof(T), typeof(LoadFromResourcesAttribute)))
                        {
                            LoadFromResourcesAttribute attr = (LoadFromResourcesAttribute)System.Attribute.GetCustomAttribute(typeof(T),
                                typeof(LoadFromResourcesAttribute));

                            GameObject instantiatedObj = Instantiate(Resources.Load<GameObject>(attr.Path));
                            instantiatedObj.name = "Temp Instance of " + typeof(T).ToString();
                            instance = instantiatedObj.GetComponent<T>();
                        }
                        else
                        {
                            instance = new GameObject("Temp Instance of " + typeof(T).ToString(), typeof(T)).GetComponent<T>();
                        }

                        if (instance == null)
                        {
                            Debug.LogError(string.Format("[{0}] Problem when creating. Instance is null.", typeof(T).FullName));
                        }
                    }

                    if (Attribute.IsDefined(typeof(T), typeof(DontDestroyOnLoadAttribute)))
                    {
                        DontDestroyOnLoad(instance.gameObject);
                    }

                    instance.Init();
                }

                return instance;
            }
        }

        /// <summary>
        /// Called when the instance is used the first time. Do not use Awake as it is used by the
        /// MonoSingleton implementation.
        /// </summary>
        public virtual void Init()
        {
        }

        /// <summary>
        /// Sets this instance as null when unity calls the On Destroy funciton.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (verbose)
            {
                Debug.Log(string.Format("[{0}] Destroyed instance. ", typeof(T).FullName));
            }

            instance = null;
        }

        /// <summary>
        /// Sets the <see cref="applicationIsQuitting"/> flag as <c>true</c> which prevents the
        /// instance to be created again when the game is shutting down.
        /// </summary>
        private void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }

        /// <summary>
        /// Sets the instance if it's not already present.
        /// </summary>
        private void Awake()
        {
            if (instance == null)
            {
                instance = this as T;

                verbose = Attribute.IsDefined(typeof(T), typeof(VerboseAttribute));

                if (Attribute.IsDefined(typeof(T), typeof(DontDestroyOnLoadAttribute)))
                {
                    DontDestroyOnLoad(instance.gameObject);
                }

                instance.Init();
            }
        }
    }
}