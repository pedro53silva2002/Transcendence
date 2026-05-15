COMPOSE_FILE = docker-compose.yml
PROJECT_NAME = transcendence

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

test:
	docker run --rm \
		-v "$(CURDIR)/backend:/app" \
		-v gradle-cache:/root/.gradle \
		-w /app \
		gradle:8-jdk21 \
		gradle test

.PHONY: all build up down clean fclean re logs ps test
