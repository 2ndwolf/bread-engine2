(() => {
  const _window = (window as any)
  const rootCanvas: HTMLCanvasElement = document.getElementById("root-canvas") as any
  const rootCtx: CanvasRenderingContext2D = rootCanvas.getContext('2d')
  rootCanvas.width = 1920
  rootCanvas.height = 1280

  interface RenderTarget {
    canvas: HTMLCanvasElement,
    ctx: CanvasRenderingContext2D
  }

  // Render targets
  // Targets should map to textures in C# / ECS, not here.
  // So no mapping is nessecary here between the two.
  const targets = new Map<string, RenderTarget>()
  const textures = new Map<string, HTMLImageElement>()

  _window.createTexture = async (textureId: string, url: string) => {
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

  _window.deleteTexture = async (textureId: string) => {
    textures.delete(textureId)
  }

  _window.createTarget = async (targetId: string, width: number, height: number) => {
    const canvas = document.createElement('canvas')
    canvas.width = 1920
    canvas.height = 1280
    canvas.id = targetId

    const ctx = canvas.getContext('2d')

    // schema for targets
    targets.set(targetId, {
      canvas,
      ctx
    })
    
    return targetId
  }

  _window.deleteTarget = async (targetId: string) => {
    targets.delete(targetId)
    const canvas = document.getElementById(targetId)
    canvas.parentElement.removeChild(canvas)
  }

  _window.drawOnTarget = (targetId: string, textureId: string, x: number, y: number) => {
    const target = targets.get(targetId)
    const texture = textures.get(textureId)

    target.ctx.drawImage(texture, x, y)
  }

  _window.drawSingleTarget = async (targetId: string, x: number, y: number) => {
    const target = targets.get(targetId)
    console.log(target.canvas.width)
    rootCtx.drawImage(target.canvas, x, y)
  }
  
  _window.clearRootCanvas = async () => {
    rootCtx.clearRect(0, 0, rootCanvas.width, rootCanvas.height)
  }

})()