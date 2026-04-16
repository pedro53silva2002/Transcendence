# Transcendence
* 42 - Common Core - Rank 5 - Transcendence: the last project of 42 Common core. *

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