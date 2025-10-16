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
        [SerializeField] private Transform _nextParentTransform;
        [SerializeField] private Transform _currentParentTransform;
        
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
            InitClotheDisplayInternal(null, null, false);
        }

        private void DoAnim()
        {
            _nextClotheDisplay.DOMove(_currentClotheDisplayPosition, _duration);
            _nextClotheDisplay.GetComponent<CanvasGroup>().DOFade(1, _duration);
            _nextClotheDisplay.DOScale(Vector3.one, _duration);
            _currentClotheDisplay.DORotate(new Vector3(0f, 0f, 90f), _duration);
            _currentClotheDisplay.DOScale(Vector3.one * 0.8f, _duration);
            _currentClotheDisplay.GetComponent<CanvasGroup>().DOFade(0, _duration).OnComplete(DoAnimPart2);
        }

        private void DoAnimPart2()
        {
            _currentClotheDisplay.SetParent(_nextParentTransform);
            _nextClotheDisplay.SetParent(_currentParentTransform);
            _currentClotheDisplay.rotation = Quaternion.Euler(0f, 0f, 0f);
            _currentClotheDisplay.localScale = Vector3.one * 0.3f;
            _currentClotheDisplay.DOScale(Vector3.one * 0.6f, _duration);
            _currentClotheDisplay.DOMove(_nextClotheDisplayPosition, _duration);
            SetSpriteOf(_nextClotheDisplaySprite, _currentClotheDisplay.GetComponent<Image>());
            // _currentClotheDisplay.GetComponent<Image>().sprite = _nextClotheDisplaySprite;
            _currentClotheDisplay.GetComponent<CanvasGroup>().DOFade(.8f, _duration).OnComplete(DoAnimPart3);
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

        private void InitClotheDisplayInternal(Sprite current, Sprite next, bool isInitialized = true)
        {
            _nextClotheDisplayPosition = _nextClotheDisplay.position;
            _currentClotheDisplayPosition = _currentClotheDisplay.position;
            _nextClotheDisplay.localScale = Vector3.one * 0.6f;
            _nextClotheDisplay.GetComponent<CanvasGroup>().alpha = .8f;
            _currentClotheDisplay.GetComponent<CanvasGroup>().alpha = 1f;
            // _currentClotheDisplay.GetComponent<Image>().sprite = current;
            SetSpriteOf(current, _currentClotheDisplay.GetComponent<Image>());
            SetSpriteOf(next, _nextClotheDisplay.GetComponent<Image>());
            // _nextClotheDisplay.GetComponent<Image>().sprite = next;
            _initialized = isInitialized;
        }

        private void SetSpriteOf(Sprite sprite, Image image)
        {
            if (sprite)
            {
                image.sprite = sprite;
                image.enabled = true;
            }
            else
            {
                image.enabled = false;
            }
        }
    }
}