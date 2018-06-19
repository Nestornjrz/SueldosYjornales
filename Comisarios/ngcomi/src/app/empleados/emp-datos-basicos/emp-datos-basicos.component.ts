import { Component, OnInit } from '@angular/core';
import { EmpleadoDto } from 'src/app/dtos/empleado-dto';
import { NgPrimeCustomService } from 'src/app/services/ng-prime-custom.service';
import { ProfesioneDto } from 'src/app/dtos/profesione-dto';
import { HttpClientService } from 'src/app/services/http-client.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-emp-datos-basicos',
  templateUrl: './emp-datos-basicos.component.html',
  styleUrls: ['./emp-datos-basicos.component.css']
})
export class EmpDatosBasicosComponent implements OnInit {
  empleado: EmpleadoDto = new EmpleadoDto();
  profesiones: ProfesioneDto[] = [];
  es = this.ngPrimeCustomService.getConfiguracionEspenol();
  constructor(private ngPrimeCustomService: NgPrimeCustomService, private _http: HttpClientService) { }
  ngOnInit() {
    this.getProfesiones();
  }

  getProfesiones() {
    this._http.getArray<ProfesioneDto>('/api/Profesiones')
      .subscribe(respuesta => {
        this.profesiones = respuesta;
      });
  }
  submitFormEmpleados(form: NgForm) {

  }

}
