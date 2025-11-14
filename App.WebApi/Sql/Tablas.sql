/*
Nombre de la base de datos: "app"
Fecha de creación:2025-11-01
Autor: Larry <it@gaspersoft.com>
Descripción: Tablas y esquema iniciales para la aplicación GuardiasApi.

Tipo de base de datos: PostgreSQL
Detalles del dialecto / tipos usados:
 - uuid: identificadores universales; uso de la extensión `uuid-ossp` y la función `uuid_generate_v4()` para valores por defecto.
 - text: textos variables.
 - numeric(10,6): coordenadas lat/lng con precisión.
 - timestamp with time zone (timestamptz): marcas de tiempo con zona horaria.
 - jsonb: campos JSON binario para estructuras (p.ej. `equipos_a_cargo`).
 - integer: enteros (p.ej. `radio` para control_point en metros).
 - MATCH SIMPLE ON UPDATE/DELETE RESTRICT: políticas de integridad referencial usadas en FKs.

* Previamente crear la DB y conectarse

 CREATE DATABASE "app";

* Conectarse a la base de datos

 CREATE SCHEMA guard;
 CREATE ROLE guardrole WITH PASSWORD 'VamosP3ru2025';
 GRANT ALL PRIVILEGES ON DATABASE "app" TO guardrole;
 GRANT ALL PRIVILEGES ON SCHEMA guard TO guardrole;
 ALTER ROLE guardrole SET search_path TO guard;
 GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA guard TO guardrole;
 ALTER ROLE guardrole LOGIN;

* Conectarse a la db con el rol guardrole
*/

SET SEARCH_PATH TO guard;

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- =====================
-- Descripción: Catálogo de tipos de configuración del sistema.
-- =====================
CREATE TABLE tipo_configuracion (
 id text NOT NULL,
 nombre text NOT NULL, -- Descripción de la configuración
 CONSTRAINT tipo_configuracion_pk PRIMARY KEY (id)
);

