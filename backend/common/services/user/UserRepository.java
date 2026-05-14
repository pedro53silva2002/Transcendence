package common.services.user;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.Optional;
import java.util.UUID;

/**
 * Data access layer for the User entity.
 *
 * This interface is the only thing needed to interact with the "users" table.
 * We don't write any SQL or implementation code — Spring Data JPA handles everything.
 *
 * How it works:
 *   - By extending JpaRepository, Spring automatically generates a full implementation
 *     of this interface at startup, including all standard CRUD operations.
 *   - For custom queries like findByEmail(), Spring Data reads the method name and
 *     generates the SQL automatically — no @Query annotation needed for simple lookups.
 *
 * JpaRepository type parameters:
 *   - User → the entity class this repository manages
 *   - UUID → the type of User's primary key (@Id field)
 *
 * Free methods inherited from JpaRepository (examples):
 *   - save(user)          → INSERT or UPDATE
 *   - findById(id)        → SELECT WHERE id = ?
 *   - delete(user)        → DELETE
 *   - findAll()           → SELECT all rows
 *   - existsById(id)      → SELECT EXISTS WHERE id = ?
 */
@Repository // Registers this interface as a Spring bean and enables exception translation
// (converts database exceptions into Spring's DataAccessException hierarchy)
public interface UserRepository extends JpaRepository<User, UUID> {

    /**
     * Finds a user by their email address.
     *
     * Spring Data JPA reads the method name and automatically generates:
     *   SELECT * FROM auth.users WHERE email = ?
     *
     * The return type is Optional<User> — meaning the result might or might not exist.
     * The caller must handle both cases (user found vs. user not found) without
     * risking a NullPointerException.
     *
     * Used by UserDetailsServiceImpl during login to look up the user by the
     * email they submitted.
     *
     * @param email the email address to search for
     * @return Optional containing the User if found, or empty Optional if not found
     */
    Optional<User> findByEmail(String email);
}