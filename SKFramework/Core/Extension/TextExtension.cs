using UnityEngine;
using UnityEngine.UI;

namespace SK.Framework
{
    public static class TextExtension
    {
        public static T SetContent<T>(this T self, string content) where T : Text
        {
            self.text = content;
            return self;
        }
        public static T SetFont<T>(this T self, Font font) where T : Text
        {
            self.font = font;
            return self;
        }
        public static T SetFontStyle<T>(this T self, FontStyle fontStyle) where T : Text
        {
            self.fontStyle = fontStyle;
            return self;
        }
        public static T SetLineSpacing<T>(this T self, float lineSpacing) where T : Text
        {
            self.lineSpacing = lineSpacing;
            return self;
        }
        public static T SetRichText<T>(this T self, bool richText) where T : Text
        {
            self.supportRichText = richText;
            return self;
        }
        public static T SetAlignment<T>(this T self, TextAnchor alignment) where T : Text
        {
            self.alignment = alignment;
            return self;
        }
        public static T SetAlignByGeometry<T>(this T self, bool alignByGeometry) where T : Text
        {
            self.alignByGeometry = alignByGeometry;
            return self;
        }
        public static T SetHorizontalOverflow<T>(this T self, HorizontalWrapMode horizontalWrapMode) where T : Text
        {
            self.horizontalOverflow = horizontalWrapMode;
            return self;
        }
        public static T SetVerticalOverflow<T>(this T self, VerticalWrapMode verticalWrapMode) where T : Text
        {
            self.verticalOverflow = verticalWrapMode;
            return self;
        }
        public static T SetBestFit<T>(this T self, bool bestFit) where T : Text
        {
            self.resizeTextForBestFit = bestFit;
            return self;
        }
    }
}