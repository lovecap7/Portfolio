using UnityEngine;

namespace mySystem
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        static T instance;

        #region Property
        public static T Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                instance = FindAnyObjectByType<T>();
                if (instance == null)
                {
                    Debug.LogError(typeof(T) + "is nothing");
                }
                return instance;
            }
        }
        public static T Get { get { return instance; } }
        #endregion

        protected virtual void Awake()
        {
            if (instance != null && instance != this)
            {
                Debug.LogError(typeof(T) + " is multiple created", this);
                return;
            }

            instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}