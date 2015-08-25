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
	        @EmpleadoID BigInt, @PrestamoSimpleID BigInt, @GenerarDevitoSn Bit
	
	SELECT @PrestamoSimpleID = PrestamoSimpleID,
	       @EmpleadoID = EmpleadoID,
	       @Monto = Monto,
	       @NroCuotas = Cuotas,
		   @Fecha1erVencimiento = Fecha1erVencimiento,
		   @UsuarioID = UsuarioID,
		   @GenerarDevitoSn = GenerarDevitoSn
    FROM inserted

	IF UPDATE(GenerarDevitoSn) and @GenerarDevitoSn = 1
	BEGIN
		--Se carga la cabecera de los movimientos
		DECLARE @MovEmpleadoID bigint
											INSERT INTO [Sj].[MovEmpleados]
           ([FechaMovimiento]
           ,[Descripcion]
           ,[UsuarioID]
           ,[MomentoCarga])
     VALUES
           (GETDATE()
           ,'Generacion Automatica del devito por prestamo'
           ,@UsuarioID
           ,GETDATE())
		SELECT @MovEmpleadoID = SCOPE_IDENTITY()
	
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