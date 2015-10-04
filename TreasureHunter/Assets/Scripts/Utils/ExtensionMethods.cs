using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Treasure_Hunter.Utils
{
    public static class ExtensionMethods
    {
        public static void SetAlphaChannel(this Image image, float alpha)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        }

        public static void SetAlphaChannel(this Text text, float alpha)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        }

        public static void SetAlphaChannel(this SpriteRenderer sprite, float alpha)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
        }
    }
}
