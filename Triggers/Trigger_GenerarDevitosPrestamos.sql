SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER Sj.Trigger_GenerarDevitosPrestamos
    ON  [Sj].[PrestamosSimples]
    AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Monto money, @NroCuotas int, @Fecha1erVencimiento DateTime, @UsuarioID BigInt, 
	        @EmpleadoID BigInt, @PrestamoSimpleID BigInt, @GenerarDevitoSn Bit,
			@MovEmpleadoID BigInt = null
	
	SELECT @PrestamoSimpleID = PrestamoSimpleID,
	       @EmpleadoID = EmpleadoID,
	       @Monto = Monto,
	       @NroCuotas = Cuotas,
		   @Fecha1erVencimiento = Fecha1erVencimiento,
		   @UsuarioID = UsuarioID,
		   @GenerarDevitoSn = GenerarDevitoSn,
		   @MovEmpleadoID = MovEmpleadoID
    FROM inserted

	--Se averigua el empleado
	DECLARE @Nombres varchar(50), @Apellidos varchar(50)
	SELECT @Nombres = Nombres,
	       @Apellidos = Apellidos
	FROM [Sj].[Empleados]
	WHERE EmpleadoID = @EmpleadoID

	IF UPDATE(GenerarDevitoSn) and @GenerarDevitoSn = 1 and ISNULL(@MovEmpleadoID,1) = 1 
	BEGIN
		--Se carga la cabecera de los movimientos		
		INSERT INTO [Sj].[MovEmpleados]
           ([FechaMovimiento]
           ,[Descripcion]
           ,[UsuarioID]
           ,[MomentoCarga])
        VALUES
           (GETDATE()
           ,'Generacion Automatica del devito por prestamo (' + @Nombres + ' ' + @Apellidos + ')'
           ,@UsuarioID
           ,GETDATE())
		SELECT @MovEmpleadoID = SCOPE_IDENTITY()

		IF @@ERROR <> 0
		BEGIN
			RAISERROR ('Ocurrio el error al intentar insertar en la tabla MovEmpleados', 16, 1);
			PRINT 'Trigger_GenerarDevitosPrestamos';
			ROLLBACK TRANSACTION;
			RETURN
		END
		--Se carga los detalles del movimiento
		DECLARE @MontoCuota money, @cnt int = 0, @FechaVencimiento DateTime
		SET @MontoCuota = @Monto / @NroCuotas

		SET @FechaVencimiento = @Fecha1erVencimiento
		WHILE @cnt < @NroCuotas
		BEGIN
		   INSERT INTO [Sj].[MovEmpleadosDets]
			   ([MovEmpleadoID]
			   ,[EmpleadoID]
			   ,[DevCred]
			   ,[Monto]
			   ,[MesAplicacion]
			   ,[LiquidacionConceptoID])
		   VALUES
			   (@MovEmpleadoID
			   ,@EmpleadoID
			   ,1--Devito
			   ,@MontoCuota
			   ,@FechaVencimiento
			   ,4)--Concepto de Prestamo
		   IF @@ERROR <> 0
		   BEGIN
				RAISERROR ('Ocurrio el error al intentar insertar en la tabla MovEmpleadosDets', 16, 1);
				PRINT 'Trigger_GenerarDevitosPrestamos';
				ROLLBACK TRANSACTION;
				RETURN
		   END
		   --Se agrega un mes a en cada loop
		   SET @FechaVencimiento = DATEADD(MONTH,1,@FechaVencimiento)
		   SET @cnt = @cnt + 1;
	    END;
		--Se marca la tabla PrestamosSimples para saber que MovEmpleadoID se genero
		-- al realizar el devito
		UPDATE [Sj].[PrestamosSimples]
		SET [MovEmpleadoID] = @MovEmpleadoID     
		WHERE PrestamoSimpleID = @PrestamoSimpleID
	END
END