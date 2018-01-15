using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game_logic
{
    public static class ColorCreator
    {
        //using these standard colors for the first X colors because they are nice and distinct
        private static Dictionary<int, Color> StandardColors = new Dictionary<int, Color>()
        {
            {0, Color.black}, 
            {1, Color.blue}, 
            {2, Color.cyan},
            {3, Color.green}, 
            {4, Color.magenta},
            {5, Color.red}, 
            {6, Color.yellow},
        };
        
        
        public static Dictionary<int, Color> GetColorPerCluster(IEnumerable<int> allUniqueClusterNumbers)
        {
            Dictionary<int, Color> colorsPerCluster = new Dictionary<int, Color>();
            Debug.Log("Cluster count: " + allUniqueClusterNumbers.Count());
            
            foreach (var uniqueClusterNumber in allUniqueClusterNumbers)
            {
                if (StandardColors.ContainsKey(uniqueClusterNumber))
                    colorsPerCluster.Add(uniqueClusterNumber, StandardColors[uniqueClusterNumber]);                    
                else
                    //Gets a random color for each unique int
                    colorsPerCluster.Add(uniqueClusterNumber, Random.ColorHSV());    
            }
            return colorsPerCluster;
        }
    }
}