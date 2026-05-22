-- ============================================
-- PharmaSmart - Script 10
-- Package PKG_PHARMASMART_BI
-- Business Intelligence
-- Ejecutar como PHARMA_USER
-- ============================================

SET SERVEROUTPUT ON;
SET ECHO ON;

-- Especificación del Package
CREATE OR REPLACE PACKAGE PKG_PHARMASMART_BI AS
    
    -- Top productos más/menos vendidos
    FUNCTION OBTENER_TOP_PRODUCTOS(p_top NUMBER DEFAULT 10, p_orden VARCHAR2 DEFAULT 'MAYOR') 
        RETURN T_TOP_PRODUCTO_TABLA PIPELINED;
    
    -- Resumen de ventas por período
    PROCEDURE RESUMEN_VENTAS(p_desde DATE, p_hasta DATE, p_cursor OUT SYS_REFCURSOR);
    
    -- Productos próximos a vencer
    PROCEDURE PRODUCTOS_POR_VENCER(p_dias NUMBER DEFAULT 30, p_cursor OUT SYS_REFCURSOR);
    
    -- Resumen de inventario actual
    PROCEDURE RESUMEN_INVENTARIO(p_cursor OUT SYS_REFCURSOR);
    
END PKG_PHARMASMART_BI;
/

-- Cuerpo del Package
CREATE OR REPLACE PACKAGE BODY PKG_PHARMASMART_BI AS

    FUNCTION OBTENER_TOP_PRODUCTOS(p_top NUMBER DEFAULT 10, p_orden VARCHAR2 DEFAULT 'MAYOR') 
        RETURN T_TOP_PRODUCTO_TABLA PIPELINED IS
    BEGIN
        IF p_orden = 'MAYOR' THEN
            FOR rec IN (
                SELECT p.CODIGO, p.NOMBRE, SUM(dv.CANTIDAD) AS UNIDADES,
                       SUM(dv.CANTIDAD * dv.PRECIO_APLICADO) AS TOTAL
                FROM DETALLE_VENTA dv
                JOIN LOTE l ON dv.LOTE_ID = l.ID
                JOIN PRODUCTO p ON l.PRODUCTO_ID = p.ID
                JOIN VENTA v ON dv.VENTA_ID = v.ID
                WHERE v.ANULADA = 0
                GROUP BY p.CODIGO, p.NOMBRE
                ORDER BY SUM(dv.CANTIDAD) DESC
                FETCH FIRST p_top ROWS ONLY
            ) LOOP
                PIPE ROW (T_TOP_PRODUCTO(rec.CODIGO, rec.NOMBRE, rec.UNIDADES, rec.TOTAL));
            END LOOP;
        ELSE
            FOR rec IN (
                SELECT p.CODIGO, p.NOMBRE, SUM(dv.CANTIDAD) AS UNIDADES,
                       SUM(dv.CANTIDAD * dv.PRECIO_APLICADO) AS TOTAL
                FROM DETALLE_VENTA dv
                JOIN LOTE l ON dv.LOTE_ID = l.ID
                JOIN PRODUCTO p ON l.PRODUCTO_ID = p.ID
                JOIN VENTA v ON dv.VENTA_ID = v.ID
                WHERE v.ANULADA = 0
                GROUP BY p.CODIGO, p.NOMBRE
                ORDER BY SUM(dv.CANTIDAD) ASC
                FETCH FIRST p_top ROWS ONLY
            ) LOOP
                PIPE ROW (T_TOP_PRODUCTO(rec.CODIGO, rec.NOMBRE, rec.UNIDADES, rec.TOTAL));
            END LOOP;
        END IF;
    END;

    PROCEDURE RESUMEN_VENTAS(p_desde DATE, p_hasta DATE, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT TO_CHAR(FECHA_VENTA, 'DD/MM/YYYY') AS FECHA,
                   COUNT(*) AS TOTAL_VENTAS,
                   SUM(TOTAL) AS TOTAL_FACTURADO,
                   SUM(DESCUENTO_TOTAL) AS TOTAL_DESCUENTOS
            FROM VENTA
            WHERE FECHA_VENTA BETWEEN p_desde AND p_hasta AND ANULADA = 0
            GROUP BY TO_CHAR(FECHA_VENTA, 'DD/MM/YYYY'), TRUNC(FECHA_VENTA)
            ORDER BY TRUNC(FECHA_VENTA) DESC;
    END;

    PROCEDURE PRODUCTOS_POR_VENCER(p_dias NUMBER DEFAULT 30, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT l.CODIGO_LOTE, p.NOMBRE AS PRODUCTO, l.FECHA_VENCIMIENTO,
                   l.CANTIDAD_ACTUAL,
                   TRUNC(l.FECHA_VENCIMIENTO - SYSDATE) AS DIAS_RESTANTES
            FROM LOTE l
            JOIN PRODUCTO p ON l.PRODUCTO_ID = p.ID
            WHERE l.ESTADO = 1 AND l.CANTIDAD_ACTUAL > 0
              AND l.FECHA_VENCIMIENTO BETWEEN SYSDATE AND SYSDATE + p_dias
            ORDER BY l.FECHA_VENCIMIENTO ASC;
    END;

    PROCEDURE RESUMEN_INVENTARIO(p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT p.CODIGO, p.NOMBRE, 
                   COUNT(l.ID) AS TOTAL_LOTES,
                   SUM(l.CANTIDAD_ACTUAL) AS STOCK_TOTAL,
                   SUM(l.CANTIDAD_ACTUAL * l.PRECIO_VENTA) AS VALOR_INVENTARIO
            FROM PRODUCTO p
            LEFT JOIN LOTE l ON p.ID = l.PRODUCTO_ID AND l.ESTADO = 1 AND l.ACTIVO = 1
            WHERE p.ACTIVO = 1
            GROUP BY p.CODIGO, p.NOMBRE
            ORDER BY p.NOMBRE;
    END;

END PKG_PHARMASMART_BI;
/

COMMIT;

SELECT 'Package BI creado: PKG_PHARMASMART_BI' AS MENSAJE FROM DUAL;

EXIT;