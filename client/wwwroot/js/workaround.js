(function() {
  window.TexImage2DAsync = async (gl, width, height, data) => {
    console.log(width);
    console.log(height);
    console.log(data.length);
    // var gl = canvas.getContext("webgl");
    let tempData = new Uint8Array(data.length);
    for(var i = 0; i < data.length; i++){
      tempData[i] = data[i];
    }
    console.log(tempData.length);
    gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGBA, width, height,0, gl.RGBA, gl.UNSIGNED_BYTE , tempData);
  }
})()