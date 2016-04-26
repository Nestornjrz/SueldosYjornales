using SYJ.Application.Dto;
using SYJ.Application.Dto.Mtess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYJ.Domain.Managers.Mtess {
    public class SueldosYjornalesManagers {
        public List<SueldoYjornaleDto> ListadoSueldosYjoranales() {
            EmpleadosManagers em = new EmpleadosManagers();
            MovEmpleadosDetsManagers medm = new MovEmpleadosDetsManagers();
            HistoricoSalariosManagers hsm = new HistoricoSalariosManagers();

            var empleados = em.ListadoEmpleados();
            var years = new List<int> { 2015 };
            List<int> meses = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };


            foreach (EmpleadoDto empleado in empleados) {
                //Se empieza a cargar el empleado
                SueldoYjornaleDto syjDto = new SueldoYjornaleDto();
                syjDto.NroPatronal = 77399;
                syjDto.Documento = empleado.NroCedula.ToString();
                syjDto.FormaDePago = 'M';//Mensualero
                ///Se calcula el precio unitario
                var salarioYcargo = hsm.SalarioYCargoActual(empleado.EmpleadoID);
                if (!salarioYcargo.Error) {
                    var historicoSalarioCargo = (HistoricoSalarioDto)salarioYcargo.ObjetoDto;
                    syjDto.ImporteUnitario = historicoSalarioCargo.Monto / 30;//Para mensualeros
                }
                foreach (var ano in years) {
                    //Enero
                    syjDto.S_Ene = medm.TotalSalarioPercibidoMes(1, ano, empleado.EmpleadoID);
                    if (syjDto.S_Ene > 0) {
                        syjDto.H_Ene = medm.TotalHorasTrabajadas(1,ano,empleado.EmpleadoID);
                    }
                    //Febrero
                    syjDto.S_Feb = medm.TotalSalarioPercibidoMes(2, ano, empleado.EmpleadoID);
                    if (syjDto.S_Feb >0) {
                        syjDto.H_Ene = medm.TotalHorasTrabajadas(2, ano, empleado.EmpleadoID);
                    }
                    //Marzo
                    syjDto.S_Mar = medm.TotalSalarioPercibidoMes(3, ano, empleado.EmpleadoID);
                    if (syjDto.S_Mar > 0) {
                        syjDto.H_Mar = medm.TotalHorasTrabajadas(3, ano, empleado.EmpleadoID);
                    }
                    //Abril
                    syjDto.S_Abr = medm.TotalSalarioPercibidoMes(4, ano, empleado.EmpleadoID);
                    if (syjDto.S_Abr > 0) {
                        syjDto.H_Abr = medm.TotalHorasTrabajadas(4, ano, empleado.EmpleadoID);
                    }
                    //Mayo
                    syjDto.S_May = medm.TotalSalarioPercibidoMes(5, ano, empleado.EmpleadoID);
                    if (syjDto.S_May > 0) {
                        syjDto.H_May = medm.TotalHorasTrabajadas(5, ano, empleado.EmpleadoID);
                    }
                    //Junio
                    syjDto.S_Jun = medm.TotalSalarioPercibidoMes(6, ano, empleado.EmpleadoID);
                    if (syjDto.S_Jun > 0) {
                        syjDto.H_Jun = medm.TotalHorasTrabajadas(6, ano, empleado.EmpleadoID);
                    }
                    //Julio
                    syjDto.S_Jul = medm.TotalSalarioPercibidoMes(7, ano, empleado.EmpleadoID);
                    if (syjDto.S_Jul > 0) {
                        syjDto.H_Jul = medm.TotalHorasTrabajadas(7, ano, empleado.EmpleadoID);
                    }
                    //Agosto
                    syjDto.S_Ago = medm.TotalSalarioPercibidoMes(8, ano, empleado.EmpleadoID);
                    if (syjDto.S_Ago > 0) {
                        syjDto.H_Ago = medm.TotalHorasTrabajadas(8, ano, empleado.EmpleadoID);
                    }
                    //Setiembre
                    syjDto.S_Set = medm.TotalSalarioPercibidoMes(9, ano, empleado.EmpleadoID);
                    if (syjDto.S_Set > 0) {
                        syjDto.H_Set = medm.TotalHorasTrabajadas(9, ano, empleado.EmpleadoID);
                    }
                    //Octubre
                    syjDto.S_Oct = medm.TotalSalarioPercibidoMes(10, ano, empleado.EmpleadoID);
                    if (syjDto.S_Oct > 0) {
                        syjDto.H_Oct = medm.TotalHorasTrabajadas(10, ano, empleado.EmpleadoID);
                    }
                    //Noviembre
                    syjDto.S_Nov = medm.TotalSalarioPercibidoMes(11, ano, empleado.EmpleadoID);
                    if (syjDto.S_Nov > 0) {
                        syjDto.H_Nov = medm.TotalHorasTrabajadas(11, ano, empleado.EmpleadoID);
                    }
                    //Diciembre
                    syjDto.S_Dic = medm.TotalSalarioPercibidoMes(12, ano, empleado.EmpleadoID);
                    if (syjDto.S_Dic > 0) {
                        syjDto.H_Dic = medm.TotalHorasTrabajadas(12, ano, empleado.EmpleadoID);
                    }
                    //Agunaldo
                    syjDto.Aguinaldo = medm.Aguinaldo(ano, empleado.EmpleadoID);

                }
            }
            throw new NotImplementedException();
        }
    }
}
