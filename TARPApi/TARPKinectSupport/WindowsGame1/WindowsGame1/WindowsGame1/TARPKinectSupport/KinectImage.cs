using System;
using Microsoft.Xna.Framework;

namespace TARPKinectSupport
{
    class KinectImage
    {
        public Color[] Color {  set; get; }
        public Vector3[][] Skeleton {  set; get; }
        public float[][] Depth {  set; get; }
        public byte[][] PlayerIndexes {  set; get; }
    }
}
