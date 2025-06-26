using StartSceneUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ErrorUI : MonoBehaviour
{
    [Header("TextMeshProUGUI"), SerializeField]
    private TextMeshProUGUI _errorText;
    private Action _oneTimeAction;
    private const float TIME = 5f;

    private void OnEnable()
    {
        var uiReference = FindFirstObjectByType<UIReference>();
        if(uiReference != null)
        {
            var mainMenu = uiReference.MainMenu;
            Action action = null;
            action = () =>
            {
                mainMenu.EnableButtons(true);
                _oneTimeAction -= action;
            };

            _oneTimeAction += action;
            mainMenu.EnableButtons(false);
        }

        StartCoroutine(EnableTimer());
    }

    private IEnumerator EnableTimer()
    {
        yield return new WaitForSeconds(TIME);
        _oneTimeAction?.Invoke();
        gameObject.SetActive(false);
    }

    public void SetErrorMessage(ErrorMessage message)
    {
        _errorText.text = message.Message;
    }
}
