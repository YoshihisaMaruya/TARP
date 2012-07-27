using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using Microsoft.Kinect;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TARPKinectSupport
{
    class SkeletonFrameClass
    {
        private KinectSensor kinectSensor;

        //フレーム取得用
        public Skeleton[] SkeletonArrayData { private set; get; }

        public float SizeScale;
        
        //骨格の各関節の位置
        public Vector3[][] skeletonList
        {
            private set;
            get;
        }

        //コンストラクタ
        public SkeletonFrameClass( KinectSensor sensor, float sizeScale ) 
        {
            this.kinectSensor = sensor;
            this.SizeScale = sizeScale;

            this.AllocateMemory();
        }

        //メモリ割り当て
        private void AllocateMemory()
        {
            #region 領域確保
            this.skeletonList = new Vector3[TARPKinectHelper.MAX_SKELTON_NUM][];

            for (int i = 0; i < TARPKinectHelper.MAX_SKELTON_NUM; i++)
            {
                this.skeletonList[i] = new Vector3[TARPKinectHelper.MAX_SKELTON_NUM];
            }
            #endregion       
        }

        #region 骨格情報の取得
        private bool skeleton_is_exist = false;
        public void getFrame(int timeout_ms)
        {
            lock (this)
            {
                using (SkeletonFrame skeletonFrame = this.kinectSensor.SkeletonStream.OpenNextFrame(timeout_ms))
                {
                    if (skeletonFrame != null)
                    {
                        if (!this.skeleton_is_exist)
                        {
                            this.SkeletonArrayData = new Skeleton[skeletonFrame.SkeletonArrayLength];
                            this.skeleton_is_exist = true;
                        }
                        skeletonFrame.CopySkeletonDataTo(SkeletonArrayData);

                        int num_skelton = 0;
                        foreach (Skeleton data in SkeletonArrayData)
                        {
                            // 骨格情報が完全に取得されている場合
                            if (SkeletonTrackingState.Tracked == data.TrackingState)
                            {
                                // Set joints
                                foreach (Joint joint in data.Joints)
                                {
                                    skeletonList[num_skelton][(int)joint.JointType].X = -joint.Position.X * 1000 * this.SizeScale;
                                    skeletonList[num_skelton][(int)joint.JointType].Y = joint.Position.Y * 1000 * this.SizeScale;
                                    skeletonList[num_skelton][(int)joint.JointType].Z = joint.Position.Z * 1000 * this.SizeScale;
                                }

                                num_skelton++;

                                if (num_skelton >= TARPKinectHelper.MAX_SKELTON_NUM)
                                    break;
                            }
                        }

                        //取得出来なかった部分はゼロベクトルで埋めておく
                        for (int i = num_skelton; i < TARPKinectHelper.MAX_SKELTON_NUM; i++)
                        {
                            for (int j = 0; j < TARPKinectHelper.MAX_JOINT_NUM; j++)
                            {
                                this.skeletonList[i][j] = Vector3.Zero;
                            }
                        }
                    }
                    else
                    {
                        // skeletonFrame is null because the request did not arrive in time
                    }
                }
            }
        }
        #endregion
    }
}
