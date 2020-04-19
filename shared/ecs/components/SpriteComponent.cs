using System.Collections.Generic;
using Audrey;

using Shared.Ecs.Components.Wait;

namespace Shared.Ecs.Components.Sprites {

  // public class SpriteComponentList: IComponent{
  //   public List<SpriteComponent> spriteComponents;
  // }
/**
 * Component for sprites
 * 
 * 
 * @param frameSize 
 * The width and height of the frames.
 * @param fileName 
 * Defaults to "", The filename of the
 * sprite sheet. Defaults to the first
 * texture in the entity's renderTarget
 * if not set. 
 * @param spriteOffset 
 * Defaults to [0,0], the x and y offset 
 * to start at in the image.
 * @param statesWidth
 * Defaults to 1.
 * How many states there
 * are across the spritesheet.
 * @param statesHeight
 * Defaults to 1.
 * How many states there
 * are across the spritesheet
 * @param grids
 * Defaults to `[[1]]`, will read that
 * quantity of columns / rows before looping/stopping.
 * (if `leftToRight` -> columns).
 * If hasStates is disabled, will take into
 * account the first value only.
 * Otherwise, the array index of `grids`
 * corresponding to the current state will
 * be taken into account.
 * This is a 2 dimensional array,
 * Enter values e.g.: [[1],[3],[64]...].
 * Or [[12,1],[9],[6]...] if the first two
 * rows / columns are included in the same state.
 * Where the numbers are the amount of
 * columns / rows per state.
 * 
 *   Also, but not in the constructor
 *   @param frameDurations 
 *   Defaults to [.5], the duration in seconds
 *   of each frame, if length is 1, that
 *   duration is applied to each frame. If there
 *   is only one member in the array, all frames
 *   will have that duration. If a frame time is
 *   set to `0`, it will be skipped.
 *   @param leftToRight 
 *   Defaults to `true`, if true, will read frames
 *   from left to right, then from top to bottom;
 *   if false, from top to bottom then left to right.
 *   @param reversed
 *   Defaults to `false`, If true, reverses the
 *   reading order of the frames (see `leftToRight`)
 *   @param wasReversed
 *   Set along with reversed, defaults
 *   to false.
 *   @param frameOffset
 *   Defaults to [0,0], the gap between frames
 *   @param currentFrame
 *   Defaults to `0`, the current frame
 *   @param loop
 *   Defaults to `0`, how many times
 *   the animation loops.
 *   System adds the `flag.PlaySpriteDone`
 *   component to an entity that finished
 *   playing its animation if nothing was
 *   set in `setBackTo`.
 *   -1 will make the animation loop indefinitely.
 *   @param hasStates
 *   Defaults to `false`, If true will not read the
 *   following rows/columns as part of the same animation
 *   @param currentState
 *   Defaults to `0`, The column or row to read from
 *   (depending on `leftToRight`). Will only work if
 *   `hasStates` is set to true.
 *   @param useDirectionComponent
 *   Defaults to true; if set, and if the entity
 *   has a directionComponent will give every state
 *   4 (0-3) rows or columns and use the entity's
 *   directionComponent to determine which row or
 *   column to use inside the given state.
 *   @param setBackTo
 *   Defaults to -1, (disabled)
 *   The state to set back to after an
 *   an animation has finished looping.
 *   @param play
 *   Whether if the animation plays
 *   Defaults to true, add a PlaySpriteComponent
 *   flag and set to false to have only the
 *   current frame render
 * 
 */
public class SpriteComponent: WaitComponent {
    
    /**
     * The width and height of the frames
     */
    public int[] frameSize;

    /**
     * Defaults to [0,0], the x and y offset
     * to start at in the image
     */
    public int[] spriteOffset = {0,0};

    /**
     * Defaults to `[[1]]`, will read that
     * quantity of columns / rows before looping/stopping.
     * (if `leftToRight` -> columns).
     * If hasStates is disabled, will take into
     * account the first value only.
     * Otherwise, the array index of `grids`
     * corresponding to the current state will
     * be taken into account.
     * This is a 2 dimensional array,
     * Enter values e.g.: [[1],[3],[64]...].
     * Or [[12,1],[9],[6]...] if the first two
     * rows / columns are included in the same state.
     * Where the numbers are the amount of
     * columns / rows per state.
     */
    public int[][] grids;

    /**
     * Defaults to [0,0], the gap between frames
     */
    public int[] frameOffset = {0,0};

    /**
     * Defaults to [.5], the duration in seconds
     * of each frame, if length is 1, that duration
     * is applied to each frame.
     * If there is only one member in the array, 
     * all frames will have that duration.
     * If a frame time is set to 0, it will be skipped.
     * (takes `leftToRight` into account)
     */
    public float[] frameDurations = {.5f};

    /**
     * Defaults to `true`, if true, will read
     * frames from left to right, then from
     * top to bottom; if false, from top to
     * bottom then left to right
     */
    public bool leftToRight = true;

    /**
     * Defaults to `0`, the current frame
     */
    public int currentFrame = 0;

    /**
     * Defaults to `0`, how many times
     * the animation loops.
     * System adds the `flag.PlaySpriteDone`
     * component to an entity that finished
     * playing its animation if nothing was
     * set in `setBackTo`.
     * -1 will make the animation loop indefinitely.
     */
    public int loop = 0;

    /**
     * Defaults to `false`, If true will
     * not read the following rows/columns
     * as part of the same animation
     */
    public bool hasStates = false;

    /**
     * Internal, do not set
     */
    public int previousState = 0;

    /**
     * Set along with reversed, defaults
     * to false.
     */
    public bool wasReversed = false;

    /**
     * The filename of the sprite sheet.
     * Defaults to the first texture in the entity's
     * renderTarget if not set. 
     * 
     */
    public string[] fileNames;

    /**
    *   Defaults to true; if set, if `hasStates` is true
    *   and if the entity
    *   has a directionComponent will give every state
    *   4 (0-3) rows or columns and use the entity's
    *   directionComponent to determine which row or
    *   column to use inside the given state.
    *   Every direction will use the same speed
    *   so treat it as just one row / column
    *   in `frameDurations`.
    */
    public bool useDirectionComponent = true;

    /**
     * Defaults to -1, (disabled)
     * The state to set back to after an
     * an animation has finished looping.
     */
    public int setBackTo = -1;

    /**
     * How many states there
     * are across the spritesheet
     * 
     */
    public int statesWidth = 1;

    /**
     * How many states there
     * are across the spritesheet
     */
    public int statesHeight = 1;



    public SpriteComponent(int[] frameSize,
                int renderTargetIndex = -1,
                string[] fileNames = null,
                int[] spriteOffset = null,
                int statesWidth = -1,
                int statesHeight = -1,
                int[][] grids = null){

        frameSize    = frameSize;
        
        fileNames    = fileNames ?? new string[1]{""};
        spriteOffset = spriteOffset ?? new int[2]{0,0};
        statesWidth  = statesWidth == -1 ? 1 : statesWidth;
        statesHeight = statesHeight == -1 ? 1 : statesHeight;
        grids        = grids ?? new int[1][];
    }
  }
}
