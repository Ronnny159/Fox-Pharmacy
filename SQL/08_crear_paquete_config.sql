-- ============================================
-- PharmaSmart - Script 08 (COMPLETO)
-- Package PKG_PHARMASMART_CONFIG
-- Incluye: LOGIN, REGISTRO, GESTIÓN DE USUARIOS
-- Ejecutar como PHARMA_USER
-- ============================================

SET SERVEROUTPUT ON;
SET ECHO ON;

CREATE OR REPLACE PACKAGE PKG_PHARMASMART_CONFIG AS
    
    -- ========================================
    -- FUNCIONES DE LOGIN Y SEGURIDAD
    -- ========================================
    
    -- Función para hashear contraseña (SHA256)
    FUNCTION HASH_PASSWORD(p_password VARCHAR2) RETURN VARCHAR2;
    
    -- Login: retorna ID_USUARIO si credenciales son correctas, NULL en caso contrario
    FUNCTION LOGIN(p_nombre_usuario VARCHAR2, p_password VARCHAR2) RETURN NUMBER;
    
    -- Login con información completa (cursor)
    PROCEDURE LOGIN_COMPLETO(p_nombre_usuario VARCHAR2, p_password VARCHAR2, p_cursor OUT SYS_REFCURSOR);
    
    -- Verificar si usuario existe
    FUNCTION USUARIO_EXISTE(p_nombre_usuario VARCHAR2) RETURN BOOLEAN;
    
    -- Verificar si documento ya está registrado
    FUNCTION DOCUMENTO_EXISTE(p_documento VARCHAR2) RETURN BOOLEAN;
    
    -- ========================================
    -- REGISTRO DE NUEVOS USUARIOS
    -- ========================================
    
    -- Registrar nuevo usuario (retorna TRUE si éxito, FALSE si error)
    FUNCTION REGISTRAR_USUARIO(
        p_nombre_usuario VARCHAR2,
        p_password VARCHAR2,
        p_nombre_completo VARCHAR2,
        p_rol CHAR,
        p_documento_identidad VARCHAR2
    ) RETURN BOOLEAN;
    
    -- Registrar nuevo usuario con mensaje de error (versión con OUT)
    PROCEDURE REGISTRAR_USUARIO_PROC(
        p_nombre_usuario VARCHAR2,
        p_password VARCHAR2,
        p_nombre_completo VARCHAR2,
        p_rol CHAR,
        p_documento_identidad VARCHAR2,
        p_success OUT BOOLEAN,
        p_mensaje OUT VARCHAR2
    );
    
    -- ========================================
    -- GESTIÓN DE USUARIOS (CRUD completo)
    -- ========================================
    
    -- Cambiar contraseña
    FUNCTION CAMBIAR_CONTRASENA(
        p_id_usuario NUMBER,
        p_password_actual VARCHAR2,
        p_password_nueva VARCHAR2
    ) RETURN BOOLEAN;
    
    -- Cambiar contraseña por administrador (sin validar actual)
    PROCEDURE RESETEAR_CONTRASENA(
        p_id_usuario NUMBER,
        p_password_nueva VARCHAR2,
        p_id_administrador NUMBER
    );
    
    -- Actualizar perfil de usuario
    PROCEDURE ACTUALIZAR_USUARIO(
        p_id_usuario NUMBER,
        p_nombre_completo VARCHAR2,
        p_rol CHAR,
        p_estado CHAR,
        p_id_administrador NUMBER
    );
    
    -- Cambiar estado de usuario (Activar/Inactivar)
    PROCEDURE CAMBIAR_ESTADO_USUARIO(
        p_id_usuario NUMBER,
        p_nuevo_estado CHAR,
        p_id_administrador NUMBER
    );
    
    -- Eliminar usuario (baja lógica)
    PROCEDURE ELIMINAR_USUARIO(
        p_id_usuario NUMBER,
        p_id_administrador NUMBER
    );
    
    -- ========================================
    -- CONSULTAS DE USUARIOS
    -- ========================================
    
    PROCEDURE OBTENER_USUARIO_POR_CREDENCIALES(p_nombre VARCHAR2, p_hash VARCHAR2, p_cursor OUT SYS_REFCURSOR);
    PROCEDURE OBTENER_USUARIO_POR_ID(p_id_usuario NUMBER, p_cursor OUT SYS_REFCURSOR);
    PROCEDURE OBTENER_USUARIO_POR_NOMBRE(p_nombre VARCHAR2, p_cursor OUT SYS_REFCURSOR);
    PROCEDURE OBTENER_USUARIO_POR_DOCUMENTO(p_doc VARCHAR2, p_cursor OUT SYS_REFCURSOR);
    PROCEDURE OBTENER_TODOS_USUARIOS(p_cursor OUT SYS_REFCURSOR);
    PROCEDURE OBTENER_USUARIOS_ACTIVOS(p_cursor OUT SYS_REFCURSOR);
    PROCEDURE OBTENER_USUARIOS_POR_ROL(p_rol CHAR, p_cursor OUT SYS_REFCURSOR);
    
    -- ========================================
    -- PARÁMETROS DEL SISTEMA
    -- ========================================
    
    FUNCTION OBTENER_PARAMETRO(p_clave VARCHAR2) RETURN VARCHAR2;
    PROCEDURE ACTUALIZAR_DESCUENTO_GENERAL(p_nuevo_valor VARCHAR2, p_id_usuario NUMBER, p_motivo VARCHAR2, p_ip VARCHAR2 DEFAULT NULL);
    PROCEDURE ACTUALIZAR_DESCUENTO_PRODUCTO(p_id_producto NUMBER, p_descuento NUMBER, p_id_usuario NUMBER, p_motivo VARCHAR2, p_ip VARCHAR2 DEFAULT NULL);
    PROCEDURE REGISTRAR_AJUSTE(p_id_lote NUMBER, p_tipo CHAR, p_cantidad NUMBER, p_motivo VARCHAR2, p_id_responsable NUMBER);
    
    -- ========================================
    -- HISTORIAL Y AUDITORÍA
    -- ========================================
    
    PROCEDURE OBTENER_HISTORIAL_PARAMETROS(p_clave VARCHAR2, p_cursor OUT SYS_REFCURSOR);
    PROCEDURE OBTENER_HISTORIAL_POR_USUARIO(p_id_usuario NUMBER, p_cursor OUT SYS_REFCURSOR);
    PROCEDURE OBTENER_HISTORIAL_POR_FECHAS(p_desde DATE, p_hasta DATE, p_cursor OUT SYS_REFCURSOR);
    PROCEDURE OBTENER_HISTORIAL_DESCUENTOS(p_id_producto NUMBER, p_cursor OUT SYS_REFCURSOR);
    PROCEDURE OBTENER_TODO_HISTORIAL_DESCUENTO(p_cursor OUT SYS_REFCURSOR);
    
    -- ========================================
    -- PRODUCTOS
    -- ========================================
    
    PROCEDURE OBTENER_PRODUCTO_POR_CODIGO(p_codigo VARCHAR2, p_cursor OUT SYS_REFCURSOR);
    PROCEDURE OBTENER_PRODUCTO_POR_ID(p_id_producto NUMBER, p_cursor OUT SYS_REFCURSOR);
    PROCEDURE OBTENER_PRODUCTO_POR_NOMBRE(p_nombre VARCHAR2, p_cursor OUT SYS_REFCURSOR);
    PROCEDURE OBTENER_TODOS_PRODUCTOS(p_cursor OUT SYS_REFCURSOR);
    PROCEDURE INSERTAR_PRODUCTO(p_codigo VARCHAR2, p_nombre VARCHAR2, p_descripcion VARCHAR2, p_es_controlado CHAR, p_stock_minimo NUMBER, p_descuento NUMBER);
    PROCEDURE ACTUALIZAR_PRODUCTO(p_id_producto NUMBER, p_nombre VARCHAR2, p_descripcion VARCHAR2, p_es_controlado CHAR, p_stock_minimo NUMBER, p_descuento NUMBER);
    PROCEDURE ELIMINAR_PRODUCTO(p_id_producto NUMBER);
    
    -- ========================================
    -- CLIENTES
    -- ========================================
    
    PROCEDURE OBTENER_CLIENTE_POR_DOCUMENTO(p_doc VARCHAR2, p_cursor OUT SYS_REFCURSOR);
    PROCEDURE OBTENER_CLIENTE_POR_ID(p_id_cliente NUMBER, p_cursor OUT SYS_REFCURSOR);
    PROCEDURE OBTENER_CLIENTE_POR_CHAT_ID(p_chat VARCHAR2, p_cursor OUT SYS_REFCURSOR);
    PROCEDURE OBTENER_CLIENTES_FIDELIZACION(p_cursor OUT SYS_REFCURSOR);
    PROCEDURE INSERTAR_CLIENTE(p_doc VARCHAR2, p_nombre VARCHAR2, p_tel VARCHAR2, p_correo VARCHAR2, p_chat VARCHAR2, p_med VARCHAR2);
    PROCEDURE ACTUALIZAR_CLIENTE(p_id_cliente NUMBER, p_nombre VARCHAR2, p_tel VARCHAR2, p_correo VARCHAR2, p_chat VARCHAR2, p_med VARCHAR2);
    
    -- ========================================
    -- PARÁMETROS
    -- ========================================
    
    PROCEDURE OBTENER_TODOS_PARAMETROS(p_cursor OUT SYS_REFCURSOR);
    PROCEDURE INSERTAR_PARAMETRO(p_clave VARCHAR2, p_valor VARCHAR2, p_desc VARCHAR2);
    
