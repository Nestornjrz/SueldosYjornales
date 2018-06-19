import { Component, OnInit } from '@angular/core';
import { HttpClientService } from 'src/app/services/http-client.service';
import { EmpleadoDto } from 'src/app/dtos/empleado-dto';

@Component({
  selector: 'app-emp-list-datos-basicos',
  templateUrl: './emp-list-datos-basicos.component.html',
  styleUrls: ['./emp-list-datos-basicos.component.css']
})
export class EmpListDatosBasicosComponent implements OnInit {
  empleados: EmpleadoDto[] = [];
  constructor(private _http: HttpClientService) { }

  ngOnInit() {
    this.getListadoEmpleados();
  }

  getListadoEmpleados() {
    this._http.getArray<EmpleadoDto>('/api/Empleados/ConUsuarioIndeterminado')
      .subscribe(respuesta => {
        this.empleados = respuesta;
      });
  }

}