-- =====================
-- Descripción: Valores de configuración por tipo.
-- =====================
CREATE TABLE configuracion (
 id text NOT NULL,
 valor text NOT NULL,
 CONSTRAINT configuracion_pk PRIMARY KEY (id),
 CONSTRAINT configuracion_fk_tipo_configuracion FOREIGN KEY (id)
 REFERENCES tipo_configuracion (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);

-- =====================
-- Descripción: Catálogo de roles del sistema (GUARDIA, SUPERVISOR, ADMIN, ...).
-- =====================
CREATE TABLE rol (
 id text NOT NULL,
 nombre text NOT NULL,
 CONSTRAINT rol_pk PRIMARY KEY (id),
 CONSTRAINT rol_codigo_unq UNIQUE (nombre)
);

-- Insertar roles iniciales (IDs de2 dígitos)
INSERT INTO rol (id, nombre) VALUES
 ('00', 'ADMIN'),
 ('01', 'GUARDIA'),
 ('02', 'SUPERVISOR'),
 ('03', 'CONTROL'),
 ('04', 'LEGAL_REP');
 
-- =====================
-- Descripción: Catálogo de estados de usuario
-- =====================
CREATE TABLE usuario_estado (
 id text NOT NULL,
 nombre text NOT NULL,
 CONSTRAINT usuario_estado_pk PRIMARY KEY (id)
);

INSERT INTO usuario_estado (id,nombre) VALUES
 ('A','ACTIVO'),
 ('I','INACTIVO');

-- =====================
-- Descripción: Catálogo de tipos de documento (p.ej. DNI, PASAPORTE, CEX).
-- =====================
CREATE TABLE tipo_documento (
 id text NOT NULL,
 nombre text NOT NULL,
 CONSTRAINT tipo_documento_pk PRIMARY KEY (id)
);

INSERT INTO tipo_documento (id, nombre) VALUES
 ('1','DNI'),
 ('7','PASAPORTE'),
 ('4','CARNET_EXTRANJERIA');

-- =====================
-- Descripción: Catálogo de turnos (1,2,3,...).
-- =====================
CREATE TABLE turno (
	id integer NOT NULL,
	nombre text NOT NULL,
	CONSTRAINT turno_pk PRIMARY KEY (id)
);

INSERT INTO turno (id, nombre) VALUES
	(1, 'DIURNO'),
	(2, 'NOCTURNO');

-- =========================================
-- Descripción: Catálogo de estados posibles para una alerta de pánico.
-- =========================================
CREATE TABLE panic_alert_estado (
 id text NOT NULL,
 nombre text NOT NULL,
 CONSTRAINT panic_alert_estado_pk PRIMARY KEY (id)
);

-- Insertar estados iniciales (IDs de2 dígitos)
INSERT INTO panic_alert_estado (id, nombre) VALUES
 ('01', 'ENVIADA'),
 ('02', 'ATENDIDA'),
 ('03', 'CANCELADA');

-- =====================
-- Descripción: Catálogo de tipos de adjuntos para alertas de pánico.
-- =====================
CREATE TABLE adjunto_tipo (
 id text NOT NULL,
 nombre text NOT NULL,
 CONSTRAINT adjunto_tipo_pk PRIMARY KEY (id)
);

-- Insertar tipos iniciales (IDs de2 dígitos)
INSERT INTO adjunto_tipo (id, nombre) VALUES
 ('01', 'imagen'),
 ('02', 'audio'),
 ('03', 'texto');

-- =====================
-- Descripción: Usuarios del sistema con credenciales y auditoría.
-- =====================
CREATE TABLE usuario (
 id uuid NOT NULL DEFAULT uuid_generate_v4(),
 nombre_usuario text NOT NULL,
 email text,
 contrasena_hash text NOT NULL,
 sello_seguridad uuid NOT NULL DEFAULT uuid_generate_v4(), -- Un valor aleatorio que debería cambiar cuando se modifiquen credenciales
 telefono text,
 tipo_documento_id text,
 numero_documento text,
 estado text NOT NULL DEFAULT 'A', -- 'A': activo, 'I': inactivo, 'E': eliminado
-- auditoría
 created_at timestamp with time zone DEFAULT now(),
 created_by uuid,
 updated_at timestamp with time zone,
 updated_by uuid,
 deleted_at timestamp with time zone,
 CONSTRAINT usuario_pk PRIMARY KEY (id),
 CONSTRAINT usuario_usuario_unq UNIQUE (nombre_usuario),
 CONSTRAINT usuario_email_unq UNIQUE (email),
 CONSTRAINT usuario_fk_estado FOREIGN KEY (estado) REFERENCES usuario_estado (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT usuario_fk_tipo_documento FOREIGN KEY (tipo_documento_id) REFERENCES tipo_documento (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT usuario_fk_created_by FOREIGN KEY (created_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT usuario_fk_updated_by FOREIGN KEY (updated_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);

--Usuario 'admin' con clave "AaBbCcDs12345"
INSERT INTO usuario(nombre_usuario,contrasena_hash) VALUES('admin','ALiah/YdxclgLLhWoIw11aa8F4RcCP1b6f0l12wyENzsRmxPWYPn7I+v4pF93Bc8qg==');

-- =====================
-- Descripción: Relación N:N entre usuarios y roles.
-- =====================
CREATE TABLE usuario_rol (
 usuario_id uuid NOT NULL,
 rol_id text NOT NULL,
 CONSTRAINT usuario_rol_pk PRIMARY KEY (usuario_id, rol_id),
 CONSTRAINT usuario_rol_fk_usuario FOREIGN KEY (usuario_id) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE CASCADE,
 CONSTRAINT usuario_rol_fk_rol FOREIGN KEY (rol_id) REFERENCES rol (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);

INSERT INTO usuario_rol SELECT id,'00' FROM usuario WHERE nombre_usuario='admin'; 

-- =====================
-- Descripción: Clientes (empresas). `usuario_id` es el usuario responsable.
-- =====================
CREATE TABLE cliente (
 id uuid NOT NULL DEFAULT uuid_generate_v4(),
 razon_social text,
 ruc text,
-- auditoría
 created_at timestamp with time zone DEFAULT now(),
 created_by uuid,
 updated_at timestamp with time zone,
 updated_by uuid,
 deleted_at timestamp with time zone,
 CONSTRAINT cliente_pk PRIMARY KEY (id),
 CONSTRAINT cliente_ruc_unq UNIQUE (ruc),
 CONSTRAINT cliente_fk_created_by FOREIGN KEY (created_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT cliente_fk_updated_by FOREIGN KEY (updated_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);

-- =====================
-- Descripción: Relación N:N entre `cliente` y `usuario`.
-- =====================
CREATE TABLE cliente_usuario (
 cliente_id uuid NOT NULL,
 usuario_id uuid NOT NULL,
-- auditoría
 created_at timestamp with time zone DEFAULT now(),
 created_by uuid,
 updated_at timestamp with time zone,
 updated_by uuid,
 deleted_at timestamp with time zone,
 CONSTRAINT cliente_usuario_pk PRIMARY KEY (cliente_id, usuario_id),
 CONSTRAINT cliente_usuario_fk_cliente FOREIGN KEY (cliente_id) REFERENCES cliente (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT cliente_usuario_fk_usuario FOREIGN KEY (usuario_id) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT cliente_usuario_fk_created_by FOREIGN KEY (created_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT cliente_usuario_fk_updated_by FOREIGN KEY (updated_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);

-- =====================
-- Descripción: Unidades o ubicaciones pertenecientes a un cliente. Permite jerarquía.
-- =====================
CREATE TABLE unidad (
 id uuid NOT NULL DEFAULT uuid_generate_v4(),
 cliente_id uuid NOT NULL,
 unidad_id_padre uuid,
 nombre text NOT NULL,
 direccion text,
-- auditoría
 created_at timestamp with time zone DEFAULT now(),
 created_by uuid,
 updated_at timestamp with time zone,
 updated_by uuid,
 deleted_at timestamp with time zone,
 CONSTRAINT unidad_pk PRIMARY KEY (id),
 CONSTRAINT unidad_fk_cliente FOREIGN KEY (cliente_id) REFERENCES cliente (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT unidad_fk_unidad FOREIGN KEY (unidad_id_padre) REFERENCES unidad (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT unidad_fk_created_by FOREIGN KEY (created_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT unidad_fk_updated_by FOREIGN KEY (updated_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);

-- =====================
-- Descripción: Asignaciones de usuarios a unidades (N:N).
-- =====================
CREATE TABLE usuario_unidad (
 usuario_id uuid NOT NULL,
 unidad_id uuid NOT NULL,
 CONSTRAINT usuario_unidad_pk PRIMARY KEY (usuario_id, unidad_id),
 CONSTRAINT usuario_unidad_fk_usuario FOREIGN KEY (usuario_id) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT usuario_unidad_fk_unidad FOREIGN KEY (unidad_id) REFERENCES unidad (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);

-- =====================
-- Descripción: Puestos dentro de una unidad (ej. guardias asignados a un puesto físico).
-- =====================
CREATE TABLE puesto (
 id uuid NOT NULL DEFAULT uuid_generate_v4(),
 unidad_id uuid NOT NULL, -- FK a unidad.id
 nombre text NOT NULL, -- Nombre del puesto (único)
 lat numeric(10,6),
 lng numeric(10,6),
 -- auditoría
 created_at timestamp with time zone DEFAULT now(),
 created_by uuid,
 updated_at timestamp with time zone,
 updated_by uuid,
 deleted_at timestamp with time zone,
 CONSTRAINT puesto_pk PRIMARY KEY (id),
 CONSTRAINT puesto_nombre_unq UNIQUE (nombre),
 CONSTRAINT puesto_fk_unidad FOREIGN KEY (unidad_id) REFERENCES unidad (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT puesto_fk_created_by FOREIGN KEY (created_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT puesto_fk_updated_by FOREIGN KEY (updated_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);

-- =====================
-- Descripción: Asignaciones de turnos a puestos (tabla intermedia).
-- Relaciona un `turno` (turno.id) con un `puesto` (puesto.id). Incluye campos de auditoría para trazabilidad.
-- =====================
CREATE TABLE puesto_turno (
	puesto_id uuid NOT NULL,
    turno_id integer NOT NULL,
	CONSTRAINT puesto_turno_pk PRIMARY KEY (puesto_id,turno_id),
	CONSTRAINT puesto_turno_fk_turno FOREIGN KEY (turno_id) REFERENCES turno(id) ON DELETE RESTRICT ON UPDATE RESTRICT,
	CONSTRAINT puesto_turno_fk_puesto FOREIGN KEY (puesto_id) REFERENCES puesto(id) ON DELETE RESTRICT ON UPDATE RESTRICT
);

-- =====================
-- Descripción: Asignaciones de usuarios a unidades (N:N).
-- =====================
CREATE TABLE usuario_puesto (
 usuario_id uuid NOT NULL,
 puesto_id uuid NOT NULL,
 CONSTRAINT usuario_puesto_pk PRIMARY KEY (usuario_id, unidad_id),
 CONSTRAINT usuario_puesto_fk_usuario FOREIGN KEY (usuario_id) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT usuario_puesto_fk_puesto FOREIGN KEY (puesto_id) REFERENCES puesto (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);

-- =====================
-- Descripción: Registra sesiones de usuario (inicio/fin), foto inicial, equipos a cargo y cierre.
-- =====================
CREATE TABLE sesion_usuario (
 id uuid NOT NULL DEFAULT uuid_generate_v4(),
 usuario_id uuid NOT NULL,
 cliente_id uuid,
 unidad_id uuid NOT NULL,
 puesto_id uuid,
 turno_id integer,
 fecha_inicio timestamp with time zone DEFAULT now(),
 ruta_foto_inicio text,
-- Cierre (único por sesión)
 fecha_fin timestamp with time zone,
 equipos_a_cargo jsonb, -- checklist consolidado
 otros_detalle text,
 descripcion_cierre text,
-- auditoría
 created_at timestamp with time zone DEFAULT now(),
 created_by uuid,
 updated_at timestamp with time zone,
 updated_by uuid,
 deleted_at timestamp with time zone,
 CONSTRAINT sesion_usuario_pk PRIMARY KEY (id),
 CONSTRAINT sesion_usuario_fk_usuario FOREIGN KEY (usuario_id) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT sesion_usuario_fk_cliente FOREIGN KEY (cliente_id) REFERENCES cliente (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT sesion_usuario_fk_unidad FOREIGN KEY (unidad_id) REFERENCES unidad (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT sesion_usuario_fk_puesto FOREIGN KEY (puesto_id) REFERENCES puesto (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT sesion_usuario_fk_turno FOREIGN KEY (turno_id) REFERENCES turno (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT sesion_usuario_fk_created_by FOREIGN KEY (created_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT sesion_usuario_fk_updated_by FOREIGN KEY (updated_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);

-- =====================
-- Descripción: Evidencias (fotos) asociadas a una sesión.
-- =====================
CREATE TABLE sesion_usuario_evidencia (
 id uuid NOT NULL DEFAULT uuid_generate_v4(),
 sesion_usuario_id uuid NOT NULL,
 ruta_foto text,
-- auditoría
 created_at timestamp with time zone DEFAULT now(),
 created_by uuid,
 updated_at timestamp with time zone,
 updated_by uuid,
 deleted_at timestamp with time zone,
 CONSTRAINT sesion_usuario_evidencia_pk PRIMARY KEY (id),
 CONSTRAINT sesion_usuario_evidencia_fk_sesion FOREIGN KEY (sesion_usuario_id) REFERENCES sesion_usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT sesion_usuario_evidencia_fk_created_by FOREIGN KEY (created_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT sesion_usuario_evidencia_fk_updated_by FOREIGN KEY (updated_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);

-- =========================================
-- Descripción: Registro de alertas de pánico enlazadas a sesiones.
-- =========================================
CREATE TABLE panic_alert (
 id uuid NOT NULL DEFAULT uuid_generate_v4(),
 sesion_usuario_id uuid NOT NULL,
 fecha_hora timestamp with time zone DEFAULT now(),
 lat numeric(10,6),
 lng numeric(10,6),
 mensaje text, --Se actualizar al momento de atender la emergencia	
 estado_id text NOT NULL,
-- auditoría
 created_at timestamp with time zone DEFAULT now(),
 created_by uuid,
 updated_at timestamp with time zone,
 updated_by uuid,
 deleted_at timestamp with time zone,
 CONSTRAINT panic_alert_pk PRIMARY KEY (id),
 CONSTRAINT panic_alert_fk_sesion FOREIGN KEY (sesion_usuario_id) REFERENCES sesion_usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT panic_alert_fk_estado FOREIGN KEY (estado_id) REFERENCES panic_alert_estado (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT panic_alert_fk_created_by FOREIGN KEY (created_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT panic_alert_fk_updated_by FOREIGN KEY (updated_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);

-- =====================
-- Descripción: Archivos (imagen/audio/texto) asociados a una panic_alert.
-- =====================
CREATE TABLE panic_alert_adjunto (
 id uuid NOT NULL DEFAULT uuid_generate_v4(),
 panic_alert_id uuid NOT NULL,
 adjunto_tipo_id text NOT NULL,
 ruta text NOT NULL,
 -- auditoría
 created_at timestamp with time zone DEFAULT now(),
 created_by uuid,
 updated_at timestamp with time zone,
 updated_by uuid,
 deleted_at timestamp with time zone,
 CONSTRAINT panic_alert_adjunto_pk PRIMARY KEY (id),
 CONSTRAINT panic_alert_adjunto_fk_alert FOREIGN KEY (panic_alert_id) REFERENCES panic_alert (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT panic_alert_adjunto_fk_adjunto_tipo FOREIGN KEY (adjunto_tipo_id) REFERENCES adjunto_tipo (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT panic_alert_adjunto_fk_created_by FOREIGN KEY (created_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT panic_alert_adjunto_fk_updated_by FOREIGN KEY (updated_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);

-- =====================
-- Descripción: Registra la recepción/atención de una alerta de pánico por un usuario receptor.
-- =====================
CREATE TABLE panic_alert_recepcion (
 id uuid NOT NULL DEFAULT uuid_generate_v4(),
 panic_alert_id uuid NOT NULL, -- FK a panic_alert.id
 usuario_receptor_id uuid NOT NULL, -- FK a usuario.id
 fecha_hora timestamp with time zone DEFAULT now(),
 -- auditoría
 created_at timestamp with time zone DEFAULT now(),
 created_by uuid,
 updated_at timestamp with time zone,
 updated_by uuid,
 deleted_at timestamp with time zone,
 CONSTRAINT panic_alert_recepcion_pk PRIMARY KEY (id),
 CONSTRAINT panic_alert_recepcion_fk_alert FOREIGN KEY (panic_alert_id) REFERENCES panic_alert (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT panic_alert_recepcion_fk_usuario FOREIGN KEY (usuario_receptor_id) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT panic_alert_recepcion_fk_created_by FOREIGN KEY (created_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT panic_alert_recepcion_fk_updated_by FOREIGN KEY (updated_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);


-- =========================================
-- Descripción: Puntos de control (QR/GPS) asociados a unidad.
-- =========================================
CREATE TABLE control_point (
 id uuid NOT NULL DEFAULT uuid_generate_v4(),
 unidad_id uuid NOT NULL,
 nombre text NOT NULL,
 codigo_qr text,
 lat numeric(10,6),
 lng numeric(10,6),
 radio integer, -- radio en metros (opcional)
 -- auditoría
 created_at timestamp with time zone DEFAULT now(),
 created_by uuid,
 updated_at timestamp with time zone,
 updated_by uuid,
 deleted_at timestamp with time zone,
 CONSTRAINT control_point_pk PRIMARY KEY (id),
 CONSTRAINT control_point_nombre_unq UNIQUE (nombre),
 CONSTRAINT control_point_codigo_qr_unq UNIQUE (codigo_qr),
 CONSTRAINT control_point_fk_unidad FOREIGN KEY (unidad_id) REFERENCES unidad (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT control_point_fk_created_by FOREIGN KEY (created_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT control_point_fk_updated_by FOREIGN KEY (updated_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);

-- =========================================
-- Descripción: Registro de rondas (puntos de control o lecturas GPS) realizadas durante una sesión.
-- =========================================
CREATE TABLE ronda (
 id uuid NOT NULL DEFAULT uuid_generate_v4(),
 sesion_usuario_id uuid, -- FK a sesion_usuario.id (opcional)
 control_point_id uuid, -- FK a control_point.id (NULL si lectura por GPS sin control_point)
 fecha_hora timestamp with time zone DEFAULT now(),
 lat numeric(10,6),
 lng numeric(10,6),
 descripcion text,
-- auditoría
 created_at timestamp with time zone DEFAULT now(),
 created_by uuid,
 updated_at timestamp with time zone,
 updated_by uuid,
 deleted_at timestamp with time zone,
 CONSTRAINT ronda_pk PRIMARY KEY (id),
 CONSTRAINT ronda_fk_sesion FOREIGN KEY (sesion_usuario_id) REFERENCES sesion_usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT ronda_fk_control_point FOREIGN KEY (control_point_id) REFERENCES control_point (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT ronda_fk_created_by FOREIGN KEY (created_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT ronda_fk_updated_by FOREIGN KEY (updated_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);

-- =========================================
-- Descripción: Archivos/adjuntos (fotos) asociados a una ronda.
-- =========================================
CREATE TABLE ronda_adjunto (
 id uuid NOT NULL DEFAULT uuid_generate_v4(),
 ronda_id uuid NOT NULL, -- FK a ronda.id
 ruta text,
 -- auditoría
 created_at timestamp with time zone DEFAULT now(),
 created_by uuid,
 updated_at timestamp with time zone,
 updated_by uuid,
 deleted_at timestamp with time zone,
 CONSTRAINT ronda_adjunto_pk PRIMARY KEY (id),
 CONSTRAINT ronda_adjunto_fk_ronda FOREIGN KEY (ronda_id) REFERENCES ronda (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT ronda_adjunto_fk_created_by FOREIGN KEY (created_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT ronda_adjunto_fk_updated_by FOREIGN KEY (updated_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);

-- =====================
-- Descripción: Catálogo de estados para alive_check (2 dígitos)
-- =====================
CREATE TABLE alive_check_estado (
 id text NOT NULL,
 nombre text NOT NULL,
 CONSTRAINT alive_check_estado_pk PRIMARY KEY (id)
);

INSERT INTO alive_check_estado (id, nombre) VALUES
 ('01', 'ENVIADA'),
 ('02', 'ACEPTADA'),
 ('03', 'NO_ACEPTADA'),
 ('04', 'EXPIRADA');

-- =====================
-- Descripción: Mensajes de verificación "Hombre Vivo" enviados desde control/supervisor hacia la sesión de un guardia.
-- =====================
CREATE TABLE alive_check (
 id uuid NOT NULL DEFAULT uuid_generate_v4(),
 usuario_emisor_id uuid NOT NULL, -- FK a usuario(id) (CONTROL o SUPERVISOR)
 sesion_usuario_dest_id uuid, -- FK a sesion_usuario(id) (sesión del guardia)
 fecha_hora_envio timestamp with time zone DEFAULT now(),
 timeout_seg int, -- p.ej.12
 estado_id text NOT NULL DEFAULT '01', -- FK a alive_check_estado(id)
 -- auditoría
 created_at timestamp with time zone DEFAULT now(),
 created_by uuid,
 updated_at timestamp with time zone,
 updated_by uuid,
 deleted_at timestamp with time zone,
 CONSTRAINT alive_check_pk PRIMARY KEY (id),
 CONSTRAINT alive_check_fk_usuario_emisor FOREIGN KEY (usuario_emisor_id) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT alive_check_fk_sesion_dest FOREIGN KEY (sesion_usuario_dest_id) REFERENCES sesion_usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT alive_check_fk_estado FOREIGN KEY (estado_id) REFERENCES alive_check_estado (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);

-- =====================
-- Descripción: Respuestas a los alive_check por parte del guardia (sesión destino).
-- =====================
CREATE TABLE alive_check_respuesta (
 id uuid NOT NULL DEFAULT uuid_generate_v4(),
 alive_check_id uuid NOT NULL,
 sesion_usuario_respuesta_id uuid, -- FK a sesion_usuario(id)
 fecha_hora_respuesta timestamp with time zone,
 aceptado boolean,
 ruta_foto text,
 lat numeric(10,6),
 lng numeric(10,6),
 -- auditoría
 created_at timestamp with time zone DEFAULT now(),
 created_by uuid,
 updated_at timestamp with time zone,
 updated_by uuid,
 deleted_at timestamp with time zone,
 CONSTRAINT alive_check_respuesta_pk PRIMARY KEY (id),
 CONSTRAINT alive_check_respuesta_fk_alive_check FOREIGN KEY (alive_check_id) REFERENCES alive_check (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT alive_check_respuesta_fk_sesion_respuesta FOREIGN KEY (sesion_usuario_respuesta_id) REFERENCES sesion_usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);

-- =========================================
-- Asignaciones (por cliente/unidad)
-- Descripción: Tareas/Asignaciones asociadas a un cliente/unidad y opcionalmente asignadas a un usuario.
-- =========================================
CREATE TABLE asignacion (
 id uuid NOT NULL DEFAULT uuid_generate_v4(),
 cliente_id uuid,
 unidad_id uuid,
 codigo text,
 tipo text,
 direccion text,
 descripcion text,
 lat numeric(10,6),
 lng numeric(10,6),
 usuario_asignado_id uuid,
 estado text, -- PENDIENTE | EN_PROCESO | ATENDIDA | CANCELADA
 fecha_creacion timestamp with time zone DEFAULT now(),
 -- auditoría
 created_at timestamp with time zone DEFAULT now(),
 created_by uuid,
 updated_at timestamp with time zone,
 updated_by uuid,
 deleted_at timestamp with time zone,
 CONSTRAINT asignacion_pk PRIMARY KEY (id),
 CONSTRAINT asignacion_fk_cliente FOREIGN KEY (cliente_id) REFERENCES cliente (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT asignacion_fk_unidad FOREIGN KEY (unidad_id) REFERENCES unidad (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT asignacion_fk_usuario_asignado FOREIGN KEY (usuario_asignado_id) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT asignacion_fk_created_by FOREIGN KEY (created_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT asignacion_fk_updated_by FOREIGN KEY (updated_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);


-- =========================================
-- Descripción: Catálogo de tipos de acción para eventos de asignación.
-- Valores: 'I' = INICIAR, 'F' = FINALIZAR, 'C' = CANCELAR
-- =========================================
CREATE TABLE asignacion_evento_tipo (
 id text NOT NULL,
 nombre text NOT NULL,
 CONSTRAINT asignacion_evento_tipo_pk PRIMARY KEY (id)
);

INSERT INTO asignacion_evento_tipo (id, nombre) VALUES
 ('I', 'INICIAR'),
 ('F', 'FINALIZAR'),
 ('C', 'CANCELAR');

-- =========================================
-- Descripción: Eventos/acciones realizadas sobre una asignación (p.ej. inicio, finalización, cancelación).
-- =========================================
CREATE TABLE asignacion_evento (
 id uuid NOT NULL DEFAULT uuid_generate_v4(),
 asignacion_id uuid NOT NULL,
 sesion_usuario_id uuid,
 fecha_hora timestamp with time zone,
 accion_id text NOT NULL DEFAULT 'I', -- FK a asignacion_evento_tipo(id)
 -- auditoría
 created_at timestamp with time zone DEFAULT now(),
 created_by uuid,
 updated_at timestamp with time zone,
 updated_by uuid,
 deleted_at timestamp with time zone,
 CONSTRAINT asignacion_evento_pk PRIMARY KEY (id),
 CONSTRAINT asignacion_evento_fk_asignacion FOREIGN KEY (asignacion_id) REFERENCES asignacion (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT asignacion_evento_fk_sesion FOREIGN KEY (sesion_usuario_id) REFERENCES sesion_usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT asignacion_evento_fk_accion FOREIGN KEY (accion_id) REFERENCES asignacion_evento_tipo (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT asignacion_evento_fk_created_by FOREIGN KEY (created_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
 CONSTRAINT asignacion_evento_fk_updated_by FOREIGN KEY (updated_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);

-- =====================
-- Catálogo independiente de categorías de evento
-- =====================
CREATE TABLE evento_categoria (
    id text NOT NULL,
    nombre text NOT NULL,
    CONSTRAINT evento_categoria_pk PRIMARY KEY (id)
);

INSERT INTO evento_categoria (id, nombre) VALUES
 ('I', 'INCIDENCIA'),
 ('O', 'OCURRENCIA');

-- =====================
-- Unifica la tabla incidencia_tipo y ocurrencia_tipo
-- =====================
CREATE TABLE evento_tipo (
    id uuid NOT NULL DEFAULT uuid_generate_v4(),
    cliente_id uuid, -- NULL = global
    nombre text NOT NULL,
    categoria_id text NOT NULL,
    created_at timestamptz DEFAULT now(),
    created_by uuid,
    updated_at timestamptz,
    updated_by uuid,
    deleted_at timestamptz,
    CONSTRAINT evento_tipo_pk PRIMARY KEY (id),
    CONSTRAINT evento_tipo_unq UNIQUE (cliente_id, nombre, categoria_id),
    CONSTRAINT evento_tipo_fk_cliente FOREIGN KEY (cliente_id) REFERENCES cliente (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
    CONSTRAINT evento_tipo_fk_categoria FOREIGN KEY (categoria_id) REFERENCES evento_categoria (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
    CONSTRAINT evento_tipo_fk_created_by FOREIGN KEY (created_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
    CONSTRAINT evento_tipo_fk_updated_by FOREIGN KEY (updated_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);

-- =====================
-- Unifica la tabla ocurrencia y incidencia
-- =====================
CREATE TABLE evento (
    id uuid NOT NULL DEFAULT uuid_generate_v4(),
    sesion_usuario_id uuid,      
    ronda_id uuid,               
    puesto_id uuid,              
    tipo_evento_id uuid NOT NULL,
    descripcion text,
    fecha_hora timestamptz DEFAULT now(),
    lat numeric(10,6),
    lng numeric(10,6),
    turno_id integer,
    -- campos de auditoría (mantener esquema existente)
    created_at timestamptz DEFAULT now(),
    created_by uuid,
    updated_at timestamptz,
    updated_by uuid,
    deleted_at timestamptz,
    CONSTRAINT evento_pk PRIMARY KEY (id),
    CONSTRAINT evento_fk_sesion FOREIGN KEY (sesion_usuario_id) REFERENCES sesion_usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
    CONSTRAINT evento_fk_ronda FOREIGN KEY (ronda_id) REFERENCES ronda (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
    CONSTRAINT evento_fk_puesto FOREIGN KEY (puesto_id) REFERENCES puesto (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
    CONSTRAINT evento_fk_tipo FOREIGN KEY (tipo_evento_id) REFERENCES evento_tipo (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
    CONSTRAINT evento_fk_created_by FOREIGN KEY (created_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
    CONSTRAINT evento_fk_updated_by FOREIGN KEY (updated_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);

-- =====================
-- Unifica la tabla incidencia_adjunto y ocurrencia_adjunto 
-- =====================
CREATE TABLE evento_adjunto (
    id uuid NOT NULL DEFAULT uuid_generate_v4(),
    evento_id uuid NOT NULL,
    adjunto_tipo_id text NOT NULL,
	ruta text NOT NULL,
    created_at timestamptz DEFAULT now(),
    created_by uuid,
    updated_at timestamptz,
    updated_by uuid,
    deleted_at timestamptz,
    CONSTRAINT evento_adjunto_pk PRIMARY KEY (id),
    CONSTRAINT evento_adjunto_fk_evento FOREIGN KEY (evento_id) REFERENCES evento (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
    CONSTRAINT evento_adjunto_fk_adjunto_tipo FOREIGN KEY (adjunto_tipo_id) REFERENCES adjunto_tipo (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
	CONSTRAINT evento_adjunto_fk_created_by FOREIGN KEY (created_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT,
	CONSTRAINT evento_adjunto_fk_updated_by FOREIGN KEY (updated_by) REFERENCES usuario (id) MATCH SIMPLE ON UPDATE RESTRICT ON DELETE RESTRICT
);

/* Limpiar tablas (deshacer todo el script)

DROP TABLE IF EXISTS evento_adjunto;
DROP TABLE IF EXISTS evento;
DROP TABLE IF EXISTS evento_tipo;
DROP TABLE IF EXISTS evento_categoria;

DROP TABLE IF EXISTS asignacion_evento;
DROP TABLE IF EXISTS asignacion_evento_tipo;
DROP TABLE IF EXISTS asignacion;

DROP TABLE IF EXISTS alive_check_respuesta;
DROP TABLE IF EXISTS alive_check;
DROP TABLE IF EXISTS alive_check_estado;

DROP TABLE IF EXISTS ronda_adjunto;
DROP TABLE IF EXISTS ronda;
DROP TABLE IF EXISTS control_point;

DROP TABLE IF EXISTS panic_alert_recepcion;
DROP TABLE IF EXISTS panic_alert_adjunto;
DROP TABLE IF EXISTS adjunto_tipo;
DROP TABLE IF EXISTS panic_alert;
DROP TABLE IF EXISTS panic_alert_estado;

DROP TABLE IF EXISTS sesion_usuario_evidencia;
DROP TABLE IF EXISTS sesion_usuario;

DROP TABLE IF EXISTS puesto_turno;
DROP TABLE IF EXISTS turno;
DROP TABLE IF EXISTS puesto;

DROP TABLE IF EXISTS usuario_unidad;
DROP TABLE IF EXISTS unidad;

DROP TABLE IF EXISTS cliente_usuario;
DROP TABLE IF EXISTS cliente;

DROP TABLE IF EXISTS usuario_rol;
DROP TABLE IF EXISTS usuario;

DROP TABLE IF EXISTS tipo_documento;
DROP TABLE IF EXISTS usuario_estado;
DROP TABLE IF EXISTS rol;

DROP TABLE IF EXISTS configuracion;
DROP TABLE IF EXISTS tipo_configuracion;

--Ejecutar al final
DROP EXTENSION IF EXISTS "uuid-ossp";

*/