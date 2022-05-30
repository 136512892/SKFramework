using UnityEngine;

namespace SK.Framework
{
    public static class CanvasGroupExtension 
    {
        public static CanvasGroup SetAlpha(this CanvasGroup self, float alpha)
        {
            self.alpha = alpha;
            return self;
        }
        public static CanvasGroup SetBlocksRaycasts(this CanvasGroup self, bool blocksRaycasts)
        {
            self.blocksRaycasts = blocksRaycasts;
            return self;
        }
        public static CanvasGroup SetIgnoreParentGroups(this CanvasGroup self, bool ignoreParentGroups)
        {
            self.ignoreParentGroups = ignoreParentGroups;
            return self;
        }
        public static CanvasGroup SetInteractable(this CanvasGroup self, bool interactable)
        {
            self.interactable = interactable;
            return self;
        }
    }
}