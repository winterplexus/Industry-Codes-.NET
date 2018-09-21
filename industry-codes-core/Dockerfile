FROM microsoft/dotnet:latest AS build-env
WORKDIR /app

COPY *.sln .
COPY IndustryCodes/*.csproj ./IndustryCodes/
COPY IndustryCodes.Utility/*.csproj ./IndustryCodes.Utility/

RUN dotnet restore

COPY IndustryCodes/. ./IndustryCodes/
COPY IndustryCodes.Utility/. ./IndustryCodes.Utility/

WORKDIR /app/IndustryCodes
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1.3-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build-env /app/IndustryCodes/out ./
ENTRYPOINT ["dotnet", "IndustryCodes.dll"]