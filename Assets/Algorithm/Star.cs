using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star
{
    public static readonly int UNASSIGNED_CLUSTER_NO = -1;
    
    public readonly int Id;
    public int ClusterNumber;

    public Vector3 Position;
    public bool IsNoise;
    public bool Visited;

    public Star(int id, Vector3 position)
    {
        Id = id;
        Position = position;
        ClusterNumber = UNASSIGNED_CLUSTER_NO;
    }
}
