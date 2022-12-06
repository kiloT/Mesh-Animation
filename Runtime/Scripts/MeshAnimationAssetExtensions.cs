namespace CodeWriter.MeshAnimation
{
    using UnityEngine;

    public static class MeshAnimationAssetExtensions
    {
        private static readonly int AnimationTimeProp = Shader.PropertyToID("_AnimTime");
        private static readonly int AnimationLoopProp = Shader.PropertyToID("_AnimLoop");

        public static void Play(this MeshAnimationAsset asset,
            MaterialPropertyBlock block,
            string animationName,
            float speed = 1f,
            float? normalizedTime = 0f)
        {
            MeshAnimationAsset.AnimationData data = GetAnimationData(asset, animationName);

            if (data == null)
            {
                return;
            }

            var start = data.startFrame;
            var length = data.lengthFrames;
            var s = speed / Mathf.Max(data.lengthSeconds, 0.01f);
            var time = normalizedTime.HasValue
                ? Time.timeSinceLevelLoad - Mathf.Clamp01(normalizedTime.Value) / s
                : block.GetVector(AnimationTimeProp).z;

            block.SetFloat(AnimationLoopProp, data.looping ? 1 : 0);
            block.SetVector(AnimationTimeProp, new Vector4(start, length, s, time));
        }
        
        public static AnimationClip GetAnimationClip(this MeshAnimationAsset asset, int index)
        {
            if (asset.animationClips.Length <= index)
            {
                return null;
            }
            
            return asset.animationClips[index];
        }

        public static AnimationClip GetAnimationClip(this MeshAnimationAsset asset, string animationName)
        {
            foreach (AnimationClip animationClip in asset.animationClips)
            {
                if (animationClip.name == animationName)
                {
                    return animationClip;
                }
            }

            return null;
        }
        
        public static MeshAnimationAsset.AnimationData GetAnimationData(this MeshAnimationAsset asset, int index)
        {       
            if (asset.animationData.Count <= index)
            {
                return null;
            }
            
            return asset.animationData[index];
        }
        
        public static MeshAnimationAsset.AnimationData GetAnimationData(this MeshAnimationAsset asset, string animationName)
        {
            foreach (var animationData in asset.animationData)
            {
                if (animationData.name == animationName)
                {
                    return animationData;
                }
            }

            return null;
        }
    }
}