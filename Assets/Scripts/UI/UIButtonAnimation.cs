using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Breach
{
    public class UIButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] private TextMeshProUGUI _buttonText;

        private Button _button;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _rectTransform = GetComponent<RectTransform>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_button.interactable)
                _rectTransform.DOScale(1.2f, 0.1f).SetUpdate(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_button.interactable)
                _rectTransform.DOScale(1f, 0.1f).SetUpdate(true); ;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_button.interactable)
                _rectTransform.DOShakePosition(0.15f, 3).SetUpdate(true); ;
        }

        private void LateUpdate()
        {
            if (!_button.interactable)
                _buttonText.color = Color.gray;
            else
                _buttonText.color = Color.white;
        }
    }
}