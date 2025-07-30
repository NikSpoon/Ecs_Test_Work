using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine;

public class GraphSmokeTest : MonoBehaviour
{
    public AnimationClip idle;

    void Start()
    {
        var animator = GetComponent<Animator>();
        var graph = PlayableGraph.Create();
        var output = AnimationPlayableOutput.Create(graph, "out", animator);
        var play = AnimationClipPlayable.Create(graph, idle);

        output.SetSourcePlayable(play);
        graph.Play();
    }
}