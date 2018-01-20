using System;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace Algorithm.KDTree
{
    public class KDNode<T> where T: AbstractThreeDimensionalPoint
    {
        public T value;
        public int depth;
        public bool IsLeaf;

        public KDNode<T> parent;
        public KDNode<T> lesserChild;
        public KDNode<T> greaterChild;

        public void Write()
        {
            Debug.Log("| VALUE: " + value.Position + ", IsLeaf = " + IsLeaf + ", Depth = " 
                      + depth + ", parent = ");
            
            if(parent != null)
                parent.Write();
        }

        public override bool Equals(System.Object other)
        {
            if (!(other is KDNode<T>))
                return false;
            
            var otherNode = (KDNode<T>) other;
            return value.Position.Equals(otherNode.value.Position);
        }
    }
}