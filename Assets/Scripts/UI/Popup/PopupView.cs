using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popup
{
    public class PopupView : MonoBehaviour
    {
        [SerializeField] private TMP_Text headerText;
        [SerializeField] private TMP_Text contentText;
        [SerializeField] private RectTransform contentRect;
        [SerializeField] private RectTransform popupRect;
        [SerializeField] private Button closeButton;

        private void Awake()
        {
            closeButton.onClick.AddListener(Hide);
        }

        private void OnDestroy()
        {
            closeButton.onClick.RemoveAllListeners();
        }

        public void Show(string header, string content)
        {
            headerText.text = header;
            contentText.text = content;

            UpdatePopupSize(content);
            gameObject.SetActive(true);
        }

        private void UpdatePopupSize(string content)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
            
            float newHeight = 200 + (content.Length * 2);
            
            newHeight = Mathf.Clamp(newHeight, 200, 6000f);
            
            contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, newHeight);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}