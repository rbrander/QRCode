@echo off
rem This is how we do it (compile)
rem
rem
rem

c:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe /debug *.cs

rem If there is an error compiling, exit
if ERRORLEVEL 1 goto error

echo Success Compiling!
rem else run the program

if "%~1" == "/norun" goto end
call QRCodeEncoder.exe
goto end

:error
echo.
echo Error Compiling!


:end
echo.
