/*
Created: 15/06/2015
Modified: 07/07/2015
Model: Sueldos y Jornales
Database: MS SQL Server 2014
*/


-- Create tables section -------------------------------------------------

-- Table Sj.Empleados

CREATE TABLE [Sj].[Empleados]
(
 [EmpleadoID] Bigint IDENTITY(1,1) NOT NULL,
 [SucursalID] Int NOT NULL,
 [Nombres] Varchar(50) NOT NULL,
 [Apellidos] Varchar(50) NOT NULL,
 [FechaNacimiento] Date NOT NULL,
 [Sexo] Int NOT NULL,
 [NroCedula] Int NOT NULL,
 [EstadoCivilID] Int NOT NULL,
 [NacionalidadID] Int NOT NULL,
 [NumeroIps] Int NULL,
 [NumeroMjt] Int NULL,
 [ProfesionID] Int NOT NULL,
 [CantidadHijos] Int NOT NULL,
 [UsuarioID] Bigint NOT NULL,
 [MomentoCarga] Datetime2 NOT NULL
)
go
EXEC sp_addextendedproperty 'MS_Description', 'Si es 1 es masculino, si es 2 es femenino', 'SCHEMA', 'Sj', 'TABLE', 'Empleados', 'COLUMN', 'Sexo'
go
EXEC sp_addextendedproperty 'MS_Description', 'Numero del ministerio de justicia y trabajo', 'SCHEMA', 'Sj', 'TABLE', 'Empleados', 'COLUMN', 'NumeroMjt'
go

-- Create indexes for table Sj.Empleados

CREATE INDEX [IX_Empleados:SucursalID] ON [Sj].[Empleados] ([SucursalID])
go

CREATE INDEX [IX_Empleados:EstadoCivilID] ON [Sj].[Empleados] ([EstadoCivilID])
go

CREATE INDEX [IX_Empleados:NacionalidadID] ON [Sj].[Empleados] ([NacionalidadID])
go

CREATE UNIQUE INDEX [IX_Unique_Empleados:NroCedula] ON [Sj].[Empleados] ([NroCedula])
go

CREATE INDEX [IX_Empleados:ProfesionID] ON [Sj].[Empleados] ([ProfesionID])
go

CREATE INDEX [IX_Empleados:UsuarioID] ON [Sj].[Empleados] ([UsuarioID])
go

CREATE UNIQUE INDEX [IX_Unique_Empleados_Nombres_Apellidos] ON [Sj].[Empleados] ([Nombres],[Apellidos])
go

-- Add keys for table Sj.Empleados

ALTER TABLE [Sj].[Empleados] ADD CONSTRAINT [pk_Empleados] PRIMARY KEY ([EmpleadoID])
go

-- Table Sj.Sucursales

CREATE TABLE [Sj].[Sucursales]
(
 [SucursalID] Int IDENTITY(1,1) NOT NULL,
 [EmpresaID] Int NOT NULL,
 [NombreSucursal] Varchar(50) NOT NULL,
 [Abreviatura] Varchar(5) NOT NULL,
 [Descripcion] Text NULL
)
go

-- Create indexes for table Sj.Sucursales

CREATE INDEX [IX_Sucursales:EmpresaID] ON [Sj].[Sucursales] ([EmpresaID])
go

CREATE UNIQUE INDEX [IX_Unique_NombreSucursal] ON [Sj].[Sucursales] ([NombreSucursal])
go

-- Add keys for table Sj.Sucursales

ALTER TABLE [Sj].[Sucursales] ADD CONSTRAINT [pk_Sucursales] PRIMARY KEY ([SucursalID])
go

-- Table Sj.Empresas

CREATE TABLE [Sj].[Empresas]
(
 [EmpresaID] Int IDENTITY(1,1) NOT NULL,
 [NombreEmpresa] Varchar(50) NOT NULL,
 [Descripcion] Varchar(100) NOT NULL,
 [Abreviatura] Varchar(5) NOT NULL,
 [Ruc] Varchar(50) NOT NULL
)
go

