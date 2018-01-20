using UnityEngine;

namespace Algorithm
{
    public abstract class AbstractThreeDimensionalPoint
    {
        public abstract int Id { get; set; }
        public abstract Vector3 Position { get; set; }
    }
}