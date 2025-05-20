using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class LoadingController : MonoBehaviour
{
    [Header("LoadingUI"), SerializeField] private GameObject _loadingUI;
    [Header("Icon"), SerializeField] private Image _icon;
    [Header("IconBlinkSpeed"), SerializeField] private float _blinkSpeed;
    private Coroutine _imageBlinkCoroutine;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        UIManager.LoadingUIController += StartImageBlinkCoroutine;
        SceneManager.sceneLoaded += LoadComplete;
    }

    private void OnDestroy()
    {
        UIManager.LoadingUIController -= StartImageBlinkCoroutine;
        SceneManager.sceneLoaded -= LoadComplete;
    }

    private void LoadComplete(Scene scene, LoadSceneMode mode)
    {
        StartImageBlinkCoroutine(false);
    }

    public void StartLoadingSceneWaitForSeconds(string sceneName, string disableUIName = null)
    {
        StartCoroutine(LoadingSceneCoroutine(sceneName, disableUIName));
    }

    private IEnumerator LoadingSceneCoroutine(string sceneName, string disableUIName)
    {
        if (disableUIName != null)
            UIManager.Instance.DisableUI(disableUIName);

        var asyncOperaction = AllowSceneActivationFalse(sceneName);
        StartImageBlinkCoroutine(true);

        yield return new WaitForSeconds(3f);
       asyncOperaction.allowSceneActivation = true;
    }

    public void StartLoadingSceneGameDataAsync(string sceneName, string disableUIName)
    {
        StartCoroutine(LoadingSceneGameDataCoroutine(sceneName, disableUIName));
    }

    private IEnumerator LoadingSceneGameDataCoroutine(string sceneName, string disableUIName)
    {
        UIManager.Instance.DisableUI(disableUIName);

        var asyncOperaction = AllowSceneActivationFalse(sceneName);
        StartImageBlinkCoroutine(true);

        Task task = DataManager.Instance.LoadAllData();
        while (!task.IsCompleted)
            yield return null;
        
        //StartImageBlinkCoroutine(false);
        asyncOperaction.allowSceneActivation = true;
    }

    private void StartImageBlinkCoroutine(bool isStart)
    {
        if (isStart)
        {
            if(_imageBlinkCoroutine == null)
            {
                _loadingUI.SetActive(true);
                _imageBlinkCoroutine = StartCoroutine(ImageBlink());
            }
        }
        else
        {
            if(_imageBlinkCoroutine != null)
            {
                StopCoroutine(_imageBlinkCoroutine);
                _imageBlinkCoroutine = null;
            }

            _loadingUI.SetActive(false);
        }
    }

    private IEnumerator ImageBlink()
    {
        Color color = _icon.color;

        while (true)
        {
            yield return StartCoroutine(Blink(color, 0f, _blinkSpeed));
            yield return StartCoroutine(Blink(color, 1f, _blinkSpeed));
        }
    }

    private IEnumerator Blink(Color color, float targetAlpha, float durationTime)
    {
        var startAlpha = color.a;
        var elapsed = 0f;

        while(elapsed < durationTime)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsed / durationTime);
            _icon.color = color;

            yield return null;
        }

        color.a = targetAlpha;
        _icon.color = color;
    }

    private AsyncOperation AllowSceneActivationFalse(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        return asyncOperation;
    }

}