-- Add keys for table Sj.Empresas

ALTER TABLE [Sj].[Empresas] ADD CONSTRAINT [pk_Empresas] PRIMARY KEY ([EmpresaID])
go

-- Table Sj.EstadoCiviles

CREATE TABLE [Sj].[EstadoCiviles]
(
 [EstadoCivilID] Int NOT NULL,
 [NombreEstadoCivil] Varchar(50) NOT NULL
)
go
EXEC sp_addextendedproperty 'MS_Description', '1 va a ser soltero, 2 casado, 3 divorciado, 4 viudo', 'SCHEMA', 'Sj', 'TABLE', 'EstadoCiviles', 'COLUMN', 'EstadoCivilID'
go

INSERT INTO [Sj].[EstadoCiviles]
           ([EstadoCivilID]
           ,[NombreEstadoCivil])
     VALUES
           (1,'Soltero'),(2,'Casado'),(3,'Divorciado'),(4,'Viudo')
GO

-- Add keys for table Sj.EstadoCiviles

ALTER TABLE [Sj].[EstadoCiviles] ADD CONSTRAINT [pk_EstadoCiviles] PRIMARY KEY ([EstadoCivilID])
go

-- Table Sj.Nacionalidades

CREATE TABLE [Sj].[Nacionalidades]
(
 [NacionalidadID] Int NOT NULL,
 [NombreNacionalidad] Varchar(50) NOT NULL,
 [Abreviatura] Varchar(5) NOT NULL
)
go
EXEC sp_addextendedproperty 'MS_Description', 'Se deve codificar las nacionalidades', 'SCHEMA', 'Sj', 'TABLE', 'Nacionalidades', 'COLUMN', 'NacionalidadID'
go

INSERT INTO [Sj].[Nacionalidades]
           ([NacionalidadID]
           ,[NombreNacionalidad]
           ,[Abreviatura])
     VALUES
           (1 , 'Paraguay' ,'Py'),
           (2 , 'Argentina' ,'Ar')

-- Create indexes for table Sj.Nacionalidades

CREATE UNIQUE INDEX [IX_Unique_NombreNacionalidad] ON [Sj].[Nacionalidades] ([NombreNacionalidad])
go

-- Add keys for table Sj.Nacionalidades

ALTER TABLE [Sj].[Nacionalidades] ADD CONSTRAINT [pk_Nacionalidades] PRIMARY KEY ([NacionalidadID])
go

-- Table Sj.HistoricoDirecciones

CREATE TABLE [Sj].[HistoricoDirecciones]
(
 [HistoricoDireccionID] Bigint IDENTITY(1,1) NOT NULL,
 [EmpleadoID] Bigint NOT NULL,
 [Direccion] Text NOT NULL,
 [UsuarioID] Bigint NOT NULL,
 [MomentoCarga] Datetime2 NOT NULL
)
go

-- Create indexes for table Sj.HistoricoDirecciones

CREATE INDEX [IX_HistoricoDirecciones:EmpleadoID] ON [Sj].[HistoricoDirecciones] ([EmpleadoID])
go

CREATE INDEX [IX_HistoricoDirecciones:UsuarioID] ON [Sj].[HistoricoDirecciones] ([UsuarioID])
go

-- Add keys for table Sj.HistoricoDirecciones

ALTER TABLE [Sj].[HistoricoDirecciones] ADD CONSTRAINT [pk_HistoricoDirecciones] PRIMARY KEY ([HistoricoDireccionID])
go

-- Table Sj.HistoricoSalarios

CREATE TABLE [Sj].[HistoricoSalarios]
(
 [HistoricoSalarioID] Bigint IDENTITY(1,1) NOT NULL,
 [EmpleadoID] Bigint NOT NULL,
 [Monto] Money NOT NULL,
 [CargoID] Int NOT NULL,
 [Observacion] Text NOT NULL,
 [FechaSalario] Date NOT NULL,
 [UsuarioID] Bigint NOT NULL,
 [MomentoCarga] Datetime2 NOT NULL
)
go

