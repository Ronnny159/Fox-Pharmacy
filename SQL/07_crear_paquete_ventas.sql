-- ============================================
-- PharmaSmart - Script 07
-- Package PKG_PHARMASMART_VENTAS
-- Gestión de ventas y facturación
-- Ejecutar como PHARMA_USER
-- ============================================

SET SERVEROUTPUT ON;
SET ECHO ON;

-- Especificación del Package
CREATE OR REPLACE PACKAGE PKG_PHARMASMART_VENTAS AS
    
    -- Crear una venta completa
    PROCEDURE CREAR_VENTA(
        p_usuario_id NUMBER,
        p_cliente_id NUMBER,
        p_subtotal NUMBER,
        p_descuento_total NUMBER,
        p_total NUMBER,
        p_venta_id OUT NUMBER,
        p_numero_factura OUT VARCHAR2
    );
    
    -- Insertar detalle de venta
    PROCEDURE INSERTAR_DETALLE(
        p_venta_id NUMBER,
        p_lote_id NUMBER,
        p_cantidad NUMBER,
        p_precio_aplicado NUMBER,
        p_descuento_unitario NUMBER
    );
    
    -- Anular venta
    PROCEDURE ANULAR_VENTA(p_venta_id NUMBER, p_usuario_id NUMBER);
    
    -- Obtener venta por ID
    FUNCTION OBTENER_VENTA(p_venta_id NUMBER) RETURN T_VENTA_INFO;
    
    -- Obtener ventas por usuario
    PROCEDURE OBTENER_VENTAS_USUARIO(p_usuario_id NUMBER, p_cursor OUT SYS_REFCURSOR);
    
    -- Obtener ventas por rango de fechas
    PROCEDURE OBTENER_VENTAS_FECHAS(p_desde DATE, p_hasta DATE, p_cursor OUT SYS_REFCURSOR);
    
END PKG_PHARMASMART_VENTAS;
/

-- Cuerpo del Package
CREATE OR REPLACE PACKAGE BODY PKG_PHARMASMART_VENTAS AS

    PROCEDURE CREAR_VENTA(
        p_usuario_id NUMBER,
        p_cliente_id NUMBER,
        p_subtotal NUMBER,
        p_descuento_total NUMBER,
        p_total NUMBER,
        p_venta_id OUT NUMBER,
        p_numero_factura OUT VARCHAR2
    ) IS
        v_numero VARCHAR2(20);
    BEGIN
        v_numero := 'FAC-' || TO_CHAR(SYSDATE, 'YYYYMMDD') || '-' || 
                    LPAD(SEQ_NUMERO_FACTURA.NEXTVAL, 5, '0');
        
        INSERT INTO VENTA (NUMERO_FACTURA, FECHA_VENTA, USUARIO_ID, CLIENTE_ID, 
                           SUBTOTAL, DESCUENTO_TOTAL, TOTAL)
        VALUES (v_numero, SYSDATE, p_usuario_id, p_cliente_id, 
                p_subtotal, p_descuento_total, p_total)
        RETURNING ID INTO p_venta_id;
        
        p_numero_factura := v_numero;
        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN ROLLBACK; RAISE;
    END;

    PROCEDURE INSERTAR_DETALLE(
        p_venta_id NUMBER,
        p_lote_id NUMBER,
        p_cantidad NUMBER,
        p_precio_aplicado NUMBER,
        p_descuento_unitario NUMBER
    ) IS
        v_cantidad_actual NUMBER;
        v_nuevo_estado NUMBER;
    BEGIN
        SELECT CANTIDAD_ACTUAL INTO v_cantidad_actual
        FROM LOTE WHERE ID = p_lote_id FOR UPDATE;
        
        IF v_cantidad_actual < p_cantidad THEN
            RAISE_APPLICATION_ERROR(-20001, 'Stock insuficiente. Disponible: ' || v_cantidad_actual);
        END IF;
        
        INSERT INTO DETALLE_VENTA (VENTA_ID, LOTE_ID, CANTIDAD, PRECIO_APLICADO, DESCUENTO_UNITARIO)
        VALUES (p_venta_id, p_lote_id, p_cantidad, p_precio_aplicado, p_descuento_unitario);
        
        v_nuevo_estado := CASE WHEN (v_cantidad_actual - p_cantidad) = 0 THEN 2 ELSE 1 END;
        
        UPDATE LOTE SET CANTIDAD_ACTUAL = CANTIDAD_ACTUAL - p_cantidad, ESTADO = v_nuevo_estado
        WHERE ID = p_lote_id;
        
        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN ROLLBACK; RAISE;
    END;

    PROCEDURE ANULAR_VENTA(p_venta_id NUMBER, p_usuario_id NUMBER) IS
        v_anulada NUMBER;
        CURSOR c_detalles IS
            SELECT LOTE_ID, CANTIDAD FROM DETALLE_VENTA WHERE VENTA_ID = p_venta_id;
    BEGIN
        SELECT ANULADA INTO v_anulada FROM VENTA WHERE ID = p_venta_id FOR UPDATE;
        
        IF v_anulada = 1 THEN
            RAISE_APPLICATION_ERROR(-20002, 'La venta ya esta anulada.');
        END IF;
        
        FOR v_det IN c_detalles LOOP
            UPDATE LOTE
            SET CANTIDAD_ACTUAL = CANTIDAD_ACTUAL + v_det.CANTIDAD,
                ESTADO = CASE WHEN CANTIDAD_ACTUAL + v_det.CANTIDAD > 0 THEN 1 ELSE ESTADO END
            WHERE ID = v_det.LOTE_ID;
        END LOOP;
        
        UPDATE VENTA SET ANULADA = 1, FECHA_ANULACION = SYSDATE, ANULADA_POR = p_usuario_id
        WHERE ID = p_venta_id;
        
        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN ROLLBACK; RAISE;
    END;

    FUNCTION OBTENER_VENTA(p_venta_id NUMBER) RETURN T_VENTA_INFO IS
        v_result T_VENTA_INFO;
    BEGIN
        SELECT T_VENTA_INFO(ID, NUMERO_FACTURA, FECHA_VENTA, TOTAL, ANULADA)
        INTO v_result FROM VENTA WHERE ID = p_venta_id;
        RETURN v_result;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN RETURN NULL;
    END;

    PROCEDURE OBTENER_VENTAS_USUARIO(p_usuario_id NUMBER, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT ID, NUMERO_FACTURA, FECHA_VENTA, SUBTOTAL, DESCUENTO_TOTAL, TOTAL, ANULADA
            FROM VENTA WHERE USUARIO_ID = p_usuario_id AND ANULADA = 0
            ORDER BY FECHA_VENTA DESC;
    END;

    PROCEDURE OBTENER_VENTAS_FECHAS(p_desde DATE, p_hasta DATE, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT ID, NUMERO_FACTURA, FECHA_VENTA, SUBTOTAL, DESCUENTO_TOTAL, TOTAL, ANULADA
            FROM VENTA WHERE FECHA_VENTA BETWEEN p_desde AND p_hasta AND ANULADA = 0
            ORDER BY FECHA_VENTA DESC;
    END;

END PKG_PHARMASMART_VENTAS;
/

COMMIT;

SELECT 'Package VENTAS creado: PKG_PHARMASMART_VENTAS' AS MENSAJE FROM DUAL;

EXIT;