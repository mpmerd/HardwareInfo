# HardwareInfo

Este proyecto contiene un script en C# que muestra las características del hardware del equipo donde se ejecuta. El script es **multiplataforma** y funciona tanto en **Mac** como en **Windows**.

## Características principales
- Detección automática del sistema operativo (Mac o Windows)
- Muestra información relevante del hardware y sistema:
  - Modelo y fabricante
  - Procesador (marca, núcleos físicos y lógicos)
  - Memoria RAM
  - Almacenamiento
  - Red (MAC, IP)
  - Pantalla
  - Batería (si aplica)
  - Sistema operativo y versión
  - Tiempo de actividad
  - GPU
  - Dispositivos USB conectados
  - Bluetooth
  - Placa base (solo Windows)

## Ejecución

1. Instala .NET SDK si no lo tienes ([descargar aquí](https://dotnet.microsoft.com/download))
2. Abre una terminal en la carpeta del proyecto
3. Ejecuta:
   ```bash
   dotnet run
   ```

## Notas
- En Mac, utiliza comandos como `system_profiler`, `sysctl`, `df`, etc.
- En Windows, utiliza comandos como `wmic`, `ipconfig`, `powershell`, etc.
- El script muestra la información directamente en la terminal.

## Autor
Creado por Maikel Pelaez y GitHub Copilot
