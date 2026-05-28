-- ============================================
-- PharmaSmart - Script 06 (CORREGIDO)
-- Package PKG_PHARMASMART_FEFO
-- Ejecutar como PHARMA_USER
-- ============================================

SET SERVEROUTPUT ON;
SET ECHO ON;

CREATE OR REPLACE PACKAGE PKG_PHARMASMART_FEFO AS
    
    FUNCTION OBTENER_LOTE_POR_ID(p_id_lote NUMBER) RETURN T_LOTE_INFO;
    
    PROCEDURE OBTENER_LOTES_POR_PRODUCTO(p_id_producto NUMBER, p_cursor OUT SYS_REFCURSOR);
    
    PROCEDURE OBTENER_LOTES_ACTIVOS(p_cursor OUT SYS_REFCURSOR);
    
    FUNCTION SELECCIONAR_FEFO(p_id_producto NUMBER) RETURN T_LOTE_INFO;
    
    PROCEDURE INSERTAR_LOTE(
        p_codigo_lote VARCHAR2,
        p_id_producto NUMBER,
        p_fecha_fabricacion DATE,
        p_fecha_vencimiento DATE,
        p_precio_compra NUMBER,
        p_precio_venta NUMBER,
        p_cantidad_inicial NUMBER
    );
    
    PROCEDURE ACTUALIZAR_LOTE(
        p_id_lote NUMBER,
        p_cantidad_actual NUMBER,
        p_estado CHAR
    );
    
    PROCEDURE ACTUALIZAR_STOCK(
        p_id_lote NUMBER,
        p_cantidad NUMBER,
        p_estado CHAR
    );
    
END PKG_PHARMASMART_FEFO;
/

CREATE OR REPLACE PACKAGE BODY PKG_PHARMASMART_FEFO AS

    FUNCTION OBTENER_LOTE_POR_ID(p_id_lote NUMBER) RETURN T_LOTE_INFO IS
        v_result T_LOTE_INFO;
    BEGIN
        SELECT T_LOTE_INFO(ID_LOTE, CODIGO_LOTE, ID_PRODUCTO, FECHA_VENCIMIENTO,
                          PRECIO_COMPRA, PRECIO_VENTA, CANTIDAD_ACTUAL, ESTADO)
        INTO v_result FROM LOTES WHERE ID_LOTE = p_id_lote AND ROWNUM = 1;
        RETURN v_result;
    EXCEPTION WHEN NO_DATA_FOUND THEN RETURN NULL;
    END;

    PROCEDURE OBTENER_LOTES_POR_PRODUCTO(p_id_producto NUMBER, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT ID_LOTE, CODIGO_LOTE, ID_PRODUCTO, FECHA_FABRICACION, FECHA_VENCIMIENTO,
                   PRECIO_COMPRA, PRECIO_VENTA, CANTIDAD_ACTUAL, CANTIDAD_INICIAL, ESTADO
            FROM LOTES WHERE ID_PRODUCTO = p_id_producto AND ESTADO = 'A'
            ORDER BY FECHA_VENCIMIENTO ASC;
    END;

    PROCEDURE OBTENER_LOTES_ACTIVOS(p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT ID_LOTE, CODIGO_LOTE, ID_PRODUCTO, FECHA_FABRICACION, FECHA_VENCIMIENTO,
                   PRECIO_COMPRA, PRECIO_VENTA, CANTIDAD_ACTUAL, CANTIDAD_INICIAL, ESTADO
            FROM LOTES WHERE ESTADO = 'A' ORDER BY FECHA_VENCIMIENTO ASC;
    END;

    FUNCTION SELECCIONAR_FEFO(p_id_producto NUMBER) RETURN T_LOTE_INFO IS
        v_result T_LOTE_INFO;
    BEGIN
        SELECT T_LOTE_INFO(ID_LOTE, CODIGO_LOTE, ID_PRODUCTO, FECHA_VENCIMIENTO,
                          PRECIO_COMPRA, PRECIO_VENTA, CANTIDAD_ACTUAL, ESTADO)
        INTO v_result
        FROM (
            SELECT ID_LOTE, CODIGO_LOTE, ID_PRODUCTO, FECHA_VENCIMIENTO,
                   PRECIO_COMPRA, PRECIO_VENTA, CANTIDAD_ACTUAL, ESTADO
            FROM LOTES
            WHERE ID_PRODUCTO = p_id_producto AND CANTIDAD_ACTUAL > 0
              AND ESTADO = 'A' AND FECHA_VENCIMIENTO > SYSDATE
            ORDER BY FECHA_VENCIMIENTO ASC, PRECIO_COMPRA ASC
        ) WHERE ROWNUM = 1;
        RETURN v_result;
    EXCEPTION WHEN NO_DATA_FOUND THEN RETURN NULL;
    END;

    PROCEDURE INSERTAR_LOTE(
        p_codigo_lote VARCHAR2, p_id_producto NUMBER, p_fecha_fabricacion DATE,
        p_fecha_vencimiento DATE, p_precio_compra NUMBER, p_precio_venta NUMBER,
        p_cantidad_inicial NUMBER
    ) IS
    BEGIN
        INSERT INTO LOTES (CODIGO_LOTE, ID_PRODUCTO, FECHA_FABRICACION, FECHA_VENCIMIENTO,
                          PRECIO_COMPRA, PRECIO_VENTA, CANTIDAD_ACTUAL, CANTIDAD_INICIAL, ESTADO)
        VALUES (p_codigo_lote, p_id_producto, p_fecha_fabricacion, p_fecha_vencimiento,
                p_precio_compra, p_precio_venta, p_cantidad_inicial, p_cantidad_inicial, 'A');
        COMMIT;
    END;

    PROCEDURE ACTUALIZAR_LOTE(p_id_lote NUMBER, p_cantidad_actual NUMBER, p_estado CHAR) IS
    BEGIN
        UPDATE LOTES SET CANTIDAD_ACTUAL = p_cantidad_actual, ESTADO = p_estado WHERE ID_LOTE = p_id_lote;
        COMMIT;
    END;

    PROCEDURE ACTUALIZAR_STOCK(p_id_lote NUMBER, p_cantidad NUMBER, p_estado CHAR) IS
    BEGIN
        UPDATE LOTES SET CANTIDAD_ACTUAL = p_cantidad, ESTADO = p_estado WHERE ID_LOTE = p_id_lote;
        COMMIT;
    END;

END PKG_PHARMASMART_FEFO;
/

COMMIT;
PROMPT Package FEFO creado.
EXIT;