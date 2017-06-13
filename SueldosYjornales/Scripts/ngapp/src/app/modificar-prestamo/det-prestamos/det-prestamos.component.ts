import { Component, OnInit, Input, DoCheck } from '@angular/core';
import { EmpleadoDto } from "app/dtos/empleado-dto";
import { PrestamosSimplesService } from "app/services/prestamos-simples.service";
import { PrestamoSimpleDto } from "app/dtos/prestamo-simple-dto";

@Component({
  selector: 'app-det-prestamos',
  templateUrl: './det-prestamos.component.html',
  styleUrls: ['./det-prestamos.component.css']
})
export class DetPrestamosComponent implements OnInit, DoCheck {
  @Input() empleadoSelect: EmpleadoDto;
  prestamosSimplesDto: PrestamoSimpleDto[] = [];
  empleadoID_old: number = 0;
  constructor(private _prestamosSimplesService: PrestamosSimplesService) { }
  ngDoCheck(): void {
    if (this.empleadoSelect) {
      if (this.empleadoSelect.empleadoID !== this.empleadoID_old) {
        this.getByEmpleadoIDConCuotas();
        this.empleadoID_old = this.empleadoSelect.empleadoID;
      }
    }
  }

  ngOnInit() {
  }

  // Funciones
  getByEmpleadoIDConCuotas() {
    this._prestamosSimplesService.GetByEmpleadoIDConCuotas(this.empleadoSelect.empleadoID)
      .subscribe((prestamosSimpleDto) => {
        this.prestamosSimplesDto = prestamosSimpleDto;
      });
  }

}
