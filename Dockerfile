FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CoHabit.API.csproj", "./"]
RUN dotnet restore "CoHabit.API.csproj"
COPY . .
RUN dotnet build "CoHabit.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoHabit.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoHabit.API.dll"]