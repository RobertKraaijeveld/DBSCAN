using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DbscanClusterer : MonoBehaviour
{
    public int MinimumNeighboursAmount;
    public float Epsilon;

    private List<Star> inputStars;


    public List<Star> GetClusteredStars(List<Star> originalStars)
    {
        inputStars = originalStars;
        int clusterCounter = 0;

        foreach (var star in inputStars)
        {
            //If we already processed this star, skip it
            if (star.Visited)
                continue;

            star.Visited = true;


            var neighbours = GetNeighbours(star);

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

                        var seedStarsNeighbours = GetNeighbours(currentSeedPoint);
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
        return inputStars.Where(s => s.IsNoise == false).ToList();
    }


    private List<Star> GetNeighbours(Star star)
    {
        var neighbours = new List<Star>();

        foreach (var otherStar in inputStars)
        {
            if (otherStar.Id != star.Id && Vector3.Distance(otherStar.Position, star.Position) <= Epsilon)
                neighbours.Add(otherStar);
        }
        return neighbours;
    }
}