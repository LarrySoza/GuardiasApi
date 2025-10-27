CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE roles
(
	rol_id text NOT NULL,
	nombre text NOT NULL,
	CONSTRAINT roles_pk PRIMARY KEY(rol_id)
);

CREATE TABLE usuarios
(
	usuario_id uuid NOT NULL DEFAULT uuid_generate_v4(),
	fecha_registro timestamp with time zone NOT NULL DEFAULT now(),--fecha interna del sistema
    nombre_usuario text NOT NULL,--podria colocar aqui un nombre del usuario o su correo
    clave_hash text NOT NULL,--hash usando PBKDF2
    correo text,
    correo_confirmado boolean NOT NULL DEFAULT false,
    sello_seguridad uuid NOT NULL DEFAULT uuid_generate_v4(),--Un valor aleatorio que debería cambiar siempre que se modifiquen las credenciales de un usuario (se cambie la contraseña por ejemplo)
    bloqueado boolean NOT NULL DEFAULT false,--indica si el usuario esta bloqueado
    fecha_desbloqueo timestamp with time zone,--la fecha en la cual el usuario se desbloquea
    contador_error_clave integer NOT NULL DEFAULT 0,--un conteo del número de veces que intento colocar una clave incorrecta se usa para determinar si se debe bloquear el usuario
    activo boolean NOT NULL DEFAULT true,
    CONSTRAINT usuario_pk PRIMARY KEY(usuario_id),
    CONSTRAINT usuario_unq UNIQUE(nombre_usuario),
    CONSTRAINT correo_unq UNIQUE(correo)
);

CREATE TABLE usuarios_roles
(
	usuario_id uuid NOT NULL,
	rol_id text NOT NULL,
	CONSTRAINT usuarios_roles_pk PRIMARY KEY(usuario_id, rol_id),
	CONSTRAINT usuarios_fkey FOREIGN KEY(usuario_id)
		REFERENCES usuarios(usuario_id) MATCH SIMPLE
		ON UPDATE RESTRICT ON DELETE CASCADE,
	CONSTRAINT roles_fkey FOREIGN KEY(rol_id)
		REFERENCES roles(rol_id) MATCH SIMPLE
		ON UPDATE RESTRICT ON DELETE RESTRICT
);

CREATE TABLE tipos_configuraciones
(
	tipo_configuracion_id text NOT NULL,
	nombre text NOT NULL,--Descripcion de la configuracion
	CONSTRAINT tipos_configuraciones_pk PRIMARY KEY(tipo_configuracion_id)
);

CREATE TABLE configuraciones
(
	tipo_configuracion_id text NOT NULL,
	valor text NOT NULL,
	CONSTRAINT configuraciones_pk PRIMARY KEY (tipo_configuracion_id),
	CONSTRAINT tipos_configuraciones_fk FOREIGN KEY (tipo_configuracion_id)
		REFERENCES tipos_configuraciones(tipo_configuracion_id) MATCH SIMPLE
		ON UPDATE RESTRICT ON DELETE RESTRICT
);

INSERT INTO roles VALUES('A','Administrador');
INSERT INTO roles VALUES('U','Usuario');

--Usuario 'admin' con clave "AaBbCcDs12345"
INSERT INTO usuarios(nombre_usuario,clave_hash) VALUES('admin','ALiah/YdxclgLLhWoIw11aa8F4RcCP1b6f0l12wyENzsRmxPWYPn7I+v4pF93Bc8qg==');
INSERT INTO usuarios_roles SELECT usuario_id,'A' FROM usuarios WHERE nombre_usuario='admin'; 

--Usuario 'demo' con clave "12345"
INSERT INTO usuarios(nombre_usuario,clave_hash) VALUES('demo','AAhNSN2Xs3kSkWj4C40kxzqu8/0lWBrcOs017+xxZ/rBFHLf+fhgXjo192hEEvA6Sg==');
INSERT INTO usuarios_roles SELECT usuario_id,'U' FROM usuarios WHERE nombre_usuario='demo'; 


INSERT INTO tipos_configuraciones VALUES
('jwtConfig','La configuración de la autentificación JWT');

--Esta tabla almacenará las áreas geográficas definidas
CREATE TABLE areas (
    area_id uuid NOT NULL DEFAULT uuid_generate_v4(),
    nombre text NOT NULL,
    coordenadas jsonb NOT NULL,
    fecha_registro timestamp with time zone NOT NULL DEFAULT now(),
    CONSTRAINT areas_pk PRIMARY KEY(area_id),
    CONSTRAINT areas_unq UNIQUE(nombre)
);
