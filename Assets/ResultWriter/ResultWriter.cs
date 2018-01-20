using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ResultWriter
{
    public class ResultWriter
    {
        public static void WriteClustersToJson(List<Star> clusteredStars)
        {
            //Todo change to wasd controls

            //File opening
            var jsonString = "[\n";

            var allClusterNumbers = clusteredStars.Select(s => s.ClusterNumber).Distinct();
            foreach (var clusterNumber in allClusterNumbers)
            {
                var starsWithinThisCluster = clusteredStars.Where(s => s.ClusterNumber == clusterNumber);
                
                //Cluster obj opening
                jsonString += "\t { \n";
                jsonString += "\"Cluster no.\" : \"" + clusterNumber + "\", ";
                
                //Cluster array opening
                jsonString += "\"Contents\" : [ \n";
                foreach (var starInCluster in starsWithinThisCluster)
                    jsonString += StarToJson(starInCluster);                
                
                //Cluster array close
                jsonString += "]";

                //Cluster obj close
                jsonString += "},";
            }
            //File closing
            jsonString += "\n]";
            
            File.WriteAllText(@"Assets/Result.json", jsonString);
        }

        private static string StarToJson(Star s)
        {
            var json = "{ \n";
            json += "\"Star id\" : \"" + s.Id + "\", ";
            json += "\"Vector\" : \"" + s.Position + "\" },";

            return json;
        }
        
    }
}