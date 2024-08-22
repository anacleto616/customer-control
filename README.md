# Customer Control

## Pré requisitos

- Tenha instalado o .NET 8 (https://dotnet.microsoft.com/pt-br/download)
- Instale o Docker (https://www.docker.com/)

## Como rodar a aplicação

- Abra seu terminal e clone o projeto numa pasta de sua preferência: `git clone https://github.com/anacleto616/customer-control.git`
- Entre na pasta: `cd customer-control`
- Execute o docker: `docker compose -f infra/compose.yaml up -d`
- Entre na pasta da api: `cd CustomerControl.Api`
- Execute: `dotnet run`