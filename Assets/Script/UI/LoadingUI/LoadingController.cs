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
    private AsyncOperation AllowSceneActivationFalse(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        return asyncOperation;
    }

    public async UniTask LoadingSceneGameDataAsync(string sceneName, string disableUIName)
    {
        UIManager.Instance.DisableUI(disableUIName);
        StartImageBlinkCoroutine(true);

        var loadAllData = DataManager.Instance.LoadAllData();
        await loadAllData;

        await UniTask.Delay(2000);
        await SceneManager.LoadSceneAsync(sceneName);
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
        while (true)
        {
            yield return StartCoroutine(Blink(0f, _blinkSpeed));
            yield return StartCoroutine(Blink(1f, _blinkSpeed));
        }
    }

    private IEnumerator Blink(float targetAlpha, float durationTime)
    {
        var startAlpha = _icon.color.a;
        var elapsed = 0f;

        while(elapsed < durationTime)
        {
            elapsed += Time.deltaTime;
            var newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / durationTime);
           
            var color = _icon.color;
            color.a = newAlpha;
            _icon.color = color;

            yield return null;
        }

        var finalColor = _icon.color;
        finalColor.a = targetAlpha;
        _icon.color = finalColor;
    }


}
