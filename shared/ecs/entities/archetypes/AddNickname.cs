using static Shared.Engine.World;

using System.Threading.Tasks;
using Audrey;
/**

  Adds nickname below player

**/

namespace Shared.Ecs.Entities{

  public static partial class Archetypes{

    public static async Task<Entity> AddNickname(this Entity @this){
    

      return @this;
    }
  }
}
