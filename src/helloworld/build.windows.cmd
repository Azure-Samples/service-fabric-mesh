@echo off
call ..\scripts\windows\build_and_publish.cmd helloworld.imageinfo.txt %*
exit /b %ERRORLEVEL%
