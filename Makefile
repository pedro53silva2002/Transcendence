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

# ----------------------------
# Gradle / Java safe clean
# ----------------------------
gradle-clean:
	@echo "Running Gradle clean and removing build artifacts..."
	./gradlew clean
	# Remove rebuildable directories safely
	find . -type d \( -name "build" -o -name ".gradle" -o -name ".tmp" -o -name "caches" \) -prune -exec rm -rf {} +
	@echo "Gradle clean complete!"

fclean: clean
	docker system prune -af --volumes

re: fclean up

logs:
	docker compose -f $(COMPOSE_FILE) -p $(PROJECT_NAME) logs -f

logs-%:
	docker compose -f $(COMPOSE_FILE) -p $(PROJECT_NAME) logs -f $*

ps:
	docker compose -f $(COMPOSE_FILE) -p $(PROJECT_NAME) ps

dev-infra:
	docker compose -f $(COMPOSE_FILE) -f $(COMPOSE_DEV_FILE) -p $(PROJECT_NAME) up -d $(DEV_INFRA_SERVICES)

dev: dev-infra
	@echo ""
	@echo "Infra is up (postgres, redis, minio)."
	@echo "Open two terminals and run:"
	@echo "  make dev-backend"
	@echo "  make dev-frontend"

.PHONY: all build up down clean fclean re logs ps test dev dev-infra dev-backend dev-frontend
