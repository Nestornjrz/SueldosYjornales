import { Component, OnInit } from '@angular/core';
import { EmpleadoDto } from 'src/app/dtos/empleado-dto';

@Component({
  selector: 'app-emp-datos-basicos',
  templateUrl: './emp-datos-basicos.component.html',
  styleUrls: ['./emp-datos-basicos.component.css']
})
export class EmpDatosBasicosComponent implements OnInit {
  empleado: EmpleadoDto = new EmpleadoDto();
  constructor() { }

  ngOnInit() {
  }

}
