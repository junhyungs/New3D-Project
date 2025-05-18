using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Menu : MonoBehaviour
{
    public enum ArrowDirection
    {
        Left, Right
    }

    [Header("Panel"), SerializeField] private GameObject _panel;
    [Header("ArrowImages"), SerializeField] private GameObject[] _arrowImages;
    [Header("Distance"), SerializeField] private float _distance;
    [Header("Duration"), SerializeField] private float _duration;

    private Vector3 _initLeftImagePosition;
    private Vector3 _initRightImagePosition;

    private void Awake()
    {
        SetActiveArrowImage(false);

        _initLeftImagePosition = _arrowImages[(int)ArrowDirection.Left].transform.localPosition;
        _initRightImagePosition = _arrowImages[(int)ArrowDirection.Right].transform.localPosition;
    }

    public void SelectedMenu()
    {
        SetActiveArrowImage(true);
        AnimateArrowImage();
    }

    public void DeSelectMenu()
    {
        var leftArrow = DoRewind(ArrowDirection.Left);
        var rightArrow = DoRewind(ArrowDirection.Right);

        leftArrow.localPosition = _initLeftImagePosition;
        rightArrow.localPosition = _initRightImagePosition;
        
        SetActiveArrowImage(false);
    }

    private void SetActiveArrowImage(bool active)
    {
        foreach (GameObject arrowImage in _arrowImages)
            arrowImage.SetActive(active);
    }

    private void AnimateArrowImage()
    {
        var leftArrow = DoRewind(ArrowDirection.Left);
        var rightArrow = DoRewind(ArrowDirection.Right);

        var leftEndPosition = _initLeftImagePosition.x - _distance;
        var rightEndPosition = _initRightImagePosition.x + _distance;

        leftArrow.DOLocalMoveX(leftEndPosition, _duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

        rightArrow.DOLocalMoveX(rightEndPosition, _duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType .Yoyo);
    }

    private Transform DoRewind(ArrowDirection arrowDirection)
    {
        var transform = _arrowImages[(int)arrowDirection].transform;

        transform.DORewind();
        return transform;
    }

    public void PressedMenu(GameObject mainManuUI)
    {
        if(_panel != null)
        {
            _panel.SetActive(true);
            mainManuUI.SetActive(false);

            return;
        }

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif        
    }
}
