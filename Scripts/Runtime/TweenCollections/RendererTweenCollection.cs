using FullCircleTween.Attributes;
using FullCircleTween.Core;
using UnityEngine;
using UnityEngine.Scripting;

namespace FullCircleTween.TweenCollections
{
    [TweenCollection]
    public static class RendererTweenCollection
    {
        private static readonly int ColorId = Shader.PropertyToID("_Color");
        [Preserve] public static Tween<Color> TweenPropertyColor(this Renderer target, Color toValue, float duration)
        {
            var propertyBlock = new MaterialPropertyBlock();
            return Tween<Color>.To(target, () =>
            {
                target.GetPropertyBlock(propertyBlock);
                return propertyBlock.GetColor(ColorId);
            }, value =>
            {
                propertyBlock.SetColor(ColorId, value);
                target.SetPropertyBlock(propertyBlock);
            }, toValue, duration);
        }
        
        private static readonly int TintId = Shader.PropertyToID("_Tint");
        [Preserve] public static Tween<Color> TweenPropertyTint(this Renderer target, Color toValue, float duration)
        {
            var propertyBlock = new MaterialPropertyBlock();
            return Tween<Color>.To(target, () =>
            {
                target.GetPropertyBlock(propertyBlock);
                return propertyBlock.GetColor(TintId);
            }, value =>
            {
                propertyBlock.SetColor(TintId, value);
                target.SetPropertyBlock(propertyBlock);
            }, toValue, duration);
        }
    }
}