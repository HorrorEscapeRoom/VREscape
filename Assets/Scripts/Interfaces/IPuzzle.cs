namespace Assets.Scripts.Interfaces
{
    public interface IPuzzle
    {
        public Orchestrator Orchestrator { get; set; }
        public int PuzzleID { get; set; }
        public EnumPuzzleType PuzzleType { get; set; }       
        public void RegisterWithOrchestrator();
        public void OnPuzzleComplete();
    }
}
