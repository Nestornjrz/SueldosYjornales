import { Component, OnInit } from '@angular/core';
import { EmpleadoDto } from 'src/app/dtos/empleado-dto';
import { NgPrimeCustomService } from 'src/app/services/ng-prime-custom.service';
import { ProfesioneDto } from 'src/app/dtos/profesione-dto';
import { HttpClientService } from 'src/app/services/http-client.service';
import { NgForm } from '@angular/forms';
import { SucursaleDto } from 'src/app/dtos/sucursale-dto';
import { MensajeDto } from 'src/app/dtos/mensaje-dto';
import { SexoDto } from 'src/app/dtos/sexo-dto';
import { EstadoCivileDto } from 'src/app/dtos/estado-civile-dto';
import { NacionalidadeDto } from 'src/app/dtos/nacionalidade-dto';

@Component({
  selector: 'app-emp-datos-basicos',
  templateUrl: './emp-datos-basicos.component.html',
  styleUrls: ['./emp-datos-basicos.component.css']
})
export class EmpDatosBasicosComponent implements OnInit {
  empleado: EmpleadoDto = new EmpleadoDto();
  profesiones: ProfesioneDto[] = [];
  sucursales: SucursaleDto[] = [];
  estadosCiviles: EstadoCivileDto[] = [];
  nacionalidades: NacionalidadeDto[] = [];
  respuestaSubmitFormEmpleados: MensajeDto = new MensajeDto();
  sexos: SexoDto[] = [
    { sexoID: 1, nombreSexo: 'Masculino' },
    { sexoID: 2, nombreSexo: 'Femenino' }];
  es = this.ngPrimeCustomService.getConfiguracionEspenol();
  constructor(private ngPrimeCustomService: NgPrimeCustomService, private _http: HttpClientService) { }
  ngOnInit() {
    this.getProfesiones();
    this.getSucursales();
    this.getEstadoCivil();
    this.getNacionalidades();
  }

  getProfesiones() {
    this._http.getArray<ProfesioneDto>('/api/Profesiones')
      .subscribe(respuesta => {
        this.profesiones = respuesta;
      });
  }
  getSucursales() {
    this._http.getArray<SucursaleDto>('/api/Sucursales')
      .subscribe(respuesta => {
        this.sucursales = respuesta;
      });
  }
  getEstadoCivil() {
    this._http.getArray<EstadoCivileDto>('/api/EstadosCiviles')
      .subscribe(respuesta => {
        this.estadosCiviles = respuesta;
      });
  }
  getNacionalidades() {
    this._http.getArray<NacionalidadeDto>('/api/Nacionalidades')
      .subscribe(respuesta => {
        this.nacionalidades = respuesta;
      });
  }
  submitFormEmpleados(form: NgForm) {
    this._http.postObject(this.empleado, '/api/Empleados')
      .subscribe(respuesta => {
        this.respuestaSubmitFormEmpleados = respuesta;
        if (!respuesta.error) {
          this.empleado.empleadoID = (<EmpleadoDto>(respuesta.objetoDto)).empleadoID;
        }
      });
  }
  nuevaCarga(form: NgForm) {
    this.empleado = new EmpleadoDto();
    form.resetForm(form);
  }
}
