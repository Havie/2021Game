// Animancer // Copyright 2020 Kybernetik //

using System.Collections.Generic;
using UnityEngine;

namespace Animancer
{
    /// <summary>
    /// A <see cref="ScriptableObject"/> based <see cref="ITransition"/>s which can create a
    /// <see cref="ClipState"/> when passed into <see cref="AnimancerPlayable.Play(ITransition)"/>.
    /// </summary>
    /// <remarks>
    /// When adding a <see cref="CreateAssetMenuAttribute"/> to any derived classes, you can use
    /// <see cref="Strings.MenuPrefix"/> and <see cref="Strings.AssetMenuOrder"/>.
    /// </remarks>
    [HelpURL(Strings.APIDocumentationURL + "/" + nameof(AnimancerTransition))]
    public abstract class AnimancerTransition : ScriptableObject, ITransition, IAnimationClipSource
    {
        /************************************************************************************************************************/

        /// <summary>
        /// Returns the <see cref="ITransition"/> wrapped by this <see cref="ScriptableObject"/>.
        /// </summary>
        public abstract ITransition GetTransition();

        /************************************************************************************************************************/

        /// <summary>Wraps <see cref="ITransition.FadeDuration"/>.</summary>
        public float FadeDuration => GetTransition().FadeDuration;

        /// <summary>Wraps <see cref="IHasKey.Key"/>.</summary>
        public object Key => GetTransition().Key;

        /// <summary>Wraps <see cref="ITransition.FadeMode"/>.</summary>
        public FadeMode FadeMode => GetTransition().FadeMode;

        /// <summary>Wraps <see cref="ITransition.CreateState"/>.</summary>
        public AnimancerState CreateState() => GetTransition().CreateState();

        /// <summary>Wraps <see cref="ITransition.Apply"/>.</summary>
        public void Apply(AnimancerState state) => GetTransition().Apply(state);

        /************************************************************************************************************************/

        /// <summary>Wraps <see cref="AnimancerUtilities.GatherFromSource(ICollection{AnimationClip}, object)"/>.</summary>
        public void GetAnimationClips(List<AnimationClip> clips) => clips.GatherFromSource(GetTransition());

        /************************************************************************************************************************/
    }

    /************************************************************************************************************************/

    /// <summary>
    /// An <see cref="AnimancerTransition"/> which uses a generic field for its <see cref="ITransition"/>.
    /// </summary>
    [HelpURL(Strings.APIDocumentationURL + "/" + nameof(AnimancerTransition) + "_1")]
    public class AnimancerTransition<T> : AnimancerTransition where T : ITransition
    {
        /************************************************************************************************************************/

        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("_Animation")]
        private T _Transition;

        /// <summary>[<see cref="SerializeField"/>]
        /// The <see cref="ITransition"/> wrapped by this <see cref="ScriptableObject"/>.
        /// <para></para>
        /// WARNING: the <see cref="AnimancerState.Transition{TState}.State"/> holds the post recently played state, so
        /// if you are sharing this transition between multiple objects it will only remember one of them.
        /// <para></para>
        /// You can use <see cref="AnimancerPlayable.StateDictionary.GetOrCreate(ITransition)"/> or
        /// <see cref="AnimancerLayer.GetOrCreateState(ITransition)"/> to get or create the state for a
        /// specific object.
        /// </summary>
        public ref T Transition => ref _Transition;

        /// <summary>
        /// Returns the <see cref="ITransition"/> wrapped by this <see cref="ScriptableObject"/>.
        /// </summary>
        public override ITransition GetTransition() => _Transition;

        /************************************************************************************************************************/
    }
}

/************************************************************************************************************************/

#if UNITY_EDITOR
namespace Animancer.Editor
{
    [UnityEditor.CustomEditor(typeof(AnimancerTransition), true)]
    internal class AnimancerTransitionEditor : ScriptableObjectEditor { }
}
#endif