EXEC sp_addextendedproperty 'MS_Description', 'Guarda el historico del salario y del cargo en el momento en donde se le asigna, puede cambiar de salario y de cargo o solo de salario o solo de cargo.
Si solo cambia de salario entonces es un aumento', 'SCHEMA', 'Sj', 'TABLE', 'HistoricoSalarios', NULL, NULL
go

-- Create indexes for table Sj.HistoricoSalarios

CREATE INDEX [IX_HistoricoSalarios:EmpleadoID] ON [Sj].[HistoricoSalarios] ([EmpleadoID])
go

CREATE INDEX [IX_HistoricoSalarios:CargoID] ON [Sj].[HistoricoSalarios] ([CargoID])
go

CREATE INDEX [IX_HistoricoSalarios:UsuarioID] ON [Sj].[HistoricoSalarios] ([UsuarioID])
go

-- Add keys for table Sj.HistoricoSalarios

ALTER TABLE [Sj].[HistoricoSalarios] ADD CONSTRAINT [pk_HistoricoSalarios] PRIMARY KEY ([HistoricoSalarioID])
go

-- Table Sj.HistoricoTelefonos

CREATE TABLE [Sj].[HistoricoTelefonos]
(
 [HistoricoTelefonoID] Bigint IDENTITY(1,1) NOT NULL,
 [EmpleadoID] Bigint NOT NULL,
 [Telefonos] Varchar(200) NOT NULL,
 [FechaTelefonos] Date NOT NULL,
 [UsuarioID] Bigint NOT NULL,
 [MomentoCarga] Datetime2 NOT NULL
)
go

-- Create indexes for table Sj.HistoricoTelefonos

CREATE INDEX [IX_HistoricoTelefonos:EmpleadoID] ON [Sj].[HistoricoTelefonos] ([EmpleadoID])
go

CREATE INDEX [IX_HistoricoTelefonos:UsuarioID] ON [Sj].[HistoricoTelefonos] ([UsuarioID])
go

-- Add keys for table Sj.HistoricoTelefonos

ALTER TABLE [Sj].[HistoricoTelefonos] ADD CONSTRAINT [pk_HistoricoTelefonos] PRIMARY KEY ([HistoricoTelefonoID])
go

-- Table Sj.Cargos

CREATE TABLE [Sj].[Cargos]
(
 [CargoID] Int IDENTITY(1,1) NOT NULL,
 [NombreCargo] Varchar(50) NOT NULL,
 [Abreviatura] Varchar(5) NOT NULL
)
go

-- Add keys for table Sj.Cargos

ALTER TABLE [Sj].[Cargos] ADD CONSTRAINT [pk_Cargos] PRIMARY KEY ([CargoID])
go

-- Table Jornaleros

CREATE TABLE [Jornaleros]
(
 [JornaleroID] Bigint IDENTITY(1,1) NOT NULL
)
go

-- Add keys for table Jornaleros

ALTER TABLE [Jornaleros] ADD CONSTRAINT [Key2] PRIMARY KEY ([JornaleroID])
go

-- Table Sj.HistoricoIngresoSalidas

CREATE TABLE [Sj].[HistoricoIngresoSalidas]
(
 [HistoricoIngresoSalidaID] Bigint IDENTITY(1,1) NOT NULL,
 [EmpleadoID] Bigint NOT NULL,
 [FechaIngreso] Date NOT NULL,
 [FechaSalida] Date NOT NULL,
 [UsuarioID] Bigint NOT NULL,
 [MomentoCarga] Datetime2 NOT NULL
)
go

-- Create indexes for table Sj.HistoricoIngresoSalidas

CREATE INDEX [IX_HistoricoIngresoSalidas:EmpleadoID] ON [Sj].[HistoricoIngresoSalidas] ([EmpleadoID])
go

