package backend.common.query;

import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.jdbc.core.RowMapper;

import java.lang.reflect.Field;
import java.lang.reflect.Method;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Timestamp;
import java.sql.Date;
import java.sql.Time;
import java.time.Instant;
import java.time.LocalDate;
import java.time.LocalTime;
import java.math.BigDecimal;
import java.util.HashMap;
import java.util.Map;
import java.util.UUID;
import java.util.concurrent.ConcurrentHashMap;


/**
 * It converts database rows to Java objects automatically.
 * example:
 * | user_id | user_name | created_at       |
 * | ------- | --------- | ---------------- |
 * | 123     | m_miguelo | 2026-04-30 13:30 |
 *
 * class User {
 *     int userId;
 *     String userName;
 *     Instant createdAt;
 * }
 *
 */
public class ReflectiveRowMapper<T> implements RowMapper<T> {
    private final Class<T> type; //which java class to map
    private final ObjectMapper objectMapper; // Jackson library tool to convert JSON strings
    private final Map<String, Field> columnToField; // camelCase -> Field

    // Global cache to avoid re-discovering fields on every row
    private static final Map<Class<?>, Map<String, Field>> CACHE = new ConcurrentHashMap<>();

    public ReflectiveRowMapper(Class<T> type, ObjectMapper objectMapper) {
        this.type = type;
        this.objectMapper = objectMapper;
        this.columnToField = CACHE.computeIfAbsent(type, ReflectiveRowMapper::discoverFields);
    }

    // uses reflection to allow the program to work with structure of objects at runtime dynamically,
    // instead at compile time.
    // Class<?> represents a Java class at runtime
    private static Map<String, Field> discoverFields(Class<?> cls) {
        Map<String, Field> map = new HashMap<>();
        Class<?> cur = cls;
        // goes up the inheritance chain (example user -> person -> object)
        while (cur != null && cur != Object.class)
        {
            // get all fields declared in this class
            for (Field f : cur.getDeclaredFields())
            {
                //this allow us to access private fields (by reflection)
                f.setAccessible(true);
                //Cache the field by its name
                map.putIfAbsent(f.getName(), f);
            }
            cur = cur.getSuperclass();
        }
        return map;
    }

