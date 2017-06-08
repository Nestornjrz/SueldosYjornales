import { Component, OnInit } from '@angular/core';
import { EmpleadosService } from "app/services/empleados.service";
import { EmpleadoDto } from "app/dtos/empleado-dto";

@Component({
  selector: 'app-list-empleados',
  templateUrl: './list-empleados.component.html',
  styleUrls: ['./list-empleados.component.css']
})
export class ListEmpleadosComponent implements OnInit {
  empleados: EmpleadoDto[];
  constructor(private _empleadosService: EmpleadosService) {}

  ngOnInit() {
        this._empleadosService.getEmpleados()
      .subscribe((empleados) => {
        this.empleados = empleados;
      });
  }

}
