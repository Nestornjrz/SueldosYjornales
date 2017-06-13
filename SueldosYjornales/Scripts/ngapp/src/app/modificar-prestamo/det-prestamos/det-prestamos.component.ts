import { Component, OnInit, Input } from '@angular/core';
import { EmpleadoDto } from "app/dtos/empleado-dto";

@Component({
  selector: 'app-det-prestamos',
  templateUrl: './det-prestamos.component.html',
  styleUrls: ['./det-prestamos.component.css']
})
export class DetPrestamosComponent implements OnInit {

  @Input()   empleadoSelect:EmpleadoDto;;
  constructor() { }

  ngOnInit() {
  }

}
