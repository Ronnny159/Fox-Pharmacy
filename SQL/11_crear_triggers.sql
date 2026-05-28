-- ============================================
-- PharmaSmart - Script 11 (CORREGIDO)
-- Creación de Triggers
-- Ejecutar como PHARMA_USER
-- ============================================

SET SERVEROUTPUT ON;
SET ECHO ON;

PROMPT Creando triggers...

-- Trigger de alerta de inflación
CREATE OR REPLACE TRIGGER TRG_ALERTA_INFLACION
    BEFORE INSERT ON LOTES
    FOR EACH ROW
DECLARE
    v_precio_anterior NUMBER(10,2);
    v_incremento NUMBER(5,2);
    v_umbral NUMBER(5,2) := 5.00;
    v_codigo_producto VARCHAR2(15);
    v_umbral_str VARCHAR2(30);
BEGIN
    BEGIN SELECT VALOR INTO v_umbral_str FROM PARAMETROS_SISTEMA WHERE CLAVE = 'UMBRAL_ALERTA_INFLACION';
        v_umbral := TO_NUMBER(TRIM(v_umbral_str)); EXCEPTION WHEN NO_DATA_FOUND THEN v_umbral := 5.00; END;
    BEGIN SELECT PRECIO_COMPRA INTO v_precio_anterior FROM LOTES
        WHERE ID_PRODUCTO = :NEW.ID_PRODUCTO AND ESTADO IN ('A','B') AND ROWNUM = 1 ORDER BY FECHA_CREACION DESC;
        IF v_precio_anterior > 0 THEN v_incremento := ((:NEW.PRECIO_COMPRA - v_precio_anterior) / v_precio_anterior) * 100;
            IF v_incremento >= v_umbral THEN SELECT CODIGO INTO v_codigo_producto FROM PRODUCTOS WHERE ID_PRODUCTO = :NEW.ID_PRODUCTO;
                INSERT INTO ALERTAS_INFLACION (ID_PRODUCTO, CODIGO_PRODUCTO, PRECIO_ANTERIOR, PRECIO_NUEVO, PORCENTAJE_INCREMENTO)
                VALUES (:NEW.ID_PRODUCTO, v_codigo_producto, v_precio_anterior, :NEW.PRECIO_COMPRA, ROUND(v_incremento, 2)); END IF; END IF;
    EXCEPTION WHEN NO_DATA_FOUND THEN NULL; END;
END;
/

-- Trigger de verificación de vencimiento
CREATE OR REPLACE TRIGGER TRG_VERIFICAR_VENCIMIENTO
    BEFORE INSERT OR UPDATE ON LOTES
    FOR EACH ROW
BEGIN
    IF :NEW.FECHA_VENCIMIENTO <= :NEW.FECHA_FABRICACION THEN
        RAISE_APPLICATION_ERROR(-20005, 'Fecha de vencimiento debe ser posterior a fabricacion.'); END IF;
    IF :NEW.FECHA_VENCIMIENTO <= SYSDATE THEN :NEW.ESTADO := 'V'; END IF;
END;
/

COMMIT;
PROMPT Triggers creados: 2
EXIT;