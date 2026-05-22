-- ============================================
-- PharmaSmart - Script 04
-- Creación de Secuencias
-- Ejecutar como PHARMA_USER
-- ============================================

SET SERVEROUTPUT ON;
SET ECHO ON;

CREATE SEQUENCE SEQ_NUMERO_FACTURA
    START WITH 1000
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;

COMMIT;

SELECT 'Secuencia creada: SEQ_NUMERO_FACTURA' AS MENSAJE FROM DUAL;
SELECT SEQUENCE_NAME, MIN_VALUE, MAX_VALUE, INCREMENT_BY FROM USER_SEQUENCES;

EXIT;