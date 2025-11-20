@echo off
setlocal enabledelayedexpansion

for /D %%f in (ContadorImpressao.*) do (
    set "nome=%%f"
    ren "%%f" "Etiqueta.!nome:~18!"
)

rem Renomear arquivos .csproj dentro das novas pastas
for /D %%d in (Etiqueta.*) do (
    for %%p in ("%%d\ContadorImpressao*.csproj") do (
        set "proj=%%~nxp"
        ren "%%p" "Etiqueta.!proj:~18!"
    )
)

endlocal
pause
