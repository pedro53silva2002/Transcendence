package backend.common.query;

import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;
import java.lang.annotation.ElementType;

/**
* It's a marker you put on Java fields to tell the system "this field contains JSON in the database."
*/
@Retention(RetentionPolicy.RUNTIME)
@Target(ElementType.FIELD)
public @interface JsonColumn {
    Class<?> type();
}