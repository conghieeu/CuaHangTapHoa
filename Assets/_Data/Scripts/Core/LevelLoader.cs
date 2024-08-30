using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    /// <summary> Hiện ứng chuyển cảnh </summary>
    public class LevelLoader : MonoBehaviour
    {
        public Animator _anim;
        public float transitionTime = 1f;

        public void LoadNextLevel()
        {
            StartCoroutine(LoadLevel(GameScene.Demo));
        }

        IEnumerator LoadLevel(String sceneName)
        {
            _anim.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadScene(sceneName);
        }


    }
}
