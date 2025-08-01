using Unity.Entities;

public enum AnimationState : byte
{
    Idle,
    Walk,
    Attack,
    Jump,
    Timer
}

public struct AnimationStateData : IComponentData
{
    public AnimationState Current;
}