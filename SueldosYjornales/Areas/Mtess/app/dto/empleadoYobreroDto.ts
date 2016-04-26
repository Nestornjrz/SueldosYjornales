module app.dto {
    export interface IEmpleadoYobreroDto {      
        nroPatronal: number;   
        documento: string;        
        nombre: string;      
        apellido: string;    
        sexo: string;        
        estadoCivil: string;        
        fechaNac: Date;        
        nacionalidad: string;        
        domicilio: string;        
        fechaNacMenor: Date;        
        hijosMenores: number;        
        cargo: string;        
        profesion: string;        
        fechaEntrada: Date;
        horarioTrabajo: string;        
        menorEscapa: string;        
        menorEsEscolar: string;        
        fechaSalida: Date;        
        motivoSalida: string;        
        estado: string;
    }
}