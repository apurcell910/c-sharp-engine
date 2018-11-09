namespace SharpSlugsEngine.Input
{
    /// <summary>
    /// Stub for now, I'll add some generic functionality here later
    /// </summary>
    public abstract class GameController
    {
        internal abstract string Path { get; }

        public abstract ControllerType Type { get; }

        internal abstract void Update();
    }

    public enum ControllerType
    {
        Xbox,
        Xbox360,
        XboxOne,
        XboxOneS,
        Playstation3,
        Playstation4
    }
}
