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
      await DrawImage();
    }

    public static async Task DrawImage() {
      /*
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
      */
    }

    /*
    public static async Task DrawImage() {
      //await jsRuntime.InvokeAsync<string>("window.drawImage", new object[]{"null"});
      //await jsRuntime.InvokeAsync<object>("initGL", new object[]{"null"});
      //await jsRuntime.InvokeAsync<object>("window.loadImageAndCreateTextureInfo", new object[]{"https://localhost:5001/assets/doll.png"});
      //await jsRuntime.InvokeAsync<object>("window.drawImage", new object[]{"null"});

      var program = 5; //GL.CreateProgramFromScripts(gl, ["drawImage-vertex-shader", "drawImage-fragment-shader"]);

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