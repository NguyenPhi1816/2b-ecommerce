@echo off
setlocal
set DOTNET_ENVIRONMENT=Development
dotnet ef dbcontext scaffold Name=Default Npgsql.EntityFrameworkCore.PostgreSQL ^
  --context AppDbContext ^
  --output-dir Infrastructure/Models ^
  --context-dir Infrastructure/Persistence ^
  --schema public ^
  --use-database-names ^
  --no-pluralize --force %*
