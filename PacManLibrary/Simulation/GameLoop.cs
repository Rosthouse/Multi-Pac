using System;
using System.Collections.Generic;
using PacManShared.Enums;
using PacManShared.GameplayBehaviour;
using PacManShared.Util.TimeStamps;

namespace PacManShared.Simulation
{
    public delegate void FinishedSimulationEvent(object sender, EventArgs e);

    public delegate void StartingSimulationEvent(object sender, EventArgs e);

    enum SimulationState
    {
        Simulating,
        Idling
    }

    public class GameLoop
    {
        //Gameplay releated fields
        private PlayBehaviour playBehaviour;
        private DeathBehaviour deathBehaviour;


        // Key commands
        private Queue<Command> inputQueue;
        
        // Timers
        private SimulationGameTime simulationGameTime;

        private double simulationStep;

        private int idleTimer;
        private readonly int defaultIdleTimer;

        private int defaultSimulationStep = 20;


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

        public void Update(IGameTime gameTime, GameStateManager gameStateManager)
        {
            
            switch (gameStateManager.GameState)
            {
                case GameState.Playing:
                    this.playBehaviour.Update(gameTime, gameStateManager);
                    break;
                case GameState.Death:
                    this.deathBehaviour.Update(gameTime, gameStateManager);
                    break;
            }
        }

        public void SimulationLoop(IGameTime gameTime, GameStateManager gameStateManager)
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
                            simulationStep = Convert.ToInt32(inputQueue.Peek().time);
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

                    if(inputQueue.Count > 0)
                    {
                        simulationStep = inputQueue.Peek().time;
                    }

                    startSimulating = gameTime.TotalGameTime.Milliseconds;

                    GamePlaySimulation(gameStateManager);

                    int i = 0;
                    OnFinishedSimulation(gameStateManager, gameTime);
                    idleTimer = defaultIdleTimer;
                    break;
            }
        }

        private void GamePlaySimulation(GameStateManager gameStateManager)
        {
            if (inputQueue.Count > 0)
            {
                simulationStep = inputQueue.Peek().time;
            }


            simulationGameTime.SetElapsedMilliseconds(simulationStep);

            Update(simulationGameTime, gameStateManager);

            while(inputQueue.Count >0)
            {
                
                Command nextCommand = inputQueue.Dequeue();

                gameStateManager.getFromId(nextCommand.ID).Direction = nextCommand.direction;

                while(simulationStep > 0)
                {
                    if(simulationStep > defaultSimulationStep)
                    {
                        simulationGameTime.SetElapsedMilliseconds(defaultSimulationStep);
                        simulationStep -= defaultSimulationStep;

                    } else
                    {
                        simulationGameTime.SetElapsedMilliseconds(simulationStep);
                        simulationStep = 0;
                    }


                    Update(simulationGameTime, gameStateManager);
                }
                

                if(inputQueue.Count > 0)
                {
                    simulationStep = Convert.ToInt32(NextSimulationStep(nextCommand, inputQueue.Peek()));
                    simulationGameTime.SimulationTime -= simulationStep;   
                } else
                {
                    simulationGameTime.SimulationTime -= defaultSimulationStep;
                }
            }


            while(simulationGameTime.SimulationTime > 0)
            {
                simulationGameTime.SimulationTime -= defaultSimulationStep;
                simulationGameTime.SetElapsedMilliseconds(defaultSimulationStep);
                Update(simulationGameTime, gameStateManager);
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
            gameStateManager.CreateTimestamp(gameTime.TotalGameTime.TotalMilliseconds);


            EventArgs e = new EventArgs();
            if(FinishedSimulation != null)
            {
                FinishedSimulation(this, e);
            }
        }

        #endregion - Eventhandeling

        public double NextSimulationStep(Command x, Command y)
        {
            double time = x.time - y.time;

            if(time == 0)
            {
                return defaultSimulationStep;
            }

            return Math.Abs(time);
        }
    }
}
