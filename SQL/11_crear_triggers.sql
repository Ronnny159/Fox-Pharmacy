-- ============================================
-- PharmaSmart - Script 11
-- Creación de Triggers
-- Ejecutar como PHARMA_USER
-- ============================================

SET SERVEROUTPUT ON;
SET ECHO ON;

-- Trigger de alerta de inflación
CREATE OR REPLACE TRIGGER TRG_ALERTA_INFLACION
    BEFORE INSERT ON LOTE
    FOR EACH ROW
DECLARE
    v_precio_anterior NUMBER(10,2);
    v_incremento      NUMBER(5,2);
    v_umbral          NUMBER(5,2) := 5.00;
    v_codigo_producto VARCHAR2(50);
    v_umbral_str      VARCHAR2(50);
BEGIN
    BEGIN
        SELECT VALOR INTO v_umbral_str
        FROM PARAMETRO_SISTEMA
        WHERE CLAVE = 'UMBRAL_ALERTA_INFLACION' AND ACTIVO = 1;
        v_umbral := TO_NUMBER(TRIM(v_umbral_str));
    EXCEPTION
        WHEN NO_DATA_FOUND THEN v_umbral := 5.00;
        WHEN VALUE_ERROR THEN v_umbral := 5.00;
    END;
    
    BEGIN
        SELECT PRECIO_COMPRA INTO v_precio_anterior
        FROM LOTE
        WHERE PRODUCTO_ID = :NEW.PRODUCTO_ID
          AND ACTIVO = 1 AND ESTADO IN (1, 2)
          AND ROWNUM = 1
        ORDER BY FECHA_CREACION DESC;
        
        IF v_precio_anterior IS NOT NULL AND v_precio_anterior > 0 THEN
            v_incremento := ((:NEW.PRECIO_COMPRA - v_precio_anterior) / v_precio_anterior) * 100;
            
            IF v_incremento >= v_umbral THEN
                BEGIN
                    SELECT CODIGO INTO v_codigo_producto FROM PRODUCTO WHERE ID = :NEW.PRODUCTO_ID;
                    
                    INSERT INTO ALERTA_INFLACION (PRODUCTO_ID, CODIGO_PRODUCTO, PRECIO_ANTERIOR,
                                                 PRECIO_NUEVO, PORCENTAJE_INCREMENTO)
                    VALUES (:NEW.PRODUCTO_ID, v_codigo_producto, v_precio_anterior,
                            :NEW.PRECIO_COMPRA, ROUND(v_incremento, 2));
                EXCEPTION
                    WHEN NO_DATA_FOUND THEN NULL;
                END;
            END IF;
        END IF;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN NULL;
        WHEN VALUE_ERROR THEN NULL;
    END;
END;
/

-- Trigger de verificación de vencimiento
CREATE OR REPLACE TRIGGER TRG_VERIFICAR_VENCIMIENTO
    BEFORE INSERT OR UPDATE ON LOTE
    FOR EACH ROW
BEGIN
    IF :NEW.FECHA_VENCIMIENTO <= :NEW.FECHA_FABRICACION THEN
        RAISE_APPLICATION_ERROR(-20005, 'La fecha de vencimiento debe ser posterior a la de fabricacion.');
    END IF;
    
    IF :NEW.FECHA_VENCIMIENTO <= SYSDATE THEN
        :NEW.ESTADO := 3;
    END IF;
END;
/

-- Trigger de auditoría de usuario
CREATE OR REPLACE TRIGGER TRG_BI_AUDITORIA_USUARIO
    BEFORE INSERT ON USUARIO
    FOR EACH ROW
BEGIN
    IF :NEW.FECHA_CREACION IS NULL THEN
        :NEW.FECHA_CREACION := SYSDATE;
    END IF;
END;
/

COMMIT;

SELECT 'Triggers creados: 3' AS MENSAJE FROM DUAL;
SELECT TRIGGER_NAME, STATUS FROM USER_TRIGGERS ORDER BY TRIGGER_NAME;

EXIT;
