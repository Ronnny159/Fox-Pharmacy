-- ============================================
-- PharmaSmart - Script 06
-- Package PKG_PHARMASMART_FEFO
-- Inventario y algoritmo FEFO
-- Ejecutar como PHARMA_USER
-- ============================================

SET SERVEROUTPUT ON;
SET ECHO ON;

-- Especificación del Package
CREATE OR REPLACE PACKAGE PKG_PHARMASMART_FEFO AS
    
    -- Obtener lote por ID
    FUNCTION OBTENER_LOTE_POR_ID(p_lote_id NUMBER) RETURN T_LOTE_INFO;
    
    -- Obtener lotes por producto (usando cursor)
    PROCEDURE OBTENER_LOTES_POR_PRODUCTO(p_producto_id NUMBER, p_cursor OUT SYS_REFCURSOR);
    
    -- Obtener todos los lotes activos
    PROCEDURE OBTENER_LOTES_ACTIVOS(p_cursor OUT SYS_REFCURSOR);
    
    -- Seleccionar lote FEFO (el más próximo a vencer)
    FUNCTION SELECCIONAR_FEFO(p_producto_id NUMBER) RETURN T_LOTE_INFO;
    
    -- Insertar nuevo lote
    PROCEDURE INSERTAR_LOTE(
        p_codigo_lote VARCHAR2,
        p_producto_id NUMBER,
        p_fecha_fabricacion DATE,
        p_fecha_vencimiento DATE,
        p_precio_compra NUMBER,
        p_precio_venta NUMBER,
        p_cantidad_inicial NUMBER
    );
    
    -- Actualizar stock de lote
    PROCEDURE ACTUALIZAR_STOCK(
        p_lote_id NUMBER,
        p_cantidad NUMBER,
        p_estado NUMBER
    );
    
END PKG_PHARMASMART_FEFO;
/

-- Cuerpo del Package
CREATE OR REPLACE PACKAGE BODY PKG_PHARMASMART_FEFO AS

    FUNCTION OBTENER_LOTE_POR_ID(p_lote_id NUMBER) RETURN T_LOTE_INFO IS
        v_result T_LOTE_INFO;
    BEGIN
        SELECT T_LOTE_INFO(ID, CODIGO_LOTE, PRODUCTO_ID, FECHA_VENCIMIENTO,
                          PRECIO_COMPRA, PRECIO_VENTA, CANTIDAD_ACTUAL, ESTADO)
        INTO v_result
        FROM LOTE WHERE ID = p_lote_id AND ROWNUM = 1;
        RETURN v_result;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN RETURN NULL;
    END;

    PROCEDURE OBTENER_LOTES_POR_PRODUCTO(p_producto_id NUMBER, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT ID, CODIGO_LOTE, PRODUCTO_ID, FECHA_VENCIMIENTO,
                   PRECIO_COMPRA, PRECIO_VENTA, CANTIDAD_ACTUAL, ESTADO
            FROM LOTE
            WHERE PRODUCTO_ID = p_producto_id AND ACTIVO = 1
            ORDER BY FECHA_VENCIMIENTO ASC;
    END;

    PROCEDURE OBTENER_LOTES_ACTIVOS(p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT ID, CODIGO_LOTE, PRODUCTO_ID, FECHA_VENCIMIENTO,
                   PRECIO_COMPRA, PRECIO_VENTA, CANTIDAD_ACTUAL, ESTADO
            FROM LOTE
            WHERE ACTIVO = 1 AND ESTADO = 1
            ORDER BY FECHA_VENCIMIENTO ASC;
    END;

    FUNCTION SELECCIONAR_FEFO(p_producto_id NUMBER) RETURN T_LOTE_INFO IS
        v_result T_LOTE_INFO;
    BEGIN
        SELECT T_LOTE_INFO(ID, CODIGO_LOTE, PRODUCTO_ID, FECHA_VENCIMIENTO,
                          PRECIO_COMPRA, PRECIO_VENTA, CANTIDAD_ACTUAL, ESTADO)
        INTO v_result
        FROM (
            SELECT ID, CODIGO_LOTE, PRODUCTO_ID, FECHA_VENCIMIENTO,
                   PRECIO_COMPRA, PRECIO_VENTA, CANTIDAD_ACTUAL, ESTADO
            FROM LOTE
            WHERE PRODUCTO_ID = p_producto_id
              AND CANTIDAD_ACTUAL > 0
              AND ESTADO = 1
              AND FECHA_VENCIMIENTO > SYSDATE
            ORDER BY FECHA_VENCIMIENTO ASC, PRECIO_COMPRA ASC
        ) WHERE ROWNUM = 1;
        RETURN v_result;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN RETURN NULL;
    END;

    PROCEDURE INSERTAR_LOTE(
        p_codigo_lote VARCHAR2,
        p_producto_id NUMBER,
        p_fecha_fabricacion DATE,
        p_fecha_vencimiento DATE,
        p_precio_compra NUMBER,
        p_precio_venta NUMBER,
        p_cantidad_inicial NUMBER
    ) IS
    BEGIN
        INSERT INTO LOTE (CODIGO_LOTE, PRODUCTO_ID, FECHA_FABRICACION, FECHA_VENCIMIENTO,
                         PRECIO_COMPRA, PRECIO_VENTA, CANTIDAD_ACTUAL, CANTIDAD_INICIAL, ESTADO)
        VALUES (p_codigo_lote, p_producto_id, p_fecha_fabricacion, p_fecha_vencimiento,
                p_precio_compra, p_precio_venta, p_cantidad_inicial, p_cantidad_inicial, 1);
        COMMIT;
    END;

    PROCEDURE ACTUALIZAR_STOCK(
        p_lote_id NUMBER,
        p_cantidad NUMBER,
        p_estado NUMBER
    ) IS
    BEGIN
        UPDATE LOTE SET CANTIDAD_ACTUAL = p_cantidad, ESTADO = p_estado
        WHERE ID = p_lote_id;
        COMMIT;
    END;

END PKG_PHARMASMART_FEFO;
/

COMMIT;

SELECT 'Package FEFO creado: PKG_PHARMASMART_FEFO' AS MENSAJE FROM DUAL;

EXIT;