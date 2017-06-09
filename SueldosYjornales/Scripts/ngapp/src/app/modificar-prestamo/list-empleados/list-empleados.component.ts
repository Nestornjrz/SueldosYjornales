import { Component, OnInit, DoCheck } from '@angular/core';

// import { debounce } from "lodash";
import * as _ from "lodash";

import { EmpleadosService } from "app/services/empleados.service";
import { EmpleadoDto } from "app/dtos/empleado-dto";
import { SucursaleDto } from "app/dtos/sucursale-dto";

export class GroupBySucursal {
  constructor(sucursal: SucursaleDto) {
    this.sucursal = sucursal;
  }
  sucursal: SucursaleDto;
  empleados: EmpleadoDto[] = [];
}

@Component({
  selector: 'app-list-empleados',
  templateUrl: './list-empleados.component.html',
  styleUrls: ['./list-empleados.component.css']
})
export class ListEmpleadosComponent implements OnInit, DoCheck {
  empleados: EmpleadoDto[] = [];
  empleadosPorSucursal: GroupBySucursal[];
  selectedValues: string[] = ['activos'];
  cantSelectedValues: number = 0;
  cantSelectedValuesOld: number = 0;
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
        this.empleados.forEach((emp)=>{
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
    let empleadosPorSucursal: GroupBySucursal[] = [];
    sucursales.forEach(sucursal => {
      let grupo = new GroupBySucursal(sucursal);
      grupo.empleados = _.filter(empleados, (e: EmpleadoDto) => {
        let pasaSn: boolean = false;
        //var activos = _.find(this.selectedValues, (v) => { return v == "activos" })
        if (_.find(this.selectedValues, (v) => { return v == "activos" }) == "activos") {
          if (e.activo == true) { pasaSn = true; }
        }
        if (_.find(this.selectedValues, (v) => { return v == "salidos" }) == "salidos") {
          if (e.activo == false) { pasaSn = true; }
        }
        return e.sucursale.sucursalID == sucursal.sucursalID && pasaSn;
      })
      empleadosPorSucursal.push(grupo);
    });
    console.log(empleadosPorSucursal);
    return empleadosPorSucursal;
  }
  private selectedValuesChanges() {
    this.empleadosPorSucursal = this.agruparPorSucursal(this.empleados);
  }
}
