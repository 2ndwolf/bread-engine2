(function() {
  const rootCanvas = document.getElementById("root-canvas")
  const rootCtx = rootCanvas.getContext('2d')

  // Render targets
  const targets = new Map()
  // Textures
  const textures = new Map()

  window.createTarget = async (id, x, y) => {
    const canvas = document.createElement('canvas')
    canvas.width = 1920
    canvas.height = 1080

    const ctx = canvas.getContext('2d')

    // schema
    targets.set(id, {
      canvas,
      ctx,
      x,
      y,
      // TODO: Implement multiple textures per target in the future.
      // textures: new Map() // textureKey, textureArray? or texture
      // activeTexture: {key, index}
    })
    
    return id
  }

  window.drawOnTarget = (id) => {
    const target = targets.get(id)
    const texture = textures.get(id)
    target.ctx.drawImage(texture, 0, 0)
  }

  window.setTargetPos = (id, x, y) => {
    const target = targets.get(id)
    target.x = x
    target.y = y
  }

  /*
  // TODO: Implement multiple textures per target in the future.
  // textures: new Map() // textureKey, textureArray? or texture
  // activeTexture: {key, index}
  window.addTexToTarget = (id) => {
    
  }

  window.removeTexFromTarget = (id) => {

  }
  */

  window.drawAllTargets = async () => {
    rootCtx.clearRect(0, 0, rootCanvas.width, rootCanvas.height)
    targets.forEach((target, id) => {
      rootCtx.drawImage(target.canvas, target.x, target.y)
    })
  }

  window.createImage = async (url, id) => {
    // Create new texture object.
    const img = new Image()
    img.id = id
    img.src = url

    // Wait until texture loads
    await new Promise((resolve) => {
      img.onload = (ev) => {
        resolve(ev)
      }
    })

    textures.set(id, img)
  }

  (async () => {
    await window.createImage("https://localhost:5001/assets/doll.png", 'derp')
    await window.createTarget('derp', 0, 0)
    await window.drawOnTarget('derp')
    await window.drawAllTargets()
  })()

})()