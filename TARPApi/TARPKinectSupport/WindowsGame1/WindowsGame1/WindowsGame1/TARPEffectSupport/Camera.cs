
#region Using Statements
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using Jitter;
using Jitter.Dynamics;
using Jitter.Collision;
using Jitter.LinearMath;
using Jitter.Collision.Shapes;
using Jitter.Dynamics.Constraints;
using Jitter.Dynamics.Joints;
using System.Reflection;
using Jitter.Forces;
using System.Diagnostics;
using SingleBodyConstraints = Jitter.Dynamics.Constraints.SingleBody;
using Jitter.DataStructures;
#endregion

namespace Support
{
     public class Camera : GameComponent
    {
        private Matrix view;
        private Matrix projection;
        public const float scale = 1 / 100.0f;

        //カメラ位置
        private Vector3 defaultCameraPosition = new Vector3(0.0f, 0.0f, 0.0f);
        private Vector3 cameraTarget = new Vector3(0.0f, 0.0f, 5000.0f * scale);
        private Vector3 defaultObjectPosition = new Vector3(0.0f, 0.0f, 2000.0f * scale);

        //角度
        public const float Horizontal_Degree = 57.0f / 2.0f;
        public const float Vertical_Degree = 43.0f / 2.0f;
        //public static readonly float H_tang_max = (float)Math.Tan(MathHelper.ToRadians(Horizontal_Degree));
        public const float H_tang_max = 0.5429557f; //Excelで事前計算
        //public static readonly float V_tang_max = (float)Math.Tan(MathHelper.ToRadians(Vertical_Degree));
        public const float V_tang_max = 0.393910476f;   //Excelで事前計算

        public const float xna_view_degree = 38.18917597f;  //Excelで事前計算
        public const float xna_view_radian = 0.666526859f;  //Excelで事前計算
        public const float xna_aspect_ratio = 1.378373344f; //Excelで事前計算

        //深度
        public float max_depth = 5000 * scale;
        public float min_depth = 1000 * scale;

        /// <summary>
        /// Gets camera view matrix.
        /// </summary>
        public Matrix View { get { return view; } }
        /// <summary>
        /// Gets or sets camera projection matrix.
        /// </summary>
        public Matrix Projection { get { return projection; } set { projection = value; } }
        /// <summary>
        /// Gets camera view matrix multiplied by projection matrix.
        /// 

       public Camera(Game game)
            : base(game)
        {
            
            projection = Matrix.CreatePerspectiveFieldOfView(
                   Camera.xna_view_radian,
                   Camera.xna_aspect_ratio,
                   min_depth,
                   max_depth
               );

            view = Matrix.CreateLookAt(
               this.defaultCameraPosition,
               this.cameraTarget,
               Vector3.Up
               );
        }
        
    }
}
