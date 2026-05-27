-- ============================================
-- PharmaSmart - Script 09
-- Package PKG_PHARMASMART_ALERTAS
-- Alertas y notificaciones
-- Ejecutar como PHARMA_USER
-- ============================================

SET SERVEROUTPUT ON;
SET ECHO ON;

-- Especificación del Package
CREATE OR REPLACE PACKAGE PKG_PHARMASMART_ALERTAS AS
    
    -- Obtener alertas de inflación pendientes
    PROCEDURE OBTENER_ALERTAS_INFLACION(p_cursor OUT SYS_REFCURSOR);
    
    -- Marcar alerta como atendida
    PROCEDURE MARCAR_ALERTA_ATENDIDA(p_alerta_id NUMBER, p_usuario_id NUMBER);
    
    -- Actualizar lotes vencidos
    PROCEDURE ACTUALIZAR_LOTES_VENCIDOS;
    
    -- Enviar alerta de fidelización
    PROCEDURE ENVIAR_ALERTA_FIDELIZACION(
        p_cliente_id NUMBER,
        p_tipo VARCHAR2,
        p_mensaje VARCHAR2,
        p_producto_id NUMBER DEFAULT NULL,
        p_lote_id NUMBER DEFAULT NULL
    );
    
    -- Obtener alertas por cliente
    PROCEDURE OBTENER_ALERTAS_CLIENTE(p_cliente_id NUMBER, p_cursor OUT SYS_REFCURSOR);
    
    -- Insertar historial de parámetros
    PROCEDURE INSERTAR_HISTORIAL_PARAMETRO(
        p_clave_parametro VARCHAR2,
        p_valor_anterior VARCHAR2,
        p_valor_nuevo VARCHAR2,
        p_motivo VARCHAR2,
        p_modificado_por_id NUMBER,
        p_direccion_ip VARCHAR2 DEFAULT NULL
    );
    
END PKG_PHARMASMART_ALERTAS;
/

-- Cuerpo del Package
CREATE OR REPLACE PACKAGE BODY PKG_PHARMASMART_ALERTAS AS

    PROCEDURE OBTENER_ALERTAS_INFLACION(p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT ai.ID, ai.PRODUCTO_ID, ai.CODIGO_PRODUCTO, p.NOMBRE AS NOMBRE_PRODUCTO,
                   ai.PRECIO_ANTERIOR, ai.PRECIO_NUEVO, ai.PORCENTAJE_INCREMENTO,
                   ai.FECHA_ALERTA, ai.ATENDIDA
            FROM ALERTA_INFLACION ai
            JOIN PRODUCTO p ON ai.PRODUCTO_ID = p.ID
            WHERE ai.ATENDIDA = 0 AND ai.ACTIVO = 1
            ORDER BY ai.FECHA_ALERTA DESC;
    END;

    PROCEDURE MARCAR_ALERTA_ATENDIDA(p_alerta_id NUMBER, p_usuario_id NUMBER) IS
    BEGIN
        UPDATE ALERTA_INFLACION
        SET ATENDIDA = 1, ATENDIDA_POR = p_usuario_id, FECHA_ATENCION = SYSDATE
        WHERE ID = p_alerta_id;
        COMMIT;
    END;

    PROCEDURE ACTUALIZAR_LOTES_VENCIDOS IS
        v_actualizados NUMBER;
    BEGIN
        UPDATE LOTE SET ESTADO = 3
        WHERE FECHA_VENCIMIENTO <= SYSDATE AND ESTADO = 1 AND CANTIDAD_ACTUAL > 0;
        v_actualizados := SQL%ROWCOUNT;
        COMMIT;
    END;

    PROCEDURE ENVIAR_ALERTA_FIDELIZACION(
        p_cliente_id NUMBER,
        p_tipo VARCHAR2,
        p_mensaje VARCHAR2,
        p_producto_id NUMBER DEFAULT NULL,
        p_lote_id NUMBER DEFAULT NULL
    ) IS
    BEGIN
        INSERT INTO ALERTA_FIDELIZACION (CLIENTE_ID, PRODUCTO_ID, LOTE_ID, TIPO_ALERTA, MENSAJE)
        VALUES (p_cliente_id, p_producto_id, p_lote_id, p_tipo, p_mensaje);
        COMMIT;
    END;

    PROCEDURE OBTENER_ALERTAS_CLIENTE(p_cliente_id NUMBER, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT ID, CLIENTE_ID, TIPO_ALERTA, MENSAJE, FECHA_ENVIO, ENVIADA, LEIDA
            FROM ALERTA_FIDELIZACION
            WHERE CLIENTE_ID = p_cliente_id AND ACTIVO = 1
            ORDER BY FECHA_ENVIO DESC;
    END;
    
    PROCEDURE INSERTAR_HISTORIAL_PARAMETRO(
        p_clave_parametro VARCHAR2,
        p_valor_anterior VARCHAR2,
        p_valor_nuevo VARCHAR2,
        p_motivo VARCHAR2,
        p_modificado_por_id NUMBER,
        p_direccion_ip VARCHAR2 DEFAULT NULL
    ) IS
    BEGIN
        INSERT INTO HISTORIAL_PARAMETRO (
            CLAVE_PARAMETRO, VALOR_ANTERIOR, VALOR_NUEVO, 
            MOTIVO, MODIFICADO_POR_ID, DIRECCION_IP
        ) VALUES (
            p_clave_parametro, p_valor_anterior, p_valor_nuevo,
            p_motivo, p_modificado_por_id, p_direccion_ip
        );
        COMMIT;
    END;

END PKG_PHARMASMART_ALERTAS;
/

COMMIT;

SELECT 'Package ALERTAS creado: PKG_PHARMASMART_ALERTAS' AS MENSAJE FROM DUAL;

EXIT;