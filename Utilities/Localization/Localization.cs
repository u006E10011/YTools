using TMPro;
using UnityEngine;

namespace YTools
{
    public class Localization : MonoBehaviour
    {
        [SerializeField] private string _ru;
        [SerializeField] private string _en;

        [SerializeField] private bool _isAutoSet = true;
        [SerializeField] private bool _updateOfTick = true;

        [SerializeField] private bool _isTranslateByKey;
        [SerializeField] private string _key;

        [SerializeField] private TMP_Text _text;

        public LocalizationData Data => LocalizationProvider.Data;

        private void OnValidate()
        {
            if (_text == null)
                _text = GetComponent<TMP_Text>();

            SetFont();
        }

        private void OnEnable()
        {
            if (_isAutoSet)
                SetTranslate();
        }

        private void Update()
        {
            if(_updateOfTick)
                SetTranslate();
        }

        public Localization SetTranslate(string end = "")
        {
            if (_isTranslateByKey && string.IsNullOrEmpty(_key) == false)
                _text.text = LocalizationProvider.Get(_key) + end;
            else
                _text.text = (LocalizationProvider.Type == LanguageType.RU ? _ru : _en) + end;

            return this;
        }

        public Localization SetTranslate(string key, string end = "")
        {
            if (string.IsNullOrEmpty(key) == false)
                _text.text = LocalizationProvider.Get(key) + end;

            return this;
        }

        public Localization Add(string text)
        {
            _text.text += text;
            return this;
        }

        public Localization Set(string text)
        {
            _text.text = text;
            return this;
        }

        private void SetFont()
        {
            if (_text && Data && Data.Font)
                _text.font = Data.Font;
        }
    }
}