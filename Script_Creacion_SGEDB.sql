-- Servidor: .\SQLEXPRESS
-- Database: SGEDB

-- -----------------------------------------------------
-- DROP
-- -----------------------------------------------------

DROP TABLE SGE.Administrador
DROP TABLE SGE.ActuadorPorRegla
DROP TABLE SGE.DispositivoPorActuador
DROP TABLE SGE.EstadoDispositivo
DROP TABLE SGE.DispositivoPorCliente
DROP TABLE SGE.DispositivoGenerico
DROP TABLE SGE.Actuador
DROP TABLE SGE.Regla
DROP TABLE SGE.Sensor
DROP TABLE SGE.Cliente
DROP TABLE SGE.Usuario
DROP TABLE SGE.Categoria
DROP TABLE SGE.Transformador
DROP TABLE SGE.Zona
GO
DROP SCHEMA SGE
GO

-- -----------------------------------------------------
-- Schema SGE
-- -----------------------------------------------------

CREATE SCHEMA SGE
GO

-- -----------------------------------------------------
-- Table SGE.Usuario
-- -----------------------------------------------------

CREATE TABLE SGE.Usuario (
  usua_idUsuario INT IDENTITY,
  usua_nombre VARCHAR(100) NOT NULL,
  usua_apellido VARCHAR(100) NOT NULL,
  usua_domicilio VARCHAR(100) NOT NULL,
  usua_username VARCHAR(45) NOT NULL,
  usua_password VARCHAR(64) NOT NULL,
  PRIMARY KEY (usua_idUsuario))
GO

-- -----------------------------------------------------
-- Table SGE.Administrador
-- -----------------------------------------------------

CREATE TABLE SGE.Administrador (
  admin_idUsuario INT NOT NULL,
  admin_fechaAlta DATETIME NOT NULL,
  PRIMARY KEY (admin_idUsuario),
  FOREIGN KEY (admin_idUsuario) REFERENCES SGE.Usuario (usua_idUsuario))
GO

-- -----------------------------------------------------
-- Table SGE.Zona
-- -----------------------------------------------------

CREATE TABLE SGE.Zona (
  zona_idZona INT IDENTITY,
  zona_latitud DECIMAL(9,7),
  zona_longitud DECIMAL(10,7),
  PRIMARY KEY (zona_idZona))
GO

-- -----------------------------------------------------
-- Table SGE.Transformador
-- -----------------------------------------------------

CREATE TABLE SGE.Transformador (
  trans_idTransformador INT IDENTITY,
  trans_activo BIT NOT NULL,
  trans_radar VARCHAR(45),
  trans_zona INT,
  PRIMARY KEY (trans_idTransformador),
  FOREIGN KEY (trans_zona) REFERENCES SGE.Zona (zona_idZona))
GO

-- -----------------------------------------------------
-- Table SGE.Categoria
-- -----------------------------------------------------

CREATE TABLE SGE.Categoria (
  categ_idCategoria INT NOT NULL,
  categ_consumo_min SMALLINT NOT NULL,
  categ_consumo_max SMALLINT NOT NULL,
  categ_cargoFijo DECIMAL(6,3) NOT NULL,
  categ_cargoVariable DECIMAL(7,6) NOT NULL,
  PRIMARY KEY (categ_idCategoria))
GO

-- -----------------------------------------------------
-- Table SGE.Cliente
-- -----------------------------------------------------

CREATE TABLE SGE.Cliente (
  clie_idUsuario INT NOT NULL,
  clie_telefono VARCHAR(15) NOT NULL,
  clie_fechaAlta DATETIME NOT NULL,
  clie_doc_numero VARCHAR(255) NOT NULL,
  clie_doc_tipo VARCHAR(100) NOT NULL,
  clie_Categoria INT NOT NULL,
  clie_puntos INT NOT NULL,
  clie_transformador INT NOT NULL,
  PRIMARY KEY (clie_idUsuario),
  FOREIGN KEY (clie_idUsuario) REFERENCES SGE.Usuario (usua_idUsuario),
  FOREIGN KEY (clie_transformador) REFERENCES SGE.Transformador (trans_idTransformador),
  FOREIGN KEY (clie_Categoria) REFERENCES SGE.Categoria (categ_idCategoria))
GO

-- -----------------------------------------------------
-- Table SGE.Sensor
-- -----------------------------------------------------

CREATE TABLE SGE.Sensor (
  sensor_idSensor INT IDENTITY,
  sensor_idCliente INT,
  sensor_detalle VARCHAR(45),
  sensor_magnitud INT,
  PRIMARY KEY (sensor_idSensor),
  FOREIGN KEY (sensor_idCliente) REFERENCES SGE.Cliente (clie_idUsuario))
GO

-- -----------------------------------------------------
-- Table SGE.Regla
-- -----------------------------------------------------

