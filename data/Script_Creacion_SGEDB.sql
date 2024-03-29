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
DROP TABLE SGE.EstadoSensor
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
  zona_latitud DECIMAL(9,7) NOT NULL,  
  zona_longitud DECIMAL(10,7) NOT NULL,  
  zona_radio INT NOT NULL,
  PRIMARY KEY (zona_idZona))
GO

-- -----------------------------------------------------
-- Table SGE.Transformador
-- -----------------------------------------------------

CREATE TABLE SGE.Transformador (
  trans_idTransformador INT IDENTITY,
  trans_activo BIT NOT NULL,
  trans_latitud DECIMAL(9,7) NOT NULL,
  trans_longitud DECIMAL(10,7) NOT NULL,  
  trans_zona INT NOT NULL,
  PRIMARY KEY (trans_idTransformador),
  FOREIGN KEY (trans_zona) REFERENCES SGE.Zona (zona_idZona))
GO

-- -----------------------------------------------------
-- Table SGE.Categoria
-- -----------------------------------------------------

CREATE TABLE SGE.Categoria (
  categ_idCategoria VARCHAR(3) NOT NULL,
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
  clie_latitud DECIMAL(9,7) NOT NULL,
  clie_longitud DECIMAL(10,7) NOT NULL,  
  clie_telefono VARCHAR(15) NOT NULL,
  clie_fechaAlta DATETIME NOT NULL,
  clie_doc_numero VARCHAR(255) NOT NULL,
  clie_doc_tipo VARCHAR(100) NOT NULL,
  clie_categoria VARCHAR(3) NOT NULL,
  clie_puntos INT NOT NULL,
  clie_transformador INT,
  clie_autoSimplex BIT NOT NULL,
  PRIMARY KEY (clie_idUsuario),
  FOREIGN KEY (clie_idUsuario) REFERENCES SGE.Usuario (usua_idUsuario),
  FOREIGN KEY (clie_transformador) REFERENCES SGE.Transformador (trans_idTransformador),
  FOREIGN KEY (clie_categoria) REFERENCES SGE.Categoria (categ_idCategoria))
GO

-- -----------------------------------------------------
-- Table SGE.Sensor
-- -----------------------------------------------------

CREATE TABLE SGE.Sensor (
  sensor_idSensor INT IDENTITY,
  sensor_idCliente INT NOT NULL,
  sensor_detalle VARCHAR(45),
  sensor_magnitud INT,
  sensor_eliminado DATETIME,
  PRIMARY KEY (sensor_idSensor),
  FOREIGN KEY (sensor_idCliente) REFERENCES SGE.Cliente (clie_idUsuario))
GO

-- -----------------------------------------------------
-- Table SGE.Regla
-- -----------------------------------------------------

CREATE TABLE SGE.Regla (
  regla_idRegla INT IDENTITY,
  regla_idSensor INT NOT NULL,
  regla_detalle VARCHAR(45),
  regla_valor INT,
  regla_operador VARCHAR(2),
  regla_accion VARCHAR(16),
  PRIMARY KEY (regla_idRegla),
  FOREIGN KEY (regla_idSensor) REFERENCES SGE.Sensor (sensor_idSensor))
GO

-- -----------------------------------------------------
-- Table SGE.Actuador
-- -----------------------------------------------------

CREATE TABLE SGE.Actuador (
  actua_idActuador INT IDENTITY,
  actua_idCliente INT NOT NULL,
  actua_detalle VARCHAR(45) NOT NULL,
  PRIMARY KEY (actua_idActuador),
  FOREIGN KEY (actua_idCliente) REFERENCES SGE.Usuario (usua_idUsuario))
GO

-- -----------------------------------------------------
-- Table SGE.DispositivoGenerico
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
  dpc_numero INT NOT NULL,
  dpc_estado TINYINT,
  dpc_fechaEstado DATETIME,
  dpc_usoDiario TINYINT,
  dpc_convertido BIT NOT NULL,
  dpc_eliminado DATETIME,
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
  edisp_estado TINYINT NOT NULL,
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
-- Table SGE.EstadoSensor
-- -----------------------------------------------------

CREATE TABLE SGE.EstadoSensor (
  esensor_idEstadoSensor INT IDENTITY,
  esensor_idSensor INT NOT NULL,
  esensor_magnitud INT NOT NULL,
  PRIMARY KEY (esensor_idEstadoSensor, esensor_idSensor, esensor_magnitud),
  FOREIGN KEY (esensor_idSensor) REFERENCES SGE.Sensor (sensor_idSensor))
GO

-- -----------------------------------------------------
-- Insert
-- -----------------------------------------------------

