using System.Collections.Generic;
using UnityEngine;

namespace Common.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 Median(Vector3 point1, Vector3 point2) 
        {
            return (point1 + (point2 - point1) * .5f);
        }
        
        public static Vector3 Center(List<Vector3> positions)
        {
            Bounds bound = new Bounds(positions[0], Vector3.zero);
            for(int i = 0; i < positions.Count; i++)
            {
                bound.Encapsulate(positions[i]);
            }
            return bound.center;
        }
    }
}