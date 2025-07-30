using Unity.Entities;
using UnityEngine;
using Unity.NetCode.Hybrid;

public static class GhostAnimationControllerExtensions
{
    public static Animator GetAnimatorFromPresentation(this GhostAnimationController controller)
    {
        var ghostOwners = Object.FindObjectsByType<GhostPresentationGameObjectEntityOwner>(FindObjectsSortMode.None);

        foreach (var owner in ghostOwners)
        {
            if (owner == null)
                continue;

            var animCtrl = owner.GetComponent<GhostAnimationController>();
            if (animCtrl == controller)
            {
                return owner.GetComponentInChildren<Animator>();
            }
        }

        Debug.LogError("[GhostAnim] Presentation GameObject not found or missing Animator.");
        return null;
    }
}
