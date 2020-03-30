(function() {
  const canvas = document.getElementById("root-canvas")
  const gl = canvas.getContext('webgl')

  // Texture Dictionary
  const textures = new Map()
  // Render Target Dictionary
  const targets = new Map()

  const rootTexture = gl.createTexture()
  gl.bindTexture(gl.TEXTURE_2D, rootTexture)

  // Variables
  // setup glSL program
  let program
  // look up where the vertex data needs to go.
  let positionLocation
  let texcoordLocation

  // lookup uniforms
  let matrixLocation
  let textureLocation

  // Create a buffer.
  let positionBuffer
  // Create a buffer for texture coords
  let texcoordBuffer
  
  window.createImage = async (url, id) => {
    // Create new image object.
    const img = new Image()
    img.id = id
    img.src = url

    // Wait until image loads
    await new Promise((resolve) => {
      img.onload = (ev) => {
        resolve(ev.path[0])
      }
    })

    // Get image width and heeight
    const imgWidth = img.width || img.naturalWidth
    const imgHeight = img.height || img.naturalHeight

    
    // Draw image on canvas and get data back in RBGA format.
    canvas2DCtx.drawImage(img, 0, 0, imgWidth, imgHeight)
    const data = canvas2DCtx.getImageData(0, 0, imgWidth, imgHeight).data

    // Clear canvas.
    canvas2DCtx.clearRect(0, 0, img.width, img.height)
    
    // Send data back to C# as JSON so it can be rendered by Webgl.
    return JSON.stringify({
      data: Array.from(data)
    })

  }

  window.initializeGL = async () => {
    // setup glSL program
    program = webglUtils.createProgramFromScripts(gl, ["drawImage-vertex-shader", "drawImage-fragment-shader"])

    // look up where the vertex data needs to go.
    positionLocation = gl.getAttribLocation(program, "a_position")
    texcoordLocation = gl.getAttribLocation(program, "a_texcoord")

    // lookup uniforms
    matrixLocation = gl.getUniformLocation(program, "u_matrix")
    textureLocation = gl.getUniformLocation(program, "u_texture")

    // Create a buffer.
    positionBuffer = gl.createBuffer()
    gl.bindBuffer(gl.ARRAY_BUFFER, positionBuffer)

    // Put a unit quad in the buffer
    var positions = [
      0, 0,
      0, 1,
      1, 0,
      1, 0,
      0, 1,
      1, 1,
    ]
    gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(positions), gl.STATIC_DRAW)

    texcoordBuffer = gl.createBuffer()
    gl.bindBuffer(gl.ARRAY_BUFFER, texcoordBuffer)

    // Put texcoords in the buffer
    var texcoords = [
      0, 0,
      0, 1,
      1, 0,
      1, 0,
      0, 1,
      1, 1,
    ]
    gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(texcoords), gl.STATIC_DRAW)
  }

  window.drawRootTarget = async (dstX, dstY) => {
      //webglUtils.resizeCanvasToDisplaySize(gl.canvas)


      // Bind framebuffer
      gl.bindFramebuffer(gl.FRAMEBUFFER, null)
      
      // Bind texture
      gl.bindTexture(gl.TEXTURE_2D, rootTexture);

      // Tell Webgl to use our shader program pair
      gl.useProgram(program)

      // Setup the attributes to pull data from our buffers
      gl.bindBuffer(gl.ARRAY_BUFFER, positionBuffer)
      gl.enableVertexAttribArray(positionLocation)
      gl.vertexAttribPointer(positionLocation, 2, gl.FLOAT, false, 0, 0)
      gl.bindBuffer(gl.ARRAY_BUFFER, texcoordBuffer)
      gl.enableVertexAttribArray(texcoordLocation)
      gl.vertexAttribPointer(texcoordLocation, 2, gl.FLOAT, false, 0, 0)

      // this matrix will convert from pixels to clip space
      var matrix = m4.orthographic(0, gl.canvas.width, gl.canvas.height, 0, -1, 1)

      // this matrix will translate our quad to dstX, dstY
      matrix = m4.translate(matrix, dstX, dstY, 0)

      // this matrix will scale our 1 unit quad
      // from 1 unit to gl.canvas.width, gl.canvas.height units
      matrix = m4.scale(matrix, gl.canvas.width, gl.canvas.height, 1)

      // Set the matrix.
      gl.uniformMatrix4fv(matrixLocation, false, matrix)

      // Tell the shader to get the texture from texture unit 0
      gl.uniform1i(textureLocation, 0)

      // draw the quad (2 triangles, 6 vertices)
      gl.drawArrays(gl.TRIANGLES, 0, 6)
  }

  window.createTarget = async (id) => {
    const fb = gl.createFramebuffer()
    gl.bindFramebuffer(gl.FRAMEBUFFER, fb);

    targets.set(id, fb)
  }

  // Draw on the texture.
  window.blitOnTarget = (id, width, height) => {
    const fb = targets.get(id)
    const texture = textures.get(id).tex

    // render to our targetTexture by binding the framebuffer
    gl.bindFramebuffer(gl.FRAMEBUFFER, fb);
 
    // render cube with our 3x2 texture
    gl.bindTexture(gl.TEXTURE_2D, texture);
 
    // Tell WebGL how to convert from clip space to pixels
    gl.viewport(0, 0, width, height); // Target texture width and height
 
    // Clear the attachment(s).
    gl.clearColor(0, 0, 1, 1);   // clear to blue
    gl.clear(gl.COLOR_BUFFER_BIT| gl.DEPTH_BUFFER_BIT);
  }

  window.blitOnRoot = () => {
    targets.forEach((target, id) => {
      // render to our targetTexture by binding the framebuffer
      gl.bindFramebuffer(gl.FRAMEBUFFER, target);
  
      // render cube with our 3x2 texture
      gl.bindTexture(gl.TEXTURE_2D, textures.get(id).tex);
  
      // Tell WebGL how to convert from clip space to pixels
      gl.viewport(0, 0, gl.canvas.width, gl.canvas.height);
  
      // Clear the attachment(s).
      gl.clearColor(0, 0, 1, 1);   // clear to blue
      gl.clear(gl.COLOR_BUFFER_BIT| gl.DEPTH_BUFFER_BIT);
    })
  }

  window.clearTexture = async (id) => {
    textures.get(id)
  }

  window.loadImageAndCreateTextureInfo = async (url, id) => {
    const tex = gl.createTexture()
    gl.bindTexture(gl.TEXTURE_2D, tex)
    // Fill the texture with a 1x1 blue pixel.
    gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGBA, 1, 1, 0, gl.RGBA, gl.UNSIGNED_BYTE,
                  new Uint8Array([0, 0, 255, 255]))

    
    // let's assume all images are not a power of 2
    gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE)
    gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE)
    gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR)

    var textureInfo = {
      id: id,
      width: 1,   // we don't know the size until it loads
      height: 1,
      tex: tex
    }

    var img = new Image()
    img.src = url

    // Wait until image loads
    await new Promise((resolve) => {
      img.onload = (ev) => {
        resolve(ev.path[0])
      }
    })

    const imgWidth = img.width || img.naturalWidth
    const imgHeight = img.height || img.naturalHeight

    textureInfo.width = imgWidth
    textureInfo.height = imgHeight
    textureInfo.text

    gl.bindTexture(gl.TEXTURE_2D, tex)
    gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGBA, gl.RGBA, gl.UNSIGNED_BYTE, img)

    textures.set(id, textureInfo)

    return JSON.stringify(textureInfo)
  }

  
  (async () => {
    await initializeGL()
    const { id, width, height } = await loadImageAndCreateTextureInfo("https://localhost:5001/assets/doll.png")
    await createTarget()
    //await blitOnTarget(id, width, height)
    //await blitOnRoot()
    //await drawRootTarget(0, 0)
  })()
  
})()