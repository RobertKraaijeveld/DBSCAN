using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Algorithm.KDTree;
using UnityEngine;

public class DbscanClusterer : MonoBehaviour
{
    public int MinimumNeighboursAmount;
    public float Epsilon;

    private List<KDNode<Star>> inputStarsNodes;


    public List<Star> GetClusteredStars(KDTree<Star> starTree)
    {
        inputStarsNodes = starTree.TreeToList(starTree.RootNode).ToList();

        
        int clusterCounter = 0;

        foreach (var starNode in inputStarsNodes)
        {
            //If we already processed this star, skip it
            if (starNode.value.Visited)
                continue;

            starNode.value.Visited = true;

            var KNearestNeighboursFinder = new KDNearestNeighbours<Star>(starTree.RootNode);
            var neighbours = KNearestNeighboursFinder.FindNearestNeighbour(starNode.value.Position, Epsilon);

            //If not enough neighbours, label as noise and continue.
            if (neighbours.Count < MinimumNeighboursAmount)
            {
                starNode.value.IsNoise = true;
            }
            else
            {
                //Else, start new cluster.
                clusterCounter++;
                starNode.value.ClusterNumber = clusterCounter;

                //Expanding the new cluster
                var seedSet = neighbours;
                
                while(seedSet.Count > 0)
                {
                    var currentSeedPoint = seedSet[0];
                    
                    if (currentSeedPoint.value.Visited == false)
                    {
                        currentSeedPoint.value.Visited = true;

                        var seedStarsNeighbours = KNearestNeighboursFinder.FindNearestNeighbour(currentSeedPoint.value.Position, Epsilon);
                        if (seedStarsNeighbours.Count >= MinimumNeighboursAmount)
                            seedSet.AddRange(seedStarsNeighbours);

                        if (currentSeedPoint.value.ClusterNumber == Star.UNASSIGNED_CLUSTER_NO)
                            currentSeedPoint.value.ClusterNumber = clusterCounter;
                    }
                    
                    //Doing this to avoid infinite loop
                    seedSet.Remove(currentSeedPoint);
                }
            }
        }
        return inputStarsNodes.Where(s => s.value.IsNoise == false).Select(s => s.value).ToList();
    }
}