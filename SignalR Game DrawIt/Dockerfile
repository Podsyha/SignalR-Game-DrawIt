FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SignalR Game DrawIt/SignalR Game DrawIt.csproj", "SignalR Game DrawIt/"]
RUN dotnet restore "SignalR Game DrawIt/SignalR Game DrawIt.csproj"
COPY . .
WORKDIR "/src/SignalR Game DrawIt"
RUN dotnet build "SignalR Game DrawIt.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SignalR Game DrawIt.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SignalR Game DrawIt.dll"]
