-- ============================================
-- PharmaSmart - Script 08
-- Package PKG_PHARMASMART_CONFIG
-- Configuración, descuentos y auditoría
-- Ejecutar como PHARMA_USER
-- ============================================

SET SERVEROUTPUT ON;
SET ECHO ON;

-- Especificación del Package
CREATE OR REPLACE PACKAGE PKG_PHARMASMART_CONFIG AS
    
    -- Obtener valor de un parámetro
    FUNCTION OBTENER_PARAMETRO(p_clave VARCHAR2) RETURN VARCHAR2;
    
    -- Actualizar descuento general (solo admin)
    PROCEDURE ACTUALIZAR_DESCUENTO_GENERAL(
        p_nuevo_valor VARCHAR2,
        p_usuario_id NUMBER,
        p_motivo VARCHAR2,
        p_ip VARCHAR2 DEFAULT NULL
    );
    
    -- Actualizar descuento individual de producto
    PROCEDURE ACTUALIZAR_DESCUENTO_PRODUCTO(
        p_producto_id NUMBER,
        p_descuento NUMBER,
        p_usuario_id NUMBER,
        p_motivo VARCHAR2,
        p_ip VARCHAR2 DEFAULT NULL
    );
    
    -- Registrar ajuste de inventario
    PROCEDURE REGISTRAR_AJUSTE(
        p_lote_id NUMBER,
        p_tipo VARCHAR2,
        p_cantidad NUMBER,
        p_motivo VARCHAR2,
        p_responsable_id NUMBER
    );
    
    -- Obtener historial de cambios de parámetros
    PROCEDURE OBTENER_HISTORIAL_PARAMETROS(
        p_clave VARCHAR2,
        p_cursor OUT SYS_REFCURSOR
    );
    
    -- Obtener historial de descuentos por producto
    PROCEDURE OBTENER_HISTORIAL_DESCUENTOS(
        p_producto_id NUMBER,
        p_cursor OUT SYS_REFCURSOR
    );
    
END PKG_PHARMASMART_CONFIG;
/

