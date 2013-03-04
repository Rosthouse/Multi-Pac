using PacManShared.Enums;
using PacManShared.LevelClasses.Cells;

namespace PacManShared.Controllers
{
    public class EmptyController: Controller
    {
        private Direction direction;
        private string name;
        private MovObjType movObjType;

        public EmptyController(int id) : base(id)
        {
            this.direction = Direction.None;
            this.name = "Emtpy";
            this.movObjType = MovObjType.Empty;
        }

        public EmptyController(Direction direction, string name, int id,  MovObjType movObjType): base(id)
        {
            this.name = name;
            this.direction = direction;
            this.movObjType = movObjType;
        }


        public override Direction Direction
        {
            get { return this.direction; }
            set { this.direction = value; }
        }

        public override string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public static EmptyController Empty(int UID)
        {
            return new EmptyController(UID);
        }


        public override MovObjType MovObjType
        {
            get { return this.movObjType; }
        }


        public override void Update(Cell currentCell)
        {
            //Do nothing in here, since this is the empty controller
        }
    }
}
