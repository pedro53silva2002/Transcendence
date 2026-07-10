#!/bin/sh
set -e

DB_HOST="${POSTGRES_HOST:-postgres-db}"
DB_PORT="${POSTGRES_PORT:-5432}"
DB_NAME="${POSTGRES_DB}"
DB_USER="${POSTGRES_USER}"
DB_PASS="${POSTGRES_PASSWORD}"

schema_exists() {
    PGPASSWORD="$DB_PASS" psql -h "$DB_HOST" -p "$DB_PORT" -U "$DB_USER" -d "$DB_NAME" \
        -tAc "SELECT 1 FROM information_schema.schemata WHERE schema_name = 'auth' LIMIT 1;" \
        2>/dev/null | grep -q 1
}

if schema_exists; then
    echo "[entrypoint] Database schema already present — skipping migrations."
else
    echo "[entrypoint] Database schema not found — running migrations."
    dotnet Trippie.dll --migrate
fi

echo "[entrypoint] Starting Trippie backend."
exec dotnet Trippie.dll