CREATE INDEX [IX_HistoricoIngresoSalidas:UsuarioID] ON [Sj].[HistoricoIngresoSalidas] ([UsuarioID])
go

-- Add keys for table Sj.HistoricoIngresoSalidas

ALTER TABLE [Sj].[HistoricoIngresoSalidas] ADD CONSTRAINT [pk_HistoricoIngresoSalidas] PRIMARY KEY ([HistoricoIngresoSalidaID])
go

-- Table Sj.Profesiones

CREATE TABLE [Sj].[Profesiones]
(
 [ProfesionID] Int IDENTITY(1,1) NOT NULL,
 [NombreProfesion] Varchar(50) NOT NULL,
 [Abreviatura] Varchar(5) NOT NULL,
 [Descripcion] Varchar(200) NULL
)
go

-- Add keys for table Sj.Profesiones

ALTER TABLE [Sj].[Profesiones] ADD CONSTRAINT [pk_Profesiones] PRIMARY KEY ([ProfesionID])
go

-- Table Sj.HistoricosHorarios

CREATE TABLE [Sj].[HistoricosHorarios]
(
 [HistoricosHorarioID] Bigint IDENTITY(1,1) NOT NULL,
 [EmpleadoID] Bigint NOT NULL,
 [HoraEntradaManana] Time NULL,
 [HoraSalidaManana] Time NULL,
 [HoraEntradaTarde] Time NULL,
 [HoraSalidaTarde] Time NULL,
 [HoraEntradaNoche] Time NULL,
 [HoraSalidaNoche] Time NULL,
 [UsuarioID] Bigint NOT NULL,
 [MomentoCarga] Datetime2 NOT NULL
)
go

-- Create indexes for table Sj.HistoricosHorarios

CREATE INDEX [IX_HistoricosHorarios:EmpleadoID] ON [Sj].[HistoricosHorarios] ([EmpleadoID])
go

CREATE INDEX [IX_HistoricosHorarios:UsuarioID] ON [Sj].[HistoricosHorarios] ([UsuarioID])
go

-- Add keys for table Sj.HistoricosHorarios

ALTER TABLE [Sj].[HistoricosHorarios] ADD CONSTRAINT [pk_HistoricosHorarios] PRIMARY KEY ([HistoricosHorarioID])
go

-- Table Sj.Usuarios

CREATE TABLE [Sj].[Usuarios]
(
 [UsuarioID] Bigint IDENTITY(1,1) NOT NULL,
 [NombreUsuario] Varchar(50) NOT NULL,
 [UserID] Uniqueidentifier NOT NULL,
 [CorreoElectronico] Varchar(50) NOT NULL
)
go

-- Add keys for table Sj.Usuarios

ALTER TABLE [Sj].[Usuarios] ADD CONSTRAINT [Key3] PRIMARY KEY ([UsuarioID])
go

-- Table Sj.UbicacionSucUsuarios

CREATE TABLE [Sj].[UbicacionSucUsuarios]
(
 [UsuarioID] Bigint NOT NULL,
 [SucursalID] Int NOT NULL,
 [Observacion] Text NULL
)
go

-- Create indexes for table Sj.UbicacionSucUsuarios

CREATE INDEX [IX_UbicacionSucUsuarios_SucursalID] ON [Sj].[UbicacionSucUsuarios] ([SucursalID])
go

-- Add keys for table Sj.UbicacionSucUsuarios

ALTER TABLE [Sj].[UbicacionSucUsuarios] ADD CONSTRAINT [pfk_UbicacionSucUsuarios] PRIMARY KEY ([UsuarioID])
go

-- Create relationships section ------------------------------------------------- 

