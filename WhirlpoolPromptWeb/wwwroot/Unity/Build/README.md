# Carpeta de compilación de Unity WebGL

Copia aquí tus archivos de Unity compilados. La estructura esperada es:

- `wwwroot/Unity/Build/Build.loader.js`
- `wwwroot/Unity/Build/Build.framework.js`
- `wwwroot/Unity/Build/Build.data`
- `wwwroot/Unity/Build/Build.wasm`

Si tus archivos tienen nombres basados en tu proyecto (por ejemplo, `MiJuego.loader.js`), puedes renombrarlos a `Build.*` o modificar el script de inicialización en la vista `Views/Game/Index.cshtml` para que coincidan con tus nombres de archivo exactos.
