@echo off
setlocal enabledelayedexpansion

set "source_folder=C:\3D_prac\_ProjectC\Assets\ProjectC"
set "destination_folder=C:\3D_prac\ProjectC_Doxygen\ProjectC"

:: コピー先フォルダのファイルを削除
if exist "%destination_folder%\*" (
    del /q "%destination_folder%\*"
)

:: コピー先フォルダが存在しなければ作成
if not exist "%destination_folder%" mkdir "%destination_folder%"

:: .csファイルをコピー（ProjectCフォルダ内のサブディレクトリ構造を保持）
for /r "%source_folder%" %%f in (*.cs) do (
    set "full_path=%%f"
    set "relative_path=!full_path:%source_folder%=!"
    
    :: コピー先のフルパスを作成
    set "dest_path=%destination_folder%!relative_path!"
    
    :: 必要なディレクトリを作成
    mkdir "!dest_path:~0,-4!" 2>nul

    :: ファイルをコピー
    copy "!full_path!" "!dest_path!"
)

echo コピーが完了しました。
