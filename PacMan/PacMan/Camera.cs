using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PacManShared.Entities.Player;

namespace PacManClient
{
    public class Camera
    {
        private bool isStatic;

        private GraphicsDevice graphicsDevice;
        public Vector2 position; //Camera Position
        protected float rotation; //Camera rotation
        public Matrix transform; //Transformation Matrix
        protected float zoom; //Camera Zoom
        private MovableObject followingObject;

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Camera():this(1.0f, Vector2.Zero, 0.0f, true)
        {
        }

        /// <summary>
        /// Constructor for a static camera
        /// </summary>
        /// <param name="zoom">Zoom factor</param>
        /// <param name="position">On-screen position</param>
        /// <param name="rotation">Rotation in radians</param>
        /// <param name="isStatic">If the camera stays at the same place the whole time</param>
        /// <param name="graphicsDevice">A graphics device</param>
        public Camera(float zoom, Vector2 position, float rotation, bool isStatic, GraphicsDevice graphicsDevice): 
            this(zoom, position, rotation, isStatic)
        {
            this.graphicsDevice = graphicsDevice;
        }

        /// <summary>
        /// Constructor for a camera following an object
        /// </summary>
        /// <param name="zoom">Zoom factor</param>
        /// <param name="rotation">Rotation in radians</param>
        /// <param name="followingObject">The object the camera has to follow</param>
        /// <param name="graphicsDevice">A graphics device</param>
        public Camera(float zoom, float rotation, MovableObject followingObject, GraphicsDevice graphicsDevice):
            this(zoom, followingObject.Center, rotation, false, graphicsDevice)
        {
            this.followingObject = followingObject;
        }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="zoom">Zoom factor</param>
        /// <param name="position">On-screen position</param>
        /// <param name="rotation">Rotation in radians</param>
        /// <param name="isStatic">If this camera is static</param>
        public Camera(float zoom, Vector2 position, float rotation, bool isStatic)
        {
            this.isStatic = isStatic;
            this.zoom = zoom;
            this.position = position;
            this.rotation = rotation;
        }

        /// <summary>
        /// Gets or sets the zoom for the camera
        /// </summary>
        public float Zoom
        {
            get { return zoom; }
            set
            {
                zoom = value;
                if (zoom < 0.1f)
                {
                    zoom = 0.1f; //Dont allow negative values
                }
            }
        }

        /// <summary>
        /// Gets or sets the graphicsdevice
        /// </summary>
        public GraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
            set { graphicsDevice = value; }
        }

        /// <summary>
        /// Gets or sets the rotation in radians
        /// </summary>
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        /// <summary>
        /// Gets or sets the center of the camera
        /// </summary>
        public Vector2 Center
        {
            get
            {
                return new Vector2(position.X + (float) graphicsDevice.Viewport.Width/2,
                                   position.Y + (float) graphicsDevice.Viewport.Height/2);
            }
            set
            {
                Position = new Vector2(value.X - (float) graphicsDevice.Viewport.Width/2,
                                       value.Y - (float) graphicsDevice.Viewport.Height/2);
            }
        }

        /// <summary>
        /// Gets or sets the position of the camera
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set
            {
                if (!IsStatic)
                {
                    position = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the static field of the camera
        /// </summary>
        public bool IsStatic
        {
            get { return isStatic; }
            set { isStatic = value; }
        }

        /// <summary>
        /// Gets a transformation matrix for the next move
        /// </summary>
        /// <param name="graphicsDevice">A graphics device</param>
        /// <returns>A transformation matrix</returns>
        public Matrix GetTransformation(GraphicsDevice graphicsDevice)
        {
            transform = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0))*
                        Matrix.CreateRotationZ(Rotation)*
                        Matrix.CreateScale(new Vector3(Zoom, Zoom, 0));
            //Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width*0.7f, graphicsDevice.Viewport.Height*0.7f, 0))


            return transform;
        }


        /// <summary>
        /// Adds a Vector2 to the current cameraposition
        /// </summary>
        /// <param name="amount">The Vector2 to add</param>
        public void Move(Vector2 amount)
        {
            position += amount;
        }

        /// <summary>
        /// Updates the camera
        /// </summary>
        public void Update()
        {
            if(!IsStatic)
            {
               this.Center = followingObject.Center; 
            }
        }
    }
}