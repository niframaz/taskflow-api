# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution file first
COPY TaskFlow.Api.slnx ./

# Copy all projects
COPY TaskFlow.Api/ TaskFlow.Api/
COPY TaskFlow.Application/ TaskFlow.Application/
COPY TaskFlow.Domain/ TaskFlow.Domain/
COPY TaskFlow.Infrastructure/ TaskFlow.Infrastructure/

# Restore dependencies for the solution
RUN dotnet restore TaskFlow.Api.slnx

# Publish API project
RUN dotnet publish TaskFlow.Api/TaskFlow.Api.csproj -c Release -o /app/out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Create logs folder
RUN mkdir -p /app/logs/TaskFlowApi

EXPOSE 5000
ENTRYPOINT ["dotnet", "TaskFlow.Api.dll"]