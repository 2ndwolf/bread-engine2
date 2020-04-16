using System;
using System.Threading.Tasks;
using Blazor.Extensions.Canvas.WebGL;

using LegendOfWorlds.Data;
using static LegendOfWorlds.Engine.World;


namespace LegendOfWorlds.Utils {

  public partial class Render{

    public static WebGLProgram program;
    public static int positionLocation, texcoordLocation;
    public static WebGLUniformLocation matrixLocation, textureLocation, textureMatrixLocation;
    public static WebGLBuffer texPositionBuffer, texcoordBuffer;

    public static float[] ortho;

    public static async Task InitWebGL(){

      await GL.ViewportAsync(0, 0, 1280, 720);
      ortho = await M4.Computations.Orthographic(0f, 1280f, 720f, 0f, 1f, 0f);

      await GL.ClearColorAsync(0f, 0f, 0.0f, 1.0f);

      WebGLShader vertexShader = await CreateShader(ShaderType.VERTEX_SHADER, Shaders.mainVertexShader);
      WebGLShader fragmentShader = await CreateShader(ShaderType.FRAGMENT_SHADER, Shaders.mainFragmentShader);

      program = await createProgram(vertexShader, fragmentShader);

      positionLocation = await GL.GetAttribLocationAsync(program, "positionLocation");
      texcoordLocation = await GL.GetAttribLocationAsync(program, "texcoordLocation");
      matrixLocation = await GL.GetUniformLocationAsync(program, "matrixLocation");
      textureMatrixLocation = await GL.GetUniformLocationAsync(program, "textureMatrixLocation");
      textureLocation = await GL.GetUniformLocationAsync(program, "textureLocation");


      texPositionBuffer = await GL.CreateBufferAsync();
      await GL.BindBufferAsync(BufferType.ARRAY_BUFFER, texPositionBuffer);
      float[] positions = {0f, 0f, 0f, 1f, 1f, 0f, 1f, 0f, 0f, 1f, 1f, 1f};
      await GL.BufferDataAsync(BufferType.ARRAY_BUFFER, positions, BufferUsageHint.STATIC_DRAW);

      texcoordBuffer = await GL.CreateBufferAsync();
      await GL.BindBufferAsync(BufferType.ARRAY_BUFFER, texcoordBuffer);
      float[] texcoords = {0f, 0f, 0f, 1f, 1f, 0f, 1f, 0f, 0f, 1f, 1f, 1f};
      await GL.BufferDataAsync(BufferType.ARRAY_BUFFER, texcoords, BufferUsageHint.STATIC_DRAW);

      await GL.DisableAsync(EnableCap.DEPTH_TEST);
      await GL.EnableAsync(EnableCap.BLEND);
      //await GL.EnableAsync(EnableCap.DITHER);
      await GL.BlendEquationAsync(BlendingEquation.FUNC_ADD);

      //Do not assume the values are premultiplied ( if alpha = 0 then rgb should = 0)
      await GL.BlendFuncSeparateAsync(BlendingMode.SRC_ALPHA, BlendingMode.ONE_MINUS_SRC_ALPHA, BlendingMode.ONE, BlendingMode.ONE_MINUS_SRC_ALPHA);


      await GL.UseProgramAsync(program);

      //Vertex
      await GL.BindBufferAsync(BufferType.ARRAY_BUFFER, texPositionBuffer);
      await GL.EnableVertexAttribArrayAsync((uint)positionLocation);
      await GL.VertexAttribPointerAsync((uint)positionLocation, 2, DataType.FLOAT, false, 0, 0);

      //Fragment
      await GL.BindBufferAsync(BufferType.ARRAY_BUFFER, texcoordBuffer);
      await GL.EnableVertexAttribArrayAsync((uint)texcoordLocation);
      await GL.VertexAttribPointerAsync((uint)texcoordLocation, 2, DataType.FLOAT, false, 0, 0);


    }

    private static async Task<WebGLShader> CreateShader(ShaderType type,string source) {
      WebGLShader shader = await GL.CreateShaderAsync(type);
      await GL.ShaderSourceAsync(shader, source);
      await GL.CompileShaderAsync(shader);
      bool success = await GL.GetShaderParameterAsync<bool>(shader, ShaderParameter.COMPILE_STATUS);
      if (success) {
        return shader;
      }

      Console.WriteLine(await GL.GetShaderInfoLogAsync(shader));
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
  }
}
