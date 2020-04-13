using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Blazor.Extensions.Canvas.WebGL;

using static LegendOfWorlds.Engine.World;
using LegendOfWorlds.Loaders;


namespace LegendOfWorlds.Utils {
  public partial class Render {

    private static Dictionary<Guid, Texture> textures = new Dictionary<Guid, Texture>();
    private static Dictionary<Guid, RenderTarget> renderTargets = new Dictionary<Guid, RenderTarget>();

    private struct RenderTarget {
        public int width, height;
        public Guid textureId;
        public float[] texFinalMatrix;

        // All that is texture info stored in texFinalMatrix
        // public int x, y, width, height;
        // public float scaleX, scaleY, radians;
    }

    private struct Texture {
      public int width, height;
      public WebGLTexture texture;
    }



    // public static ValueTask<string> createTexture(string textureId, string url) {
    //   return LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("createTexture", new string[] { textureId, url });
    // }
    public static async Task<Guid> CreateTexture(Guid textureId, string url){

      Load.ImageWithData img = await Load.LoadImage(url);

      textures[textureId] = new Texture();
      Texture tex = textures[textureId];
      tex.width = img.width;
      tex.height = img.height;

      //Create texture holder
      tex.texture = await GL.CreateTextureAsync();
      
      //Load image info in memory
      await GL.BindTextureAsync(TextureType.TEXTURE_2D, tex.texture);
      await GL.TexImage2DAsync(Texture2DType.TEXTURE_2D, 0, PixelFormat.RGBA, img.width, img.height, 0, PixelFormat.RGBA, PixelType.UNSIGNED_BYTE, img.data);
      //Do not assume textures are a power of 2
      await GL.TexParameterAsync(TextureType.TEXTURE_2D, TextureParameter.TEXTURE_WRAP_S, (float) TextureParameterValue.CLAMP_TO_EDGE);
      await GL.TexParameterAsync(TextureType.TEXTURE_2D, TextureParameter.TEXTURE_WRAP_T, (float) TextureParameterValue.CLAMP_TO_EDGE);
      await GL.TexParameterAsync(TextureType.TEXTURE_2D, TextureParameter.TEXTURE_MIN_FILTER, (float) TextureParameterValue.LINEAR);

      textures[textureId] = tex;

      return textureId;
    }

    // public static ValueTask<string> deleteTexture(string textureId) {
    //   return LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("deleteTexture", new string[] { textureId });
    // }

    // public static ValueTask<string> createTarget(string targetId, float x, float y, float width, float height) {
    //   return LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("createTarget", new object[] { targetId, x, y, width, height });
    // }
    public static async Task<Guid> CreateRenderTarget(Guid targetId, int width = 0, int height = 0){
      
      RenderTarget renderTarget = new RenderTarget();

      renderTarget.width = width;
      renderTarget.height = height;

      renderTarget.texFinalMatrix = await M4.Computations.Translation(0,0,0);

      renderTargets[targetId] = renderTarget;

      return targetId;
    }


    // public static ValueTask<string> deleteTarget(string targetId) {
    //   return LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("deleteTarget", new string[] { targetId });
    // }
    public static void DeleteTarget(Guid targetId){
      renderTargets.Remove(targetId);
    }

    public static void DeleteTexture(Guid texId){
      textures.Remove(texId);
    }

    public static void TexToTarget(Guid texId, Guid rndrId){
      // Console.WriteLine("Getting rndrId");
      RenderTarget render = renderTargets[rndrId];

      if(render.width == 0){
        render.width = textures[texId].width;
      }
      if(render.height == 0){
        render.height = textures[texId].height;
      }
      // Console.WriteLine("Binding texture to renderTarget");
      // textures[tex] = await ComputeTexScaling(textures[tex], render);
      render.textureId = texId;
      // Console.WriteLine("Putting updated renderTarget back in renderTargets");
      renderTargets[rndrId] = render;
    }

    // public static ValueTask<string> drawOnTarget(string targetId, string textureId, float x, float y) {
    //   return LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("drawOnTarget", new object[] { targetId, textureId, x, y });
    // }

    // public static ValueTask<string> drawSingleTarget(string targetId, float x, float y) {
    //   return LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("drawSingleTarget", new object[] { targetId, x, y });
    // }
    public static async Task DrawSingleTarget(Guid targetId, int x, int y){

      RenderTarget rT = renderTargets[targetId];

      await GL.BindTextureAsync(TextureType.TEXTURE_2D, textures[rT.textureId].texture);

      float[] matrix = new float[16];

      unchecked {
        matrix = await M4.Computations.Translate(ortho, (float)x, (float)y, 0);
        matrix = await M4.Computations.Scale(matrix, (float)rT.width, (float)rT.height, 0);
      }

      await GL.UniformMatrixAsync(matrixLocation, false, matrix);

      //The texture's matrix
      await GL.UniformMatrixAsync(textureMatrixLocation, false, rT.texFinalMatrix);
      
      await GL.UniformAsync(textureLocation, 0);

      await GL.BeginBatchAsync();
        await GL.DrawArraysAsync(Primitive.TRIANGLES, 0, 6);
      await GL.EndBatchAsync();
    }


    // Replaced with await GL.ClearAsync(BufferBits.COLOR_BUFFER_BIT);
    // public static ValueTask<string> clearRootCanvas() {
    //   return LegendOfWorlds.Engine.World.jsRuntime.InvokeAsync<string>("clearRootCanvas", null);
    // }
    
  }
}