using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.Managers
{
    public class NextClotheDisplayManager : MonoBehaviour
    {
        public static NextClotheDisplayManager Instance {get; private set;}
        private bool _initialized;
        
        [SerializeField] private RectTransform _nextClotheDisplay;
        [SerializeField] private RectTransform _currentClotheDisplay;
        private Vector3 _nextClotheDisplayPosition;
        private Vector3 _currentClotheDisplayPosition;
        [SerializeField] private float _duration = 1f;
        private Sprite _nextClotheDisplaySprite;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _nextClotheDisplayPosition = _nextClotheDisplay.position;
            _currentClotheDisplayPosition = _currentClotheDisplay.position;
            _nextClotheDisplay.GetComponent<CanvasGroup>().alpha = .5f;
        }

        private void DoAnim()
        {
            _nextClotheDisplay.DOMove(_currentClotheDisplayPosition, _duration);
            _nextClotheDisplay.GetComponent<CanvasGroup>().DOFade(1, _duration);
            _currentClotheDisplay.DORotate(new Vector3(0f, 0f, 180f), _duration);
            _currentClotheDisplay.GetComponent<CanvasGroup>().DOFade(0, _duration).OnComplete(DoAnimPart2);
        }

        private void DoAnimPart2()
        {
            _currentClotheDisplay.rotation = Quaternion.Euler(0f, 0f, 0f);
            _currentClotheDisplay.DOMove(_nextClotheDisplayPosition, _duration);
            _currentClotheDisplay.GetComponent<Image>().sprite = _nextClotheDisplaySprite;
            _currentClotheDisplay.GetComponent<CanvasGroup>().DOFade(.5f, _duration).OnComplete(DoAnimPart3);
        }

        private void DoAnimPart3()
        {
            (_nextClotheDisplay, _currentClotheDisplay) = (_currentClotheDisplay, _nextClotheDisplay);
        }

        private void ConfigNewClotheDisplayInternal(Sprite current, Sprite next)
        {
            if (!_initialized)
            {
                InitClotheDisplay(current, next);
                return;
            }
            _nextClotheDisplaySprite = next;
            DoAnim();
        }

        public static void ConfigNewClotheDisplay(Sprite current, Sprite next)
        {
            Instance.ConfigNewClotheDisplayInternal(current, next);
        }
        
        public static void InitClotheDisplay(Sprite current, Sprite next)
        {
            Instance.InitClotheDisplayInternal(current, next);
        }

        private void InitClotheDisplayInternal(Sprite current, Sprite next)
        {
            _currentClotheDisplay.GetComponent<CanvasGroup>().alpha = 1f;
            _currentClotheDisplay.GetComponent<Image>().sprite = current;
            _nextClotheDisplay.GetComponent<Image>().sprite = next;
            _initialized = true;
        }
    }
}