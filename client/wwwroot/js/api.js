(function() {
  const rootCanvas = document.getElementById("root-canvas")
  const rootCtx = rootCanvas.getContext('2d')

  // Render targets
  // Targets should map to textures in C# / ECS, not here.
  // So no mapping is nessecary here between the two.
  const targets = new Map()
  const textures = new Map()

  window.createTexture = async (textureId, url) => {
    // Create new texture object.
    const img = new Image()
    img.id = textureId
    img.src = url

    // Wait until texture loads
    await new Promise((resolve) => {
      img.onload = (ev) => {
        resolve(ev)
      }
    })

    textures.set(textureId, img)

    return textureId
  }

  window.deleteTexture = async (textureId) => {
    textures.delete(textureId)
  }

  window.createTarget = async (targetId, x, y, width, height) => {
    const canvas = document.createElement('canvas')
    canvas.width = width
    canvas.height = height
    canvas.id = targetId

    const ctx = canvas.getContext('2d')

    // schema for targets
    targets.set(targetId, {
      canvas,
      ctx,
      x,
      y
    })
    
    return targetId
  }

  window.deleteTarget = async (targetId) => {
    targets.delete(targetId)
    const canvas = document.getElementById(targetId)
    canvas.parentElement.removeChild(canvas)
  }

  window.drawOnTarget = (targetId, textureId, x, y) => {
    const target = targets.get(targetId)
    const texture = textures[textureId]
    target.ctx.drawImage(texture, x, y)
  }

  window.setTargetPos = (targetId, x, y) => {
    const target = targets.get(targetId)
    target.x = x
    target.y = y
  }

  window.drawSingleTarget = async () => {
    rootCtx.drawImage(target.canvas, target.x, target.y)
  }
  
  window.drawAllTargets = async () => {
    targets.forEach((target, targetId) => {
      rootCtx.drawImage(target.canvas, target.x, target.y)
    })
  }

  window.clearRootCanvas = async () => {
    rootCtx.clearRect(0, 0, rootCanvas.width, rootCanvas.height)
  }

  (async () => {
    /*
    const index = await window.createTexture("herp", "https://localhost:5001/assets/doll.png")
    await window.createTarget('derp', 0, 0)
    await window.drawOnTarget('derp', )
    await window.drawAllTargets()
    */
  })()

})()