-- Cuerpo del Package
CREATE OR REPLACE PACKAGE BODY PKG_PHARMASMART_CONFIG AS

    FUNCTION OBTENER_PARAMETRO(p_clave VARCHAR2) RETURN VARCHAR2 IS
        v_valor VARCHAR2(50);
    BEGIN
        SELECT VALOR INTO v_valor
        FROM PARAMETRO_SISTEMA WHERE CLAVE = p_clave AND ACTIVO = 1;
        RETURN v_valor;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN RETURN NULL;
    END;

    PROCEDURE ACTUALIZAR_DESCUENTO_GENERAL(
        p_nuevo_valor VARCHAR2,
        p_usuario_id NUMBER,
        p_motivo VARCHAR2,
        p_ip VARCHAR2 DEFAULT NULL
    ) IS
        v_rol NUMBER;
        v_valor_anterior VARCHAR2(50);
    BEGIN
        SELECT ROL INTO v_rol FROM USUARIO WHERE ID = p_usuario_id;
        IF v_rol != 1 THEN
            RAISE_APPLICATION_ERROR(-20004, 'Solo el administrador puede modificar este parametro.');
        END IF;
        
        SELECT VALOR INTO v_valor_anterior
        FROM PARAMETRO_SISTEMA WHERE CLAVE = 'PORCENTAJE_DESCUENTO_VENCIMIENTO';
        
        UPDATE PARAMETRO_SISTEMA SET VALOR = p_nuevo_valor
        WHERE CLAVE = 'PORCENTAJE_DESCUENTO_VENCIMIENTO';
        
        INSERT INTO HISTORIAL_PARAMETRO (CLAVE_PARAMETRO, VALOR_ANTERIOR, VALOR_NUEVO, 
                                        MOTIVO, MODIFICADO_POR_ID, DIRECCION_IP)
        VALUES ('PORCENTAJE_DESCUENTO_VENCIMIENTO', v_valor_anterior, p_nuevo_valor,
                p_motivo, p_usuario_id, p_ip);
        
        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN ROLLBACK; RAISE;
    END;

    PROCEDURE ACTUALIZAR_DESCUENTO_PRODUCTO(
        p_producto_id NUMBER,
        p_descuento NUMBER,
        p_usuario_id NUMBER,
        p_motivo VARCHAR2,
        p_ip VARCHAR2 DEFAULT NULL
    ) IS
        v_rol NUMBER;
        v_producto PRODUCTO%ROWTYPE;
        v_accion VARCHAR2(20);
    BEGIN
        SELECT ROL INTO v_rol FROM USUARIO WHERE ID = p_usuario_id;
        IF v_rol != 1 THEN
            RAISE_APPLICATION_ERROR(-20004, 'Solo el administrador puede modificar descuentos.');
        END IF;
        
        SELECT * INTO v_producto FROM PRODUCTO WHERE ID = p_producto_id;
        
        IF v_producto.DESCUENTO_PROXIMIDAD_VENCIMIENTO IS NULL AND p_descuento IS NOT NULL THEN
            v_accion := 'CREAR';
        ELSIF v_producto.DESCUENTO_PROXIMIDAD_VENCIMIENTO IS NOT NULL AND p_descuento IS NULL THEN
            v_accion := 'ELIMINAR';
        ELSE
            v_accion := 'MODIFICAR';
        END IF;
        
        INSERT INTO HISTORIAL_DESCUENTO_PRODUCTO (
            PRODUCTO_ID, CODIGO_PRODUCTO, NOMBRE_PRODUCTO,
            DESCUENTO_ANTERIOR, DESCUENTO_NUEVO, ACCION, MOTIVO,
            MODIFICADO_POR_ID, DIRECCION_IP
        ) VALUES (
            p_producto_id, v_producto.CODIGO, v_producto.NOMBRE,
            v_producto.DESCUENTO_PROXIMIDAD_VENCIMIENTO, p_descuento,
            v_accion, p_motivo, p_usuario_id, p_ip
        );
        
        UPDATE PRODUCTO SET DESCUENTO_PROXIMIDAD_VENCIMIENTO = p_descuento
        WHERE ID = p_producto_id;
        
        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN ROLLBACK; RAISE;
    END;

    PROCEDURE REGISTRAR_AJUSTE(
        p_lote_id NUMBER,
        p_tipo VARCHAR2,
        p_cantidad NUMBER,
        p_motivo VARCHAR2,
        p_responsable_id NUMBER
    ) IS
        v_nuevo_estado NUMBER;
        v_cantidad_resultante NUMBER;
    BEGIN
        INSERT INTO AJUSTE_INVENTARIO (LOTE_ID, TIPO, CANTIDAD, MOTIVO, RESPONSABLE_ID)
        VALUES (p_lote_id, p_tipo, p_cantidad, p_motivo, p_responsable_id);
        
        SELECT CANTIDAD_ACTUAL + p_cantidad INTO v_cantidad_resultante
        FROM LOTE WHERE ID = p_lote_id;
        
        IF v_cantidad_resultante <= 0 THEN 
            v_nuevo_estado := 3;
        ELSIF p_tipo = 'Vencido' THEN 
            v_nuevo_estado := 3;
        ELSIF p_tipo = 'RetiroLegal' THEN 
            v_nuevo_estado := 4;
        ELSE 
            SELECT ESTADO INTO v_nuevo_estado FROM LOTE WHERE ID = p_lote_id;
        END IF;
        
        UPDATE LOTE SET CANTIDAD_ACTUAL = v_cantidad_resultante, ESTADO = v_nuevo_estado
        WHERE ID = p_lote_id;
        
        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN ROLLBACK; RAISE;
    END;

    PROCEDURE OBTENER_HISTORIAL_PARAMETROS(
        p_clave VARCHAR2,
        p_cursor OUT SYS_REFCURSOR
    ) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT ID, CLAVE_PARAMETRO, VALOR_ANTERIOR, VALOR_NUEVO, MOTIVO,
                   FECHA_CAMBIO, MODIFICADO_POR_ID, DIRECCION_IP
            FROM HISTORIAL_PARAMETRO WHERE CLAVE_PARAMETRO = p_clave
            ORDER BY FECHA_CAMBIO DESC;
    END;

    PROCEDURE OBTENER_HISTORIAL_DESCUENTOS(
        p_producto_id NUMBER,
        p_cursor OUT SYS_REFCURSOR
    ) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT ID, PRODUCTO_ID, CODIGO_PRODUCTO, NOMBRE_PRODUCTO,
                   DESCUENTO_ANTERIOR, DESCUENTO_NUEVO, ACCION, MOTIVO,
                   FECHA_CAMBIO, MODIFICADO_POR_ID
            FROM HISTORIAL_DESCUENTO_PRODUCTO WHERE PRODUCTO_ID = p_producto_id
            ORDER BY FECHA_CAMBIO DESC;
    END;

END PKG_PHARMASMART_CONFIG;
/

COMMIT;

SELECT 'Package CONFIG creado: PKG_PHARMASMART_CONFIG' AS MENSAJE FROM DUAL;

EXIT;