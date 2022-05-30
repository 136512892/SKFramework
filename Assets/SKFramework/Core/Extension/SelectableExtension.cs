using UnityEngine;
using UnityEngine.UI;

namespace SK.Framework
{
    public static class SelectableExtension
    {
        public static T SetInteractable<T>(this T self, bool interactable) where T : Selectable
        {
            self.interactable = interactable;
            return self;
        }
        public static T SetTransition<T>(this T self, Selectable.Transition transition) where T : Selectable
        {
            self.transition = transition;
            return self;
        }
        public static T SetTargetGraphic<T>(this T self, Graphic targetGraphic) where T : Selectable
        {
            self.targetGraphic = targetGraphic;
            return self;
        }
        public static T SetNormalColor<T>(this T self, Color normalColor) where T : Selectable
        {
            ColorBlock colorBlock = self.colors;
            colorBlock.normalColor = normalColor;
            self.colors = colorBlock;
            return self;
        }
        public static T SetHighlightedColor<T>(this T self, Color highlightedColor) where T : Selectable
        {
            ColorBlock colorBlock = self.colors;
            colorBlock.highlightedColor = highlightedColor;
            self.colors = colorBlock;
            return self;
        }
        public static T SetPressedColor<T>(this T self, Color pressedColor) where T : Selectable
        {
            ColorBlock colorBlock = self.colors;
            colorBlock.pressedColor = pressedColor;
            self.colors = colorBlock;
            return self;
        }
        public static T SetSelectedColor<T>(this T self, Color selectedColor) where T : Selectable
        {
            ColorBlock colorBlock = self.colors;
            colorBlock.selectedColor = selectedColor;
            self.colors = colorBlock;
            return self;
        }
        public static T SetDisabledColor<T>(this T self, Color disabledColor) where T : Selectable
        {
            ColorBlock colorBlock = self.colors;
            colorBlock.disabledColor = disabledColor;
            self.colors = colorBlock;
            return self;
        }
        public static T SetColorMultiplier<T>(this T self, float colorMultiplier) where T : Selectable
        {
            ColorBlock colorBlock = self.colors;
            colorBlock.colorMultiplier = colorMultiplier;
            self.colors = colorBlock;
            return self;
        }
        public static T SetFadeDuration<T>(this T self, float fadeDuration) where T : Selectable
        {
            ColorBlock colorBlock = self.colors;
            colorBlock.fadeDuration = fadeDuration;
            self.colors = colorBlock;
            return self;
        }
        public static T SetHighlightedSprite<T>(this T self, Sprite highlightedSprite) where T : Selectable
        {
            SpriteState spriteState = self.spriteState;
            spriteState.highlightedSprite = highlightedSprite;
            self.spriteState = spriteState;
            return self;
        }
        public static T SetPressedSprite<T>(this T self, Sprite pressedSprite) where T : Selectable
        {
            SpriteState spriteState = self.spriteState;
            spriteState.pressedSprite = pressedSprite;
            self.spriteState = spriteState;
            return self;
        }
        public static T SetSelectedSprite<T>(this T self, Sprite selectedSprite) where T : Selectable
        {
            SpriteState spriteState = self.spriteState;
            spriteState.selectedSprite = selectedSprite;
            self.spriteState = spriteState;
            return self;
        }
        public static T SetDisabledSprite<T>(this T self, Sprite disabledSprite) where T : Selectable
        {
            SpriteState spriteState = self.spriteState;
            spriteState.disabledSprite = disabledSprite;
            self.spriteState = spriteState;
            return self;
        }
    }
}