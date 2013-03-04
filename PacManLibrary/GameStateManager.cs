using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PacManShared.Entities.Player;
using PacManShared.Enums;
using PacManShared.LevelClasses;
using PacManShared.Simulation;
using PacManShared.Util.TimeStamps;

namespace PacManShared
{
    /// <summary>
    /// This clas holds all the current game objects. This class exists to separate object handeling from game logic.
    /// </summary>
    public class GameStateManager
    {
        private Level level;

        private List<MovableObject> movableObjects;
        private LinkedList<TimeStamp> timeStamps;
        private TimeStampManager timeStampManager;
        private GameState gameState;

        /// <summary>
        /// Default constructor, initializes all objects
        /// </summary>
        public GameStateManager()
        {
            this.level = null;
            this.movableObjects = new List<MovableObject>();
            gameState = GameState.Playing;
            timeStamps = new LinkedList<TimeStamp>();
            timeStampManager = new TimeStampManager(200);
        }

        /// <summary>
        /// The current gamestate
        /// </summary>
        public GameState GameState
        {
            get { return gameState; }
            set { gameState = value; }
        }

        /// <summary>
        /// The current level
        /// </summary>
        public Level Level
        {
            get { return this.level; }
            set { this.level = value; }
        }

        /// <summary>
        /// List of all Movable objects (Pacmans, Ghosts)
        /// </summary>
        public List<MovableObject> MovableObjects
        {
            get { return movableObjects; }
        }

        public LinkedList<TimeStamp> TimeStamps
        {
            get { return timeStamps; }
        }

        public TimeStampManager TimeStampManager
        {
            get { return timeStampManager; }
        }

        /// <summary>
        /// Adds players. Supports params
        /// </summary>
        /// <param name="movableObjects"></param>
        public void AddPlayers(params MovableObject[] movableObjects)
        {
            foreach (MovableObject o in movableObjects)
            {
                this.movableObjects.Add(o); 
            }
              
        }

        /// <summary>
        /// Removes a player
        /// </summary>
        /// <param name="movableObject">The player to remove</param>
        public void RemovePlayer(MovableObject movableObject)
        {
            movableObjects.Remove(movableObject);
        }

        /// <summary>
        /// Removes a player
        /// </summary>
        /// <param name="name">The name of the player</param>
        public void RemovePlayer(String name)
        {
            MovableObject toRemove = null;
            foreach (MovableObject movableObject in movableObjects)
            {
                if(movableObject.Name == name)
                {
                    toRemove = movableObject;
                    break;
                }
            }

            if(toRemove != null)
            {
                movableObjects.Remove(toRemove);
            }
        }

        /// <summary>
        /// Removes a player
        /// </summary>
        /// <param name="ID">the ID of the player</param>
        public void RemovePlayer(int ID)
        {
            MovableObject toRemove = null;
            foreach (MovableObject movableObject in movableObjects)
            {
                if(movableObject.ID == ID)
                {
                    toRemove = movableObject;
                    break;
                }
            }

            if(toRemove != null)
            {
                movableObjects.Remove(toRemove);
            }
        
        }

        /// <summary>
        /// Creates a timestamp for the given time
        /// </summary>
        /// <param name="time">The time value since the game (the actuall gameplay) started</param>
        public void CreateTimestamp(double time)
        {
            LevelStruct levelStruct = level.GetLevelStruct();

            List<MovObjStruct> movObjStructs = new List<MovObjStruct>();

            foreach (MovableObject movableObject in MovableObjects)
            {
                movObjStructs.Add(movableObject.GetStruct());
            }

            TimeStamp timeStamp = new TimeStamp(time, levelStruct, movObjStructs, this.gameState);

            TimeStamps.AddFirst(timeStamp);
            timeStampManager.Push(timeStamp);

            if(TimeStamps.Count > 1000)
            {
                TimeStamps.RemoveLast();
            }
        }

        public void ChangeFormerGameState(int time, params MovObjStruct[] movObjStruct)
        {
            double timeDifference = TimeStamps.First.Value.time - time;
            SetToFormerTimeStamp(time);

            for(int i = 0; i<movObjStruct.Length;i++)
            {
                foreach (MovableObject movableObject in movableObjects)
                {
                   //MovableObject check = movableObjects.Find(MovableObject.FindAfterID(movObjStruct[i].ID, movableObject));
                }
                
            }
        }

        /// <summary>
        /// Resets the gamestate manager to the closest possible time frame, given through the parameter
        /// </summary>
        /// <param name="time">The time to which the gamestate manager should reset to</param>
        public void SetToFormerTimeStamp(double time)
        {
            TimeStamp latest = GetTimeStamp(time);

            this.Level.ApplyStructure(latest.levelStruct);
            this.gameState = latest.gameState;

            List<MovObjStruct> movObjStructs = latest.movableObjects;
            movObjStructs.Sort(MovObjStruct.Compare);
            movableObjects.Sort(MovableObject.Compare);

            for(int i = 0; i<movableObjects.Count; i++)
            {
                movableObjects[i].ApplyStruct(latest.movableObjects[i]);
            }
        }

        public TimeStamp GetTimeStamp(double time)
        {
            TimeStamp latest = new TimeStamp();

            //We search for the first timestamp which has a higher time then the parameter, and use the preceeding timestamp to reset the manager
            //because this is the closest one we can get to the time parameter given and the first to have a valid game state
            

            while(TimeStamps.Count > 0)
            {
                TimeStamp current = TimeStamps.First.Value;
                TimeStamps.RemoveFirst();

                if (current.time < time)
                {
                    latest = current;
                    break;
                }
            }

            return latest;
        }

        /// <summary>
        /// Gets a list of all the specified movable objects inside the gamestate manager
        /// </summary>
        /// <typeparam name="T">Has to be a MovableObject or one of its subclasse<s/typeparam>
        /// <returns></returns>
        public List<T> GetAllOfType<T>() where T:MovableObject
        {
            List<T> list = new List<T>();

            foreach (MovableObject movableObject in movableObjects)
            {
                T testObject = movableObject as T;

                if(testObject != null)
                {
                    list.Add(testObject);
                }
            }

            return list;
        }

        public MovableObject getFromId(int id)
        {
            foreach (MovableObject movOb in movableObjects)
            {
                if (id == movOb.ID)
                {
                    return movOb;
                }
            }
            return null;
        }

        
        public Stack<Command> RollBack(double time, int localId)
        {
            TimeStamp t = timeStampManager.Peek();
            Stack<Command> commands = new Stack<Command>();

            while(t.time > time)
            {
                t = timeStampManager.Pop();
                foreach (MovObjStruct movableObject in t.movableObjects)
                {
                    if(movableObject.ID == localId)
                    {
                        commands.Push(new Command(movableObject.direction, movableObject.ID, t.time));
                    }
                }
            } 

            return commands;
        }
    }
}