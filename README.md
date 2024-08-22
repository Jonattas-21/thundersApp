# Task App
This is a simple task app that allows you to add, delete, and change status an complete CRUD using .net core and has some technicals keypoints.

### Container Diagram

![This is an alt text.](https://raw.githubusercontent.com/Jonattas-21/thundersApp/master/Docs/ContainerDiagram.JPG?token=GHSAT0AAAAAACU4IYKTAAUACVF76QR4M2QCZWGVALQ "Container Diagram.")

### Use Case Diagram
![This is an alt text.](https://raw.githubusercontent.com/Jonattas-21/thundersApp/master/Docs/UsecaseDiagram.JPG?token=GHSAT0AAAAAACU4IYKSKV5XAJYSVJNW5DXGZWGVCNQ "Use Case Diagram.")

## Technicals keypoints
* Decoupled Code between business domain and others infrastructure features.
* Clean Archtecture based.
* Unit Tests implemented.
* Database code first approach.
* Docker support and ready to use.
* Full Solid pattern based
* Cache API approach
* Swagger Api

## Tools Requiriments

### Angular install

```sh
npm install -g @angular/cli@17
```

### dotNet install
```sh

# Baixar o script de instalação
Invoke-WebRequest -Uri "https://download.visualstudio.microsoft.com/download/pr/4b9dc13b-c8b4-4e4f-95f0-4d4b3e53b8a6/85809f9a4741ff6a60d7e2baf571ce2f/aspnetcore-runtime-8.0.0-win-x64.exe" -OutFile "dotnet-sdk-installer.exe"

# Executar o instalador
Start-Process -FilePath ".\dotnet-sdk-installer.exe" -ArgumentList "/quiet" -NoNewWindow -Wait
```

## How to run

1. Configure Database Postgres
2. Run the Entity-Framework Migrations
3. Execute stratupscript in /script or use Swagger to add origin entity
4. Execute **thundersApp** project the is the software Backend
5. Execute **Frontend** project the is the software frontend

It is also posible to run both DockerFile or adjuste and run the `docker-compose` in root project.


## [TODO] Next Steps for this project

* Implement a CI/CD pipeline
* Implement Logs in ELK approach
* Configure keycloack for security
* Attach Postman collection for tests
