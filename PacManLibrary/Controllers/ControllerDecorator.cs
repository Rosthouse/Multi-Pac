namespace PacManShared.Controllers
{
    public abstract class ControllerDecorator: Controller
    {
        protected Controller controller;

        public ControllerDecorator(Controller controller, int id) : base(id)
        {
            this.controller = controller;
        }
    }
}
