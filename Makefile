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
	@if [ ! -f backend/gradle/wrapper/gradle-wrapper.jar ]; then \
		cd backend && gradle wrapper; \
	fi
	cd backend && ./gradlew test

.PHONY: all build up down clean fclean re logs ps test
