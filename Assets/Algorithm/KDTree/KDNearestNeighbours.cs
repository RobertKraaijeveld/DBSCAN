using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Algorithm.KDTree
{
    public class KDNearestNeighbours<T> where T: AbstractThreeDimensionalPoint
    {
        private KDNode<T> RootNode;
        private HashSet<KDNode<T>> AlreadyExamined = new HashSet<KDNode<T>>();
        private HashSet<KDNode<T>> results = new HashSet<KDNode<T>>();

        
        public KDNearestNeighbours(KDNode<T> rootNode)
        {
            this.RootNode = rootNode;
        }
        
        public List<KDNode<T>> FindNearestNeighbour(Vector3 inputVector, float Epsilon, int axis = -1) //-1 so root starts at axis 0/X-axis
        {
            //Finding the closest leaf node
            KDNode<T> currentNode = this.RootNode;
            while (!currentNode.IsLeaf)
            {
                var inputsVectorValueForThisDepth = inputVector[GetNextAxis(axis)];
                var currentNodesValueForThisDepth = currentNode.value.Position[GetNextAxis(axis)];

                if (inputsVectorValueForThisDepth >= currentNodesValueForThisDepth)
                {
                    //Input is greater, so go right
                    currentNode = currentNode.greaterChild;
                }
                else
                {
                    //Input is lesser, go left
                    currentNode = currentNode.lesserChild;
                }
            }
            var closestLeafYet = currentNode;


            //Go back up the tree, looking for better ones
            var betterNode = closestLeafYet;


            while (betterNode != null)
            {
                //Search node
                searchNode(inputVector, betterNode, axis++);
                betterNode = betterNode.parent;
            }


            //Load up the collection of the results
            List<KDNode<T>> collection = new List<KDNode<T>>();

            foreach (KDNode<T> resultNode in results)
            {
                //Select only the neighbours (out of the K closest) who are within epsilon
                if(Vector3.Distance(resultNode.value.Position, inputVector) <= Epsilon)
                    collection.Add(resultNode);
            }
            return collection;
        }

        private void searchNode(Vector3 inputValue, KDNode<T> node, int axis)
        {
            AlreadyExamined.Add(node);

            //Search node
            KDNode<T> lastNode = null;
            Double lastDistance = Double.MaxValue;
            
            if (results.Count > 0)
            {
                lastNode = results.Last();
                lastDistance = Vector3.Distance(lastNode.value.Position, inputValue); 
            }
            Double nodeDistance = Vector3.Distance(node.value.Position, inputValue);
            
            
            if (nodeDistance.CompareTo(lastDistance) < 0)
            {
                if (lastNode != null) results.Remove(lastNode);
                results.Add(node);
            }
            else if (nodeDistance.Equals(lastDistance))
            {
                results.Add(node);
            }
            else 
            {
                results.Add(node);
            }
            lastNode = results.Last();
            lastDistance = Vector3.Distance(lastNode.value.Position, inputValue);

            KDNode<T> lesser = node.lesserChild;
            KDNode<T> greater = node.greaterChild;

            //Search children branches, if axis aligned distance is less than current distance
            if (lesser != null && !AlreadyExamined.Contains(lesser))
            {
                AlreadyExamined.Add(lesser);

                double nodePoint = Double.MinValue;
                double valuePlusDistance = Double.MinValue;
                if (axis == 0)
                {
                    nodePoint = node.value.Position.x;
                    valuePlusDistance = inputValue.x - lastDistance;
                }
                else if (axis == 1)
                {
                    nodePoint = node.value.Position.y;
                    valuePlusDistance = inputValue.y - lastDistance;
                }
                else
                {
                    nodePoint = node.value.Position.z;
                    valuePlusDistance = inputValue.z - lastDistance;
                }
                bool lineIntersectsCube = ((valuePlusDistance <= nodePoint) ? true : false);

                //Continue down lesser branch
                if (lineIntersectsCube) searchNode(inputValue, lesser, axis);
            }
            
            if (greater != null && !AlreadyExamined.Contains(greater))
            {
                AlreadyExamined.Add(greater);

                double nodePoint = Double.MinValue;
                double valuePlusDistance = Double.MaxValue;
                if (axis == 0)
                {
                    nodePoint = node.value.Position.x;
                    valuePlusDistance = inputValue.x + lastDistance;
                }
                else if (axis == 1)
                {
                    nodePoint = node.value.Position.y;
                    valuePlusDistance = inputValue.y + lastDistance;
                }
                else
                {
                    nodePoint = node.value.Position.z;
                    valuePlusDistance = inputValue.z + lastDistance;
                }
                bool lineIntersectsCube = ((valuePlusDistance >= nodePoint) ? true : false);

                //Continue down greater branch
                if (lineIntersectsCube) searchNode(inputValue, greater, axis);
            }
        }

        public int GetNextAxis(int currAxis)
        {
            if (currAxis == 0)
                return 1;
            else if (currAxis == 1)
            {
                return 2;
            }
            else if (currAxis == 2)
            {
                return 0;
            }
            else
            {
                return 0;
            }
        }
    }
}