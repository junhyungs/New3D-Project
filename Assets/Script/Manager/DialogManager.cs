using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class DialogManager : Singleton_MonoBehaviour<DialogManager>
{
    [Header("DialogObject")]
    [SerializeField] private GameObject _backGroundObject;
    [SerializeField] private TextMeshProUGUI _npcNameText;
    [SerializeField] private TextMeshProUGUI _dialogText;
    private WaitForSeconds _typingSpeed = new WaitForSeconds(0.05f);

    [Header("DO_Tween")]
    [SerializeField] private RectTransform _backGroundRect;
    [SerializeField] private float _durationTime;
    private Vector3 _minScale = Vector3.zero;
    private Vector3 _maxScale = Vector3.one;

    //private void Awake()
    //{
    //    DataManager.Instance.TestLoadDialogData(); //테스트 코드
    //}

    private void ResetText()
    {
        _npcNameText.text = string.Empty;
        _dialogText.text = string.Empty;
    }
    
    public IEnumerator StartDialog(string name, List<string> dialogList)
    {
        _backGroundObject.SetActive(true);
        ResetText();

        _npcNameText.text = name;
        yield return BackGroundImageScale(_maxScale);

        foreach(var dialogText in dialogList)
        {
            yield return StartCoroutine(TypingMessage(dialogText));
            yield return new WaitUntil(() => Keyboard.current.enterKey.wasPressedThisFrame);
        }

        yield return BackGroundImageScale(_minScale);
        _backGroundObject.SetActive(false);
    }

    private IEnumerator TypingMessage(string message)
    {
        _dialogText.text = string.Empty;
        foreach(var letter in message)
        {
            _dialogText.text += letter;
            yield return _typingSpeed;
        }
    }

    private IEnumerator BackGroundImageScale(Vector3 scale)
    {
        yield return _backGroundRect.DOScale(scale, _durationTime).WaitForCompletion();
    }
}
