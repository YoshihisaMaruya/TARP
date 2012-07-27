using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Support
{
    class DebugConsole
    {
        public static Timer.MyTimerList timerList = new Timer.MyTimerList("result");
        private static int prev_outputSecond = 0;
        private static int updateCount = 0;

        public static void WriteLine(string str) 
        {
            System.Diagnostics.Debug.WriteLine(str);
        }

        public static void Update(GameTime gameTime) 
        {
            updateCount++;

            //5秒おきにデバッグ出力画面へ時間の表示
            int gameSecond = gameTime.TotalGameTime.Seconds;
            if (
                (prev_outputSecond != gameSecond) &&
                (gameSecond % 5 == 0)
                )
            {
                DebugConsole.timerList.printToConsole();
                DebugConsole.timerList.clearAllTimer();
                prev_outputSecond = gameSecond;

                if (gameTime.TotalGameTime.TotalMilliseconds > 0)
                {
                    DebugConsole.WriteLine("fps: " + (updateCount * 1000.0 / gameTime.TotalGameTime.TotalMilliseconds).ToString());
                }
            }


        }
    }
}
