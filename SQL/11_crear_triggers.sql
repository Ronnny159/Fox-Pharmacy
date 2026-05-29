-- ============================================
-- PharmaSmart - Script 11 (CORREGIDO)
-- Creación de Triggers (se crean DESHABILITADOS)
-- Ejecutar como PHARMA_USER
-- ============================================

SET SERVEROUTPUT ON;
SET ECHO ON;

PROMPT ========================================
PROMPT Creando triggers (deshabilitados)...
PROMPT ========================================

-- ============================================
-- Trigger de alerta de inflación
-- ============================================
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
    -- Obtener umbral desde parámetros
    BEGIN
        SELECT VALOR INTO v_umbral_str
        FROM PARAMETROS_SISTEMA
        WHERE CLAVE = 'UMBRAL_ALERTA_INFLACION';
        v_umbral := TO_NUMBER(TRIM(v_umbral_str));
    EXCEPTION
        WHEN NO_DATA_FOUND THEN v_umbral := 5.00;
        WHEN VALUE_ERROR THEN v_umbral := 5.00;
    END;

    -- Buscar el precio del lote anterior del mismo producto
    BEGIN
        SELECT PRECIO_COMPRA INTO v_precio_anterior
        FROM (
            SELECT PRECIO_COMPRA
            FROM LOTES
            WHERE ID_PRODUCTO = :NEW.ID_PRODUCTO
              AND ESTADO IN ('A', 'B')
            ORDER BY ID_LOTE DESC
        )
        WHERE ROWNUM = 1;

        IF v_precio_anterior IS NOT NULL AND v_precio_anterior > 0 THEN
            v_incremento := ((:NEW.PRECIO_COMPRA - v_precio_anterior) / v_precio_anterior) * 100;

            IF v_incremento >= v_umbral THEN
                SELECT CODIGO INTO v_codigo_producto
                FROM PRODUCTOS
                WHERE ID_PRODUCTO = :NEW.ID_PRODUCTO;

                INSERT INTO ALERTAS_INFLACION (
                    ID_PRODUCTO, CODIGO_PRODUCTO, PRECIO_ANTERIOR,
                    PRECIO_NUEVO, PORCENTAJE_INCREMENTO
                ) VALUES (
                    :NEW.ID_PRODUCTO, v_codigo_producto, v_precio_anterior,
                    :NEW.PRECIO_COMPRA, ROUND(v_incremento, 2)
                );
            END IF;
        END IF;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN NULL;
        WHEN VALUE_ERROR THEN NULL;
    END;
END;
/

-- Deshabilitar inmediatamente después de crear
ALTER TRIGGER TRG_ALERTA_INFLACION DISABLE;

-- ============================================
-- Trigger de verificación de vencimiento
-- ============================================
CREATE OR REPLACE TRIGGER TRG_VERIFICAR_VENCIMIENTO
    BEFORE INSERT OR UPDATE ON LOTES
    FOR EACH ROW
BEGIN
    IF :NEW.FECHA_VENCIMIENTO <= :NEW.FECHA_FABRICACION THEN
        RAISE_APPLICATION_ERROR(-20005, 'Fecha de vencimiento debe ser posterior a fabricacion.');
    END IF;
    
    IF :NEW.FECHA_VENCIMIENTO <= SYSDATE THEN
        :NEW.ESTADO := 'V';
    END IF;
END;
/

-- Este trigger SÍ se deja habilitado porque valida datos
-- (no depende de otras tablas ni genera inserts secundarios)

COMMIT;

PROMPT Triggers creados correctamente.
PROMPT TRG_ALERTA_INFLACION: DESHABILITADO (se activa al final de insertar_datos.sql)
PROMPT TRG_VERIFICAR_VENCIMIENTO: HABILITADO

EXIT;