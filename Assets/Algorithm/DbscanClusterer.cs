using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DbscanClusterer : MonoBehaviour
{
    public int MinimumNeighboursAmount;
    public float Epsilon;

    private List<Star> inputStarsNodes;


    public List<Star> GetClusteredStars(List<Star> input)
    {
        inputStarsNodes = input;
        var tree = new KDTree.KDTree<Star>(3);
        input.ForEach(p => tree.AddPoint(new double[3]{p.Position.x, p.Position.y, p.Position.z}, p));

        
        int clusterCounter = 0;
        foreach (var star in inputStarsNodes)
        {
            //If we already processed this star, skip it
            if (star.Visited)
                continue;

            star.Visited = true;

            //Todo: will the visited.false stuff be carried over?
            var neighbours = RegionQuery(tree, star.Position, Epsilon);
            
            //If not enough neighbours, label as noise and continue.
            if (neighbours.Count < MinimumNeighboursAmount)
            {
                star.IsNoise = true;
            }
            else
            {
                //Else, start new cluster.
                clusterCounter++;
                star.ClusterNumber = clusterCounter;
                
                //Expanding the new cluster
                var seedSet = neighbours;
                
                while(seedSet.Count > 0)
                {
                    var currentSeedPoint = seedSet[0];
                    
                    if (currentSeedPoint.Visited == false)
                    {
                        currentSeedPoint.Visited = true;

                        var seedStarsNeighbours = RegionQuery(tree, currentSeedPoint.Position, Epsilon);
                        if (seedStarsNeighbours.Count >= MinimumNeighboursAmount)
                            seedSet.AddRange(seedStarsNeighbours);

                        if (currentSeedPoint.ClusterNumber == Star.UNASSIGNED_CLUSTER_NO)
                            currentSeedPoint.ClusterNumber = clusterCounter;
                    }
                    
                    //Doing this to avoid infinite loop
                    seedSet.Remove(currentSeedPoint);
                }
            }
        }
        return inputStarsNodes.Where(s => s.IsNoise == false).ToList();
    }

    private List<Star> RegionQuery(KDTree.KDTree<Star> tree, Vector3 position, float Eps)
    {
        var pIter = tree.NearestNeighbors(new double[3] { position.x, position.y, position.z }, MinimumNeighboursAmount, Eps);
        return pIter.ToList();
    }
}