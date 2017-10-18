select * from aml.TRANSACCION_CLIENTE

CREATE PROCEDURE [dbo].[WS_UIF]
	@TIPO_REPORTE varchar(10),
	@CODIGO_TRANSACCION varchar(50),
	@xml xml output
AS
	IF @TIPO_REPORTE = '1' --Reporte de transaccion en efectivo
	BEGIN
   	set @xml =(select * from dbo.[UIF1-01] a
		WHERE a.idRegistroBancario = 'FASD445555'
		for xml path('detalleTransacciones'), elements xsinil 
		);
	END	
RETURN 0


select * from dbo.[UIF1-01] a
		WHERE a.idRegistroBancario = 'FASD445555'
		for xml path('detalleTransacciones'), 
		ELEMENTS xsinil 