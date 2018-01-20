using UnityEngine;

public class Star 
{
    public static readonly int UNASSIGNED_CLUSTER_NO = -1;
    
    public int Id { get; set; }
    public int ClusterNumber;

    public Vector3 Position { get; set; }
    public bool IsNoise;
    public bool Visited;

    public Star(int id, Vector3 position)
    {
        Id = id;
        Position = position;
        ClusterNumber = UNASSIGNED_CLUSTER_NO;
    }
}
