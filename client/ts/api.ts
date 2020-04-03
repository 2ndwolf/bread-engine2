(() => {
  const _window = (window as any)
  const rootCanvas: HTMLCanvasElement = document.getElementById("root-canvas") as any
  const rootCtx: CanvasRenderingContext2D = rootCanvas.getContext('2d')

  interface RenderTarget {
    canvas: HTMLCanvasElement,
    ctx: CanvasRenderingContext2D,
    x: number,
    y: number
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

  _window.createTarget = async (targetId: string, x: number, y: number, width: number, height: number) => {
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

  _window.deleteTarget = async (targetId: string) => {
    targets.delete(targetId)
    const canvas = document.getElementById(targetId)
    canvas.parentElement.removeChild(canvas)
  }

  _window.drawOnTarget = (targetId: string, textureId: string, x: number, y: number) => {
    const target = targets.get(targetId)
    const texture = textures[textureId]
    target.ctx.drawImage(texture, x, y)
  }

  _window.setTargetPos = (targetId: string, x: number, y: number) => {
    const target = targets.get(targetId)
    target.x = x
    target.y = y
  }

  _window.drawSingleTarget = async (targetId: string) => {
    const target = targets.get(targetId)
    rootCtx.drawImage(target.canvas, target.x, target.y)
  }
  
  _window.drawAllTargets = async () => {
    targets.forEach((target: RenderTarget, targetId: string) => {
      rootCtx.drawImage(target.canvas, target.x, target.y)
    })
  }

  _window.clearRootCanvas = async () => {
    rootCtx.clearRect(0, 0, rootCanvas.width, rootCanvas.height)
  }

})()