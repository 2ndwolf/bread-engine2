#if WEBGL
namespace Shared.Data{

    public static class Shaders{
      public static string mainVertexShader = 
      @"
        attribute vec2 positionLocation;
        attribute vec2 texcoordLocation;
        
        uniform mat4 matrixLocation;
        uniform mat4 textureMatrixLocation;
        
        varying vec2 v_texcoord;

        void main(void) {
            gl_Position = matrixLocation * vec4(positionLocation, 0, 1);
            v_texcoord = (textureMatrixLocation * vec4(texcoordLocation, 0, 1)).xy;
        }
        
      ";

      public static string mainFragmentShader =
      @"
        precision mediump float;
        
        varying vec2 v_texcoord;
        
        uniform sampler2D textureLocation;
        
        void main(void) {
          gl_FragColor = texture2D(textureLocation, v_texcoord);
        }

      ";

    //https://webglfundamentals.org/webgl/lessons/webgl-fundamentals.html
    public static string testVertexShader = 
      @"
        attribute vec2 positionLocation;
        attribute vec2 texcoordLocation;

        //attribute vec2 ScreenWH;
        //attribute vec2 imageWH;
        //attribute vec2 imageXY;

        //uniform vec2 imgWH;
        //uniform vec2 imgXY;
        
        uniform mat4 matrixLocation;
        uniform mat4 textureMatrixLocation;
        
        varying vec2 v_texcoord;
        
        mediump mat4 ortho = mat4(
          2, 0, 0, 0,
          0, -2, 0, 0,
          0, 0, -1, 0,
          -1, 1, 0, 1
        );

        void main(void) {

        
        mediump mat4 position = mat4(
          (ortho[0][0]) * 64.0 / 250.0, 0, 0, 10.0 / 250.0,
          0, (ortho[1][1]) * 64.0 / 250.0, 0, 10.0 / 250.0,
          0, 0, -1, 1,
          -1, 1, 0, 1
        );

        mediump mat4 texPosition = mat4(
          1, 0, 0, 0,
          0, 1, 0, 0,
          0, 0, 0, 0,
          0, 0, 0, 1
        );
        

        gl_Position = position * vec4(positionLocation, 0, 1);
        v_texcoord = (texPosition * vec4(texcoordLocation, 0, 1)).xy;

        //imgWH = (imageWH.x / (ScreenWH.x / 2), imageWH.y / (ScreenWH.y / 2));
        //imgXY = (imageXY.x / (ScreenWH.x / 2), imageXY.y / (ScreenWH.y / 2))
        }
        
      ";

      public static string testFragmentShader =
      @"
        precision mediump float;
        
        varying vec2 v_texcoord;
        
        uniform sampler2D textureLocation;
        
        void main(void) {
          //vec4 fG = texture2D(textureLocation, v_texcoord);
          //gl_FragColor = vec4(fG.a) * fG + vec4(1.0 - fG.a) * fG;
          gl_FragColor = texture2D(textureLocation, v_texcoord);
        }

      ";
    }
}
#endif