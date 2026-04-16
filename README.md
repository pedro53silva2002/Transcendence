# Transcendence
* 42 - Common Core - Rank 5 - Transcendence: the last project of 42 Common core. *

<img width="1181" height="846" alt="image" src="https://github.com/user-attachments/assets/9e11d7db-e97e-4cdd-a989-ce5a3325af0e" />

Porject Architecture

transcendence/
├── README.md                   # Project documentation
├── .env                        # Environment variables
├── .gitignore
├── docker-compose.yml          # Service orchestration
├── nginx/
│   └── nginx.conf
├── backend/
│   ├── src/                    # code logic in JAVA
│   ├── Dockerfile
│   └── build.gradle            # tool build for compile and manage dependencies for JAVA
└── frontend/
    ├── Dockerfile
    └── src/                    # code logic in typescript, angular, tailwindcss