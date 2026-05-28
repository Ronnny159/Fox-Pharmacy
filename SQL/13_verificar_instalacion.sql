-- ============================================
-- PharmaSmart - Script 13 (CORREGIDO)
-- Verificación de Instalación
-- Ejecutar como PHARMA_USER
-- ============================================

SET SERVEROUTPUT ON;
SET LINESIZE 200;

DECLARE
    v_tablas NUMBER; v_indices NUMBER; v_secuencias NUMBER; v_tipos NUMBER;
    v_packages NUMBER; v_triggers NUMBER; v_users NUMBER; v_prods NUMBER;
    v_lotes NUMBER; v_clientes NUMBER; v_params NUMBER;
    v_lote T_LOTE_INFO; v_param_valor VARCHAR2(30);
BEGIN
    SELECT COUNT(*) INTO v_tablas FROM USER_TABLES;
    SELECT COUNT(*) INTO v_indices FROM USER_INDEXES WHERE INDEX_NAME LIKE 'IDX_%';
    SELECT COUNT(*) INTO v_secuencias FROM USER_SEQUENCES;
    SELECT COUNT(*) INTO v_tipos FROM USER_TYPES WHERE TYPE_NAME LIKE 'T_%';
    SELECT COUNT(*) INTO v_packages FROM USER_OBJECTS WHERE OBJECT_TYPE = 'PACKAGE' AND STATUS = 'VALID';
    SELECT COUNT(*) INTO v_triggers FROM USER_OBJECTS WHERE OBJECT_TYPE = 'TRIGGER' AND STATUS = 'VALID';
    SELECT COUNT(*) INTO v_users FROM USUARIOS;
    SELECT COUNT(*) INTO v_prods FROM PRODUCTOS;
    SELECT COUNT(*) INTO v_lotes FROM LOTES;
    SELECT COUNT(*) INTO v_clientes FROM CLIENTES;
    SELECT COUNT(*) INTO v_params FROM PARAMETROS_SISTEMA;
    
    DBMS_OUTPUT.PUT_LINE('===========================================');
    DBMS_OUTPUT.PUT_LINE('PHARMASMART - VERIFICACION DE INSTALACION');
    DBMS_OUTPUT.PUT_LINE('===========================================');
    DBMS_OUTPUT.PUT_LINE('Tablas:      ' || v_tablas || ' (esperado: 12)');
    DBMS_OUTPUT.PUT_LINE('Indices:     ' || v_indices || ' (esperado: 22)');
    DBMS_OUTPUT.PUT_LINE('Secuencias:  ' || v_secuencias || ' (esperado: 5)');
    DBMS_OUTPUT.PUT_LINE('Tipos:       ' || v_tipos || ' (esperado: 7)');
    DBMS_OUTPUT.PUT_LINE('Packages:    ' || v_packages || ' (esperado: 5)');
    DBMS_OUTPUT.PUT_LINE('Triggers:    ' || v_triggers || ' (esperado: 2)');
    DBMS_OUTPUT.PUT_LINE('Usuarios:    ' || v_users || ' (esperado: 3)');
    DBMS_OUTPUT.PUT_LINE('Productos:   ' || v_prods || ' (esperado: 5)');
    DBMS_OUTPUT.PUT_LINE('Lotes:       ' || v_lotes || ' (esperado: 8)');
    DBMS_OUTPUT.PUT_LINE('Clientes:    ' || v_clientes || ' (esperado: 2)');
    DBMS_OUTPUT.PUT_LINE('Parametros:  ' || v_params || ' (esperado: 6)');
    
    BEGIN
        v_lote := PKG_PHARMASMART_FEFO.SELECCIONAR_FEFO(1);
        DBMS_OUTPUT.PUT_LINE('FEFO: Lote=' || v_lote.CODIGO_LOTE || ' Vence=' || TO_CHAR(v_lote.FECHA_VENCIMIENTO,'DD/MM/YYYY') || ' Stock=' || v_lote.CANTIDAD_ACTUAL);
    EXCEPTION WHEN OTHERS THEN DBMS_OUTPUT.PUT_LINE('FEFO: ERROR - ' || SQLERRM); END;
    
    BEGIN
        v_param_valor := PKG_PHARMASMART_CONFIG.OBTENER_PARAMETRO('PORCENTAJE_DESCUENTO_VENCIMIENTO');
        DBMS_OUTPUT.PUT_LINE('Parametro: ' || NVL(v_param_valor, 'No encontrado'));
    EXCEPTION WHEN OTHERS THEN DBMS_OUTPUT.PUT_LINE('Parametro: ERROR - ' || SQLERRM); END;
    
    IF v_tablas >= 12 AND v_packages >= 5 AND v_users >= 3 THEN
        DBMS_OUTPUT.PUT_LINE('ESTADO: INSTALACION COMPLETA'); ELSE DBMS_OUTPUT.PUT_LINE('ESTADO: REVISAR'); END IF;
END;
/

EXIT;