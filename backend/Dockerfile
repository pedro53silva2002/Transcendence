FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY Trippie.sln ./
COPY src/Trippie.csproj ./src/
RUN dotnet restore

COPY src/ ./src/
RUN dotnet publish ./src/Trippie.csproj -c Release -o /out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# postgresql-client provides psql, used by entrypoint.sh to detect whether the
# schema already exists before deciding to run migrations.
# curl is used by the compose healthcheck (the aspnet image ships no wget/curl).
RUN apt-get update \
    && apt-get install -y --no-install-recommends postgresql-client curl \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /out .

# Migration SQL files are not part of the published output, so copy them
# explicitly. Program.cs resolves them relative to the app base directory.
COPY --from=build /app/src/Common/Database/Migrations ./Common/Database/Migrations

COPY entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["/app/entrypoint.sh"]
