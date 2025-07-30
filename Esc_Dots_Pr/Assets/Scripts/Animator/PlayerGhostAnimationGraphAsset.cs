using System.Collections.Generic;
using Unity.NetCode.Hybrid;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

[CreateAssetMenu(fileName = "PlayerGhostAnimationGraph", menuName = "NetCode/GhostAnimationGraphAsset")]
public class PlayerGhostAnimationGraphAsset : GhostAnimationGraphAsset
{
    [SerializeField] private AnimationClip idleClip;
    [SerializeField] private AnimationClip runClip;

    public override Playable CreatePlayable(GhostAnimationController controller, PlayableGraph graph, List<GhostPlayableBehaviour> behaviours)
    {
        var mixer = AnimationMixerPlayable.Create(graph, 2);

        var idlePlayable = AnimationClipPlayable.Create(graph, idleClip);
        idlePlayable.SetTime(0);
        idlePlayable.SetDuration(idleClip.length);
        idlePlayable.SetApplyFootIK(false);
        idlePlayable.SetApplyPlayableIK(false);
        idlePlayable.SetSpeed(1); // << ОБЯЗАТЕЛЬНО!

        var runPlayable = AnimationClipPlayable.Create(graph, runClip);
        runPlayable.SetTime(0);
        runPlayable.SetDuration(runClip.length);
        runPlayable.SetApplyFootIK(false);
        runPlayable.SetApplyPlayableIK(false);
        runPlayable.SetSpeed(1); // << ОБЯЗАТЕЛЬНО!

        graph.Connect(idlePlayable, 0, mixer, 0);
        graph.Connect(runPlayable, 0, mixer, 1);

        mixer.SetInputWeight(0, 1f); // idle включен
        mixer.SetInputWeight(1, 0f); // run выключен

        var animator = controller.GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("[GhostAnim] Animator not found on presentation.");
            return mixer;
        }

        var output = AnimationPlayableOutput.Create(graph, "AnimationOutput", animator);
        output.SetSourcePlayable(mixer);
        output.SetWeight(1f); // На всякий случай — убедимся что вывод активен

        var behaviour = new SimpleGhostPlayableBehaviour(mixer, controller);
        behaviours.Add(behaviour);

        return mixer;
    }

    public override void RegisterPlayableData(IRegisterPlayableData register)
    {
        // Не используется
    }
}
