using System;
using System.Net.Http;
using Blazor.Extensions.Canvas.WebGL;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LegendOfWorlds.Engine {
  public class TextureInfo {
      public string id { get; set; }
      public int width { get; set; }
      public int height { get; set; }
  }

  public partial class World {
    public static async Task RenderTest() {
      Console.WriteLine("Initializing WebGL");
      await InitWebGL();
      Console.WriteLine("WebGL initialized");

      //await RenderSquare();

      Texture texMaker = new Texture();
      await texMaker.CreateTexture();
      //RenderSquare();
    }

    public static async Task<bool> RenderSquare(){
      await GL.BeginBatchAsync(); // begin the explicit batch

await GL.ClearAsync(BufferBits.COLOR_BUFFER_BIT);
await GL.DrawArraysAsync(Primitive.TRIANGLES, 0, 3);
      await GL.EndBatchAsync(); // execute all currently batched calls

      return true;

    }

    public static async Task<bool> InitWebGL(){
      string vertexShaderSource = 
      @"
        attribute vec4 positionLocation;
        attribute vec2 texcoordLocation;
        
        uniform mat4 matrixLocation;
        uniform mat4 textureMatrixLocation;
        
        varying vec2 v_texcoord;
        
        void main(void) {
        gl_Position = matrixLocation * positionLocation;
        v_texcoord = (textureMatrixLocation * vec4(texcoordLocation, 0, 1)).xy;
        }
        
      ";

      string fragmentShaderSource =
      @"
        precision mediump float;
        
        varying vec2 v_texcoord;
        
        uniform sampler2D textureLocation;
        
        void main(void) {
        gl_FragColor = texture2D(textureLocation, v_texcoord);
        }

      ";


      WebGLShader vertexShader = await CreateShader(ShaderType.VERTEX_SHADER, vertexShaderSource);
      WebGLShader fragmentShader = await CreateShader(ShaderType.FRAGMENT_SHADER, fragmentShaderSource);

      WebGLProgram program = await createProgram(vertexShader, fragmentShader);

      var positionLocation = await GL.GetAttribLocationAsync(program, "positionLocation");
      var texcoordLocation = await GL.GetAttribLocationAsync(program, "texcoordLocation");
      var matrixLocation = await GL.GetUniformLocationAsync(program, "matrixLocation");
      var textureMatrixLocation = await GL.GetUniformLocationAsync(program, "textureMatrixLocation");
      var textureLocation = await GL.GetUniformLocationAsync(program, "textureLocation");

      var texPositionBuffer = await GL.CreateBufferAsync();
      await GL.BindBufferAsync(BufferType.ARRAY_BUFFER, texPositionBuffer);
      float[] positions = {0F, 0F, 0F, 1F, 1F, 0F, 1F, 0F, 0F, 1F, 1F, 1F};
      await GL.BufferDataAsync(BufferType.ARRAY_BUFFER, positions, BufferUsageHint.STATIC_DRAW);

      var texcoordBuffer = await GL.CreateBufferAsync();
      await GL.BindBufferAsync(BufferType.ARRAY_BUFFER, texcoordBuffer);
      float[] texcoords = {0F, 0F, 0F, 1F, 1F, 0F, 1F, 0F, 0F, 1F, 1F, 1F};
      await GL.BufferDataAsync(BufferType.ARRAY_BUFFER, texcoords, BufferUsageHint.STATIC_DRAW);

      await GL.DisableAsync(EnableCap.DEPTH_TEST);
      await GL.EnableAsync(EnableCap.BLEND);
      await GL.BlendEquationAsync(BlendingEquation.FUNC_ADD);

      //Do not assume the values are premultiplied ( if alpha = 0 then rgb should = 0)
      await GL.BlendFuncSeparateAsync(BlendingMode.SRC_ALPHA, BlendingMode.ONE_MINUS_SRC_ALPHA, BlendingMode.ONE, BlendingMode.ONE_MINUS_SRC_ALPHA);

    return true;
    }

    private static async Task<WebGLShader> CreateShader(ShaderType type,string source) {
      WebGLShader shader = await GL.CreateShaderAsync(type);
      await GL.ShaderSourceAsync(shader, source);
      await GL.CompileShaderAsync(shader);
      bool success = await GL.GetShaderParameterAsync<bool>(shader, ShaderParameter.COMPILE_STATUS);
      if (success) {
        return shader;
      }

      Console.WriteLine("Shader compilation failure.");
      await GL.DeleteShaderAsync(shader);
      return null;
    }

    private static async Task<WebGLProgram> createProgram(WebGLShader vertexShader,WebGLShader fragmentShader) {
      WebGLProgram program = await GL.CreateProgramAsync();
      await GL.AttachShaderAsync(program, vertexShader);
      await GL.AttachShaderAsync(program, fragmentShader);
      await GL.LinkProgramAsync(program);
      bool success = await GL.GetProgramParameterAsync<bool>(program, ProgramParameter.LINK_STATUS);
      if (success) {
        return program;
      }
    
      Console.WriteLine(await GL.GetProgramInfoLogAsync(program));
      //await GL.DeleteProgramAsync(program);
      return null;
    }

    public static async void DrawImage(){

    }

    public class Texture{

      public struct TextureInfo {
        public int width;
        public int height;
        public bool loaded;
        public WebGLTexture texture;
        public string name;

      }

      public async Task<TextureInfo> CreateTexture(){

        WebGLTexture glTex = await GL.CreateTextureAsync();

        TextureInfo tex = new TextureInfo();
        tex.width = 100;
        tex.height = 100;
        tex.loaded = true;
        tex.texture = glTex;
        tex.name = "woohoo";

        await GL.BindTextureAsync(TextureType.TEXTURE_2D, glTex);

        await GL.TexImage2DAsync(Texture2DType.TEXTURE_2D, 0, PixelFormat.RGBA, 1, 1, 0, PixelFormat.RGBA, PixelType.UNSIGNED_BYTE, new byte[] {0,0,255,255});

        await GL.TexParameterAsync(TextureType.TEXTURE_2D, TextureParameter.TEXTURE_WRAP_S, 33071F);
        await GL.TexParameterAsync(TextureType.TEXTURE_2D, TextureParameter.TEXTURE_WRAP_T, 33071F);
        await GL.TexParameterAsync(TextureType.TEXTURE_2D, TextureParameter.TEXTURE_MIN_FILTER, 9729F);



        return tex;
      }

    }

      /*
    public static async Task DrawImage() {
      var res = await jsRuntime.InvokeAsync<string>("loadImageAndCreateTextureInfo", new object[]{
        "https://localhost:5001/assets/doll.png", 
        Guid.NewGuid().ToString()
      });
      var textureInfo = JsonSerializer.Deserialize<TextureInfo>(res);
      await jsRuntime.InvokeAsync<string>("drawImage", new object[] {
        textureInfo.id, 
        0,
        0
      });
      await jsRuntime.InvokeAsync<string>("drawImageOnTexture", new object[] {
        textureInfo.id, 
        0,
        0
      });
    }
      */

    /*

    public static async Task DrawImage() {
      //await jsRuntime.InvokeAsync<string>("window.drawImage", new object[]{"null"});
      //await jsRuntime.InvokeAsync<object>("initGL", new object[]{"null"});
      //await jsRuntime.InvokeAsync<object>("window.loadImageAndCreateTextureInfo", new object[]{"https://localhost:5001/assets/doll.png"});
      //await jsRuntime.InvokeAsync<object>("window.drawImage", new object[]{"null"});

      var program = CreateProgramAsync().Result;

      //var program = GL.CreateProgramFromScripts(gl, new string[] {"drawImage-vertex-shader", "drawImage-fragment-shader"});

      var positionBuffer = await GL.CreateBufferAsync();
      var texcoordBuffer = await GL.CreateBufferAsync();
      var positionLocation = await GL.GetAttribLocationAsync(program, "a_position");
      var texcoordLocation = await GL.GetAttribLocationAsync(program, "a_texcoord");
      var matrixLocation = await GL.GetUniformLocationAsync(program, "u_matrix");
      var textureLocation = await GL.GetUniformLocationAsync(program, "u_texture");


      await GL.BindTextureAsync(TextureType.TEXTURE_2D, tex);
      // GL.UseProgram(program);
      await GL.BindBufferAsync(BufferType.ARRAY_BUFFER, positionBuffer);
      await GL.EnableVertexAttribArrayAsync(positionLocation);

      await GL.VertexAttribPointerAsync(positionLocation, 2, PixelType.FLOAT, false, 0, 0);
      await GL.BindBufferAsync(BufferType.ARRAY_BUFFER, texcoordBuffer);
      await GL.EnableVertexAttribArrayAsync(texcoordLocation);
      await GL.VertexAttribPointerAsync(texcoordLocation, 2, PixelType.FLOAT, false, 0, 0);

      // this matrix will convert from pixels to clip space
      var matrix = m4.orthographic(0, GL.canvas.width, GL.canvas.height, 0, -1, 1);

      // this matrix will translate our quad to dstX, dstY
      matrix = m4.translate(matrix, dstX, dstY, 0);

      // this matrix will scale our 1 unit quad
      // from 1 unit to texWidth, texHeight units
      matrix = m4.scale(matrix, texWidth, texHeight, 1);

      // Set the matrix.
      GL.uniformMatrix4fv(matrixLocation, false, matrix);

      // Tell the shader to get the texture from texture unit 0
      await GL.UniformAsync(textureLocation, 0);

      // draw the quad (2 triangles, 6 vertices)
      await GL.DrawArraysAsync(Primitive.TRIANGLES, 0, 6);
    }

    public static async Task<WebGLTexture> CreateTextureInfo() {
      // ImageData imgData = await JSRuntime.InvokeAsync<ImageData>("window.createImage", new string[] { "https://localhost:5001/assets/doll.png", "UUID" });
      //var test = Newtonsoft.Json.JsonConvert.DeserializeObject(imgData.toString());

      var http = new HttpClient();
      var res = await http.GetAsync("https://localhost:5001/assets/doll.png");
      var byteArray = await res.Content.ReadAsByteArrayAsync();
      var img = new LegendOfWorlds.Utils.Image(byteArray, "png");
      var imgData = img.completeByteImage;

      // WebGL
      var tex = await GL.CreateTextureAsync();
      
      await GL.BindTextureAsync(TextureType.TEXTURE_2D, tex);
      await GL.TexParameterAsync(TextureType.TEXTURE_2D, TextureParameter.TEXTURE_WRAP_S, (float) TextureParameterValue.CLAMP_TO_EDGE);
      await GL.TexParameterAsync(TextureType.TEXTURE_2D, TextureParameter.TEXTURE_WRAP_T, (float) TextureParameterValue.CLAMP_TO_EDGE);
      await GL.TexParameterAsync(TextureType.TEXTURE_2D, TextureParameter.TEXTURE_MIN_FILTER, (float) TextureParameterValue.LINEAR);

      await GL.BindTextureAsync(TextureType.TEXTURE_2D, tex);
      await GL.TexImage2DAsync<byte>(Texture2DType.TEXTURE_2D, 0, PixelFormat.RGBA, 0, 0, PixelFormat.RGBA, PixelType.UNSIGNED_BYTE, imgData);

      return tex;
    }
    */

  }
}