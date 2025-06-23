using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

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
    }

    private void OnDestroy()
    {
        UIManager.LoadingUIController -= StartImageBlinkCoroutine;
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

    public async UniTask LoadingSceneGameDataAsync(string sceneName, string disableUIName)
    {
        UIManager.Instance.DisableUI(disableUIName);
        StartImageBlinkCoroutine(true);

        var loadAllData = DataManager.Instance.LoadAllData();
        await loadAllData;

        SceneManager.LoadScene(sceneName);
    }

    private AsyncOperation AllowSceneActivationFalse(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        return asyncOperation;
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


}
