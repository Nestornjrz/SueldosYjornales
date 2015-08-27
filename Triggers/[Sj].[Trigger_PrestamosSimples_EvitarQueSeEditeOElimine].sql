USE [SueldosJornales]
GO
/****** Object:  Trigger [Sj].[Trigger_GenerarDevitosPrestamos]    Script Date: 25/08/2015 10:49:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter TRIGGER [Sj].[Trigger_PrestamosSimples_EvitarQueSeEditeOElimine]
    ON  [Sj].[PrestamosSimples]
    AFTER UPDATE, DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Del_GenerarDevitoSn Bit

	SELECT @Del_GenerarDevitoSn = GenerarDevitoSn
	FROM deleted	

	IF  @Del_GenerarDevitoSn = 1 and 
	    (
		  UPDATE(Fecha1erVencimiento) or UPDATE(Monto) or
		  UPDATE(Cuotas)              or UPDATE(Observacion)
		)
	BEGIN
		RAISERROR ('Una ves que se genero el devito no se puede EDITAR NI BORRAR EL PRESTAMO ', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN 	
	END	
END