END PKG_PHARMASMART_CONFIG;
/

CREATE OR REPLACE PACKAGE BODY PKG_PHARMASMART_CONFIG AS

    -- ========================================
    -- FUNCIÓN: HASH_PASSWORD
    -- ========================================
    FUNCTION HASH_PASSWORD(p_password VARCHAR2) RETURN VARCHAR2 IS
        -- En Oracle, usamos STANDARD_HASH para SHA256
        v_hash VARCHAR2(128);
    BEGIN
        -- STANDARD_HASH está disponible en Oracle 12c+
        v_hash := RAWTOHEX(STANDARD_HASH(UTL_I18N.STRING_TO_RAW(p_password, 'AL32UTF8'), 'SHA256'));
        RETURN v_hash;
    EXCEPTION
        WHEN OTHERS THEN
            -- Fallback: DBMS_CRYPTO (requiere permisos adicionales)
            RETURN NULL;
    END;
    
    -- ========================================
    -- FUNCIÓN: LOGIN
    -- Retorna ID_USUARIO si credenciales correctas y usuario activo
    -- ========================================
    FUNCTION LOGIN(p_nombre_usuario VARCHAR2, p_password VARCHAR2) RETURN NUMBER IS
        v_id_usuario NUMBER;
        v_hash VARCHAR2(128);
        v_estado CHAR(1);
    BEGIN
        -- Hashear la contraseña ingresada
        v_hash := HASH_PASSWORD(p_password);
        
        -- Buscar usuario con esas credenciales y estado activo
        SELECT ID_USUARIO, ESTADO INTO v_id_usuario, v_estado
        FROM USUARIOS
        WHERE NOMBRE_USUARIO = UPPER(TRIM(p_nombre_usuario))
          AND HASH_CONTRASENA = v_hash;
        
        -- Verificar estado
        IF v_estado = 'I' THEN
            RETURN -1; -- Usuario inactivo
        END IF;
        
        RETURN v_id_usuario;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RETURN NULL; -- Credenciales incorrectas
        WHEN OTHERS THEN
            RETURN NULL;
    END;
    
    -- ========================================
    -- PROCEDURE: LOGIN_COMPLETO
    -- Retorna toda la información del usuario
    -- ========================================
    PROCEDURE LOGIN_COMPLETO(p_nombre_usuario VARCHAR2, p_password VARCHAR2, p_cursor OUT SYS_REFCURSOR) IS
        v_hash VARCHAR2(128);
    BEGIN
        v_hash := HASH_PASSWORD(p_password);
        
        OPEN p_cursor FOR
            SELECT ID_USUARIO, NOMBRE_USUARIO, NOMBRE_COMPLETO, ROL, DOCUMENTO_IDENTIDAD, ESTADO
            FROM USUARIOS
            WHERE NOMBRE_USUARIO = UPPER(TRIM(p_nombre_usuario))
              AND HASH_CONTRASENA = v_hash
              AND ESTADO = 'A';
    END;
    
    -- ========================================
    -- FUNCIÓN: USUARIO_EXISTE
    -- ========================================
    FUNCTION USUARIO_EXISTE(p_nombre_usuario VARCHAR2) RETURN BOOLEAN IS
        v_count NUMBER;
    BEGIN
        SELECT COUNT(*) INTO v_count
        FROM USUARIOS
        WHERE NOMBRE_USUARIO = UPPER(TRIM(p_nombre_usuario));
        
        RETURN v_count > 0;
    END;
    
    -- ========================================
    -- FUNCIÓN: DOCUMENTO_EXISTE
    -- ========================================
    FUNCTION DOCUMENTO_EXISTE(p_documento VARCHAR2) RETURN BOOLEAN IS
        v_count NUMBER;
    BEGIN
        SELECT COUNT(*) INTO v_count
        FROM USUARIOS
        WHERE DOCUMENTO_IDENTIDAD = TRIM(p_documento);
        
        RETURN v_count > 0;
    END;
    
    -- ========================================
    -- FUNCIÓN: REGISTRAR_USUARIO
    -- Registra un nuevo usuario en el sistema
    -- ========================================
    FUNCTION REGISTRAR_USUARIO(
        p_nombre_usuario VARCHAR2,
        p_password VARCHAR2,
        p_nombre_completo VARCHAR2,
        p_rol CHAR,
        p_documento_identidad VARCHAR2
    ) RETURN BOOLEAN IS
        v_hash VARCHAR2(128);
        v_nombre_usuario VARCHAR2(30);
        v_documento VARCHAR2(15);
    BEGIN
        -- Limpiar y validar datos
        v_nombre_usuario := UPPER(TRIM(p_nombre_usuario));
        v_documento := TRIM(p_documento_identidad);
        
        -- Validar que el nombre de usuario no exista
        IF USUARIO_EXISTE(v_nombre_usuario) THEN
            RETURN FALSE;
        END IF;
        
        -- Validar que el documento no exista
        IF DOCUMENTO_EXISTE(v_documento) THEN
            RETURN FALSE;
        END IF;
        
        -- Validar rol válido
        IF p_rol NOT IN ('1', '2', '3', '4') THEN
            RETURN FALSE;
        END IF;
        
        -- Validar que la contraseña no esté vacía
        IF p_password IS NULL OR LENGTH(p_password) < 4 THEN
            RETURN FALSE;
        END IF;
        
        -- Hashear contraseña
        v_hash := HASH_PASSWORD(p_password);
        
        -- Insertar usuario
        INSERT INTO USUARIOS (
            NOMBRE_USUARIO,
            HASH_CONTRASENA,
            NOMBRE_COMPLETO,
            ROL,
            DOCUMENTO_IDENTIDAD,
            ESTADO,
            FECHA_CREACION
        ) VALUES (
            v_nombre_usuario,
            v_hash,
            UPPER(TRIM(p_nombre_completo)),
            p_rol,
            v_documento,
            'A',  -- Activo por defecto
            SYSDATE
        );
        
        COMMIT;
        RETURN TRUE;
        
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            RETURN FALSE;
    END;
    
    -- ========================================
    -- PROCEDURE: REGISTRAR_USUARIO_PROC
    -- Versión con mensaje de error detallado
    -- ========================================
    PROCEDURE REGISTRAR_USUARIO_PROC(
        p_nombre_usuario VARCHAR2,
        p_password VARCHAR2,
        p_nombre_completo VARCHAR2,
        p_rol CHAR,
        p_documento_identidad VARCHAR2,
        p_success OUT BOOLEAN,
        p_mensaje OUT VARCHAR2
    ) IS
        v_hash VARCHAR2(128);
        v_nombre_usuario VARCHAR2(30);
        v_documento VARCHAR2(15);
    BEGIN
        -- Limpiar datos
        v_nombre_usuario := UPPER(TRIM(p_nombre_usuario));
        v_documento := TRIM(p_documento_identidad);
        
        -- Validaciones
        IF v_nombre_usuario IS NULL OR LENGTH(v_nombre_usuario) < 3 THEN
            p_success := FALSE;
            p_mensaje := 'El nombre de usuario debe tener al menos 3 caracteres.';
            RETURN;
        END IF;
        
        IF p_password IS NULL OR LENGTH(p_password) < 4 THEN
            p_success := FALSE;
            p_mensaje := 'La contraseña debe tener al menos 4 caracteres.';
            RETURN;
        END IF;
        
        IF p_nombre_completo IS NULL OR LENGTH(p_nombre_completo) < 5 THEN
            p_success := FALSE;
            p_mensaje := 'El nombre completo es requerido.';
            RETURN;
        END IF;
        
        IF p_rol NOT IN ('1', '2', '3', '4') THEN
            p_success := FALSE;
            p_mensaje := 'Rol inválido. Los roles válidos son: 1=Admin, 2=Cajero, 3=Farmacéutico, 4=Auditor';
            RETURN;
        END IF;
        
        IF v_documento IS NULL OR LENGTH(v_documento) < 5 THEN
            p_success := FALSE;
            p_mensaje := 'El documento de identidad es requerido.';
            RETURN;
        END IF;
        
        -- Verificar usuario existente
        IF USUARIO_EXISTE(v_nombre_usuario) THEN
            p_success := FALSE;
            p_mensaje := 'El nombre de usuario ya está registrado.';
            RETURN;
        END IF;
        
        -- Verificar documento existente
        IF DOCUMENTO_EXISTE(v_documento) THEN
            p_success := FALSE;
            p_mensaje := 'El documento de identidad ya está registrado.';
            RETURN;
        END IF;
        
        -- Hashear contraseña
        v_hash := HASH_PASSWORD(p_password);
        
        -- Insertar
        INSERT INTO USUARIOS (
            NOMBRE_USUARIO, HASH_CONTRASENA, NOMBRE_COMPLETO,
            ROL, DOCUMENTO_IDENTIDAD, ESTADO, FECHA_CREACION
        ) VALUES (
            v_nombre_usuario, v_hash, UPPER(TRIM(p_nombre_completo)),
            p_rol, v_documento, 'A', SYSDATE
        );
        
        COMMIT;
        p_success := TRUE;
        p_mensaje := 'Usuario registrado exitosamente.';
        
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            p_success := FALSE;
            p_mensaje := 'Error al registrar usuario: ' || SQLERRM;
    END;
    
    -- ========================================
    -- FUNCIÓN: CAMBIAR_CONTRASENA
    -- ========================================
    FUNCTION CAMBIAR_CONTRASENA(
        p_id_usuario NUMBER,
        p_password_actual VARCHAR2,
        p_password_nueva VARCHAR2
    ) RETURN BOOLEAN IS
        v_hash_actual VARCHAR2(128);
        v_hash_nueva VARCHAR2(128);
        v_contador NUMBER;
    BEGIN
        -- Validar contraseña nueva
        IF p_password_nueva IS NULL OR LENGTH(p_password_nueva) < 4 THEN
            RETURN FALSE;
        END IF;
        
        -- Verificar contraseña actual
        v_hash_actual := HASH_PASSWORD(p_password_actual);
        
        SELECT COUNT(*) INTO v_contador
        FROM USUARIOS
        WHERE ID_USUARIO = p_id_usuario
          AND HASH_CONTRASENA = v_hash_actual
          AND ESTADO = 'A';
        
        IF v_contador = 0 THEN
            RETURN FALSE;
        END IF;
        
        -- Actualizar contraseña
        v_hash_nueva := HASH_PASSWORD(p_password_nueva);
        
        UPDATE USUARIOS
        SET HASH_CONTRASENA = v_hash_nueva
        WHERE ID_USUARIO = p_id_usuario;
        
        COMMIT;
        RETURN TRUE;
        
    EXCEPTION
        WHEN OTHERS THEN
            ROLLBACK;
            RETURN FALSE;
    END;
    
    -- ========================================
    -- PROCEDURE: RESETEAR_CONTRASENA (Admin)
    -- ========================================
    PROCEDURE RESETEAR_CONTRASENA(
        p_id_usuario NUMBER,
        p_password_nueva VARCHAR2,
        p_id_administrador NUMBER
    ) IS
        v_rol CHAR(1);
        v_hash_nueva VARCHAR2(128);
    BEGIN
        -- Verificar que quien resetea es administrador
        SELECT ROL INTO v_rol FROM USUARIOS WHERE ID_USUARIO = p_id_administrador;
        
        IF v_rol != '1' THEN
            RAISE_APPLICATION_ERROR(-20010, 'Solo un administrador puede resetear contraseñas.');
        END IF;
        
        -- Validar contraseña nueva
        IF p_password_nueva IS NULL OR LENGTH(p_password_nueva) < 4 THEN
            RAISE_APPLICATION_ERROR(-20011, 'La nueva contraseña debe tener al menos 4 caracteres.');
        END IF;
        
        -- Actualizar
        v_hash_nueva := HASH_PASSWORD(p_password_nueva);
        
        UPDATE USUARIOS
        SET HASH_CONTRASENA = v_hash_nueva
        WHERE ID_USUARIO = p_id_usuario;
        
        COMMIT;
    END;
    
    -- ========================================
    -- PROCEDURE: ACTUALIZAR_USUARIO
    -- ========================================
    PROCEDURE ACTUALIZAR_USUARIO(
        p_id_usuario NUMBER,
        p_nombre_completo VARCHAR2,
        p_rol CHAR,
        p_estado CHAR,
        p_id_administrador NUMBER
    ) IS
        v_rol_admin CHAR(1);
    BEGIN
        -- Verificar permisos
        SELECT ROL INTO v_rol_admin FROM USUARIOS WHERE ID_USUARIO = p_id_administrador;
        
        IF v_rol_admin != '1' THEN
            RAISE_APPLICATION_ERROR(-20012, 'Solo administradores pueden modificar usuarios.');
        END IF;
        
        -- Validar rol y estado
        IF p_rol NOT IN ('1', '2', '3', '4') THEN
            RAISE_APPLICATION_ERROR(-20013, 'Rol inválido.');
        END IF;
        
        IF p_estado NOT IN ('A', 'I') THEN
            RAISE_APPLICATION_ERROR(-20014, 'Estado inválido.');
        END IF;
        
        -- Actualizar
        UPDATE USUARIOS
        SET NOMBRE_COMPLETO = UPPER(TRIM(p_nombre_completo)),
            ROL = p_rol,
            ESTADO = p_estado
        WHERE ID_USUARIO = p_id_usuario;
        
        COMMIT;
    END;
    
    -- ========================================
    -- PROCEDURE: CAMBIAR_ESTADO_USUARIO
    -- ========================================
    PROCEDURE CAMBIAR_ESTADO_USUARIO(
        p_id_usuario NUMBER,
        p_nuevo_estado CHAR,
        p_id_administrador NUMBER
    ) IS
        v_rol_admin CHAR(1);
    BEGIN
        -- Verificar permisos
        SELECT ROL INTO v_rol_admin FROM USUARIOS WHERE ID_USUARIO = p_id_administrador;
        
        IF v_rol_admin != '1' THEN
            RAISE_APPLICATION_ERROR(-20015, 'Solo administradores pueden cambiar estados.');
        END IF;
        
        -- No permitir desactivar el propio usuario
        IF p_id_usuario = p_id_administrador AND p_nuevo_estado = 'I' THEN
            RAISE_APPLICATION_ERROR(-20016, 'No puede desactivar su propio usuario.');
        END IF;
        
        UPDATE USUARIOS
        SET ESTADO = p_nuevo_estado
        WHERE ID_USUARIO = p_id_usuario;
        
        COMMIT;
    END;
    
    -- ========================================
    -- PROCEDURE: ELIMINAR_USUARIO (Baja lógica)
    -- ========================================
    PROCEDURE ELIMINAR_USUARIO(
        p_id_usuario NUMBER,
        p_id_administrador NUMBER
    ) IS
    BEGIN
        CAMBIAR_ESTADO_USUARIO(p_id_usuario, 'I', p_id_administrador);
    END;
    
    -- ========================================
    -- FUNCIÓN: OBTENER_PARAMETRO
    -- ========================================
    FUNCTION OBTENER_PARAMETRO(p_clave VARCHAR2) RETURN VARCHAR2 IS
        v_valor VARCHAR2(30);
    BEGIN
        SELECT VALOR INTO v_valor FROM PARAMETROS_SISTEMA WHERE CLAVE = p_clave;
        RETURN v_valor;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN RETURN NULL;
    END;
    
    -- ========================================
    -- PROCEDURE: ACTUALIZAR_DESCUENTO_GENERAL
    -- ========================================
    PROCEDURE ACTUALIZAR_DESCUENTO_GENERAL(
        p_nuevo_valor VARCHAR2, 
        p_id_usuario NUMBER, 
        p_motivo VARCHAR2, 
        p_ip VARCHAR2 DEFAULT NULL
    ) IS
        v_rol CHAR(1);
        v_anterior VARCHAR2(30);
    BEGIN
        SELECT ROL INTO v_rol FROM USUARIOS WHERE ID_USUARIO = p_id_usuario;
        IF v_rol != '1' THEN
            RAISE_APPLICATION_ERROR(-20004, 'Solo administradores pueden modificar parámetros globales.');
        END IF;
        
        SELECT VALOR INTO v_anterior FROM PARAMETROS_SISTEMA WHERE CLAVE = 'PORCENTAJE_DESCUENTO_VENCIMIENTO';
        
        UPDATE PARAMETROS_SISTEMA 
        SET VALOR = p_nuevo_valor 
        WHERE CLAVE = 'PORCENTAJE_DESCUENTO_VENCIMIENTO';
        
        INSERT INTO HISTORIAL_PARAMETROS (
            CLAVE_PARAMETRO, VALOR_ANTERIOR, VALOR_NUEVO, 
            MOTIVO, ID_USUARIO, DIRECCION_IP
        ) VALUES (
            'PORCENTAJE_DESCUENTO_VENCIMIENTO', v_anterior, p_nuevo_valor, 
            p_motivo, p_id_usuario, p_ip
        );
        
        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN ROLLBACK; RAISE;
    END;
    
    -- ========================================
    -- PROCEDURE: ACTUALIZAR_DESCUENTO_PRODUCTO
    -- ========================================
    PROCEDURE ACTUALIZAR_DESCUENTO_PRODUCTO(
        p_id_producto NUMBER, 
        p_descuento NUMBER, 
        p_id_usuario NUMBER, 
        p_motivo VARCHAR2, 
        p_ip VARCHAR2 DEFAULT NULL
    ) IS
        v_rol CHAR(1);
        v_prod PRODUCTOS%ROWTYPE;
        v_accion CHAR(1);
    BEGIN
        SELECT ROL INTO v_rol FROM USUARIOS WHERE ID_USUARIO = p_id_usuario;
        IF v_rol != '1' THEN
            RAISE_APPLICATION_ERROR(-20004, 'Solo administradores pueden modificar descuentos.');
        END IF;
        
        SELECT * INTO v_prod FROM PRODUCTOS WHERE ID_PRODUCTO = p_id_producto;
        
        -- Determinar acción
        IF v_prod.DESCUENTO_PROXIMIDAD_VENCIMIENTO IS NULL AND p_descuento IS NOT NULL THEN
            v_accion := 'C';
        ELSIF v_prod.DESCUENTO_PROXIMIDAD_VENCIMIENTO IS NOT NULL AND p_descuento IS NULL THEN
            v_accion := 'E';
        ELSE
            v_accion := 'M';
        END IF;
        
        INSERT INTO HISTORIAL_DESCUENTOS (
            ID_PRODUCTO, CODIGO_PRODUCTO, NOMBRE_PRODUCTO, 
            DESCUENTO_ANTERIOR, DESCUENTO_NUEVO, ACCION, 
            MOTIVO, ID_USUARIO, DIRECCION_IP
        ) VALUES (
            p_id_producto, v_prod.CODIGO, v_prod.NOMBRE, 
            v_prod.DESCUENTO_PROXIMIDAD_VENCIMIENTO, p_descuento, v_accion, 
            p_motivo, p_id_usuario, p_ip
        );
        
        UPDATE PRODUCTOS 
        SET DESCUENTO_PROXIMIDAD_VENCIMIENTO = p_descuento 
        WHERE ID_PRODUCTO = p_id_producto;
        
        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN ROLLBACK; RAISE;
    END;
    
    -- ========================================
    -- PROCEDURE: REGISTRAR_AJUSTE
    -- ========================================
    PROCEDURE REGISTRAR_AJUSTE(
        p_id_lote NUMBER, 
        p_tipo CHAR, 
        p_cantidad NUMBER, 
        p_motivo VARCHAR2, 
        p_id_responsable NUMBER
    ) IS
        v_estado CHAR(1);
        v_cant NUMBER;
    BEGIN
        INSERT INTO AJUSTES_INVENTARIO (
            ID_LOTE, TIPO, CANTIDAD, MOTIVO, ID_RESPONSABLE
        ) VALUES (
            p_id_lote, p_tipo, p_cantidad, p_motivo, p_id_responsable
        );
        
        SELECT CANTIDAD_ACTUAL - p_cantidad INTO v_cant 
        FROM LOTES WHERE ID_LOTE = p_id_lote;
        
        IF v_cant <= 0 THEN 
            v_estado := 'V';
        ELSIF p_tipo = 'V' THEN 
            v_estado := 'V';
        ELSIF p_tipo = 'R' THEN 
            v_estado := 'C';
        ELSE 
            SELECT ESTADO INTO v_estado FROM LOTES WHERE ID_LOTE = p_id_lote;
        END IF;
        
        UPDATE LOTES 
        SET CANTIDAD_ACTUAL = v_cant, ESTADO = v_estado 
        WHERE ID_LOTE = p_id_lote;
        
        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN ROLLBACK; RAISE;
    END;
    
    -- ========================================
    -- PROCEDIMIENTOS DE CONSULTA: USUARIOS
    -- ========================================
    
    PROCEDURE OBTENER_USUARIO_POR_CREDENCIALES(p_nombre VARCHAR2, p_hash VARCHAR2, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR 
            SELECT * FROM USUARIOS 
            WHERE NOMBRE_USUARIO = p_nombre AND HASH_CONTRASENA = p_hash AND ESTADO = 'A';
    END;
    
    PROCEDURE OBTENER_USUARIO_POR_ID(p_id_usuario NUMBER, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR 
            SELECT ID_USUARIO, NOMBRE_USUARIO, NOMBRE_COMPLETO, ROL, DOCUMENTO_IDENTIDAD, ESTADO, FECHA_CREACION
            FROM USUARIOS WHERE ID_USUARIO = p_id_usuario;
    END;
    
    PROCEDURE OBTENER_USUARIO_POR_NOMBRE(p_nombre VARCHAR2, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR 
            SELECT * FROM USUARIOS WHERE NOMBRE_USUARIO = UPPER(p_nombre);
    END;
    
    PROCEDURE OBTENER_USUARIO_POR_DOCUMENTO(p_doc VARCHAR2, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR 
            SELECT * FROM USUARIOS WHERE DOCUMENTO_IDENTIDAD = p_doc;
    END;
    
    PROCEDURE OBTENER_TODOS_USUARIOS(p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR 
            SELECT ID_USUARIO, NOMBRE_USUARIO, NOMBRE_COMPLETO, ROL, DOCUMENTO_IDENTIDAD, ESTADO, FECHA_CREACION
            FROM USUARIOS ORDER BY NOMBRE_COMPLETO;
    END;
    
    PROCEDURE OBTENER_USUARIOS_ACTIVOS(p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR 
            SELECT ID_USUARIO, NOMBRE_USUARIO, NOMBRE_COMPLETO, ROL, DOCUMENTO_IDENTIDAD
            FROM USUARIOS WHERE ESTADO = 'A' ORDER BY NOMBRE_COMPLETO;
    END;
    
    PROCEDURE OBTENER_USUARIOS_POR_ROL(p_rol CHAR, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR 
            SELECT ID_USUARIO, NOMBRE_USUARIO, NOMBRE_COMPLETO, DOCUMENTO_IDENTIDAD, ESTADO
            FROM USUARIOS WHERE ROL = p_rol AND ESTADO = 'A' ORDER BY NOMBRE_COMPLETO;
    END;
    
    -- ========================================
    -- PROCEDIMIENTOS DE CONSULTA: HISTORIAL
    -- ========================================
    
    PROCEDURE OBTENER_HISTORIAL_PARAMETROS(p_clave VARCHAR2, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR 
            SELECT * FROM HISTORIAL_PARAMETROS 
            WHERE CLAVE_PARAMETRO = p_clave ORDER BY FECHA_CAMBIO DESC;
    END;
    
    PROCEDURE OBTENER_HISTORIAL_POR_USUARIO(p_id_usuario NUMBER, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR 
            SELECT * FROM HISTORIAL_PARAMETROS 
            WHERE ID_USUARIO = p_id_usuario ORDER BY FECHA_CAMBIO DESC;
    END;
    
    PROCEDURE OBTENER_HISTORIAL_POR_FECHAS(p_desde DATE, p_hasta DATE, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR 
            SELECT * FROM HISTORIAL_PARAMETROS 
            WHERE FECHA_CAMBIO BETWEEN p_desde AND p_hasta ORDER BY FECHA_CAMBIO DESC;
    END;
    
    PROCEDURE OBTENER_HISTORIAL_DESCUENTOS(p_id_producto NUMBER, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR 
            SELECT * FROM HISTORIAL_DESCUENTOS 
            WHERE ID_PRODUCTO = p_id_producto ORDER BY FECHA_CAMBIO DESC;
    END;
    
    PROCEDURE OBTENER_TODO_HISTORIAL_DESCUENTO(p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR 
            SELECT * FROM HISTORIAL_DESCUENTOS ORDER BY FECHA_CAMBIO DESC;
    END;
    
    -- ========================================
    -- PROCEDIMIENTOS: PRODUCTOS
    -- ========================================
    
    PROCEDURE OBTENER_PRODUCTO_POR_CODIGO(p_codigo VARCHAR2, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR 
            SELECT * FROM PRODUCTOS WHERE CODIGO = p_codigo AND ESTADO = 'A';
    END;
    
    PROCEDURE OBTENER_PRODUCTO_POR_ID(p_id_producto NUMBER, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR 
            SELECT * FROM PRODUCTOS WHERE ID_PRODUCTO = p_id_producto;
    END;
    
    PROCEDURE OBTENER_PRODUCTO_POR_NOMBRE(p_nombre VARCHAR2, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR 
            SELECT * FROM PRODUCTOS WHERE UPPER(NOMBRE) LIKE '%' || UPPER(p_nombre) || '%' AND ESTADO = 'A';
    END;
    
    PROCEDURE OBTENER_TODOS_PRODUCTOS(p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR 
            SELECT * FROM PRODUCTOS WHERE ESTADO = 'A' ORDER BY NOMBRE;
    END;
    
    PROCEDURE INSERTAR_PRODUCTO(
        p_codigo VARCHAR2, p_nombre VARCHAR2, p_descripcion VARCHAR2, 
        p_es_controlado CHAR, p_stock_minimo NUMBER, p_descuento NUMBER
    ) IS
    BEGIN
        INSERT INTO PRODUCTOS (CODIGO, NOMBRE, DESCRIPCION, ES_CONTROLADO, STOCK_MINIMO, DESCUENTO_PROXIMIDAD_VENCIMIENTO)
        VALUES (p_codigo, p_nombre, p_descripcion, p_es_controlado, p_stock_minimo, p_descuento);
        COMMIT;
    END;
    
    PROCEDURE ACTUALIZAR_PRODUCTO(
        p_id_producto NUMBER, p_nombre VARCHAR2, p_descripcion VARCHAR2, 
        p_es_controlado CHAR, p_stock_minimo NUMBER, p_descuento NUMBER
    ) IS
    BEGIN
        UPDATE PRODUCTOS 
        SET NOMBRE = p_nombre, DESCRIPCION = p_descripcion, ES_CONTROLADO = p_es_controlado,
            STOCK_MINIMO = p_stock_minimo, DESCUENTO_PROXIMIDAD_VENCIMIENTO = p_descuento 
        WHERE ID_PRODUCTO = p_id_producto;
        COMMIT;
    END;
    
    PROCEDURE ELIMINAR_PRODUCTO(p_id_producto NUMBER) IS
    BEGIN
        UPDATE PRODUCTOS SET ESTADO = 'I' WHERE ID_PRODUCTO = p_id_producto;
        COMMIT;
    END;
    
    -- ========================================
    -- PROCEDIMIENTOS: CLIENTES
    -- ========================================
    
    PROCEDURE OBTENER_CLIENTE_POR_DOCUMENTO(p_doc VARCHAR2, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR 
            SELECT * FROM CLIENTES WHERE DOCUMENTO = p_doc AND ESTADO = 'A';
    END;
    
    PROCEDURE OBTENER_CLIENTE_POR_ID(p_id_cliente NUMBER, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR 
            SELECT * FROM CLIENTES WHERE ID_CLIENTE = p_id_cliente;
    END;
    
    PROCEDURE OBTENER_CLIENTE_POR_CHAT_ID(p_chat VARCHAR2, p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR 
            SELECT * FROM CLIENTES WHERE CHAT_ID = p_chat AND ESTADO = 'A';
    END;
    
    PROCEDURE OBTENER_CLIENTES_FIDELIZACION(p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR 
            SELECT * FROM CLIENTES WHERE ESTADO = 'A' AND CHAT_ID IS NOT NULL ORDER BY NOMBRE_COMPLETO;
    END;
    
    PROCEDURE INSERTAR_CLIENTE(
        p_doc VARCHAR2, p_nombre VARCHAR2, p_tel VARCHAR2, 
        p_correo VARCHAR2, p_chat VARCHAR2, p_med VARCHAR2
    ) IS
    BEGIN
        INSERT INTO CLIENTES (DOCUMENTO, NOMBRE_COMPLETO, TELEFONO, CORREO, CHAT_ID, MEDICAMENTO_RECURRENTE)
        VALUES (p_doc, p_nombre, p_tel, p_correo, p_chat, p_med);
        COMMIT;
    END;
    
    PROCEDURE ACTUALIZAR_CLIENTE(
        p_id_cliente NUMBER, p_nombre VARCHAR2, p_tel VARCHAR2, 
        p_correo VARCHAR2, p_chat VARCHAR2, p_med VARCHAR2
    ) IS
    BEGIN
        UPDATE CLIENTES 
        SET NOMBRE_COMPLETO = p_nombre, TELEFONO = p_tel, CORREO = p_correo,
            CHAT_ID = p_chat, MEDICAMENTO_RECURRENTE = p_med 
        WHERE ID_CLIENTE = p_id_cliente;
        COMMIT;
    END;
    
    -- ========================================
    -- PROCEDIMIENTOS: PARÁMETROS
    -- ========================================
    
    PROCEDURE OBTENER_TODOS_PARAMETROS(p_cursor OUT SYS_REFCURSOR) IS
    BEGIN
        OPEN p_cursor FOR SELECT * FROM PARAMETROS_SISTEMA ORDER BY CLAVE;
    END;
    
    PROCEDURE INSERTAR_PARAMETRO(p_clave VARCHAR2, p_valor VARCHAR2, p_desc VARCHAR2) IS
    BEGIN
        INSERT INTO PARAMETROS_SISTEMA (CLAVE, VALOR, DESCRIPCION) 
        VALUES (p_clave, p_valor, p_desc);
        COMMIT;
    END;
    
END PKG_PHARMASMART_CONFIG;
/

COMMIT;

PROMPT ========================================
PROMPT Package CONFIG creado con:
PROMPT   - LOGIN (función y procedimiento)
PROMPT   - REGISTRAR_USUARIO (función y procedimiento)
PROMPT   - CAMBIAR_CONTRASENA
PROMPT   - RESETEAR_CONTRASENA (Admin)
PROMPT   - CRUD completo de usuarios
PROMPT ========================================

EXIT;