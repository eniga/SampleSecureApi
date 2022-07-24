#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SampleSecureApi/SampleSecureApi.csproj", "SampleSecureApi/"]
RUN dotnet restore "SampleSecureApi/SampleSecureApi.csproj"
COPY . .
WORKDIR "/src/SampleSecureApi"
RUN dotnet build "SampleSecureApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SampleSecureApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SampleSecureApi.dll"]
