build_backend:
	dotnet clean ./backend/ 
	dotnet build ./tests/
	dotnet build ./backend/ 

build_frontend:
	dotnet build ./frontend/ 

build: build_backend build_frontend
	