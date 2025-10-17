using System;
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

        private void Awake()
        {
            DisableAllDisplays();
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
                    _coatImage.enabled = true;
                    break;
                case SequenceConfig.ClotheType.Hat:
                    _hatImage.enabled = true;
                    break;
                case SequenceConfig.ClotheType.Shoe:
                    _shoesImage.enabled = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void DisableAllDisplays()
        {
            _coatImage.enabled = false;
            _hatImage.enabled = false;
            _shoesImage.enabled = false;
        }
    }
}