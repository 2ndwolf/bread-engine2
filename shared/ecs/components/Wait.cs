using MessagePack;
using Audrey;

namespace Shared.Ecs.Components.Wait {

  public enum WaitState{
    Wait,
    Standby,
    Done
  }

  public class WaitComponent: IComponent {
    public long elapsedTime;
    public WaitState waitState;
    public int secondsToWait;
  }

}

