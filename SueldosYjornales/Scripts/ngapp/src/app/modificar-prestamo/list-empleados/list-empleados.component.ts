import { Component, Output, EventEmitter, OnInit, DoCheck } from '@angular/core';

// import { debounce } from "lodash";
import * as _ from 'lodash';

import { EmpleadosService } from 'app/services/empleados.service';
import { EmpleadoDto } from 'app/dtos/empleado-dto';
import { SucursaleDto } from 'app/dtos/sucursale-dto';

export class GroupBySucursal {
  sucursal: SucursaleDto;
  empleados: EmpleadoDto[] = [];
  constructor(sucursal: SucursaleDto) {
    this.sucursal = sucursal;
  }
}

@Component({
  selector: 'app-list-empleados',
  templateUrl: './list-empleados.component.html',
  styleUrls: ['./list-empleados.component.css']
})
export class ListEmpleadosComponent implements OnInit, DoCheck {
  empleados: EmpleadoDto[] = [];
  empleadosPorSucursal: GroupBySucursal[];
  empleadoSelect:EmpleadoDto;
  @Output() empleadoSelectEvent:EventEmitter<EmpleadoDto> = new EventEmitter<EmpleadoDto>();
  selectedValues: string[] = ['activos'];
  cantSelectedValues = 0;
  cantSelectedValuesOld = 0;
  constructor(private _empleadosService: EmpleadosService) { }

  ngDoCheck(): void {
    // Se verifica si el SelectedValues no cambio
    if (this.selectedValues.length !== this.cantSelectedValuesOld && this.empleados.length > 0) {
      this.selectedValuesChanges();
      console.log(this.selectedValues);
      this.cantSelectedValuesOld = this.selectedValues.length;
    }
  }
  ngOnInit() {
    this._empleadosService.getEmpleados()
      .subscribe((empleados) => {
        this.empleados = empleados;
        this.empleados.forEach((emp) => {
          emp.edad = EmpleadoDto.getEdad(emp.fechaNacimiento);
        });
        this.empleadosPorSucursal = this.agruparPorSucursal(empleados);
      });
  }

  /// Funciones privadas
  private agruparPorSucursal(empleados: EmpleadoDto[]): GroupBySucursal[] {
    // Se hace el listado de sucursales
    let sucursales: SucursaleDto[] = [];
    this.empleados.forEach((empleado, index) => {
      sucursales.push(empleado.sucursale);
    });
    sucursales = _.uniqBy(sucursales, (x: SucursaleDto) => {
      return x.sucursalID
    });
    console.log(sucursales);
    // Se agrupa
    const empleadosPorSucursal: GroupBySucursal[] = [];
    sucursales.forEach(sucursal => {
      const grupo = new GroupBySucursal(sucursal);
      grupo.empleados = _.filter(empleados, (e: EmpleadoDto) => {
        let pasaSn = false;
        // var activos = _.find(this.selectedValues, (v) => { return v == "activos" })
        if (_.find(this.selectedValues, (v) => { return v === 'activos' }) === 'activos') {
          if (e.activo === true) { pasaSn = true; }
        }
        if (_.find(this.selectedValues, (v) => { return v === 'salidos' }) === 'salidos') {
          if (e.activo === false) { pasaSn = true; }
        }
        return e.sucursale.sucursalID === sucursal.sucursalID && pasaSn;
      })
      empleadosPorSucursal.push(grupo);
    });
    console.log(empleadosPorSucursal);
    return empleadosPorSucursal;
  }
  private selectedValuesChanges() {
    this.empleadosPorSucursal = this.agruparPorSucursal(this.empleados);
  }

  seleccionarEmpleado(empleado:EmpleadoDto){    
    this.empleadoSelect = empleado;
    this.empleadoSelectEvent.emit(this.empleadoSelect);
  }
}
