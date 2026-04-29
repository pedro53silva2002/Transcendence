//package com.transcendence;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

/**
 * Entry point of the Transcendence Spring Boot application.
 *
 * @SpringBootApplication enables:
 *   - @Configuration         → marks this class as a source of bean definitions
 *   - @EnableAutoConfiguration → auto-configures Spring based on classpath
 *   - @ComponentScan         → scans this package and sub-packages for components
 */
@SpringBootApplication
public class Application {

    public static void main(String[] args) {
        SpringApplication.run(Application.class, args);
    }
}
