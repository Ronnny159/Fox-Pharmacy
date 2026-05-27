-- ============================================
-- PharmaSmart - Script 09
-- Package PKG_PHARMASMART_ALERTAS 
-- Ejecutar como PHARMA_USER
-- ============================================

SET SERVEROUTPUT ON;
SET ECHO ON;

CREATE OR REPLACE PACKAGE PKG_PHARMASMART_ALERTAS AS
    
    PROCEDURE OBTENER_ALERTAS_INFLACION(p_cursor OUT SYS_REFCURSOR);
    PROCEDURE MARCAR_ALERTA_ATENDIDA(p_alerta_id NUMBER, p_usuario_id NUMBER);
    PROCEDURE ACTUALIZAR_LOTES_VENCIDOS;
    
    PROCEDURE ENVIAR_ALERTA_FIDELIZACION(
        p_cliente_id NUMBER, p_tipo VARCHAR2, p_mensaje VARCHAR2,
        p_producto_id NUMBER DEFAULT NULL, p_lote_id NUMBER DEFAULT NULL
    );
    
    PROCEDURE OBTENER_ALERTAS_CLIENTE(p_cliente_id NUMBER, p_cursor OUT SYS_REFCURSOR);
    
    PROCEDURE INSERTAR_HISTORIAL_PARAMETRO(
        p_clave_parametro VARCHAR2, p_valor_anterior VARCHAR2, p_valor_nuevo VARCHAR2,
        p_motivo VARCHAR2, p_modificado_por_id NUMBER, p_direccion_ip VARCHAR2 DEFAULT NULL
    );
    
    PROCEDURE INSERTAR_HISTORIAL_DESCUENTO(
        p_producto_id NUMBER, p_codigo_producto VARCHAR2, p_nombre_producto VARCHAR2,
        p_descuento_anterior NUMBER, p_descuento_nuevo NUMBER, p_accion VARCHAR2,
        p_motivo VARCHAR2, p_modificado_por_id NUMBER, p_direccion_ip VARCHAR2 DEFAULT NULL
    );
    
END PKG_PHARMASMART_ALERTAS;
/

CREATE OR REPLACE PACKAGE BODY PKG_PHARMASMART_ALERTAS AS

    PROCEDURE OBTENER_ALERTAS_INFLACION(p_cursor OUT SYS_REFCURSOR) IS
    BEGIN OPEN p_cursor FOR SELECT ai.ID, ai.PRODUCTO_ID, ai.CODIGO_PRODUCTO, p.NOMBRE AS NOMBRE_PRODUCTO,
        ai.PRECIO_ANTERIOR, ai.PRECIO_NUEVO, ai.PORCENTAJE_INCREMENTO, ai.FECHA_ALERTA, ai.ATENDIDA
        FROM ALERTA_INFLACION ai JOIN PRODUCTO p ON ai.PRODUCTO_ID = p.ID
        WHERE ai.ATENDIDA = 0 AND ai.ACTIVO = 1 ORDER BY ai.FECHA_ALERTA DESC; END;

    PROCEDURE MARCAR_ALERTA_ATENDIDA(p_alerta_id NUMBER, p_usuario_id NUMBER) IS
    BEGIN UPDATE ALERTA_INFLACION SET ATENDIDA = 1, ATENDIDA_POR = p_usuario_id, FECHA_ATENCION = SYSDATE
        WHERE ID = p_alerta_id; COMMIT; END;

    PROCEDURE ACTUALIZAR_LOTES_VENCIDOS IS
    BEGIN UPDATE LOTE SET ESTADO = 3 WHERE FECHA_VENCIMIENTO <= SYSDATE AND ESTADO = 1 AND CANTIDAD_ACTUAL > 0; COMMIT; END;

    PROCEDURE ENVIAR_ALERTA_FIDELIZACION(
        p_cliente_id NUMBER, p_tipo VARCHAR2, p_mensaje VARCHAR2,
        p_producto_id NUMBER DEFAULT NULL, p_lote_id NUMBER DEFAULT NULL
    ) IS
    BEGIN INSERT INTO ALERTA_FIDELIZACION (CLIENTE_ID, PRODUCTO_ID, LOTE_ID, TIPO_ALERTA, MENSAJE)
        VALUES (p_cliente_id, p_producto_id, p_lote_id, p_tipo, p_mensaje); COMMIT; END;

    PROCEDURE OBTENER_ALERTAS_CLIENTE(p_cliente_id NUMBER, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN OPEN p_cursor FOR SELECT ID, CLIENTE_ID, TIPO_ALERTA, MENSAJE, FECHA_ENVIO, ENVIADA, LEIDA
        FROM ALERTA_FIDELIZACION WHERE CLIENTE_ID = p_cliente_id AND ACTIVO = 1 ORDER BY FECHA_ENVIO DESC; END;

    PROCEDURE INSERTAR_HISTORIAL_PARAMETRO(
        p_clave_parametro VARCHAR2, p_valor_anterior VARCHAR2, p_valor_nuevo VARCHAR2,
        p_motivo VARCHAR2, p_modificado_por_id NUMBER, p_direccion_ip VARCHAR2 DEFAULT NULL
    ) IS
    BEGIN INSERT INTO HISTORIAL_PARAMETRO (CLAVE_PARAMETRO, VALOR_ANTERIOR, VALOR_NUEVO, MOTIVO, MODIFICADO_POR_ID, DIRECCION_IP)
        VALUES (p_clave_parametro, p_valor_anterior, p_valor_nuevo, p_motivo, p_modificado_por_id, p_direccion_ip); COMMIT; END;

    PROCEDURE INSERTAR_HISTORIAL_DESCUENTO(
        p_producto_id NUMBER, p_codigo_producto VARCHAR2, p_nombre_producto VARCHAR2,
        p_descuento_anterior NUMBER, p_descuento_nuevo NUMBER, p_accion VARCHAR2,
        p_motivo VARCHAR2, p_modificado_por_id NUMBER, p_direccion_ip VARCHAR2 DEFAULT NULL
    ) IS
    BEGIN INSERT INTO HISTORIAL_DESCUENTO_PRODUCTO (PRODUCTO_ID, CODIGO_PRODUCTO, NOMBRE_PRODUCTO,
        DESCUENTO_ANTERIOR, DESCUENTO_NUEVO, ACCION, MOTIVO, MODIFICADO_POR_ID, DIRECCION_IP)
        VALUES (p_producto_id, p_codigo_producto, p_nombre_producto, p_descuento_anterior,
                p_descuento_nuevo, p_accion, p_motivo, p_modificado_por_id, p_direccion_ip); COMMIT; END;

END PKG_PHARMASMART_ALERTAS;
/

COMMIT;
SELECT 'Package ALERTAS COMPLETO creado' AS MENSAJE FROM DUAL;
EXIT;