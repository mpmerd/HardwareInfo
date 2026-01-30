using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

class HardwareInfo
{
    static bool EsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    static bool EsMac => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

    static void Main()
    {
        string sistemaOperativo = EsWindows ? "WINDOWS" : (EsMac ? "MAC" : "LINUX");
        
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine($"║     INFORMACIÓN DE HARDWARE - {sistemaOperativo,-27} ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

        if (EsWindows)
        {
            MostrarInfoWindows();
        }
        else if (EsMac)
        {
            MostrarInfoMac();
        }
        else
        {
            Console.WriteLine("Sistema operativo no soportado.");
        }
    }

    static void MostrarInfoWindows()
    {
        // Información general del sistema
        Console.WriteLine("═══ INFORMACIÓN GENERAL ═══");
        EjecutarComandoWindows("wmic", "computersystem get manufacturer,model,systemtype /format:list");
        EjecutarComandoWindows("wmic", "bios get serialnumber /format:list");
        
        Console.WriteLine("\n═══ INFORMACIÓN DEL PROCESADOR ═══");
        EjecutarComandoWindows("wmic", "cpu get name,numberofcores,numberoflogicalprocessors,maxclockspeed /format:list");
        
        Console.WriteLine("\n═══ MEMORIA RAM ═══");
        EjecutarComandoWindows("wmic", "computersystem get totalphysicalmemory /format:list");
        MostrarMemoriaWindows();
        
        Console.WriteLine("\n═══ ALMACENAMIENTO ═══");
        EjecutarComandoWindows("wmic", "logicaldisk where drivetype=3 get deviceid,size,freespace,filesystem /format:list");
        
        Console.WriteLine("\n═══ INFORMACIÓN DE RED ═══");
        EjecutarComandoWindows("ipconfig", "/all | findstr /i \"IPv4 Physical Description\"");
        
        Console.WriteLine("\n═══ INFORMACIÓN DE PANTALLA ═══");
        EjecutarComandoWindows("wmic", "desktopmonitor get screenheight,screenwidth /format:list");
        
        Console.WriteLine("\n═══ BATERÍA (si aplica) ═══");
        EjecutarComandoWindows("wmic", "path win32_battery get estimatedchargeremaining,batterystatus /format:list");
        
        Console.WriteLine("\n═══ VERSIÓN DEL SISTEMA OPERATIVO ═══");
        EjecutarComandoWindows("wmic", "os get caption,version,buildnumber /format:list");
        
        Console.WriteLine("\n═══ TIEMPO DE ACTIVIDAD ═══");
        EjecutarComandoWindows("powershell", "-Command \"$uptime = (Get-Date) - (Get-CimInstance Win32_OperatingSystem).LastBootUpTime; Write-Host ('Tiempo activo: ' + $uptime.Days + ' días, ' + $uptime.Hours + ' horas, ' + $uptime.Minutes + ' minutos')\"");
        
        Console.WriteLine("\n═══ INFORMACIÓN DE GPU ═══");
        EjecutarComandoWindows("wmic", "path win32_videocontroller get name,adapterram,driverversion /format:list");
        
        Console.WriteLine("\n═══ DISPOSITIVOS USB CONECTADOS ═══");
        EjecutarComandoWindows("wmic", "path win32_usbcontrollerdevice get dependent /format:list | findstr /i \"Device\"");
        
        Console.WriteLine("\n═══ INFORMACIÓN DE PLACA BASE ═══");
        EjecutarComandoWindows("wmic", "baseboard get manufacturer,product,version /format:list");
    }

    static void MostrarInfoMac()
    {
        // Información general del sistema
        Console.WriteLine("═══ INFORMACIÓN GENERAL ═══");
        EjecutarComandoMac("system_profiler", "SPHardwareDataType | grep -E 'Model Name|Model Identifier|Chip|Total Number of Cores|Memory|Serial Number'");
        
        Console.WriteLine("\n═══ INFORMACIÓN DEL PROCESADOR ═══");
        EjecutarComandoMac("sysctl", "-n machdep.cpu.brand_string");
        Console.Write("Núcleos físicos: ");
        EjecutarComandoMac("sysctl", "-n hw.physicalcpu");
        Console.Write("Núcleos lógicos: ");
        EjecutarComandoMac("sysctl", "-n hw.logicalcpu");
        
        Console.WriteLine("\n═══ MEMORIA RAM ═══");
        Console.Write("Memoria total: ");
        MostrarMemoriaMac();
        
        Console.WriteLine("\n═══ ALMACENAMIENTO ═══");
        EjecutarComandoMac("df", "-h / | tail -1 | awk '{print \"Disco principal: \" $2 \" total, \" $3 \" usado, \" $4 \" disponible (\" $5 \" usado)\"}'");
        
        Console.WriteLine("\n═══ INFORMACIÓN DE RED ═══");
        Console.WriteLine("Dirección MAC (Wi-Fi):");
        EjecutarComandoMac("ifconfig", "en0 | grep ether | awk '{print $2}'");
        Console.WriteLine("Dirección IP local:");
        EjecutarComandoMac("ipconfig", "getifaddr en0");
        
        Console.WriteLine("\n═══ INFORMACIÓN DE PANTALLA ═══");
        EjecutarComandoMac("system_profiler", "SPDisplaysDataType | grep -E 'Display Type|Resolution|Retina|Main Display'");
        
        Console.WriteLine("\n═══ BATERÍA (si aplica) ═══");
        EjecutarComandoMac("pmset", "-g batt | grep -E 'InternalBattery|drawing'");
        
        Console.WriteLine("\n═══ VERSIÓN DEL SISTEMA OPERATIVO ═══");
        EjecutarComandoMac("sw_vers", "");
        
        Console.WriteLine("\n═══ TIEMPO DE ACTIVIDAD ═══");
        EjecutarComandoMac("uptime", "");
        
        Console.WriteLine("\n═══ INFORMACIÓN DE GPU ═══");
        EjecutarComandoMac("system_profiler", "SPDisplaysDataType | grep -E 'Chipset Model|VRAM|Metal|Vendor'");
        
        Console.WriteLine("\n═══ DISPOSITIVOS USB CONECTADOS ═══");
        EjecutarComandoMac("system_profiler", "SPUSBDataType | grep -E 'Product ID|Vendor ID|Manufacturer|Speed' | head -20");
        
        Console.WriteLine("\n═══ INFORMACIÓN DE BLUETOOTH ═══");
        EjecutarComandoMac("system_profiler", "SPBluetoothDataType | grep -E 'Address|Discoverable|State|Chipset' | head -10");
    }

    static void EjecutarComandoWindows(string comando, string argumentos)
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {comando} {argumentos}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process? proceso = Process.Start(psi))
            {
                if (proceso != null)
                {
                    string salida = proceso.StandardOutput.ReadToEnd();
                    proceso.WaitForExit();
                    
                    if (!string.IsNullOrWhiteSpace(salida))
                    {
                        // Limpiar líneas vacías excesivas
                        var lineas = salida.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var linea in lineas)
                        {
                            if (!string.IsNullOrWhiteSpace(linea))
                                Console.WriteLine(linea.Trim());
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al ejecutar comando: {ex.Message}");
        }
    }

    static void EjecutarComandoMac(string comando, string argumentos)
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{comando} {argumentos}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process? proceso = Process.Start(psi))
            {
                if (proceso != null)
                {
                    string salida = proceso.StandardOutput.ReadToEnd();
                    proceso.WaitForExit();
                    
                    if (!string.IsNullOrWhiteSpace(salida))
                    {
                        Console.WriteLine(salida.TrimEnd());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al ejecutar comando: {ex.Message}");
        }
    }

    static void MostrarMemoriaWindows()
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "powershell",
                Arguments = "-Command \"[math]::Round((Get-CimInstance Win32_ComputerSystem).TotalPhysicalMemory / 1GB, 2)\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process? proceso = Process.Start(psi))
            {
                if (proceso != null)
                {
                    string salida = proceso.StandardOutput.ReadToEnd();
                    proceso.WaitForExit();
                    Console.WriteLine($"Memoria total: {salida.Trim()} GB");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void MostrarMemoriaMac()
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = "-c \"sysctl -n hw.memsize\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process? proceso = Process.Start(psi))
            {
                if (proceso != null)
                {
                    string salida = proceso.StandardOutput.ReadToEnd();
                    proceso.WaitForExit();
                    
                    if (long.TryParse(salida.Trim(), out long bytes))
                    {
                        double gb = bytes / (1024.0 * 1024.0 * 1024.0);
                        Console.WriteLine($"{gb:F2} GB");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
