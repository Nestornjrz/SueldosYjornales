import { Component, OnInit } from '@angular/core';
import { EmpleadosService } from 'app/services/empleados.service';
import { EmpleadoDto } from 'app/dtos/empleado-dto';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  pageTitle = 'PRESTAMOS';

  constructor(private _empleadosService: EmpleadosService) {}
  ngOnInit(): void {

  }
}
