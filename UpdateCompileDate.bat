@echo off
REM Get the current date and time
for /f "tokens=1-6 delims=/:. " %%a in ("%date% %time%") do (
    set "year=%%d"
    set "day=%%b"
    set "month=%%c"
    set "hour=%%e"
    set "minute=%%f"
)

REM Assemble the formatted date and time
set "formatted_datetime=%year%%month%%day%.%hour%%minute%"

REM Create a C# file named CompileTime.cs
(
    echo using System;
    echo namespace Viper.CompileTime
    echo {
    echo     public static class CompileDateTime
    echo     {
    echo         public const string DateTime = "%formatted_datetime%";
    echo     }
    echo }
) > CompileTime.cs

echo CompileTime.cs generated successfully.
