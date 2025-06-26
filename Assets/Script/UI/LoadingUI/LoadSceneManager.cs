using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System;

public class LoadSceneManager : Singleton_MonoBehaviour<LoadSceneManager>
{
    [Header("UI")]
    [SerializeField] private GameObject _loadingUI;
    [SerializeField] private ErrorUI _errorUI;

    [Header("Icon")]
    [SerializeField] private Image _icon;

    [Header("IconBlinkSpeed")]
    [SerializeField]private float _blinkSpeed;

    private Coroutine _imageBlinkCoroutine;
    private const string STARTSCENENAME = "StartScene";

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void ChangeScene(string sceneName)
    {
        StartLoadingUICoroutine(true);
        UIManager.Instance.AllDisableUI();

        RunAsync(sceneName).Forget();
        async UniTask RunAsync(string sceneName)
        {
            try
            {
                await SceneManager.LoadSceneAsync(sceneName);
                await UniTask.Delay(2000);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
                await SceneManager.LoadSceneAsync(STARTSCENENAME);
            }

            StartLoadingUICoroutine(false);
        }
    }

    public void LoadSceneAndReportError(string sceneName, string message)
    {
        StartLoadingUICoroutine(true);
        UIManager.Instance.AllDisableUI();

        ErrorMessage errorMessage = new ErrorMessage();
        errorMessage.Message = message;

        RunAsync(sceneName, errorMessage).Forget();
        async UniTaskVoid RunAsync(string sceneName, ErrorMessage errorMessage)
        {
            try
            {
                await SceneManager.LoadSceneAsync(sceneName);
                if(_errorUI != null)
                {
                    _errorUI.gameObject.SetActive(true);
                    _errorUI.SetErrorMessage(errorMessage);
                }
            }
            catch(Exception ex)
            {
                Debug.Log(ex.Message);
                await SceneManager.LoadSceneAsync(STARTSCENENAME);
            }

            StartLoadingUICoroutine(false);
        }
    }

    public async UniTask LoadingSceneGameDataAsync(string sceneName, string disableUIName)
    {
        UIManager.Instance.DisableUI(disableUIName);
        StartLoadingUICoroutine(true);

        var loadAllData = DataManager.Instance.LoadAllData();
        await loadAllData;

        await UniTask.Delay(2000);
        await SceneManager.LoadSceneAsync(sceneName);
    }

    public void StartLoadingUICoroutine(bool isStart)
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

public struct ErrorMessage
{
    public string Message;
}