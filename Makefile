COMPOSE_FILE = docker-compose.yml
COMPOSE_DEV_FILE = docker-compose.dev.yml
PROJECT_NAME = transcendence
DEV_INFRA_SERVICES = postgres-db redis minio

all: up

build:
	docker compose -f $(COMPOSE_FILE) -p $(PROJECT_NAME) build

up:
	docker compose -f $(COMPOSE_FILE) -p $(PROJECT_NAME) up -d --build

down:
	docker compose -f $(COMPOSE_FILE) -p $(PROJECT_NAME) down

clean: down
	docker compose -f $(COMPOSE_FILE) -p $(PROJECT_NAME) down --volumes

fclean: clean
	docker system prune -af --volumes

re: fclean up

dev:
	docker compose -f $(COMPOSE_FILE) -f $(COMPOSE_DEV_FILE) -p $(PROJECT_NAME) up -d $(DEV_INFRA_SERVICES)
