# Unity WebGL Game Folder

Arrastra tus archivos exportados de Unity WebGL directamente a esta carpeta.

- Copia la carpeta `Build` de Unity dentro de esta carpeta (de modo que los archivos queden en `wwwroot/Unity/Build/`).
- Copia la carpeta `TemplateData` si es requerida por tu juego.
- Puedes incluir también el `index.html` original para referencias de estilos, aunque la vista de ASP.NET Core ya tiene integrado el código necesario para renderizar el juego con el header y footer globales del sitio.
