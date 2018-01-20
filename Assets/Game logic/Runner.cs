using System.Diagnostics;
 using System.Linq;
 using Algorithm.KDTree;
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
             
             //Debugging KDTree
             var debugPoints = parsedStars;
             KDTree<Star> tree = new KDTree<Star>(debugPoints, 3);

             
             var clusteredStars = clusterer.GetClusteredStars(tree);
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