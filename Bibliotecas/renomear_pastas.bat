@echo off
setlocal enabledelayedexpansion

for /D %%f in (ContadorImpressao.*) do (
    set "nome=%%f"
    ren "%%f" "Etiqueta.!nome:~18!"
)

endlocal
pause
