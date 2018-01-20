using Algorithm;
using UnityEngine;

public class Star : AbstractThreeDimensionalPoint
{
    public static readonly int UNASSIGNED_CLUSTER_NO = -1;
    
    public override readonly int Id;
    public int ClusterNumber;

    public override Vector3 Position { get; set; }
    public bool IsNoise;
    public bool Visited;

    public Star(int id, Vector3 position)
    {
        Id = id;
        Position = position;
        ClusterNumber = UNASSIGNED_CLUSTER_NO;
    }
}
