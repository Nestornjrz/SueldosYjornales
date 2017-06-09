import { Component, OnInit } from '@angular/core';
import { EmpleadoDto } from "app/dtos/empleado-dto";

@Component({
  selector: 'app-modificar-prestamo',
  templateUrl: './modificar-prestamo.component.html',
  styleUrls: ['./modificar-prestamo.component.css']
})
export class ModificarPrestamoComponent implements OnInit {
  empleadoSelect:EmpleadoDto;
  constructor() { }

  ngOnInit() {
  }
  onEmpleadoSelect(empleado: EmpleadoDto, event) {
    this.empleadoSelect = empleado;
  }
}
