@echo off
call %windir%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe ASMgenerator8080.sln /p:Configuration=Release;TargetFrameworkVersion=v3.5 /p:Platform="Any CPU" /target:Build
call .\netz.exe -s -z .\ASMgenerator8080\bin\Release\ASMgenerator8080.exe .\FastColoredTextBox.dll .\TabStrip.dll -so