INSERT INTO SGE.Categoria VALUES ('R1',0,150,18.76,0.644)
INSERT INTO SGE.Categoria VALUES ('R2',150,325,35.32,0.644)
INSERT INTO SGE.Categoria VALUES ('R3',325,400,60.71,0.681)
INSERT INTO SGE.Categoria VALUES ('R4',450,450,71.74,0.738)
INSERT INTO SGE.Categoria VALUES ('R5',450,500,110.38,0.794)
INSERT INTO SGE.Categoria VALUES ('R6',500,600,220.75,0.832)
INSERT INTO SGE.Categoria VALUES ('R7',600,700,443.59,0.851)
INSERT INTO SGE.Categoria VALUES ('R8',700,1400,545.96,0.851)
INSERT INTO SGE.Categoria VALUES ('R9',1400,32767,887.19,0.851)

INSERT INTO SGE.DispositivoGenerico VALUES('Aire-Acondicionado', '3500 frigorias',1, 0, 1.163)
INSERT INTO SGE.DispositivoGenerico VALUES('Aire-Acondicionado', '2200 frigorias', 1, 1, 1.013)
INSERT INTO SGE.DispositivoGenerico VALUES('Televisor', 'color de tubo fluorecente de 21', 0, 0, 0.075)
INSERT INTO SGE.DispositivoGenerico VALUES('Televisor', 'color de tubo fluorecente de 29 a 34', 0, 0, 0.175)
INSERT INTO SGE.DispositivoGenerico VALUES('Televisor', 'LCD de 40', 0, 0, 0.18)
INSERT INTO SGE.DispositivoGenerico VALUES('Televisor', 'LED 24', 1, 1, 0.004)
INSERT INTO SGE.DispositivoGenerico VALUES('Televisor', 'LED 32', 1, 1, 0.055)
INSERT INTO SGE.DispositivoGenerico VALUES('Televisor', 'LED 40', 1, 1, 0.08)
INSERT INTO SGE.DispositivoGenerico VALUES('Heladera', 'con freezer', 1, 1, 0.09)
INSERT INTO SGE.DispositivoGenerico VALUES('Heladera', 'sin freezer', 1, 1, 0.075)
INSERT INTO SGE.DispositivoGenerico VALUES('Lavarropas', 'automatico 5 kg con calentamiento de agua', 0, 0, 0.875)
INSERT INTO SGE.DispositivoGenerico VALUES('Lavarropas', 'automatico 5kg', 1, 1, 0.175)
INSERT INTO SGE.DispositivoGenerico VALUES('Lavarropas', 'semi-automatico 5kg', 0, 1, 0.1275)
INSERT INTO SGE.DispositivoGenerico VALUES('Ventilador', 'de pie', 0, 1, 0.09)
INSERT INTO SGE.DispositivoGenerico VALUES('Ventilador', 'de techo', 0, 1, 0.06)
INSERT INTO SGE.DispositivoGenerico VALUES('Lampara', 'halogenas de 40 w', 1, 0, 0.04)
INSERT INTO SGE.DispositivoGenerico VALUES('Lampara', 'halogenas de 60 w', 1, 0, 0.06)
INSERT INTO SGE.DispositivoGenerico VALUES('Lampara', 'halogenas de 100 w', 1, 0, 0.015)
INSERT INTO SGE.DispositivoGenerico VALUES('Lampara', 'de 11 w', 1, 1, 0.011)
INSERT INTO SGE.DispositivoGenerico VALUES('Lampara', 'de 11 w', 1, 1, 0.011)
INSERT INTO SGE.DispositivoGenerico VALUES('Lampara', 'de 15 w', 1, 1, 0.015)
INSERT INTO SGE.DispositivoGenerico VALUES('Lampara', 'de 20 w', 1, 1, 0.02)
INSERT INTO SGE.DispositivoGenerico VALUES('PC', 'de escritorio', 1, 1, 0.04)
INSERT INTO SGE.DispositivoGenerico VALUES('Microondas', 'convencional', 0, 1, 0.64)
INSERT INTO SGE.DispositivoGenerico VALUES('Plancha', 'a vapor', 0, 1, 0.75)

INSERT INTO SGE.Zona VALUES (-34.550503, -58.479966, 5000)
INSERT INTO SGE.Zona VALUES (-34.631230, -58.499324, 5000)
INSERT INTO SGE.Zona VALUES (-34.597430, -58.412424, 5000)
INSERT INTO SGE.Zona VALUES (-34.671469, -58.416247, 5000)
INSERT INTO SGE.Zona VALUES (-39.691680, -66.840570, 5000)
INSERT INTO SGE.Transformador VALUES (3, -34.604048, -58.381673, 1)
INSERT INTO SGE.Transformador VALUES (1, -34.553750, -58.468923, 1)
INSERT INTO SGE.Transformador VALUES (4, -34.661431, -58.410289, 1)

