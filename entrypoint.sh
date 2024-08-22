#!/bin/bash
# entrypoint.sh

# Rodar as migrações
echo "Rodando as migrações..."
dotnet ef database update --project /app/Infrastructure --startup-project /app/WebApi

# Iniciar a aplicação
echo "Iniciando a aplicação..."
dotnet thundersApp.dll
