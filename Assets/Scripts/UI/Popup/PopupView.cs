using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Popup
{
    public class PopupView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _headerText;
        [SerializeField] private TMP_Text _contentText;
        [SerializeField] private RectTransform _contentRect;
        [SerializeField] private RectTransform _popupRect;
        [SerializeField] private Button _closeButton;

        private void Awake()
        {
            _closeButton.onClick.AddListener(Hide);
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveAllListeners();
        }

        public void Show(string header, string content)
        {
            _headerText.text = header;
            _contentText.text = content;

            UpdatePopupSize(content);
            gameObject.SetActive(true);
        }

        private void UpdatePopupSize(string content)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_contentRect);
            
            float newHeight = 200 + (content.Length * 2);
            
            newHeight = Mathf.Clamp(newHeight, 200, 6000f);
            
            _contentRect.sizeDelta = new Vector2(_contentRect.sizeDelta.x, newHeight);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}