#!/bin/bash
# entrypoint.sh

# Rodar as migra��es
echo "Rodando as migra��es..."
dotnet ef database update --project /app/Infrastructure --startup-project /app/WebApi

# Iniciar a aplica��o
echo "Iniciando a aplica��o..."
dotnet thundersApp.dll
