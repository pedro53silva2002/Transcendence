package com.transcendence.common.search;

public enum FilterOperator {
    EQ,          // coluna = :param
    NEQ,         // coluna <> :param
    GT,          // coluna > :param
    GTE,         // coluna >= :param
    LT,          // coluna < :param
    LTE,         // coluna <= :param
    BEFORE,      // coluna < :param      (alias semântico de LT para datas)
    AFTER,       // coluna > :param      (alias semântico de GT para datas)
    BETWEEN,     // coluna BETWEEN :param1 AND :param2        — ranges (dates, numbers)
    CONTAINS,    // coluna ILIKE :param  (com % wrapping)
    STARTS_WITH, // coluna ILIKE :param%                      — prefix search
    ENDS_WITH,   // coluna ILIKE %:param                      — suffix search
    IS_NULL,     // coluna IS NULL       (sem parâmetro)
    IS_NOT_NULL, // coluna IS NOT NULL   (sem parâmetro)
    IN,          // coluna = ANY(:param) (para listas)
    NOT_IN       // coluna <> ALL(:param) or NOT IN (:param)  — complement of IN
}