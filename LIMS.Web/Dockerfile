FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
RUN apt-get update -qq && apt-get -y install libgdiplus libc6-dev

EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["LIMS.Web/LIMS.Web.csproj", "LIMS.Web/"]
COPY ["LIMS.Framework/LIMS.Framework.csproj", "LIMS.Framework/"]
COPY ["LIMS.Domain/LIMS.Domain.csproj", "LIMS.Domain/"]
COPY ["LIMS.Core/LIMS.Core.csproj", "LIMS.Core/"]
COPY ["LIMS.Services/LIMS.Services.csproj", "LIMS.Services/"]
COPY ["LIMS.Api/LIMS.Api.csproj", "LIMS.Api/"]
RUN dotnet restore "LIMS.Web/LIMS.Web.csproj"
COPY . .
WORKDIR "/src/LIMS.Web"
RUN dotnet build "LIMS.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LIMS.Web.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN chmod 755 /app/Rotativa/Linux/wkhtmltopdf

ENTRYPOINT ["dotnet", "LIMS.Web.dll"]
