using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using PacManShared.Enums;
using PacManShared.Util.TimeStamps;

namespace PacManServer.GameStateCorrection
{
    class GameStateCorrecter
    {
        private NetServer server;

        public MovObjStruct FixMovableObject(int time, Point currentCell, Vector2 offset, Direction direction, int ID, MovObjType movObjType )
        {
            MovObjStruct movObjStruct = new MovObjStruct(currentCell, offset, direction, ID, movObjType);

            //server.SendMessage(movObjStruct, server.r)
            return movObjStruct;
        }
    }
}
