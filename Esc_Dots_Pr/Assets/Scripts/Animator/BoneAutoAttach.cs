using UnityEngine;

[ExecuteInEditMode]
public class BoneAutoAttach : MonoBehaviour
{
    public GameObject CharacterRoot;

    void Update()
    {
        if (!Application.isPlaying)
        {
            foreach (Transform child in GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BoneHierarchyAuthoring>() == null)
                {
                    var bone = child.gameObject.AddComponent<BoneHierarchyAuthoring>();
                    bone.CharacterRoot = CharacterRoot;
                    bone.BoneNameOverride = child.name;
                }
            }
        }
    }
}