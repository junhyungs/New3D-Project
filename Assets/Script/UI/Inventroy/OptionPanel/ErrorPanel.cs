using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace InventoryUI
{
    public class ErrorPanel : MonoBehaviour
    {
        [Header("TextMeshProUGUI")]
        [SerializeField] private TextMeshProUGUI _errorText;

        private void OnEnable()
        {
            _errorText.text = string.Empty;
        }

        public void SetErrorMessage(string message) =>
            _errorText.text = message;
    }
}

