using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T instance = null;
        protected virtual void Start() { }
        protected virtual void Update() { }
        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }

        protected virtual void Awake()
        {
            if (instance != null)
            {
                return;
            }

         
            instance = FindObjectOfType<T>();
        }


        protected virtual void OnDestroy()
        {
            if (instance != null) instance = null;
        }

  
        public static T Instance
        {
            get
            {

                if (instance == null)
                {
                    GameObject go = new GameObject();
                    go.transform.position = Vector3.zero;
                    go.transform.rotation = Quaternion.identity;
                    go.transform.localScale = Vector3.one;
                    instance = go.AddComponent<T>();
                    instance.name = (typeof(T)).ToString();

                }

                return instance;
            }
        }
    }
