package common.services.user;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import java.util.Optional;


// @Repository marks this as a Spring bean
// JpaRepository gives you free CRUD methods (save, findById, delete, etc.)
// The two type parameters are: the entity type and the ID type
@Repository
public interface UserRepository extends JpaRepository<User, Integer> {

    // Spring Data reads the method name and generates the query automatically
    // "findBy" + "Email" → SELECT * FROM users WHERE email = ?
    Optional<User> findByEmail(String email);
}