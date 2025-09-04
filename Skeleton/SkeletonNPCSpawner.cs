using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkeletonNPCSpawner : MonoBehaviour
{
    [SerializeField] private List<SkeletonNPC> skeletons;
    public static SkeletonNPCSpawner Instance { get; set; }
    private List<SkeletonNPC> skeletonNPCs;

    private void Awake()
    {
        if (Instance != null)
        {
            // Throw error
        }

        Instance = this;
    }

    void Start()
    {
        skeletonNPCs = new List<SkeletonNPC>();
        foreach (SkeletonNPC npc in skeletons)
        {
            SkeletonNPC skele = Instantiate(npc, npc.transform.position, npc.transform.rotation);
            skele.gameObject.SetActive(true);
            skeletonNPCs.Add(skele);
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public List<SkeletonNPC> GetSkeletonNPCs()
    {
        return skeletonNPCs;
    }
}
