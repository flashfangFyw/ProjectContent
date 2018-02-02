using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* 
    Author:     fyw 
    CreateDate: 2017-06-05 12:21:32 
    Desc:       注释 
*/
namespace ffDevelopmentSpace
{
    public class SceneLoadManager : SingletonMB<SceneLoadManager>
    {

        #region public property
        public Image faderImg;
        public Color faderColor;
        public float fadeSpeed = .02f;
        #endregion
        #region private property
        private Color fadeTransparency = new Color(0, 0, 0, .04f);
        private string currentScene;
        private AsyncOperation async;
        #endregion

        #region unity function
        private void Awake()
        {
            InitData();
        }
        void OnEnable()
        {
        }
        void Start()
        {
        }
        void Update()
        {
        }
        void OnDisable()
        {
        }
        void OnDestroy()
        {
        }
        #endregion

        #region public function
        // Load a scene with a specified string name
        public void LoadScene(string sceneName)
        {
            StartCoroutine(Load(sceneName));
            StartCoroutine(FadeOut(faderImg));
        }

        // Reload the current scene
        public void ReloadScene()
        {
            LoadScene(SceneManager.GetActiveScene().name);
        }
        public void ActivateScene()
        {
            async.allowSceneActivation = true;
        }
        #endregion
        #region private function
        private void InitData()
        {
            DontDestroyOnLoad(gameObject);
            if (faderImg) faderImg.color = faderColor;
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }
        private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            currentScene = scene.name;
            StartCoroutine(FadeIn(faderImg));
        }

        //Iterate the fader transparency to 100%
        IEnumerator FadeOut(Image fader)
        {
            fader.gameObject.SetActive(true);
            while (fader.color.a < 1)
            {
                fader.color += fadeTransparency;
                yield return new WaitForSeconds(fadeSpeed);
            }
            ActivateScene(); //Activate the scene when the fade ends
        }
        // Iterate the fader transparency to 0%
        IEnumerator FadeIn(Image fader)
        {
            while (fader.color.a > 0)
            {
                fader.color -= fadeTransparency;
                yield return new WaitForSeconds(fadeSpeed);
            }
            fader.gameObject.SetActive(false);
        }
        // Begin loading a scene with a specified string asynchronously
        IEnumerator Load(string sceneName)
        {
            async = SceneManager.LoadSceneAsync(sceneName);
            async.allowSceneActivation = false;
            yield return async;
        }
        #endregion

        #region event function
        #endregion
    }
}
