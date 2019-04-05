FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["MasterPerform/MasterPerform.csproj", "MasterPerform/"]
RUN dotnet restore "MasterPerform/MasterPerform.csproj"
COPY . .
WORKDIR "/src/MasterPerform"
RUN dotnet build "MasterPerform.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MasterPerform.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MasterPerform.dll"]