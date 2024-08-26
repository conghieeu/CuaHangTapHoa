using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CuaHang
{
    public class MainMenuManager : MonoBehaviour
    {
        private AsyncOperation m_async;
        public static Action<float> Onloading;

        private void Start()
        {
            // SceneManager.LoadSceneAsync(GameScene, LoadSceneMode.Additive);
        }

        private void Update() {
            Onloading?.Invoke(m_async.progress);

            if(m_async.isDone) 
            {
                SceneManager.LoadScene("menu");
            }
        }
    }
}
