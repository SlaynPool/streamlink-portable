Imports System.Reflection
Imports System.Deployment
Imports System.Text
Imports System.Threading

<Assembly: AssemblyTitle("Streamlink for Windows")>
<Assembly: AssemblyDescription("")>
<Assembly: AssemblyCompany("Streamlink")>
<Assembly: AssemblyProduct("Streamlink for Windows")>
<Assembly: AssemblyCopyright("Streamlink")>
<Assembly: AssemblyTrademark("Streamlink for Windows")>
<Assembly: AssemblyVersion("1.0.0.0")>
<Assembly: AssemblyFileVersion("1.0.0.0")>

Module Module1

    Public url_trabajo_app As String = System.Reflection.Assembly.GetExecutingAssembly().CodeBase
    Sub Main()
        On Error Resume Next
        SetConsoleCtrlHandler(New HandlerRoutine(AddressOf ControlHandler), True)
        url_trabajo_app = url_trabajo_app.Replace("file:///", "")
        url_trabajo_app = url_trabajo_app.Replace("file:\", "")
        url_trabajo_app = System.IO.Path.GetDirectoryName(url_trabajo_app)
        url_trabajo_app = url_trabajo_app.Replace("file:///", "")
        url_trabajo_app = url_trabajo_app.Replace("file:\", "")

        Dim deteccion_path As String = url_trabajo_app.Remove(3)
        deteccion_path = deteccion_path.Remove(0, 1)
        deteccion_path = deteccion_path.Replace("/", "\")
        If deteccion_path = ":\" = False And url_trabajo_app.StartsWith("\\") = False Then
            If url_trabajo_app.StartsWith("\") Then
                url_trabajo_app = "\" & url_trabajo_app
            Else
                url_trabajo_app = "\\" & url_trabajo_app
            End If
        End If

        Console.OutputEncoding = Encoding.UTF8
        Console.InputEncoding = Encoding.UTF8

        Dim argumentos_finales As String = Environment.CommandLine
        If argumentos_finales.StartsWith(Chr(34)) Then
            argumentos_finales = argumentos_finales.Remove(0, 1)
            argumentos_finales = argumentos_finales.Remove(0, argumentos_finales.IndexOf(Chr(34)))
            If argumentos_finales.Contains(" ") Then
                argumentos_finales = argumentos_finales.Remove(0, argumentos_finales.IndexOf(" "))
            Else
                argumentos_finales = ""
            End If
        Else
            If argumentos_finales.Contains(" ") Then
                argumentos_finales = argumentos_finales.Remove(0, argumentos_finales.IndexOf(" "))
            Else
                argumentos_finales = ""
            End If
        End If

        Dim DATOS_VER_ACTUAL As String = url_trabajo_app & "\VERSION.txt"
        If IO.File.Exists(DATOS_VER_ACTUAL) Then
            DATOS_VER_ACTUAL = IO.File.ReadAllText(DATOS_VER_ACTUAL, Encoding.UTF8)
            Console.WriteLine("[Streamlink for Windows " & DATOS_VER_ACTUAL & "]")
        Else
            Console.WriteLine("[Streamlink for Windows]")
        End If
        Dim info = New ProcessStartInfo(Chr(34) & url_trabajo_app & "\Python 3.5.2\python.exe" & Chr(34), Chr(34) & url_trabajo_app & "\Streamlink\Streamlink.py" & Chr(34) & " --config " & Chr(34) & url_trabajo_app & "\streamlinkrc" & Chr(34) & " --rtmp-rtmpdump " & Chr(34) & url_trabajo_app & "\Streamlink\rtmpdump\rtmpdump.exe" & Chr(34) & " --ffmpeg-ffmpeg " & Chr(34) & url_trabajo_app & "\Streamlink\ffmpeg\ffmpeg.exe" & Chr(34) & " " & argumentos_finales)
        info.UseShellExecute = False
        Dim proc = Process.Start(info)
        proc.WaitForExit()
        'Threading.Thread.Sleep(Threading.Timeout.Infinite)
        Console.WriteLine("[End of Streamlink for Windows]")
        Environment.Exit(Proc.ExitCode)
    End Sub

    Public Function ControlHandler(ByVal ctrlType As CtrlTypes) As Boolean
        Threading.Thread.Sleep(Timeout.Infinite)
    End Function

    Public Declare Auto Function SetConsoleCtrlHandler Lib "kernel32.dll" (ByVal Handler As HandlerRoutine, ByVal Add As Boolean) As Boolean

    Public Delegate Function HandlerRoutine(ByVal CtrlType As CtrlTypes) As Boolean

    Public Enum CtrlTypes
        CTRL_C_EVENT = 0
        CTRL_BREAK_EVENT
        CTRL_CLOSE_EVENT
        CTRL_LOGOFF_EVENT = 5
        CTRL_SHUTDOWN_EVENT
    End Enum

End Module