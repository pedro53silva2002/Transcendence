# Transcendence
* 42 - Common Core - Rank 6 - Transcendence: the last project of 42 Common core. *

<img width="1181" height="846" alt="image" src="https://github.com/user-attachments/assets/9e11d7db-e97e-4cdd-a989-ce5a3325af0e" />

```
Project Architecture

transcendence/
├── README.md                           # Project documentation
├── .env                                # Environment variables
├── .gitignore
├── docker-compose.yml                  # Service orchestration configuration
├── nginx/
│   └── nginx.conf                      # Reverse proxy and static file serving
├── backend/
│   ├── build.gradle                    # tool build and manage dependencies for JAVA
│   ├── settings.gradle                 # Gradle project name configuration
│   ├── Dockerfile                      # Backend container image definition
│   └── src/
│       ├── main/
│       |   ├── com/java/
│       │   |   ├── Application.java    # Spring Boot entry point
|       |   |   (for later)
│       │   |   └── function in java
|       |   └── resources/
|       |       ├── application.yml     # Spring Boot database and server
|       |       └── db/migration/       # Flyway database schema version
|       |           └── V1__init.sql    # Initial database tables schema
│       └── test/
│           ├── java/
│           │   └── transcendence/      # Same package structure
│           └── resources/
|
└── frontend/
    ├── Dockerfile
    └── src/                            # code logic in typescript, angular, tailwindcss
```

Useful Docker commands
```
# Stop and remove all containers, networks (keeps volumes/data)
docker compose down

# Start all services in the background
docker compose up -d

# Restart everything (stop + start)
docker compose restart

# View logs from all services
docker compose logs -f

# View logs from specific service
docker compose logs -f backend

# Rebuild and start (useful after code changes)
docker compose up -d --build

# Lists all containers and show their status
docker compose ps

# Stop everything (keeps containers, just stops them)
docker compose stop

# Start stopped containers
docker compose start

# Remove everything including volumes (WARNING: deletes data!)
docker compose down -v
```

Most common practices
```
# Services already built, just start them
docker compose up -d

# After code changes:
docker compose up -d --build

# Check if it's working:
docker compose logs -f backend

# Quick restart without rebuilding
docker compose restart

# Stop everything:
docker compose down
```

To restart just the backend
```
docker compose restart backend
```

Postgresql databases commands
```
\l              # List databases
\dt             # List tables in current database
\du             # List users/roles
\d table_name   # Describe a table structure
\q              # Quit psql
\c databa_name  # Connects to a different database (shortcut after psql -U <user> -d <database>)             
```
