/*
Created: 15/06/2015
Modified: 21/08/2015
Model: Sueldos y Jornales
Database: MS SQL Server 2014
*/


-- Create tables section -------------------------------------------------

-- Table Sj.Empleados

CREATE TABLE [Sj].[Empleados]
(
 [EmpleadoID] Bigint IDENTITY(1,1) NOT NULL,
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
 [Observacion] Text NULL,
 [FechaSalario] Date NOT NULL,
 [Ips_Sn] Bit NOT NULL,
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

-- Table Sj.HistoricoIngresoSalidas

CREATE TABLE [Sj].[HistoricoIngresoSalidas]
(
 [HistoricoIngresoSalidaID] Bigint IDENTITY(1,1) NOT NULL,
 [EmpleadoID] Bigint NOT NULL,
 [FechaIngreso] Date NOT NULL,
 [FechaSalida] Date NULL,
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

-- Table Sj.HistoricoHorarios

CREATE TABLE [Sj].[HistoricoHorarios]
(
 [HistoricoHorarioID] Bigint IDENTITY(1,1) NOT NULL,
 [EmpleadoID] Bigint NOT NULL,
 [HoraEntradaManana] Datetime2 NULL,
 [HoraSalidaManana] Datetime2 NULL,
 [HoraEntradaTarde] Datetime2 NULL,
 [HoraSalidaTarde] Datetime2 NULL,
 [HoraEntradaNoche] Datetime2 NULL,
 [HoraSalidaNoche] Datetime2 NULL,
 [UsuarioID] Bigint NOT NULL,
 [MomentoCarga] Datetime2 NOT NULL
)
go

-- Create indexes for table Sj.HistoricoHorarios

CREATE INDEX [IX_HistoricoHorarios:EmpleadoID] ON [Sj].[HistoricoHorarios] ([EmpleadoID])
go

CREATE INDEX [IX_HistoricoHorarios:UsuarioID] ON [Sj].[HistoricoHorarios] ([UsuarioID])
go

-- Add keys for table Sj.HistoricoHorarios

ALTER TABLE [Sj].[HistoricoHorarios] ADD CONSTRAINT [pk_HistoricoHorarios] PRIMARY KEY ([HistoricoHorarioID])
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

-- Table Sj.HistoricoSucursales

CREATE TABLE [Sj].[HistoricoSucursales]
(
 [HistoricoSucursalID] Bigint IDENTITY(1,1) NOT NULL,
 [EmpleadoID] Bigint NOT NULL,
 [SucursalID] Int NOT NULL,
 [UsuarioID] Bigint NOT NULL,
 [MomentoCarga] Datetime2 NOT NULL
)
go

-- Create indexes for table Sj.HistoricoSucursales

CREATE INDEX [IX_HistoricoSucursales_EmpleadoID] ON [Sj].[HistoricoSucursales] ([EmpleadoID])
go

CREATE INDEX [IX_HistoricoSucursales_SucursalID] ON [Sj].[HistoricoSucursales] ([SucursalID])
go

CREATE INDEX [IX_HistoricoSucursales_UsuarioID] ON [Sj].[HistoricoSucursales] ([UsuarioID])
go

-- Add keys for table Sj.HistoricoSucursales

ALTER TABLE [Sj].[HistoricoSucursales] ADD CONSTRAINT [Key4] PRIMARY KEY ([HistoricoSucursalID])
go

-- Table Sj.Comisiones

CREATE TABLE [Sj].[Comisiones]
(
 [ComisionID] Bigint IDENTITY(1,1) NOT NULL,
 [EmpleadoID] Bigint NOT NULL,
 [FechaComision] Date NOT NULL,
 [MontoComision] Money NOT NULL,
 [Observacion] Text NOT NULL,
 [UsuarioID] Bigint NOT NULL,
 [MomentoCarga] Datetime2 NOT NULL
)
go

-- Create indexes for table Sj.Comisiones

CREATE INDEX [IX_Comisiones_EmpleadoID] ON [Sj].[Comisiones] ([EmpleadoID])
go

CREATE INDEX [IX_Comisiones_UsuarioID] ON [Sj].[Comisiones] ([UsuarioID])
go

-- Add keys for table Sj.Comisiones

ALTER TABLE [Sj].[Comisiones] ADD CONSTRAINT [pk_Comisiones] PRIMARY KEY ([ComisionID])
go

-- Table Sj.Ausencias

CREATE TABLE [Sj].[Ausencias]
(
 [AusenciaID] Bigint IDENTITY(1,1) NOT NULL,
 [EmpleadoID] Bigint NOT NULL,
 [FechaAusencia] Date NOT NULL,
 [Observacion] Text NOT NULL,
 [UsuarioID] Bigint NOT NULL,
 [MomentoCarga] Datetime2 NOT NULL
)
go

-- Create indexes for table Sj.Ausencias

CREATE INDEX [IX_Ausencias_EmpleadoID] ON [Sj].[Ausencias] ([EmpleadoID])
go

CREATE INDEX [IX_Ausencias_UsuarioID] ON [Sj].[Ausencias] ([UsuarioID])
go

-- Add keys for table Sj.Ausencias

ALTER TABLE [Sj].[Ausencias] ADD CONSTRAINT [pk_Ausencias] PRIMARY KEY ([AusenciaID])
go

-- Table Sj.Anticipos

CREATE TABLE [Sj].[Anticipos]
(
 [AnticipoID] Bigint IDENTITY(1,1) NOT NULL,
 [EmpleadoID] Bigint NOT NULL,
 [FechaAnticipo] Date NOT NULL,
 [MontoAnticipo] Money NOT NULL,
 [Observacion] Text NULL,
 [UsuarioID] Bigint NOT NULL,
 [MomentoCarga] Datetime2 NOT NULL
)
go

-- Create indexes for table Sj.Anticipos

CREATE INDEX [IX_Anticipos_EmpleadoID] ON [Sj].[Anticipos] ([EmpleadoID])
go

CREATE INDEX [IX_Anticipos_UsuarioID] ON [Sj].[Anticipos] ([UsuarioID])
go

-- Add keys for table Sj.Anticipos

ALTER TABLE [Sj].[Anticipos] ADD CONSTRAINT [pk_Anticipos] PRIMARY KEY ([AnticipoID])
go

-- Table Sj.Imagenes

CREATE TABLE [Sj].[Imagenes]
(
 [ImagenID] Int IDENTITY(1,1) NOT NULL,
 [EmpleadoID] Bigint NOT NULL,
 [TipoImagenID] Int NOT NULL,
 [Imagen] Varbinary(max) NOT NULL,
 [Extencion] Varchar(5) NOT NULL,
 [UsuarioID] Bigint NOT NULL,
 [MomentoCarga] Datetime2 NOT NULL
)
go

-- Create indexes for table Sj.Imagenes

CREATE INDEX [IX_Imagenes_UsuarioID] ON [Sj].[Imagenes] ([UsuarioID])
go

CREATE INDEX [IX_Imagenes_EmpleadoID] ON [Sj].[Imagenes] ([EmpleadoID])
go

CREATE INDEX [IX_Imagenes_TipoImagenID] ON [Sj].[Imagenes] ([TipoImagenID])
go

CREATE UNIQUE INDEX [IX_Imagenes_Unique_EmpleadoID_TipoImagenID] ON [Sj].[Imagenes] ([EmpleadoID],[TipoImagenID])
go

-- Add keys for table Sj.Imagenes

ALTER TABLE [Sj].[Imagenes] ADD CONSTRAINT [pk_Imagenes] PRIMARY KEY ([ImagenID])
go

-- Table Sj.TipoImagenes

CREATE TABLE [Sj].[TipoImagenes]
(
 [TipoImagenID] Int IDENTITY(1,1) NOT NULL,
 [NombreTipoImagen] Varchar(100) NOT NULL
)
go

INSERT INTO [Sj].[TipoImagenes]
           ([NombreTipoImagen])
     VALUES
           ('Foto Empleado'), ('Cedula Amberso'),('Cedula Reverso')

-- Add keys for table Sj.TipoImagenes

ALTER TABLE [Sj].[TipoImagenes] ADD CONSTRAINT [pk_TipoImagenes] PRIMARY KEY ([TipoImagenID])
go

-- Table Sj.PrestamosSimples

CREATE TABLE [Sj].[PrestamosSimples]
(
 [PrestamoSimpleID] Bigint IDENTITY(1,1) NOT NULL,
 [EmpleadoID] Bigint NOT NULL,
 [Monto] Money NOT NULL,
 [Cuotas] Int NOT NULL,
 [Observacion] Text NULL,
 [MovEmpleadoID] Bigint NULL
)
go
EXEC sp_addextendedproperty 'MS_Description', 'Esto es para controlar en que movimiento se genero el debito de este prestamo.', 'SCHEMA', 'Sj', 'TABLE', 'PrestamosSimples', 'COLUMN', 'MovEmpleadoID'
go

-- Create indexes for table Sj.PrestamosSimples

CREATE INDEX [IX_PrestamosSimples_EmpleadoID] ON [Sj].[PrestamosSimples] ([EmpleadoID])
go

CREATE INDEX [IX_PrestamosSimples_MovEmpleadoID] ON [Sj].[PrestamosSimples] ([MovEmpleadoID])
go

-- Add keys for table Sj.PrestamosSimples

ALTER TABLE [Sj].[PrestamosSimples] ADD CONSTRAINT [pk_PrestamosSimples] PRIMARY KEY ([PrestamoSimpleID])
go

-- Table Sj.MovEmpleados

CREATE TABLE [Sj].[MovEmpleados]
(
 [MovEmpleadoID] Bigint IDENTITY(1,1) NOT NULL,
 [FechaMovimiento] Datetime2 NOT NULL,
 [Descripcion] Varchar(300) NOT NULL
)
go

-- Add keys for table Sj.MovEmpleados

ALTER TABLE [Sj].[MovEmpleados] ADD CONSTRAINT [pk_MovEmpleados] PRIMARY KEY ([MovEmpleadoID])
go

-- Table Sj.MovEmpleadosDets

CREATE TABLE [Sj].[MovEmpleadosDets]
(
 [MovEmpleadoDetID] Bigint IDENTITY(1,1) NOT NULL,
 [LiquidacionEmpleadoID] Bigint NOT NULL,
 [EmpleadoID] Bigint NOT NULL,
 [DevCred] Bit NOT NULL,
 [Monto] Money NOT NULL,
 [MesAplicacion] Date NOT NULL,
 [LiquidacionConceptoID] Int NOT NULL
)
go
EXEC sp_addextendedproperty 'MS_Description', 'Devito es 1 y credito es 0', 'SCHEMA', 'Sj', 'TABLE', 'MovEmpleadosDets', 'COLUMN', 'DevCred'
go

-- Create indexes for table Sj.MovEmpleadosDets

CREATE INDEX [IX_MovEmpleadosDets_MovEmpleadoID] ON [Sj].[MovEmpleadosDets] ([LiquidacionEmpleadoID])
go

CREATE INDEX [IX_MovEmpleadosDets_EmpleadoID] ON [Sj].[MovEmpleadosDets] ([EmpleadoID])
go

CREATE INDEX [IX_MovEmpleadosDets_LiquidacionConceptoID] ON [Sj].[MovEmpleadosDets] ([LiquidacionConceptoID])
go

-- Add keys for table Sj.MovEmpleadosDets

ALTER TABLE [Sj].[MovEmpleadosDets] ADD CONSTRAINT [pk_MovEmpleadosDets] PRIMARY KEY ([MovEmpleadoDetID])
go

-- Table Sj.LiquidacionConceptos

CREATE TABLE [Sj].[LiquidacionConceptos]
(
 [LiquidacionConceptoID] Int NOT NULL,
 [NombreConcepto] Varchar(50) NOT NULL
)
go

INSERT INTO [Sj].[LiquidacionConceptos]
           ([LiquidacionConceptoID]
           ,[NombreConcepto])
     VALUES
           (1,'Sueldo Base'),(2,'Comision'),(3,'Anticipo'),(4,'Prestamo'),(5,'Total Pagado')

-- Add keys for table Sj.LiquidacionConceptos

ALTER TABLE [Sj].[LiquidacionConceptos] ADD CONSTRAINT [Key5] PRIMARY KEY ([LiquidacionConceptoID])
go

-- Create relationships section ------------------------------------------------- 

ALTER TABLE [Sj].[Sucursales] ADD CONSTRAINT [fk_Sucursales>Empresas] FOREIGN KEY ([EmpresaID]) REFERENCES [Sj].[Empresas] ([EmpresaID]) ON UPDATE NO ACTION ON DELETE NO ACTION
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

ALTER TABLE [Sj].[HistoricoHorarios] ADD CONSTRAINT [fk_HistoricosHorarios>Empleados] FOREIGN KEY ([EmpleadoID]) REFERENCES [Sj].[Empleados] ([EmpleadoID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[Empleados] ADD CONSTRAINT [fk_Empleados>Usuarios] FOREIGN KEY ([UsuarioID]) REFERENCES [Sj].[Usuarios] ([UsuarioID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[HistoricoHorarios] ADD CONSTRAINT [fk_HistoricosHorarios>Usuarios] FOREIGN KEY ([UsuarioID]) REFERENCES [Sj].[Usuarios] ([UsuarioID]) ON UPDATE NO ACTION ON DELETE NO ACTION
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

ALTER TABLE [Sj].[HistoricoSucursales] ADD CONSTRAINT [Fk_HistoricoSucursales->Empleados] FOREIGN KEY ([EmpleadoID]) REFERENCES [Sj].[Empleados] ([EmpleadoID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[HistoricoSucursales] ADD CONSTRAINT [fk_HistoricoSucursales->Sucursales] FOREIGN KEY ([SucursalID]) REFERENCES [Sj].[Sucursales] ([SucursalID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[HistoricoSucursales] ADD CONSTRAINT [Relationship3] FOREIGN KEY ([UsuarioID]) REFERENCES [Sj].[Usuarios] ([UsuarioID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[Comisiones] ADD CONSTRAINT [fk_Comisiones->Empleados] FOREIGN KEY ([EmpleadoID]) REFERENCES [Sj].[Empleados] ([EmpleadoID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[Comisiones] ADD CONSTRAINT [fk_Comisiones->Usuarios] FOREIGN KEY ([UsuarioID]) REFERENCES [Sj].[Usuarios] ([UsuarioID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[Ausencias] ADD CONSTRAINT [fk_Ausencias->Empleados] FOREIGN KEY ([EmpleadoID]) REFERENCES [Sj].[Empleados] ([EmpleadoID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[Ausencias] ADD CONSTRAINT [fk_Ausencias->Usuarios] FOREIGN KEY ([UsuarioID]) REFERENCES [Sj].[Usuarios] ([UsuarioID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[Anticipos] ADD CONSTRAINT [fk_Anticipos->Empleados] FOREIGN KEY ([EmpleadoID]) REFERENCES [Sj].[Empleados] ([EmpleadoID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[Anticipos] ADD CONSTRAINT [fk_Anticipos->Usuarios] FOREIGN KEY ([UsuarioID]) REFERENCES [Sj].[Usuarios] ([UsuarioID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[Imagenes] ADD CONSTRAINT [fk_Imagenes->Usuarios] FOREIGN KEY ([UsuarioID]) REFERENCES [Sj].[Usuarios] ([UsuarioID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[Imagenes] ADD CONSTRAINT [fk_Imagenes->Empleados] FOREIGN KEY ([EmpleadoID]) REFERENCES [Sj].[Empleados] ([EmpleadoID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[Imagenes] ADD CONSTRAINT [fk_imagenes->TipoImagenes] FOREIGN KEY ([TipoImagenID]) REFERENCES [Sj].[TipoImagenes] ([TipoImagenID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[PrestamosSimples] ADD CONSTRAINT [fk_PrestamosSimples->Empleados] FOREIGN KEY ([EmpleadoID]) REFERENCES [Sj].[Empleados] ([EmpleadoID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[MovEmpleadosDets] ADD CONSTRAINT [fk_MovEmpleadosDets->MovEmpleados] FOREIGN KEY ([LiquidacionEmpleadoID]) REFERENCES [Sj].[MovEmpleados] ([MovEmpleadoID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[MovEmpleadosDets] ADD CONSTRAINT [fk_MovEmpleadosDets->Empleados] FOREIGN KEY ([EmpleadoID]) REFERENCES [Sj].[Empleados] ([EmpleadoID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[PrestamosSimples] ADD CONSTRAINT [fk_PrestamosSimples->MovEmpleados] FOREIGN KEY ([MovEmpleadoID]) REFERENCES [Sj].[MovEmpleados] ([MovEmpleadoID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[MovEmpleadosDets] ADD CONSTRAINT [Fk_MovEmpleadosDets->LiquidacionConceptos] FOREIGN KEY ([LiquidacionConceptoID]) REFERENCES [Sj].[LiquidacionConceptos] ([LiquidacionConceptoID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go



