import { Component, OnInit } from '@angular/core';
import { EmpleadoDto } from 'src/app/dtos/empleado-dto';
import { NgPrimeCustomService } from 'src/app/services/ng-prime-custom.service';

@Component({
  selector: 'app-emp-datos-basicos',
  templateUrl: './emp-datos-basicos.component.html',
  styleUrls: ['./emp-datos-basicos.component.css']
})
export class EmpDatosBasicosComponent implements OnInit {
  empleado: EmpleadoDto = new EmpleadoDto();
  es = this.ngPrimeCustomService.getConfiguracionEspenol();
  constructor(private ngPrimeCustomService: NgPrimeCustomService) { }
  ngOnInit() {
  }

}
