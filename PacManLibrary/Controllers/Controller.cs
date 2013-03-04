using PacManShared.Enums;
using PacManShared.LevelClasses.Cells;

namespace PacManShared.Controllers
{
    public abstract class Controller
    {
        private Controller controller;
        private int id;

        public Controller(int id)
        {
            this.id = id;
        }

        public abstract Direction Direction { get; set; }
        public abstract string Name { get; set; }
        
        public abstract MovObjType MovObjType { get; }
        public abstract void Update(Cell currentCell);

        public int ID
        {
            get { return this.id; }
            set { this.id = value; }
        }
    }
}
