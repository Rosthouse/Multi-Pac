namespace PacManShared.Console.Commands
{
    internal interface ICommand
    {
        string Name { get; set; }
        string Description { get; set; }
        void Execute();
    }
}