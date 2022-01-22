#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PasswordMenager/PasswordMenager.csproj", "PasswordMenager/"]
RUN dotnet restore "PasswordMenager/PasswordMenager.csproj"
COPY . .
WORKDIR "/src/PasswordMenager"
RUN dotnet build "PasswordMenager.csproj" -c Release -o /app/build
RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh

FROM build AS publish
RUN dotnet publish "PasswordMenager.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["/bin/sh", "./entrypoint.sh"]