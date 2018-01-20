using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StarsParser 
{
    public static List<Star> GetParsedStars()
    {
        var parsedStars = new List<Star>();
        var starFileLines = File.ReadAllLines(@"Assets/stars.csv");
    
        //debugging
        var counter = 0;
        System.Random random = new System.Random();
        var sanitizedStarFileLines = starFileLines.Skip(1).ToArray();

        while (counter < sanitizedStarFileLines.Length)
        {
            parsedStars.Add(GetStarFromLine(sanitizedStarFileLines[counter]));
            counter++;
        }
        return parsedStars;
    }

    private static Star GetStarFromLine(string line)
    {
        var splitStarValeus = line.Split(',');
              
        var x = float.Parse(splitStarValeus[0]);
        var y = float.Parse(splitStarValeus[1]);
        var z = float.Parse(splitStarValeus[2]);
        var id = int.Parse(splitStarValeus[splitStarValeus.Length - 1]);
        
        return new Star(id, new Vector3(x,y,z));
    }
}
