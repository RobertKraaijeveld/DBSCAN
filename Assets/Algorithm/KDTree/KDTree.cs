using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking.Types;


namespace Algorithm.KDTree
{
    public class KDTree<T> where T : AbstractThreeDimensionalPoint
    {
        public readonly int K;
        public KDNode<T> RootNode;

        public KDTree(List<T> pointList, int K)
        {
            this.K = K;
            RootNode = ConstructTree(pointList, 0, null);
        }


        /*
            Creating the KDTree 
        */

        private KDNode<T> ConstructTree(List<T> pointList, int depth, KDNode<T> parent)
        {
            var node = new KDNode<T>();
            node.parent = parent;

            var currentAxis = depth % K;

            //Finding median and the points before/after it
            var orderedPoints = pointList.OrderBy(p => p.Position[currentAxis]).ToList();
            var medianPoint = orderedPoints[orderedPoints.Count / 2];

            var pointsBeforeMedian = orderedPoints.Where(v => v.Position[currentAxis] < medianPoint.Position[currentAxis]).ToList();
            var pointsAfterMedian = orderedPoints.Where(v => v.Position[currentAxis] > medianPoint.Position[currentAxis]).ToList();

            node.depth = depth;
            node.value = orderedPoints[orderedPoints.IndexOf(medianPoint)];

            //Recursing to the left and right, setting as leaf if there are no more points beside the median
            if (pointsBeforeMedian.Count > 0 && pointsAfterMedian.Count > 0)
            {
                node.lesserChild = ConstructTree(pointsBeforeMedian, depth + 1, node);
                node.greaterChild = ConstructTree(pointsAfterMedian, depth + 1, node);
            }
            else
                node.IsLeaf = true;

            return node;
        }

        private float FindMedian(List<T> orderedPointList, int currentAxis)
        {
            float median;

            //Odd amount of elements
            if (orderedPointList.Count % 2 != 0)
            {
                median = orderedPointList[orderedPointList.Count / 2].Position[currentAxis];
            }
            else
            {
                if (orderedPointList.Count > 2)
                {
                    var firstBeforeMedian = orderedPointList[(orderedPointList.Count / 2) - 1].Position[currentAxis];
                    var firstAfterMedian = orderedPointList[(orderedPointList.Count / 2) + 1].Position[currentAxis];

                    median = (firstBeforeMedian + firstAfterMedian) / 2;
                }
                else if (orderedPointList.Count == 2)
                {
                    var first = orderedPointList[0].Position[currentAxis];
                    var second = orderedPointList[1].Position[currentAxis];

                    return (first + second) / 2;
                }
                else
                    return orderedPointList[0].Position[currentAxis];
            }
            return median;
        }


        /*
            Traversal
        */

        public List<KDNode<T>> TreeToList(KDNode<T> currentNode)
        {
            //Do a depth first traversal
            var returnList = new List<KDNode<T>>();
            
            returnList.Add(RootNode);
            
            if(currentNode.lesserChild != null)
                returnList.AddRange(TreeToList(currentNode.lesserChild));
            
            if(currentNode.greaterChild != null)
                returnList.AddRange(TreeToList(currentNode.greaterChild));

            return returnList;
        }
    }
}