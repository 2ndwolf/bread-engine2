using System.Runtime.CompilerServices;
using static Shared.Engine.World;

using System.Threading.Tasks;
using Audrey;
/**

  Adds chat above player

**/

namespace Shared.Ecs.Entities{

  public static partial class Archetypes{

    public static async Task<Entity> AddChatText(this Entity @this){
    
      // @this.AddComponent();

      return @this;
    }
  }
}