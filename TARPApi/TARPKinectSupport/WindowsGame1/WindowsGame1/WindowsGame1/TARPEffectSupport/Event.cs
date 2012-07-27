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
using Support;
#endregion


namespace Support
{
    /// <summary>
    /// First person camera component for the demos, rotated by mouse.
    /// </summary>
    public class Event
    {

        Game game;
        World world;
        public Event(Game game,World world)
        {
            this.game = game;
            this.world = world;
        }

        private bool PressedOnce(Keys key)
        {
            bool keyboard = keyState.IsKeyDown(key) && !keyboardPreviousState.IsKeyDown(key);
            return keyboard;
        }

        KeyboardState keyState;
        KeyboardState keyboardPreviousState;

        public void KeyCheck()
        {
            keyState = Keyboard.GetState();

            if (PressedOnce(Keys.Escape)) game.Exit();
            if (PressedOnce(Keys.Space))
            {
             RigidBody body = new RigidBody(new SphereShape(3.0f));
             body.Position = new JVector(0, 0, 25f);
             body.LinearVelocity = new JVector(0, 0, 25f);
             world.AddBody(body);
            }
            keyboardPreviousState = keyState;

        }
    }
}