CREATE TABLE SGE.Regla (
  regla_idRegla INT IDENTITY,
  regla_idSensor INT,
  regla_valor INT,
  PRIMARY KEY (regla_idRegla),
  FOREIGN KEY (regla_idSensor) REFERENCES SGE.Sensor (sensor_idSensor))
GO

-- -----------------------------------------------------
-- Table SGE.Actuador
-- -----------------------------------------------------

CREATE TABLE SGE.Actuador (
  actua_idActuador INT IDENTITY,
  actua_detalle VARCHAR(45),
  PRIMARY KEY (actua_idActuador))
GO

-- -----------------------------------------------------
-- Table SGE.Dispositivo
-- -----------------------------------------------------

CREATE TABLE SGE.DispositivoGenerico (
  disp_idDispositivo INT IDENTITY,
  disp_dispositivo VARCHAR(45) NOT NULL,
  disp_concreto VARCHAR(100) NOT NULL,
  disp_inteligente BIT NOT NULL,
  disp_bajoConsumo BIT NOT NULL,
  disp_consumo DECIMAL(7,6) NOT NULL,
  PRIMARY KEY (disp_idDispositivo))
GO

-- -----------------------------------------------------
-- Table SGE.DispositivoPorCliente
-- -----------------------------------------------------

CREATE TABLE SGE.DispositivoPorCliente (
  dpc_idUsuario INT NOT NULL,
  dpc_idDispositivo INT NOT NULL,
  dpc_numero INT IDENTITY,
  dpc_Estado TINYINT NOT NULL,
  dpc_fechaEstado DATETIME NOT NULL,
  dpc_usoDiario SMALLINT NOT NULL,
  PRIMARY KEY (dpc_idUsuario, dpc_idDispositivo, dpc_numero),
  FOREIGN KEY (dpc_idUsuario) REFERENCES SGE.Cliente (clie_idUsuario),
  FOREIGN KEY (dpc_idDispositivo) REFERENCES SGE.DispositivoGenerico (disp_idDispositivo))
GO

-- -----------------------------------------------------
-- Table SGE.EstadoDispositivo
-- -----------------------------------------------------

CREATE TABLE SGE.EstadoDispositivo (
  edisp_idUsuario INT NOT NULL,
  edisp_idDispositivo INT NOT NULL,
  edisp_numero INT NOT NULL,
  edisp_fechaInicio DATETIME NOT NULL,
  edisp_fechaFin DATETIME NOT NULL,
  edisp_Estado TINYINT NOT NULL,
  PRIMARY KEY (edisp_idUsuario, edisp_idDispositivo , edisp_numero, edisp_fechaInicio, edisp_fechaFin),
  FOREIGN KEY (edisp_idUsuario , edisp_idDispositivo , edisp_numero) 
  REFERENCES SGE.DispositivoPorCliente (dpc_idUsuario , dpc_idDispositivo , dpc_numero))
GO

-- -----------------------------------------------------
-- Table SGE.DispositivoPorActuador
-- -----------------------------------------------------

CREATE TABLE SGE.DispositivoPorActuador (
  dpa_idActuador INT NOT NULL,
  dpa_dpc_idUsuario INT NOT NULL,
  dpa_dpc_idDispositivo INT NOT NULL,
  dpa_dpc_numero INT NOT NULL,
  PRIMARY KEY (dpa_idActuador, dpa_dpc_idUsuario, dpa_dpc_idDispositivo, dpa_dpc_numero),
  FOREIGN KEY (dpa_idActuador) REFERENCES SGE.Actuador (actua_idActuador),
  FOREIGN KEY (dpa_dpc_idUsuario, dpa_dpc_idDispositivo, dpa_dpc_numero) 
  REFERENCES SGE.DispositivoPorCliente (dpc_idUsuario, dpc_idDispositivo, dpc_numero))
GO

-- -----------------------------------------------------
-- Table SGE.ActuadorPorRegla
-- -----------------------------------------------------

CREATE TABLE SGE.ActuadorPorRegla (
  apr_idRegla INT NOT NULL,
  apr_idActuador INT NOT NULL,
  PRIMARY KEY (apr_idRegla, apr_idActuador),
  FOREIGN KEY (apr_idRegla) REFERENCES SGE.Regla (regla_idRegla),
  FOREIGN KEY (apr_idActuador) REFERENCES SGE.Actuador (actua_idActuador))
GO

-- -----------------------------------------------------
-- Insert
-- -----------------------------------------------------

INSERT INTO SGE.Usuario VALUES ('pepe', 'pepon', 'calle 123', 'pepe', '974a2be4c0f6db85c78778e367e905f6f4c1b3524505872ade3ddae1d9ef43b8')
INSERT INTO SGE.Administrador VALUES (1, CONVERT(datetime, '2018-9-15 18:20:23:000', 121))
