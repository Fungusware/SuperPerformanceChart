@echo off
cls

REM Tests !
REM dotnet test "UnitTests\UnitTests.csproj"
REM IF NOT "%ERRORLEVEL%"=="0" GOTO _FAILURE_
REM THIS WAS TAKING FOR EVER TO FINISH -- find out why

REM Variables
set year=%date:~10,4%
set day=%date:~4,2%
set month=%date:~7,2%
set hr=%time:~0,2%
set mn=%time:~3,2%
REM Fix Hours
SET hr=%hr: =0%

set VER=%year%.%month%.%day%.%hr%%mn%
echo %VER%

REM Building
rmdir /q /s publish
mkdir publish
rmdir /q /s package
mkdir package

REM Clean
echo Clean
dotnet clean "SuperPerformanceChart.sln"

REM Build
echo Pack Lib
dotnet pack "SuperPerformanceChart.csproj" --interactive --output ".\package" /p:Version=%VER%

REM Upload
echo Upload
dotnet nuget push --source "https://fungusware.pkgs.visualstudio.com/Fungusware/_packaging/Default/nuget/v3/index.json" --interactive --api-key az .\package\SuperPerformanceChart.*.nupkg

GOTO _EXIT_

:_FAILURE_
echo ******* Errors occured. See previous.
GOTO _EXIT_




: _EXIT_
pause