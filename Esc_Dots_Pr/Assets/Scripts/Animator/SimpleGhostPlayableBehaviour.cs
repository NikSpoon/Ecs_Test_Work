using Unity.NetCode;
using UnityEngine.Animations;
using UnityEngine;
using Unity.NetCode.Hybrid;
using UnityEngine.Playables;

public class SimpleGhostPlayableBehaviour : GhostPlayableBehaviour
{
    private AnimationMixerPlayable mixer;
    private readonly GhostAnimationController controller;

    public bool IsRunning;
    private bool lastIsRunning = false;

    public SimpleGhostPlayableBehaviour(AnimationMixerPlayable mixer, GhostAnimationController controller)
    {
        this.mixer = mixer;
        this.controller = controller;
    }

    public override void PreparePredictedData(NetworkTick serverTick, float deltaTime, bool isRollback)
    {
        var input = controller.GetEntityComponentData<PlayerInput>();
        IsRunning = input.IsRunning;

        if (IsRunning == lastIsRunning)
            return;

        Debug.Log($"[GhostPlayable] isRunning: {IsRunning}");
        lastIsRunning = IsRunning;

        float targetRunWeight = IsRunning ? 1f : 0f;
        float targetIdleWeight = 1f - targetRunWeight;

        mixer.SetInputWeight(0, targetIdleWeight);
        mixer.SetInputWeight(1, targetRunWeight);
    }
}
