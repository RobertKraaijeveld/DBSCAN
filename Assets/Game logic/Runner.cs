using System.Diagnostics;
using System.Linq;
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

            var parsedStars = StarsParser.GetParsedStars();
            var clusterer = this.gameObject.GetComponent<DbscanClusterer>();

            var clusteredStars = clusterer.GetClusteredStars(parsedStars);
            var colorsPerCluster = ColorCreator.GetColorPerCluster(clusteredStars.Select(s => s.ClusterNumber).Distinct());

            //Instantiating star prefabs
            foreach (var currentStar in clusteredStars)
            {
                GameObject starObjectClone = (GameObject) Instantiate(starObject, currentStar.Position, transform.rotation);
                starObjectClone.GetComponent<MeshRenderer>().material.color = colorsPerCluster[currentStar.ClusterNumber];
            }

            UnityEngine.Debug.Log("DONE: Elapsed time: " + stopWatch.ElapsedMilliseconds / 100 + "s");
        }
    }
}