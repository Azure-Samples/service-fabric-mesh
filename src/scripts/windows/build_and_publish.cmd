@echo off
setlocal

if "%1" == "" (
    echo Usage: %~nx0 imageinfo.txt
    exit /b 1
)

if not exist "%1" (
    echo "%1": image info file not found!
    exit /b 2
)

set image_name=
for /F "tokens=1,2,3,4,5,6,7 delims=," %%i in (%1) do (
    if "%%i" == "windows" (
        set image_name=%%j
        set tag_value=%%k
        set official_tag_value=%%l
        set registry_path=%%m
        set build_root=%%n
        set docker_file=%%o
    )
)

set image_name=%image_name: =%
set tag_value=%tag_value: =%
set official_tag_value=%official_tag_value: =%
set registry_path=%registry_path: =%
set build_root=%build_root: =%
set docker_file=%docker_file: =%

echo image_name = %image_name%
echo tag_value = %tag_value%
echo official_tag_value = %official_tag_value%
echo registry_path = %registry_path%
echo build_root = %build_root%
echo docker_file = %docker_file%
echo.


if "%image_name%" == "" (
    echo "%1": image information for "windows" os not found in file
    exit /b 3
)

echo Building Dev Image
set docker_command=docker build %build_root% -f %docker_file% -t %image_name%:dev-%tag_value%
echo %docker_command%
call %docker_command%
if ERRORLEVEL 1 (
    set last_error=%ERRORLEVEL%
    echo Failed to build image %image_name%:dev-%tag_value%
    exit /b %last_error%
)
echo Successfully built image %image_name%:dev-%tag_value%
echo.

if "%2" == "build_only" (
    exit /b 0
)

echo Tagging Official image
set docker_command=docker tag %image_name%:dev-%tag_value% %registry_path%/%image_name%:%official_tag_value%
echo %docker_command%
call %docker_command%
if ERRORLEVEL 1 (
    set last_error=%ERRORLEVEL%
    echo Failed to tag official image %registry_path%/%image_name%:%official_tag_value%
    exit /b %last_error%
)
echo Successfully tagged Official image %registry_path%/%image_name%:%official_tag_value%
echo.

echo Publishing Official image
set docker_command=docker push %registry_path%/%image_name%:%official_tag_value%
echo %docker_command%
rem call %docker_command%
if ERRORLEVEL 1 (
    set last_error=%ERRORLEVEL%
    echo Failed to publish official image to %registry_path%/%image_name%:%official_tag_value%
    exit /b %last_error%
)
echo Successfully published Official image to %registry_path%/%image_name%:%official_tag_value%
echo.

exit /b 0
