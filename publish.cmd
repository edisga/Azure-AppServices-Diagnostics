@echo off

:: Publish Compiler Host to Build Location
echo\
echo "------------------- Publishing Compiler Host to build directory -------------------"
echo\
dotnet publish "%~dp0src\Diagnostics.CompilerHost\Diagnostics.CompilerHost.csproj" -c Release -o "%~dp0build\antares.external.diagnostics.compilerhost.1.0.0" --no-build

IF %ERRORLEVEL% NEQ 0 (
echo "Diagnostics.CompilerHost Publish Failed."
exit /b %errorlevel%
)
echo\

powershell Compress-Archive -Path "%~dp0build\antares.external.diagnostics.compilerhost.1.0.0\*" -DestinationPath "%~dp0build\DiagnosticsCompilerHost.zip"

rmdir /q /s "%~dp0build\antares.external.diagnostics.compilerhost.1.0.0"

:: Publish Runtime Host to Build Location
echo\
echo "------------------- Publishing Runtime Host to build directory -------------------"
echo\
dotnet publish "%~dp0src\Diagnostics.RuntimeHost\Diagnostics.RuntimeHost.csproj" -c Release -o "%~dp0build\antares.external.diagnostics.runtimehost.1.0.0" --no-build

powershell Compress-Archive -Path "%~dp0build\antares.external.diagnostics.runtimehost.1.0.0\*" -DestinationPath "%~dp0build\DiagnosticsRuntimeHost.zip"

rmdir /q /s "%~dp0build\antares.external.diagnostics.runtimehost.1.0.0"

IF %ERRORLEVEL% NEQ 0 (
echo "Diagnostics.RuntimeHost Publish Failed."
exit /b %errorlevel%
)

exit /b 0

popd