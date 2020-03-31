(function() {
  const rootCanvas = document.getElementById("root-canvas")
  const rootCtx = rootCanvas.getContext('2d')

  // Render targets
  const targets = new Map()
  // Textures
  const images = new Map()

  window.createTarget = async (id, x, y) => {
    const canvas = document.createElement('canvas')
    canvas.width = 1920
    canvas.height = 1080
    
    const ctx = canvas.getContext('2d')

    targets.set(id, {
      canvas,
      ctx,
      x,
      y
    })
    
    return id
  }

  window.drawOnTarget = (id) => {
    const target = targets.get(id)
    const image = images.get(id)
    console.log(image)
    target.ctx.drawImage(image, 0, 0)
  }

  window.drawAllTargets = async () => {
    rootCtx.clearRect(0, 0, rootCanvas.width, rootCanvas.height)
    targets.forEach((target, id) => {
      console.log(target)
      rootCtx.drawImage(target.canvas, target.x, target.y)
    })
  }

  window.createImage = async (url, id) => {
    // Create new image object.
    const img = new Image()
    img.id = id
    img.src = url

    // Wait until image loads
    await new Promise((resolve) => {
      img.onload = (ev) => {
        resolve(ev)
      }
    })

    images.set(id, img)
  }
  (async () => {
    await window.createImage("https://localhost:5001/assets/doll.png", 'derp')
    await window.createTarget('derp', 0, 0)
    await window.drawOnTarget('derp')
    await window.drawAllTargets()
  })()
})()