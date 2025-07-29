using System.Collections.Generic;
using Unity.NetCode.Hybrid;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using Unity.NetCode;

[CreateAssetMenu(fileName = "PlayerGhostAnimationGraph", menuName = "NetCode/GhostAnimationGraphAsset")]
public class PlayerGhostAnimationGraphAsset : GhostAnimationGraphAsset
{
    [SerializeField] private AnimationClip idleClip;
    [SerializeField] private AnimationClip runClip;

    public override Playable CreatePlayable(GhostAnimationController controller, PlayableGraph graph, List<GhostPlayableBehaviour> behaviours)
    {
        var mixer = AnimationMixerPlayable.Create(graph, 2);

        var idlePlayable = AnimationClipPlayable.Create(graph, idleClip);
        var runPlayable = AnimationClipPlayable.Create(graph, runClip);

        graph.Connect(idlePlayable, 0, mixer, 0);
        graph.Connect(runPlayable, 0, mixer, 1);

        var behaviour = new SimpleGhostPlayableBehaviour(mixer);
        behaviours.Add(behaviour);

        mixer.SetInputWeight(0, 1f);
        mixer.SetInputWeight(1, 0f);

        return mixer;
    }
    public override void RegisterPlayableData(IRegisterPlayableData register)
    {
        // Если у тебя нет кастомных данных, оставь пустым
    }
}
public class SimpleGhostPlayableBehaviour : GhostPlayableBehaviour
{
    private AnimationMixerPlayable mixer;
    public bool IsRunning;

    private bool lastIsRunning = false;

    public SimpleGhostPlayableBehaviour(AnimationMixerPlayable mixer)
    {
        this.mixer = mixer;
    }

    public override void PreparePredictedData(NetworkTick serverTick, float deltaTime, bool isRollback)
    {
        if (IsRunning == lastIsRunning)
            return; // нет изменений — пропускаем

        lastIsRunning = IsRunning;

        float targetRunWeight = IsRunning ? 1f : 0f;
        float targetIdleWeight = 1f - targetRunWeight;

        mixer.SetInputWeight(0, targetIdleWeight);
        mixer.SetInputWeight(1, targetRunWeight);
    }
}