using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ResultWriter;
using UnityEngine;

namespace Game_logic
{
    public class Runner : MonoBehaviour
    {
        //Used for prefab instantiation
        public GameObject starObject;

        private void Awake()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            //Parsing
            var parsedStars = StarsParser.GetParsedStars();

            //Clustering
            var clusterer = this.gameObject.GetComponent<DbscanClusterer>();
            var clusteredStars = clusterer.GetClusteredStars(parsedStars);
            var colorsPerCluster =
                ColorCreator.GetColorPerCluster(clusteredStars.Select(s => s.ClusterNumber).Distinct());

            //Instantiating star prefabs
            foreach (var currentStar in clusteredStars)
            {
                GameObject starObjectClone =
                    (GameObject) Instantiate(starObject, currentStar.Position, transform.rotation);
                starObjectClone.GetComponent<MeshRenderer>().material.color =
                    colorsPerCluster[currentStar.ClusterNumber];
            }
            UnityEngine.Debug.Log("DONE: Elapsed time: " + stopWatch.ElapsedMilliseconds / 1000 + " seconds.");
            UnityEngine.Debug.Log("Amount of original points: " + parsedStars.Count);

            //Writing the result
            ResultWriter.ResultWriter.WriteClustersToJson(clusteredStars);
        }
    }
}