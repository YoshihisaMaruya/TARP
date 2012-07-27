#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
using System.IO;
using Jitter.DataStructures;
#endregion

namespace Support
{
    class DebugInfo
    {
        private VertexBuffer vertexBuffer = null;
        private SpriteBatch spriteBatch = null;
        private SpriteFont font = null;
        private GraphicsDevice graphicDevice = null;
        private float depth_line = 2000.0f * Camera.scale;

        public DebugInfo(GraphicsDevice graphicDevice,SpriteFont font)
        {
            this.graphicDevice = graphicDevice;
            init_xyz_line();
            init_draw_string(font);
        }

        private void init_xyz_line()
        {
            // 頂点バッファを作成する
            this.vertexBuffer = new VertexBuffer(graphicDevice,
                typeof(VertexPositionColor), 6, BufferUsage.None);
            VertexPositionColor[] vertices = new VertexPositionColor[6];

            vertices[0] = new VertexPositionColor(new Vector3(-1 * depth_line, 0.0f, depth_line), Color.Black);
            vertices[1] = new VertexPositionColor(new Vector3(depth_line, 0.0f, depth_line), Color.Black); //X
            vertices[2] = new VertexPositionColor(new Vector3(0.0f, -1 * depth_line, depth_line), Color.Yellow);
            vertices[3] = new VertexPositionColor(new Vector3(0.0f, depth_line, depth_line), Color.Yellow); //Y
            vertices[4] = new VertexPositionColor(new Vector3(0.0f, 0.0f, 0.0f), Color.Blue);
            vertices[5] = new VertexPositionColor(new Vector3(0.0f, 0.0f, 6000.0f), Color.Blue); //Z

            // 頂点データを書き込む
            this.vertexBuffer.SetData(vertices);
        }

        private void init_draw_string(SpriteFont font)
        {
            spriteBatch = new SpriteBatch(this.graphicDevice);
            this.font = font;
        }

        public void DrawDebug(BasicEffect basicEffect,string data){
            draw_xyz_line(basicEffect);
            draw_sprite(data);
        }

        private void draw_xyz_line(BasicEffect basicEffect)
        {
            // 描画に使用する頂点バッファをセット
            graphicDevice.SetVertexBuffer(this.vertexBuffer);
            basicEffect.World = Matrix.Identity;
            basicEffect.LightingEnabled = false;
            // パスの数だけ繰り替えし描画
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                // パスの開始
                pass.Apply();

                // ラインを描画する
                graphicDevice.DrawPrimitives(
                    PrimitiveType.LineList,
                    0,
                    3
                );
            }

        }

        private void draw_sprite(string data)
        {

            // スプライトの描画準備
            this.spriteBatch.Begin();

            // カメラの情報を表示
            this.spriteBatch.DrawString(this.font,data,new Vector2(20, 20), Color.Yellow);

            // スプライトの一括描画
            this.spriteBatch.End();
        }
    }
}
