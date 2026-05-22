-- ============================================
-- PharmaSmart - Script 13
-- Verificación de Instalación
-- Ejecutar como PHARMA_USER
-- ============================================

SET SERVEROUTPUT ON;
SET LINESIZE 200;
SET PAGESIZE 50;

DECLARE
    v_tablas NUMBER;
    v_indices NUMBER;
    v_secuencias NUMBER;
    v_tipos NUMBER;
    v_packages NUMBER;
    v_triggers NUMBER;
    v_users NUMBER;
    v_prods NUMBER;
    v_lotes NUMBER;
    v_clientes NUMBER;
    v_params NUMBER;
    v_lote T_LOTE_INFO;
BEGIN
    SELECT COUNT(*) INTO v_tablas FROM USER_TABLES;
    SELECT COUNT(*) INTO v_indices FROM USER_INDEXES WHERE INDEX_NAME LIKE 'IDX_%';
    SELECT COUNT(*) INTO v_secuencias FROM USER_SEQUENCES;
    SELECT COUNT(*) INTO v_tipos FROM USER_TYPES WHERE TYPE_NAME LIKE 'T_%';
    SELECT COUNT(*) INTO v_packages FROM USER_OBJECTS WHERE OBJECT_TYPE = 'PACKAGE' AND STATUS = 'VALID';
    SELECT COUNT(*) INTO v_triggers FROM USER_OBJECTS WHERE OBJECT_TYPE = 'TRIGGER' AND STATUS = 'VALID';
    SELECT COUNT(*) INTO v_users FROM USUARIO;
    SELECT COUNT(*) INTO v_prods FROM PRODUCTO;
    SELECT COUNT(*) INTO v_lotes FROM LOTE;
    SELECT COUNT(*) INTO v_clientes FROM CLIENTE;
    SELECT COUNT(*) INTO v_params FROM PARAMETRO_SISTEMA;
    
    DBMS_OUTPUT.PUT_LINE('');
    DBMS_OUTPUT.PUT_LINE('===========================================');
    DBMS_OUTPUT.PUT_LINE('PHARMASMART - VERIFICACION DE INSTALACION');
    DBMS_OUTPUT.PUT_LINE('===========================================');
    DBMS_OUTPUT.PUT_LINE('Usuario: PHARMA_USER');
    DBMS_OUTPUT.PUT_LINE('Fecha: ' || TO_CHAR(SYSDATE, 'DD/MM/YYYY HH24:MI:SS'));
    DBMS_OUTPUT.PUT_LINE('-------------------------------------------');
    DBMS_OUTPUT.PUT_LINE('OBJETOS DE BASE DE DATOS:');
    DBMS_OUTPUT.PUT_LINE('  Tablas:      ' || v_tablas || ' (esperado: 12)');
    DBMS_OUTPUT.PUT_LINE('  Indices:     ' || v_indices || ' (esperado: 12)');
    DBMS_OUTPUT.PUT_LINE('  Secuencias:  ' || v_secuencias || ' (esperado: 1)');
    DBMS_OUTPUT.PUT_LINE('  Tipos:       ' || v_tipos || ' (esperado: 5)');
    DBMS_OUTPUT.PUT_LINE('  Packages:    ' || v_packages || ' (esperado: 5)');
    DBMS_OUTPUT.PUT_LINE('  Triggers:    ' || v_triggers || ' (esperado: 3)');
    DBMS_OUTPUT.PUT_LINE('-------------------------------------------');
    DBMS_OUTPUT.PUT_LINE('DATOS INICIALES:');
    DBMS_OUTPUT.PUT_LINE('  Usuarios:    ' || v_users || ' (esperado: 3)');
    DBMS_OUTPUT.PUT_LINE('  Productos:   ' || v_prods || ' (esperado: 5)');
    DBMS_OUTPUT.PUT_LINE('  Lotes:       ' || v_lotes || ' (esperado: 8)');
    DBMS_OUTPUT.PUT_LINE('  Clientes:    ' || v_clientes || ' (esperado: 2)');
    DBMS_OUTPUT.PUT_LINE('  Parametros:  ' || v_params || ' (esperado: 6)');
    DBMS_OUTPUT.PUT_LINE('-------------------------------------------');
    
    -- Probar FEFO
    BEGIN
        v_lote := PKG_PHARMASMART_FEFO.SELECCIONAR_FEFO(1);
        DBMS_OUTPUT.PUT_LINE('PRUEBA FEFO:');
        DBMS_OUTPUT.PUT_LINE('  Producto 1 -> Lote: ' || v_lote.CODIGO_LOTE || 
                           ' | Vence: ' || TO_CHAR(v_lote.FECHA_VENCIMIENTO, 'DD/MM/YYYY') ||
                           ' | Stock: ' || v_lote.CANTIDAD_ACTUAL);
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('  ERROR en FEFO: ' || SQLERRM);
    END;
    
    -- Probar parámetro
    BEGIN
        DBMS_OUTPUT.PUT_LINE('PRUEBA PARAMETRO:');
        DBMS_OUTPUT.PUT_LINE('  Descuento general: ' || 
            PKG_PHARMASMART_CONFIG.OBTENER_PARAMETRO('PORCENTAJE_DESCUENTO_VENCIMIENTO'));
    EXCEPTION
        WHEN OTHERS THEN
            DBMS_OUTPUT.PUT_LINE('  ERROR en parametro: ' || SQLERRM);
    END;
    
    DBMS_OUTPUT.PUT_LINE('===========================================');
    
    IF v_tablas = 12 AND v_packages = 5 AND v_triggers = 3 
       AND v_users = 3 AND v_prods = 5 AND v_lotes = 8 THEN
        DBMS_OUTPUT.PUT_LINE('ESTADO: INSTALACION COMPLETA Y CORRECTA');
    ELSE
        DBMS_OUTPUT.PUT_LINE('ESTADO: REVISAR - Hay diferencias con lo esperado');
    END IF;
    
    DBMS_OUTPUT.PUT_LINE('===========================================');
END;
/

EXIT;