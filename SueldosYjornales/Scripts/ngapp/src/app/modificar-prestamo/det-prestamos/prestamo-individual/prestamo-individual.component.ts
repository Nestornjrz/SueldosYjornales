import { Component, OnInit, Input } from '@angular/core';
import { PrestamoSimpleDto } from "app/dtos/prestamo-simple-dto";

@Component({
  selector: 'app-prestamo-individual',
  templateUrl: './prestamo-individual.component.html',
  styleUrls: ['./prestamo-individual.component.css']
})
export class PrestamoIndividualComponent implements OnInit {
  @Input() prestamoSimple:PrestamoSimpleDto;
  constructor() { }

  ngOnInit() {
    let montoTotal = this.prestamoSimple.monto;
    let saldo =  this.prestamoSimple.monto;;
    this.prestamoSimple.cuotasMov.forEach((cuo)=>{
      if(cuo.movEmpleadoIDdeLaLiquidacion > 0){
        saldo = saldo - cuo.debito;
      }
      cuo.saldo = saldo;
    });
  }

}
