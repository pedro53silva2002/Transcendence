FROM gradle:8-jdk21 AS builder
 
WORKDIR /app
COPY . .
RUN gradle clean build -x test
 
FROM eclipse-temurin:21-jre
 
WORKDIR /app
COPY --from=builder /app/build/libs/*.jar app.jar
 
EXPOSE 5024
CMD ["java", "-jar", "app.jar"]