    private static String toCamelCase(String snake) {
        if (snake == null)
            return null;
        String[] parts = snake.split("_");
        // user_name → ["user", "name"]
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < parts.length; i++) {
            String p = parts[i];
            if (p.isEmpty())
                continue;  // skip
            if (i == 0)
                sb.append(p.toLowerCase());  // first part: lowercase
            else {
                char firstChar = Character.toUpperCase(p.charAt(0));
                String rest = p.substring(1).toLowerCase();

                sb.append(firstChar);
                sb.append(rest);
            }
            // other parts: capitalize first letter, rest lowercase
        }
        return sb.toString();
        // Result: user_name → userName
    }

    //this function is called by spring for each row the database returns.
    // it returns a Java object
    @Override
    public T mapRow(ResultSet rs, int rowNum) throws SQLException {
        try {
            T instance = type.getDeclaredConstructor().newInstance();
            var meta = rs.getMetaData();
            int colCount = meta.getColumnCount();
            for (int i = 1; i <= colCount; i++) {
                String colLabel = meta.getColumnLabel(i);
                String camel = toCamelCase(colLabel);
                Field f = columnToField.get(camel);
                if (f == null)
                    continue; // silently skip missing fields that are in DB but not in Java class

                Class<?> fieldType = f.getType();
                Object value = null;

                // handle JsonColumn annotation
                JsonColumn jsonAnn = f.getAnnotation(JsonColumn.class);
                if (jsonAnn != null) {
                    String json = rs.getString(i);
                    if (json != null) {
                        try {
                            value = objectMapper.readValue(json, objectMapper.getTypeFactory().constructType(jsonAnn.type()));
                        }
                        catch (Exception e) {
                            throw new DataMappingException("Failed to parse JSON for field " + f.getName() + " column " + colLabel, e);
                        }
                    }
                    setFieldValue(f, instance, value);
                    continue;
                }

                // primitives and boxed types
                if (fieldType == String.class) {
                    value = rs.getString(i);
                }
                else if (fieldType == UUID.class) {
                    String s = rs.getString(i);
                    value = s == null ? null : UUID.fromString(s);
                }
                else if (fieldType == int.class || fieldType == Integer.class) {
                    int v = rs.getInt(i);
                    value = rs.wasNull() ? (fieldType == int.class ? 0 : null) : v;
                }
                else if (fieldType == long.class || fieldType == Long.class) {
                    long v = rs.getLong(i);
                    value = rs.wasNull() ? (fieldType == long.class ? 0L : null) : v;
                }
                else if (fieldType == boolean.class || fieldType == Boolean.class) {
                    boolean v = rs.getBoolean(i);
                    value = rs.wasNull() ? (fieldType == boolean.class ? false : null) : v;
                }
                else if (fieldType == double.class || fieldType == Double.class) {
                    double v = rs.getDouble(i);
                    value = rs.wasNull() ? (fieldType == double.class ? 0.0 : null) : v;
                }
                else if (fieldType == BigDecimal.class) {
                    value = rs.getBigDecimal(i);
                }
                else if (fieldType == Instant.class) {
                    Timestamp ts = rs.getTimestamp(i);
                    value = ts == null ? null : ts.toInstant();
                }
                else if (fieldType == LocalDate.class) {
                    Date d = rs.getDate(i);
                    value = d == null ? null : d.toLocalDate();
                }
                else if (fieldType == LocalTime.class) {
                    Time t = rs.getTime(i);
                    value = t == null ? null : t.toLocalTime();
                }
                else if (fieldType.isEnum()) {
                    String s = rs.getString(i);
                    if (s != null) {
                        try {
                            @SuppressWarnings({ "unchecked", "rawtypes" })
                            Enum<?> e = Enum.valueOf((Class<Enum>) fieldType, s);
                            value = e;
                        }
                        catch (IllegalArgumentException iae) {
                            throw new DataMappingException("Invalid enum value '" + s + "' for field " + f.getName() + " column " + colLabel, iae);
                        }
                    }
                    else {
                        value = null;
                    }
                }
                else {
                    // fallback: try to get object and attempt direct assignment
                    value = rs.getObject(i);
                }

                setFieldValue(f, instance, value);
            }
            return instance;
        }
        catch (DataMappingException e) {
            throw e;
        }
        catch (Exception e) {
            throw new SQLException("Failed to map row to " + type.getName(), e);
        }
    }

    private void setFieldValue(Field field, T instance, Object value) throws IllegalAccessException {
        Class<?> fieldType = field.getType();
        if (value == null && fieldType.isPrimitive()) {
            value = defaultPrimitiveValue(fieldType);
        }

        Method setter = findSetter(instance.getClass(), field.getName(), fieldType);
        if (setter != null) {
            try {
                setter.invoke(instance, value);
                return;
            }
            catch (Exception e) {
                throw new DataMappingException("Failed to set field " + field.getName(), e);
            }
        }

        field.setAccessible(true);
        field.set(instance, value);
    }

    private Method findSetter(Class<?> cls, String fieldName, Class<?> fieldType) {
        String setterName = "set" + Character.toUpperCase(fieldName.charAt(0)) + fieldName.substring(1);
        try {
            return cls.getMethod(setterName, fieldType);
        }
        catch (NoSuchMethodException e) {
            Class<?> altType = alternateType(fieldType);
            if (altType != null) {
                try {
                    return cls.getMethod(setterName, altType);
                }
                catch (NoSuchMethodException ignored) {
                }
            }
            return null;
        }
    }

    private Class<?> alternateType(Class<?> fieldType) {
        if (fieldType == null) {
            return null;
        }
        if (fieldType == int.class) {
            return Integer.class;
        }
        if (fieldType == Integer.class) {
            return int.class;
        }
        if (fieldType == long.class) {
            return Long.class;
        }
        if (fieldType == Long.class) {
            return long.class;
        }
        if (fieldType == boolean.class) {
            return Boolean.class;
        }
        if (fieldType == Boolean.class) {
            return boolean.class;
        }
        if (fieldType == double.class) {
            return Double.class;
        }
        if (fieldType == Double.class) {
            return double.class;
        }
        if (fieldType == float.class) {
            return Float.class;
        }
        if (fieldType == Float.class) {
            return float.class;
        }
        if (fieldType == short.class) {
            return Short.class;
        }
        if (fieldType == Short.class) {
            return short.class;
        }
        if (fieldType == byte.class) {
            return Byte.class;
        }
        if (fieldType == Byte.class) {
            return byte.class;
        }
        if (fieldType == char.class) {
            return Character.class;
        }
        if (fieldType == Character.class) {
            return char.class;
        }
        return null;
    }

    private Object defaultPrimitiveValue(Class<?> fieldType) {
        if (fieldType == boolean.class) {
            return false;
        }
        if (fieldType == byte.class) {
            return (byte) 0;
        }
        if (fieldType == short.class) {
            return (short) 0;
        }
        if (fieldType == int.class) {
            return 0;
        }
        if (fieldType == long.class) {
            return 0L;
        }
        if (fieldType == float.class) {
            return 0f;
        }
        if (fieldType == double.class) {
            return 0d;
        }
        if (fieldType == char.class) {
            return '\u0000';
        }
        return null;
    }
}