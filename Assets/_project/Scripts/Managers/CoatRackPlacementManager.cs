using System;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.Managers
{
    public class CoatRackPlacementManager : MonoBehaviour
    {
        [SerializeField] private Image _coatImage;
        [SerializeField] private Image _hatImage;
        [SerializeField] private Image _shoesImage;
        [SerializeField] private GameObject _coatRack;
        private float _duration = 1f;

        private void Awake()
        {
            _coatImage.DOFade(0, 0);
            _hatImage.DOFade(0, 0);
            _shoesImage.DOFade(0, 0);
        }

        public void ShowPlacement([CanBeNull] GameAction action)
        {
            DisableAllDisplays();
            if (action == null || (action.ActionType != SequenceConfig.ActionType.Drop &&
                                   action.ActionType != SequenceConfig.ActionType.Pickup))
            {
                return;
            }
            switch (action.ClotheType)
            {
                case SequenceConfig.ClotheType.Coat:
                    _coatImage.DOFade(1f, _duration);
                    break;
                case SequenceConfig.ClotheType.Hat:
                    _hatImage.DOFade(1f, _duration);
                    break;
                case SequenceConfig.ClotheType.Shoe:
                    _shoesImage.DOFade(1f, _duration);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void DisableAllDisplays()
        {
            _coatImage.DOFade(0, _duration/2);
            _hatImage.DOFade(0, _duration/2);
            _shoesImage.DOFade(0, _duration/2);
        }



        private void OnBeat()
        {
            _coatRack.transform.DOScale(.85f, 0);
            _coatRack.transform.DOScale(1f, 0.2f).SetEase(Ease.OutQuad);
            
            _coatImage.transform.DOScale(.8f, 0);
            _coatImage.transform.DOScale(1f, 0.2f).SetEase(Ease.OutQuad);
            _hatImage.transform.DOScale(.8f, 0);
            _hatImage.transform.DOScale(1f, 0.2f).SetEase(Ease.OutQuad);
            _shoesImage.transform.DOScale(.8f, 0);
            _shoesImage.transform.DOScale(1f, 0.2f).SetEase(Ease.OutQuad);
        }

        private void OnEnable()
        {
            GameManager.Instance.OnBeatUnityEvent += OnBeat;
        }
        
        private void OnDisable()
        {
            GameManager.Instance.OnBeatUnityEvent -= OnBeat;
        }
    }
}