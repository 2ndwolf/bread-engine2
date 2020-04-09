using System;
using Blazor.Extensions.Canvas.WebGL;
using System.Threading.Tasks;
using System.Collections.Generic;
using LegendOfWorlds.Loaders;
using LegendOfWorlds.Data;
using M4; 


namespace LegendOfWorlds.Engine {

  public partial class World {
    public struct Texture {
        public int x, y, width, height;
        public float scaleX, scaleY, radians;
        public bool loaded;
        public WebGLTexture texture;
        public string name;

        public float[] finalMatrix;

    }
    public struct RenderTarget {
        public float width, height, x, y;
        public Texture texture;
        // public float[] finalMatrix;

    }

    public static float[] emptyMatrix = new float[16];

    public static WebGLProgram program;
    public static int positionLocation, texcoordLocation;
    public static WebGLUniformLocation matrixLocation, textureLocation, textureMatrixLocation;
    public static WebGLBuffer texPositionBuffer, texcoordBuffer;
    public static WebGLTexture glTex;

    public static Dictionary<Guid, Texture> textures = new Dictionary<Guid, Texture>();
    public static Dictionary<Guid, RenderTarget> renderTargets = new Dictionary<Guid, RenderTarget>();

    // public static Matrix identity = new Matrix();


    public static async Task RenderTest() {
      Console.WriteLine("Initializing WebGL");
      await InitWebGL();
      Console.WriteLine("WebGL initialized");


      //await RenderSquare();
      Guid rndr1 = await CreateRenderTarget(Guid.NewGuid(), 10, 10, 2048, 512);
      Guid rndr2 = await CreateRenderTarget(Guid.NewGuid(), 100, 100, 100, 100);


      Guid tex1 = await CreateTexture(Guid.NewGuid(), "./assets/images/pics1_dyl.png");
      Guid tex2 = await CreateTexture(Guid.NewGuid(), "./assets/images/bomb1.png");

      await TexToTarget(tex1, rndr1);
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

    public static async Task<Guid> CreateTexture(Guid textureId, string url){


      Load.ImageWithData png = await Load.LoadImage(url);

      Texture texture = new Texture();
      texture.x = 0;
      texture.y =  0;
      texture.width = png.width;
      texture.height = png.height;
      texture.finalMatrix = await M4.Computations.Translation(0,0,0);

      //Create texture holder
      texture.texture = await GL.CreateTextureAsync();
      
      //Load image info in memory
      await GL.BindTextureAsync(TextureType.TEXTURE_2D, texture.texture);
      await GL.TexImage2DAsync(Texture2DType.TEXTURE_2D, 0, PixelFormat.RGBA, png.width, png.height, 0, PixelFormat.RGBA, PixelType.UNSIGNED_BYTE, png.data);
      //Do not assume textures are a power of 2
      await GL.TexParameterAsync(TextureType.TEXTURE_2D, TextureParameter.TEXTURE_WRAP_S, (float) TextureParameterValue.CLAMP_TO_EDGE);
      await GL.TexParameterAsync(TextureType.TEXTURE_2D, TextureParameter.TEXTURE_WRAP_T, (float) TextureParameterValue.CLAMP_TO_EDGE);
      await GL.TexParameterAsync(TextureType.TEXTURE_2D, TextureParameter.TEXTURE_MIN_FILTER, (float) TextureParameterValue.LINEAR);

      textures[textureId] = texture;

      return textureId;
    }


    public static async Task<Guid> CreateRenderTarget(Guid targetId, int x, int y, int width, int height){
      
      RenderTarget renderTarget = new RenderTarget();


      renderTarget.width = (float)width;
      renderTarget.height = (float)height;
      renderTarget.x = (float)x;
      renderTarget.y = (float)y;
      // renderTarget.finalMatrix = new float[16];

      renderTargets[targetId] = renderTarget;

      return targetId;
    }

    public static async Task TexToTarget(Guid tex, Guid rndr){
      Console.WriteLine("Getting renderTarget");
      RenderTarget render = renderTargets[rndr];
      Console.WriteLine("Binding texture to renderTarget");
      textures[tex] = await ComputeTexScaling(textures[tex], render);
      render.texture = textures[tex];
      Console.WriteLine("Putting updated renderTarget back in renderTargets");
      renderTargets[rndr] = render;
    }

    public static async Task<Texture> ComputeTexScaling(Texture texture, RenderTarget rndr){
      texture.scaleX = 1;//rndr.naturalWidth / texture.width;
      texture.scaleY = 1;//rndr.naturalHeight / texture.height;
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