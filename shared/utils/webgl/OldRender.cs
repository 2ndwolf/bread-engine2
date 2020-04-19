/**


using System;
using Blazor.Extensions.Canvas.WebGL;
using System.Threading.Tasks;
using System.Collections.Generic;
using Shared.Loaders;
using Shared.Data;
using M4; 


namespace LegendOfWorlds.Engine.iamacat {

  public partial class World {
    public struct Texture {
        public int x, y, width, height;
        public float scaleX, scaleY, radians;
        public WebGLTexture texture;
        public float[] finalMatrix;

    }
    // public struct RenderTarget {
    //     public float width, height, x, y;
    //     public Texture texture;
    //     // public float[] finalMatrix;

    // }

    public static float[] emptyMatrix = new float[16];

    public static WebGLProgram program;
    public static int positionLocation, texcoordLocation;
    public static WebGLUniformLocation matrixLocation, textureLocation, textureMatrixLocation;
    public static WebGLBuffer texPositionBuffer, texcoordBuffer;
    public static WebGLTexture glTex;

    // public static Dictionary<Guid, Texture> textures = new Dictionary<Guid, Texture>();
    // public static Dictionary<Guid, RenderTarget> renderTargets = new Dictionary<Guid, RenderTarget>();


    public static async Task RenderTest() {
      Console.WriteLine("Initializing WebGL");
      await InitWebGL();
      Console.WriteLine("WebGL initialized");


      // Make render targets have 0, 0, 0, 0 as default values and resize them with the texture
      // Guid rndr1 = await CreateRenderTarget(Guid.NewGuid(), 0, 0, 2048, 512);
      Guid rndr2 = await CreateRenderTarget(Guid.NewGuid(), 0, 0, 192, 64);


      // Guid tex1 = await CreateTexture(Guid.NewGuid(), "./assets/images/pics1_dyl.png");
      Guid tex2 = await CreateTexture(Guid.NewGuid(), "./assets/doll.png");

      // await TexToTarget(tex1, rndr1);
      await TexToTarget(tex2, rndr2);

      //await RenderSquare();
    }

    public static async Task TRSRenderTargetTexture(RenderTarget rndr, int x, int y, int width, int height, float scaleX = float.MaxValue, float scaleY = float.MaxValue, float radians = float.MaxValue){
      Texture tex = rndr.texture;
      tex.x = x;
      tex.y = y;
      tex.width = width;
      tex.height = height;
      float[] matrix = await M4.Computations.Translation(x / width, y / height, 0);
      
        tex.scaleX = scaleX == float.MaxValue ? tex.scaleX : scaleX;
        tex.scaleY = scaleY == float.MaxValue ? tex.scaleY : scaleY;
        tex.radians = radians == float.MaxValue ? tex.radians : radians;

      if(tex.scaleX != 1 || tex.scaleY !=1){
        float offScale = 1f;
        //matrix = await M4.Computations.Scale();
      }

      tex.finalMatrix = matrix;
      rndr.texture = tex;
      //M4.Computations.Rotate
    }

    public static void RotateRenderTargetTexture(){
      Console.WriteLine("RotateRenderTargetTexture Not implemented yet.");
    }

    public static async Task ScaleRenderTargetTexture(RenderTarget rndr, float multX, float multY){


    }

    //Check if scaling the render target also scales the contained texture
    public static void ScaleRenderTarget(){

    }






    public static async Task<Texture> ComputeTexScaling(Texture texture, RenderTarget rndr){
      texture.scaleX = rndr.width / texture.width;
      texture.scaleY = rndr.height / texture.height;
      texture.finalMatrix = await M4.Computations.Scale(texture.finalMatrix, texture.scaleX, texture.scaleY, 0);

      return texture;
    }

    public static async Task goRender(){
      
      await GL.ClearAsync(BufferBits.COLOR_BUFFER_BIT);


        foreach(KeyValuePair<Guid, RenderTarget> entry in renderTargets)
        {
            var renderTarget = entry.Value;

            await GL.BindTextureAsync(TextureType.TEXTURE_2D, renderTarget.texture.texture);


            float[] matrix = await M4.Computations.Translate(ortho, renderTarget.x, renderTarget.y, 0);
            matrix = await M4.Computations.Scale(matrix, renderTarget.width, renderTarget.height, 0);

            await GL.UniformMatrixAsync(matrixLocation, false, matrix);


            //The texture's matrix
            await GL.UniformMatrixAsync(textureMatrixLocation, false, renderTarget.texture.finalMatrix);
            
            await GL.UniformAsync(textureLocation, 0);

            await GL.BeginBatchAsync();
              await GL.DrawArraysAsync(Primitive.TRIANGLES, 0, 6);
            await GL.EndBatchAsync();
        }
      
    }


  }
}
*/
