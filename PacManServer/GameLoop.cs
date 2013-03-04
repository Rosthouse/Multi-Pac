using System;
using System.Collections.Generic;
using PacManShared;
using PacManShared.Entities.Player;
using PacManShared.Enums;
using PacManShared.GameplayBehaviour;
using PacManShared.LevelClasses;
using PacManShared.Util.TimeStamps;

namespace PacManServer
{
    public struct Command
    {
        public readonly Direction direction;
        public readonly int ID;
        public readonly int time;

        public Command(Direction direction, int ID, int time)
        {
            this.direction = direction;
            this.ID = ID;
            this.time = time;
        }
    }

    public delegate void FinishedSimulationEvent(object sender, EventArgs e);

    public delegate void StartingSimulationEvent(object sender, EventArgs e);

    enum SimulationState
    {
        Simulating,
        Idling
    }

    class GameLoop
    {
        //Gameplay releated fields
        private PlayBehaviour playBehaviour;
        private DeathBehaviour deathBehaviour;


        // Key commands
        private Queue<Command> inputQueue;
        
        // Timers
        private SimulationGameTime simulationGameTime;

        private int simulationStep;

        private int idleTimer;
        private readonly int defaultIdleTimer;

        private int defaultSimulationStep;


        private SimulationState simulationState;


        //Time at events
        private int endIdling = 0;
        private int startSimulating = 0;

        //Events
        public event StartingSimulationEvent StartingSimulation;
        public event FinishedSimulationEvent FinishedSimulation;

        public GameLoop(): this(100)
        {

            
        }

        public GameLoop(int idleTimer)
        {
            playBehaviour = new PlayBehaviour();
            deathBehaviour = new DeathBehaviour();


            this.idleTimer = idleTimer;
            this.defaultIdleTimer = idleTimer;

            InputQueue = new Queue<Command>();


            simulationState = SimulationState.Idling;
        }

        public Queue<Command> InputQueue
        {
            get { return inputQueue; }
            set { inputQueue = value; }
        }

        public void Update(IGameTime gameTime, ref GameStateManager gameStateManager)
        {
            
            switch (gameStateManager.GameState)
            {
                case GameState.Playing:
                    this.playBehaviour.Update(gameTime, ref gameStateManager);
                    break;
                case GameState.Death:
                    this.deathBehaviour.Update(gameTime, ref gameStateManager);
                    break;
            }

        }

        public void SimulationLoop(IGameTime gameTime, ref GameStateManager gameStateManager)
        {
            switch (simulationState)
            {
                case SimulationState.Idling:
                    idleTimer -= gameTime.ElapsedGameTime.Milliseconds;
                    if(idleTimer <=0)
                    {
                        OnStartingSimulation();
                        
                        if(inputQueue.Count > 0)
                        {
                            simulationStep = inputQueue.Peek().time;
                        } else
                        {
                            simulationStep = defaultSimulationStep;
                        }

                        
                    }

                    break;
                case SimulationState.Simulating:
                    endIdling = gameTime.TotalGameTime.Milliseconds;

                    int simulationTime = endIdling - startSimulating;

                    simulationGameTime = new SimulationGameTime(gameTime.TotalGameTime, simulationTime);

                    startSimulating = gameTime.TotalGameTime.Milliseconds;

                    GamePlaySimulation(gameStateManager);
                    OnFinishedSimulation(gameStateManager, gameTime);
                    idleTimer = defaultIdleTimer;
                    break;
            }
        }

        private void GamePlaySimulation(GameStateManager gameStateManager)
        {
            while(inputQueue.Count > 1)
            {
                simulationGameTime.SetElapsedMilliseconds(simulationStep);

                Update(simulationGameTime, ref gameStateManager);

                Command nextCommand = inputQueue.Dequeue();

                if(inputQueue.Count > 0)
                {
                    simulationStep = NextSimulationStep(nextCommand, inputQueue.Peek());
                    simulationGameTime.SimulationTime -= simulationStep;   
                } else
                {
                    simulationGameTime.SimulationTime -= defaultSimulationStep;
                }

            }

            while(simulationGameTime.SimulationTime > 0)
            {
                Update(simulationGameTime, ref gameStateManager);
                simulationGameTime.SimulationTime -= defaultSimulationStep;
            }
        }


        #region Eventhandeling
        private void OnStartingSimulation()
        {
            this.simulationState = SimulationState.Simulating;

            EventArgs e = new EventArgs();

            if(StartingSimulation != null)
            {
                StartingSimulation(this, e);
            }
        }

        private void OnFinishedSimulation(GameStateManager gameStateManager, IGameTime gameTime)
        {
            this.simulationState = SimulationState.Idling;
            gameStateManager.CreateTimestamp(gameTime.TotalGameTime.Milliseconds);


            EventArgs e = new EventArgs();
            if(FinishedSimulation != null)
            {
                FinishedSimulation(this, e);
            }
        }

        #endregion - Eventhandeling
        public bool CheckValidChange(int time, Direction direction, ref GameStateManager gameStateManager)
        {
            TimeStamp timeStamp = gameStateManager.GetTimeStamp(time);

            //double difference = gameStateManager.TimeStamps.First.Value.time - timeStamp.time + gameStateManager.TimeStamps.;
            
            double difference = gameStateManager.TimeStampManager.Peek().time - timeStamp.time +
                                gameStateManager.TimeStampManager.Peek().time -
                                gameStateManager.TimeStampManager.ElementAt(1).time;



            //TODO: remove this, since it's only needed to allow compiling)
            return true;
        }

        private int NextSimulationStep(Command x, Command y)
        {
            int time = x.time - y.time;

            if(time == 0)
            {
                return defaultSimulationStep;
            }

            return Math.Abs(time);
        }
    }
}
