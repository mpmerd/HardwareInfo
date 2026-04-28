# HardwareInfo

Aplicación de consola multiplataforma que muestra las características del hardware del equipo donde se ejecuta. Funciona en **Windows**, **Linux** y **macOS**.

## Características

| Sección              | Windows | Linux | macOS |
|----------------------|:-------:|:-----:|:-----:|
| Información general  | ✅      | ✅    | ✅    |
| Procesador           | ✅      | ✅    | ✅    |
| Memoria RAM          | ✅      | ✅    | ✅    |
| Almacenamiento       | ✅      | ✅    | ✅    |
| Red (IP/MAC)         | ✅      | ✅    | ✅    |
| Pantalla             | ✅      | ✅    | ✅    |
| Batería              | ✅      | ✅    | ✅    |
| Versión del SO       | ✅      | ✅    | ✅    |
| Tiempo de actividad  | ✅      | ✅    | ✅    |
| GPU                  | ✅      | ✅    | ✅    |
| USB conectados       | ✅      | ✅    | ❌    |
| Placa base           | ✅      | ❌    | ❌    |
| Bluetooth            | ❌      | ❌    | ✅    |
| Temperatura sistema  | ❌      | ✅    | ❌    |

## Requisitos

- [.NET SDK](https://dotnet.microsoft.com/download) 8.0 o superior

## Ejecución

```bash
cd HardwareInfo
dotnet run
```

## Compilar (publicación)

```bash
dotnet publish -c Release -r win-x64   --self-contained true
dotnet publish -c Release -r linux-x64 --self-contained true
dotnet publish -c Release -r osx-x64   --self-contained true
```

## Cambios respecto a la versión original

### Novedades de la rama `feature/linux-support`

- **Soporte completo para Linux**: se agregó el método `MostrarInfoLinux()` que obtiene información del hardware usando comandos estándar de Linux:
  - `/etc/os-release` y `uname` para información del sistema
  - `/proc/cpuinfo` y `nproc` para CPU
  - `free` y `lsblk` / `df` para almacenamiento
  - `ip` y `/sys/class/net/*/address` para red
  - `cat /sys/class/drm/*/modes` para pantalla
  - `/sys/class/power_supply/BAT0/capacity` para batería
  - `lspci` para GPU
  - `lsusb` para USB
  - `/sys/class/thermal/thermal_zone*/temp` para temperatura

- **Renombrado del proyecto**: de `MacHardwareInfo` a `HardwareInfo`, ya que ahora es verdaderamente multiplataforma.

- **Limpieza de código**: el proyecto compila sin warnings (CS7022 eliminado al remover `Program.cs` duplicado).

## Autor

Creado por Maikel Pelaez y GitHub Copilot
