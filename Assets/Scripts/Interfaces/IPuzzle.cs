public interface IPuzzle
{
    public int PuzzleId { get;}
    public EnumPuzzleType PuzzleType { get; set; }       
    public void RegisterWithOrchestrator();
    public void UnRegisterWithOrchestrator();
    public void OnPuzzleAwake();
    public void OnPuzzleComplete();
}
