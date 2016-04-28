module app.dto {
    export interface ISueldoYjornaleDto {
        empleadoID: number;
        nombreReferencia: string;
        nroPatronal: number;
        documento: number;
        formaDePago: number;
        importeUnitario: number;
        h_Ene: number;
        s_Ene: number;
        h_Feb: number;
        s_Feb: number;
        h_Mar: number;
        s_Mar: number;
        h_Abr: number;
        s_Abr: number;
        h_May: number;
        s_May: number;
        h_Jun: number;
        s_Jun: number;
        h_Jul: number;
        s_Jul: number;
        h_Ago: number;
        s_Ago: number;
        h_Set: number;
        s_Set: number;
        h_Oct: number;
        s_Oct: number;
        h_Nov: number;
        s_Nov: number;
        h_Dic: number;
        s_Dic: number;
        h_50: number;
        s_50: number;
        h_100: number;
        s_100: number;
        aguinaldo: number;
        beneficios: number;
        bonificaciones: number;
        vacaciones: number;
        total_H: number;
        total_S: number;
        totalGeneral: number;
    }
}