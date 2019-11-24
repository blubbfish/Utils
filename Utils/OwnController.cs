namespace BlubbFish.Utils {
  public abstract class OwnController
  {
    /// <summary>
    /// Führt den Controller aus.
    /// </summary>
    public void Execute() => this.Init();
    abstract protected void Init();
    abstract public void Dispose();
  }
}
