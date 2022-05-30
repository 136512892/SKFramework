using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace SK.Framework
{
    public static class InputFieldExtension
    {
        public static T SetTextComponent<T>(this T self, Text text) where T : InputField
        {
            self.textComponent = text;
            return self;
        }
        public static T SetTextContent<T>(this T self, string content) where T : InputField
        {
            self.text = content;
            return self;
        }
        public static T SetCharacterLimit<T>(this T self, int characterLimit) where T : InputField
        {
            self.characterLimit = characterLimit;
            return self;
        }
        public static T SetContentType<T>(this T self, InputField.ContentType contentType) where T : InputField
        {
            self.contentType = contentType;
            return self;
        }
        public static T SetLineType<T>(this T self, InputField.LineType lineType) where T : InputField
        {
            self.lineType = lineType;
            return self;
        }
        public static T SetPlaceholder<T>(this T self, Text placeholder) where T : InputField
        {
            self.placeholder = placeholder;
            return self;
        }
        public static T SetCaretBlinkRate<T>(this T self, float caretBlinkRate) where T : InputField
        {
            self.caretBlinkRate = caretBlinkRate;
            return self;
        }
        public static T SetCaretWidth<T>(this T self, int caretWidth) where T : InputField
        {
            self.caretWidth = caretWidth;
            return self;
        }
        public static T SetCustomCaretColor<T>(this T self, bool customCaretColor) where T : InputField
        {
            self.customCaretColor = customCaretColor;
            return self;
        }
        public static T SetCaretColor<T>(this T self, Color caretColor) where T : InputField
        {
            self.caretColor = caretColor;
            return self;
        }
        public static T SetSelectionColor<T>(this T self, Color selectionColor) where T : InputField
        {
            self.selectionColor = selectionColor;
            return self;
        }
        public static T SetHideMobileInput<T>(this T self, bool hideMobileInput) where T : InputField
        {
            self.shouldHideMobileInput = hideMobileInput;
            return self;
        }
        public static T SetReadOnly<T>(this T self, bool readOnly) where T : InputField
        {
            self.readOnly = readOnly;
            return self;
        }
        public static T SetOnValueChanged<T>(this T self, UnityAction<string> unityAction) where T : InputField
        {
            self.onValueChanged.AddListener(unityAction);
            return self;
        }
        public static T SetOnEndEdit<T>(this T self, UnityAction<string> unityAction) where T : InputField
        {
            self.onEndEdit.AddListener(unityAction);
            return self;
        }
    }
}