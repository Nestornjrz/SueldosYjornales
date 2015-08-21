/*
Created: 15/06/2015
Modified: 21/08/2015
Model: Sueldos y Jornales
Database: MS SQL Server 2014
*/


-- Create tables section -------------------------------------------------

-- Table Sj.PrestamosSimples

CREATE TABLE [Sj].[PrestamosSimples]
(
 [PrestamoSimpleID] Bigint IDENTITY(1,1) NOT NULL,
 [EmpleadoID] Bigint NOT NULL,
 [FechaPrestamo] Date NOT NULL,
 [Monto] Money NOT NULL,
 [Cuotas] Int NOT NULL,
 [Observacion] Text NULL,
 [MovEmpleadoID] Bigint NULL,
 [UsuarioID] Bigint NOT NULL,
 [MomentoCarga] Datetime2 NOT NULL
)
go
EXEC sp_addextendedproperty 'MS_Description', 'Esto es para controlar en que movimiento se genero el debito de este prestamo.', 'SCHEMA', 'Sj', 'TABLE', 'PrestamosSimples', 'COLUMN', 'MovEmpleadoID'
go

-- Create indexes for table Sj.PrestamosSimples

CREATE INDEX [IX_PrestamosSimples_EmpleadoID] ON [Sj].[PrestamosSimples] ([EmpleadoID])
go

CREATE INDEX [IX_PrestamosSimples_MovEmpleadoID] ON [Sj].[PrestamosSimples] ([MovEmpleadoID])
go

CREATE INDEX [IX_PrestamosSimples_UsuarioID] ON [Sj].[PrestamosSimples] ([UsuarioID])
go

-- Add keys for table Sj.PrestamosSimples

ALTER TABLE [Sj].[PrestamosSimples] ADD CONSTRAINT [pk_PrestamosSimples] PRIMARY KEY ([PrestamoSimpleID])
go

-- Create relationships section ------------------------------------------------- 

ALTER TABLE [Sj].[PrestamosSimples] ADD CONSTRAINT [fk_PrestamosSimples->Empleados] FOREIGN KEY ([EmpleadoID]) REFERENCES [Sj].[Empleados] ([EmpleadoID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[PrestamosSimples] ADD CONSTRAINT [fk_PrestamosSimples->MovEmpleados] FOREIGN KEY ([MovEmpleadoID]) REFERENCES [Sj].[MovEmpleados] ([MovEmpleadoID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go

ALTER TABLE [Sj].[PrestamosSimples] ADD CONSTRAINT [fk_PrestamosSimples->Usuarios] FOREIGN KEY ([UsuarioID]) REFERENCES [Sj].[Usuarios] ([UsuarioID]) ON UPDATE NO ACTION ON DELETE NO ACTION
go