ALTER TABLE [Sj].[Sucursales] ADD CONSTRAINT [fk_Sucursales>Empresas] FOREIGN KEY ([EmpresaID]) REFERENCES [Sj].[Empresas] ([EmpresaID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[Empleados] ADD CONSTRAINT [fk_Empleados>Sucursales] FOREIGN KEY ([SucursalID]) REFERENCES [Sj].[Sucursales] ([SucursalID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[Empleados] ADD CONSTRAINT [fk_Empleados>EstadoCiviles] FOREIGN KEY ([EstadoCivilID]) REFERENCES [Sj].[EstadoCiviles] ([EstadoCivilID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[Empleados] ADD CONSTRAINT [fk_Empleados>Nacionalidades] FOREIGN KEY ([NacionalidadID]) REFERENCES [Sj].[Nacionalidades] ([NacionalidadID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[HistoricoDirecciones] ADD CONSTRAINT [fk_HistoricoDirecciones>Empleados] FOREIGN KEY ([EmpleadoID]) REFERENCES [Sj].[Empleados] ([EmpleadoID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[HistoricoSalarios] ADD CONSTRAINT [fk_HistoricoSalarios>Empleados] FOREIGN KEY ([EmpleadoID]) REFERENCES [Sj].[Empleados] ([EmpleadoID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[HistoricoTelefonos] ADD CONSTRAINT [fk_HistoricoTelefonos>Empleados] FOREIGN KEY ([EmpleadoID]) REFERENCES [Sj].[Empleados] ([EmpleadoID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[HistoricoSalarios] ADD CONSTRAINT [fk_HistoricoSalarios>Cargos] FOREIGN KEY ([CargoID]) REFERENCES [Sj].[Cargos] ([CargoID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[HistoricoIngresoSalidas] ADD CONSTRAINT [fk_HistoricoIngresoEgresos>Empleados] FOREIGN KEY ([EmpleadoID]) REFERENCES [Sj].[Empleados] ([EmpleadoID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[Empleados] ADD CONSTRAINT [fk_Empleados>Profesiones] FOREIGN KEY ([ProfesionID]) REFERENCES [Sj].[Profesiones] ([ProfesionID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[HistoricosHorarios] ADD CONSTRAINT [fk_HistoricosHorarios>Empleados] FOREIGN KEY ([EmpleadoID]) REFERENCES [Sj].[Empleados] ([EmpleadoID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[Empleados] ADD CONSTRAINT [fk_Empleados>Usuarios] FOREIGN KEY ([UsuarioID]) REFERENCES [Sj].[Usuarios] ([UsuarioID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[HistoricosHorarios] ADD CONSTRAINT [fk_HistoricosHorarios>Usuarios] FOREIGN KEY ([UsuarioID]) REFERENCES [Sj].[Usuarios] ([UsuarioID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[HistoricoIngresoSalidas] ADD CONSTRAINT [fk_HistoricoIngresoSalidas>Usuarios] FOREIGN KEY ([UsuarioID]) REFERENCES [Sj].[Usuarios] ([UsuarioID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[HistoricoSalarios] ADD CONSTRAINT [fk_HistoricoSalarios>Usuarios] FOREIGN KEY ([UsuarioID]) REFERENCES [Sj].[Usuarios] ([UsuarioID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[HistoricoDirecciones] ADD CONSTRAINT [fk_HistoricoDirecciones>Usuarios] FOREIGN KEY ([UsuarioID]) REFERENCES [Sj].[Usuarios] ([UsuarioID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[HistoricoTelefonos] ADD CONSTRAINT [fk_HistoricoTelefono>Usuarios] FOREIGN KEY ([UsuarioID]) REFERENCES [Sj].[Usuarios] ([UsuarioID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[UbicacionSucUsuarios] ADD CONSTRAINT [pfk_UbicacionSucUsuarios>Usuarios] FOREIGN KEY ([UsuarioID]) REFERENCES [Sj].[Usuarios] ([UsuarioID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[UbicacionSucUsuarios] ADD CONSTRAINT [fk_UbicacionSucUsuarios>Sucursales] FOREIGN KEY ([SucursalID]) REFERENCES [Sj].[Sucursales] ([SucursalID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go