INSERT INTO SGE.Usuario VALUES ('pepe', 'perez', 'Buenos Aires, Rivadavia 1554', 'pepe', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4') 
-- username: pepe 
-- password: 1234
INSERT INTO SGE.Administrador VALUES (1, CONVERT(datetime, '2018-9-15 18:20:23:000', 121))

INSERT INTO SGE.Usuario VALUES ('marco', 'polo', 'Buenos Aires, Av. Acoyte 555', 'marco', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4')
-- username: marco
-- password: 1234
INSERT INTO SGE.Cliente VALUES (2, -34.612051, -58.438716, '9999999999', GETDATE(), '88888888', 'DNI', 'R1', 45, 1, 0)

INSERT INTO SGE.Usuario VALUES ('juan', 'mesaglio', 'Buenos Aires, Av. Acoyte 555', 'juan', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4')
INSERT INTO SGE.Cliente VALUES (3, -34.612051, -58.438716, '44554455', GETDATE(), '45999999', 'DNI', 'R1', 0, 1, 0)
INSERT INTO SGE.Usuario VALUES ('nico', 'perez', 'Buenos Aires, Av. Acoyte 555', 'nico', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4')
INSERT INTO SGE.Cliente VALUES (4, -34.612051, -58.438716, '44554325', GETDATE(), '45992329', 'DNI', 'R1', 0, 1, 0)

INSERT INTO SGE.Usuario VALUES ('lucas', 'lopez', 'Buenos Aires, Av. Acoyte 555', 'lucas', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4')
INSERT INTO SGE.Administrador VALUES (5, CONVERT(datetime, '2018-9-15 18:20:23:000', 121))
INSERT INTO SGE.Usuario VALUES ('martin', 'lopez', 'Buenos Aires, Av. Acoyte 555', 'martin', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4')
INSERT INTO SGE.Administrador VALUES (6, CONVERT(datetime, '2018-9-15 18:20:23:000', 121))

INSERT INTO SGE.DispositivoPorCliente VALUES (2, 8, 1, 0, CONVERT(datetime,'2018-10-29 20:12:53:242',121), NULL, 0, NULL)
INSERT INTO SGE.DispositivoPorCliente VALUES (2, 9, 1, 1, CONVERT(datetime,'2018-10-29 20:12:53:242',121), NULL, 0, NULL)
INSERT INTO SGE.DispositivoPorCliente VALUES (2, 14, 1, NULL, NULL, 3, 0, NULL)
INSERT INTO SGE.DispositivoPorCliente VALUES (2, 1, 1, 0, CONVERT(datetime,'2018-11-17 10:12:53:242',121), NULL, 0, NULL)

INSERT INTO SGE.EstadoDispositivo VALUES (2, 8, 1, CONVERT(datetime,'2018-03-12 20:12:53:242',121), CONVERT(datetime,'2018-03-13 06:02:13:345',121), 0)
INSERT INTO SGE.EstadoDispositivo VALUES (2, 8, 1, CONVERT(datetime,'2018-03-13 06:02:13:346',121), CONVERT(datetime,'2018-03-13 14:08:23:545',121), 1)
INSERT INTO SGE.EstadoDispositivo VALUES (2, 8, 1, CONVERT(datetime,'2018-03-13 14:08:23:546',121), CONVERT(datetime,'2018-03-15 06:07:55:125',121), 0)
INSERT INTO SGE.EstadoDispositivo VALUES (2, 8, 1, CONVERT(datetime,'2018-03-15 06:07:55:126',121), CONVERT(datetime,'2018-03-15 23:04:35:234',121), 1)
INSERT INTO SGE.EstadoDispositivo VALUES (2, 8, 1, CONVERT(datetime,'2018-03-15 23:04:35:235',121), CONVERT(datetime,'2018-03-17 05:06:42:332',121), 0)

INSERT INTO SGE.Sensor VALUES (2, 'Sensor Temperatura', 20, NULL)
INSERT INTO SGE.Regla VALUES (1, 'Temperatura', 30, '>', 'Encender')
INSERT INTO SGE.Actuador VALUES (2, 'Actuardor-Temperatura')
INSERT INTO SGE.ActuadorPorRegla VALUES (1, 1)
INSERT INTO SGE.DispositivoPorActuador VALUES (1, 2, 1, 1)
INSERT INTO SGE.DispositivoPorActuador VALUES (1, 2, 8, 1)